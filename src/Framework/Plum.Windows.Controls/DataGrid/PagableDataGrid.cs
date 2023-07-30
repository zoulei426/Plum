using Plum.Windows.Convertors;
using Plum.Windows.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Plum.Windows.Controls
{
    public class PagableDataGrid : DataGrid
    {
        static PagableDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PagableDataGrid), new FrameworkPropertyMetadata(typeof(PagableDataGrid)));
        }

        #region Properties

        public bool IsSortingEnabled
        {
            get { return (bool)GetValue(IsSortingEnabledProperty); }
            set { SetValue(IsSortingEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsSortingEnabledProperty =
            DependencyProperty.Register("IsSortingEnabled", typeof(bool), typeof(PagableDataGrid), new PropertyMetadata(true));

        public bool IsDynamicColumns
        {
            get { return (bool)GetValue(IsDynamicColumnsProperty); }
            set { SetValue(IsDynamicColumnsProperty, value); }
        }

        public static readonly DependencyProperty IsDynamicColumnsProperty =
            DependencyProperty.Register("IsDynamicColumns", typeof(bool), typeof(PagableDataGrid), new PropertyMetadata(false));

        public IDataPagerProvider DataSource
        {
            get { return (IDataPagerProvider)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(IDataPagerProvider), typeof(PagableDataGrid), new PropertyMetadata(null, (s, a) =>
            {
                var dg = s as PagableDataGrid;
                dg.SetDataSource(a.NewValue as IDataPagerProvider);
            }));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(string), typeof(PagableDataGrid), new PropertyMetadata(null, OnFilterChanged));

        private static void OnFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dg = d as PagableDataGrid;
            dg.SetFilter(e.NewValue?.ToString());
        }

        #region PageInfo

        public long TotalCount
        {
            get { return (long)GetValue(TotalCountProperty); }
            set { SetValue(TotalCountProperty, value); }
        }

        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register("TotalCount", typeof(long), typeof(PagableDataGrid), new PropertyMetadata(0L));

        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(PagableDataGrid), new PropertyMetadata(1));

        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register(
                "PageIndex",
                typeof(int),
                typeof(PagableDataGrid),
                new PropertyMetadata(1, new PropertyChangedCallback(OnPageIndexChanged)));

        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dg = d as PagableDataGrid;
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
                typeof(PagableDataGrid),
                new PropertyMetadata(10, new PropertyChangedCallback(OnPageSizeChanged)));

        private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dg = d as PagableDataGrid;
            dg.SetPageSize((int)e.NewValue);
        }

        public ObservableCollection<int> PageSizeCollection
        {
            get { return (ObservableCollection<int>)GetValue(PageSizeCollectionProperty); }
            set { SetValue(PageSizeCollectionProperty, value); }
        }

        public static readonly DependencyProperty PageSizeCollectionProperty =
            DependencyProperty.Register(
                "PageSizeCollection",
                typeof(ObservableCollection<int>),
                typeof(PagableDataGrid),
                new PropertyMetadata(
                    new ObservableCollection<int>
                    {
                        10, 20, 50, 100, 200, 500
                    }));

        #endregion PageInfo

        public string SummaryStatistics
        {
            get { return (string)GetValue(SummaryStatisticsProperty); }
            set { SetValue(SummaryStatisticsProperty, value); }
        }

        public static readonly DependencyProperty SummaryStatisticsProperty =
            DependencyProperty.Register("SummaryStatistics", typeof(string), typeof(PagableDataGrid), new PropertyMetadata(string.Empty));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(PagableDataGrid), new PropertyMetadata(false));

        #region Internal Properties

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(PagableDataGrid), new PropertyMetadata(new PagableDataGridRefreshCommand()));

        #endregion Internal Properties

        #endregion Properties

        #region Fields

        private BackgroundWorker bgw;

        private IDataPagerProvider dataSource;

        private int pageIndex = 1;
        private int pageSize = 10;
        private string filter;

        private ObservableCollection<object> listPaging;

        private string lastOrderColumn;
        private bool lastOrder;

        #endregion Fields

        #region Events

        //public event EventHandler<InitializeDataGridColumnEventArgs> InitializeColumn;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Ctor

        public PagableDataGrid()
        {
            AutoGenerateColumns = false;

            listPaging = new ObservableCollection<object>();

            bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;

            bgw.DoWork += new DoWorkEventHandler((s, es) =>
            {
                Dispatcher.Invoke(() =>
                {
                    IsBusy = true;
                });

                RefreshPageInner();
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

        public void RefreshWithColumns()
        {
            RecreateColumns();
            if (!bgw.IsBusy)
                bgw.RunWorkerAsync();
        }

        public void Dispose()
        {
        }

        #endregion Methods - Public

        #region Methods - Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RefreshWithColumns();
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

        private void RecreateColumns()
        {
            bool auto = IsDynamicColumns;
            if (auto)
            {
                InstallDynamicColumns();
            }
            else
            {
                InstallFixedColumns();
            }
        }

        private void InstallFixedColumns()
        {
            if (dataSource is null)
                return;

            Dispatcher.Invoke(new Action(() => { IsBusy = true; }));

            Dispatcher.Invoke(new Action(() => { Columns?.Clear(); }));

            var columns = dataSource.GetProperties();

            foreach (var column in columns)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    var col = new DataGridTextColumn
                    {
                        Header = column.AliasName
                    };

                    var b = new Binding(column.PropertyName)
                    {
                        Converter = new ObjectToSingleLineStringConverter()
                    };
                    col.Binding = b;

                    Columns.Add(col);
                }));
            }

            Dispatcher.Invoke(new Action(() => { IsBusy = false; }));
        }

        private void InstallDynamicColumns()
        {
            //var properties = DataSource.GetProperties();

            //     Dispatcher.Invoke(new Action(() =>
            //     {
            //         DataGridTextColumn col;
            //         args.Column = col = new DataGridTextColumn();
            //         col.Header = property.AliasName;

            //         var b = new Binding(property.ColumnName);
            //         b.Converter = new ObjectToSingleLineStringConverter();
            //         col.Binding = b;
            //     }));
        }

        private void RefreshPageInner()
        {
            if (dataSource is null)
                return;

            var pagedResult = dataSource.PagingAsync(pageIndex - 1, pageSize, lastOrderColumn, filter);
            var summaryStatistics = dataSource.GetSumaryStatistic();

            Dispatcher.Invoke(new Action(() =>
            {
                listPaging.Clear();
                TotalCount = pagedResult.Count;

                PageCount = (int)((TotalCount + PageSize - 1) / PageSize);

                var list = pagedResult.Items;

                foreach (var item in list)
                {
                    listPaging.Add(item);
                }

                SummaryStatistics = summaryStatistics;
            }));
        }

        private void SetDataSource(IDataPagerProvider value)
        {
            dataSource = value;
            RefreshWithColumns();
        }

        private void SetPageIndex(int value)
        {
            pageIndex = value;
            Refresh();
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

    internal class PagableDataGridRefreshCommand : ICommand
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
            var pager = parameter as PagableDataGrid;

            pager.Refresh();
        }
    }
}