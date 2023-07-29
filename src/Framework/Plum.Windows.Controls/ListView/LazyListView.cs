using Plum.Windows.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Plum.Windows.Controls
{
    public class LazyListView : ListView
    {
        static LazyListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LazyListView), new FrameworkPropertyMetadata(typeof(LazyListView)));
        }

        #region Properties

        public IDataPagerProvider DataSource
        {
            get { return (IDataPagerProvider)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(IDataPagerProvider), typeof(LazyListView), new PropertyMetadata(null, (s, a) =>
            {
                var dg = s as LazyListView;
                dg.SetDataSource(a.NewValue as IDataPagerProvider);
            }));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(LazyListView), new PropertyMetadata(null, OnFilterChanged));

        private static void OnFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dg = d as LazyListView;
            dg.SetFilter(e.NewValue?.ToString());
        }

        #region PageInfo

        public long TotalCount
        {
            get { return (long)GetValue(TotalCountProperty); }
            set { SetValue(TotalCountProperty, value); }
        }

        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register("TotalCount", typeof(long), typeof(LazyListView), new PropertyMetadata(0L));

        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(LazyListView), new PropertyMetadata(1));

        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register(
                "PageIndex",
                typeof(int),
                typeof(LazyListView),
                new PropertyMetadata(1, new PropertyChangedCallback(OnPageIndexChanged)));

        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dg = d as LazyListView;
            dg.SetPageIndex((int)e.NewValue);
        }

        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register(
                "PageSize",
                typeof(int),
                typeof(LazyListView),
                new PropertyMetadata(20, new PropertyChangedCallback(OnPageSizeChanged)));

        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dg = d as LazyListView;
            dg.SetPageSize((int)e.NewValue);
        }

        #endregion PageInfo

        #endregion Properties

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(LazyListView), new PropertyMetadata(false));

        #region Internal Properties

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(LazyListView), new PropertyMetadata(new LazyListViewRefreshCommand()));

        public ICommand LazyLoadCommand
        {
            get { return (ICommand)GetValue(LazyLoadCommandProperty); }
            set { SetValue(LazyLoadCommandProperty, value); }
        }

        public static readonly DependencyProperty LazyLoadCommandProperty =
            DependencyProperty.Register("LazyLoadCommand", typeof(ICommand), typeof(LazyListView), new PropertyMetadata(new LazyLoadCommand()));

        #endregion Internal Properties

        #region Fields

        private bool _IsBusy;
        private int cntIsBusy;
        private BackgroundWorker bgw;
        private IDataPagerProvider dataSource;

        private int pageIndex = 1;
        private int pageSize = 20;
        private string filter;

        private ObservableCollection<object> listPaging;
        private ScrollViewer sv;
        private string lastOrderColumn;

        #endregion Fields

        #region Events

        //public event EventHandler<InitializeDataGridColumnEventArgs> InitializeColumn;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Ctor

        public LazyListView()
        {
            listPaging = new ObservableCollection<object>();

            bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;

            bgw.DoWork += new DoWorkEventHandler((s, es) =>
            {
                Dispatcher.Invoke(() =>
                {
                    IsBusy = true;
                });

                RefreshPageInner((int?)es.Argument);
            });

            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((s, es) =>
            {
                Dispatcher.Invoke(() =>
                {
                    IsBusy = false;
                });

                ItemsSource = listPaging;
            });
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public void Refresh()
        {
            if (!bgw.IsBusy)
                bgw.RunWorkerAsync();
        }

        public void LazyLoad()
        {
            lock (this)
            {
                if (IsBusy) return;

                var pageIndex = PageIndex + 1;
                if (pageIndex > PageCount || listPaging.Count >= TotalCount)
                {
                    return;
                }

                if (!bgw.IsBusy)
                    bgw.RunWorkerAsync(pageIndex);
            }
        }

        #endregion Methods - Public

        #region Methods - Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            sv = GetTemplateChild("sv") as ScrollViewer;
            Refresh();
        }

        #endregion Methods - Override

        #region Methods - Protected

        protected void NotifyPropertyChanged(string propertyName)
        {
            var evt = PropertyChanged;
            if (evt == null)
                return;

            evt(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods - Protected

        #region Methods - Events

        #endregion Methods - Events

        #region Methods - Private

        private void RefreshPageInner(int? addPageIndex = null)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (addPageIndex.HasValue)
                {
                    pageIndex = addPageIndex.Value;
                    if (sv is not null)
                    {
                        if (sv.VerticalOffset > 1)
                            sv.ScrollToVerticalOffset(sv.VerticalOffset - 1);
                    }
                }
                else
                {
                    // 返回首页
                    pageIndex = 1;
                    // 滚动到顶端
                    if (sv is not null)
                        sv.ScrollToTop();
                    // 清空数据
                    listPaging.Clear();
                }
            }));

            var pagedResult = dataSource.PagingAsync(pageIndex - 1, pageSize, lastOrderColumn, filter);

            Dispatcher.Invoke(new Action(() =>
            {
                TotalCount = pagedResult.Count;

                PageCount = (int)((TotalCount + PageSize - 1) / PageSize);

                var list = pagedResult.Items;

                foreach (var item in list)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        listPaging.Add(item);
                    }));
                }
            }));
        }

        private void SetDataSource(IDataPagerProvider value)
        {
            dataSource = value;
            Refresh();
        }

        private void SetPageIndex(int value)
        {
            pageIndex = value;
            LazyLoad();
        }

        private void SetPageSize(int value)
        {
            pageSize = value;
            Refresh();
        }

        private void SetFilter(string value)
        {
            filter = value;
            Refresh();
        }

        #endregion Methods - Private

        #endregion Methods
    }

    internal class LazyListViewRefreshCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var pager = parameter as LazyListView;

            pager.Refresh();
        }
    }

    internal class LazyLoadCommand : ICommand
    {
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var pager = parameter as LazyListView;

            pager.LazyLoad();
        }
    }
}