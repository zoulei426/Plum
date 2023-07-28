using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace Plum.Windows.Mvvm
{
    public abstract class NavigablePagableViewModel<TView, TSelectedItem, TSelectedChangedEvent, TRefreshEvent> :
        PagableViewModel<TView, TSelectedItem, TSelectedChangedEvent, TRefreshEvent>,
        INavigationAware
        where TView : FrameworkElement
        where TSelectedChangedEvent : PubSubEvent<TSelectedItem>, new()
        where TRefreshEvent : PubSubEvent, new()
    {
        /// <summary>
        /// 通过导航传递的参数
        /// </summary>
        public NavigationParameters NavParams { get; set; }

        public NavigablePagableViewModel(IContainerExtension container) : base(container)
        {
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            NavParams = navigationContext.Parameters;
        }
    }
}