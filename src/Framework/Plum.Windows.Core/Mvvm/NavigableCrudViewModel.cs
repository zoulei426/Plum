using Plum.Object;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;

namespace Plum.Windows.Mvvm
{
    public abstract class NavigableCrudViewModel<TModel, TKey, TCreateModel, TUpdateModel, TSelectedChangedEvent, TRefreshEvent> :
        CrudViewModel<TModel, TKey, TCreateModel, TUpdateModel, TSelectedChangedEvent, TRefreshEvent>,
        INavigationAware
        where TModel : DataViewObject<TKey>
        where TCreateModel : ValidityDvo
        where TUpdateModel : ValidityDvo
        where TSelectedChangedEvent : PubSubEvent<TModel>, new()
        where TRefreshEvent : PubSubEvent, new()
    {
        /// <summary>
        /// 通过导航传递的参数
        /// </summary>
        public NavigationParameters NavParams { get; set; }

        public NavigableCrudViewModel(IContainerExtension container) : base(container)
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