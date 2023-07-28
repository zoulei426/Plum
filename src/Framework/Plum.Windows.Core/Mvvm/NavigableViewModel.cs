using Prism.Ioc;
using Prism.Regions;

namespace Plum.Windows.Mvvm
{
    public class NavigableViewModel : ViewModelBase, INavigationAware
    {
        /// <summary>
        /// 通过导航传递的参数
        /// </summary>
        public NavigationParameters NavParams { get; set; }

        public NavigableViewModel(IContainerExtension container) : base(container)
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