using Prism.Events;
using Prism.Ioc;
using System.Windows;

namespace Plum.Windows.Mvvm
{
    public abstract class PagableViewModel<TView, TSelectedItem, TSelectedChangedEvent, TRefreshEvent> :
        ViewModelBase,
        IViewLoadedAndUnloadedAware<TView>
        where TView : FrameworkElement
        where TSelectedChangedEvent : PubSubEvent<TSelectedItem>, new()
        where TRefreshEvent : PubSubEvent, new()
    {
        public IDataPagerProvider DataSource { get; set; }

        private TSelectedItem _SelectedItem;

        public TSelectedItem SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnSelectedItemChanged(value);
            }
        }

        public PagableViewModel(IContainerExtension container) : base(container)
        {
        }

        public virtual void OnLoaded(TView view)
        {
        }

        public void OnUnloaded(TView view)
        {
            SelectedItem = default;
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            EventAggregator.GetEvent<TRefreshEvent>().Subscribe(OnRefresh);
        }

        private void OnSelectedItemChanged(TSelectedItem value)
        {
            EventAggregator.GetEvent<TSelectedChangedEvent>().Publish(value);
        }

        protected abstract void OnRefresh();
    }
}