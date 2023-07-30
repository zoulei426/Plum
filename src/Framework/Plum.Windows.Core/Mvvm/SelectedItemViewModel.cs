using Plum.Object;
using Plum.Validation;
using Plum.Windows.Params;
using Prism.Events;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Plum.Windows.Mvvm
{
    public abstract class SelectedItemViewModel<TModel, TSelectedChangedEvent, TRefreshEvent> :
        ViewModelBase
        where TModel : DataViewObject
        where TSelectedChangedEvent : PubSubEvent<TModel>, new()
        where TRefreshEvent : PubSubEvent, new()
    {
        #region Properties

        public TModel SelectedItem { get; set; }

        protected readonly IValidatorProvider ValidatorProvider;

        #endregion Properties

        #region Commands

        #endregion Commands

        #region Ctor

        public SelectedItemViewModel(IContainerExtension container) : base(container)
        {
            ValidatorProvider = Container.Resolve<IValidatorProvider>();
        }

        #endregion Ctor

        #region Methods

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            EventAggregator.GetEvent<TSelectedChangedEvent>().Subscribe(x => { SelectedItem = x; });
        }

        protected virtual void OnRefresh()
        {
            EventAggregator.GetEvent<TRefreshEvent>().Publish();
        }

        #region Methods - Abstract

        #endregion Methods - Abstract

        #endregion Methods
    }
}