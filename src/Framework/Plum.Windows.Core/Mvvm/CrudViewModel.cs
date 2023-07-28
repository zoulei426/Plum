using Plum.Object;
using Plum.Validation;
using Plum.Windows.Commands;
using Plum.Windows.Params;
using Prism.Events;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Plum.Windows.Mvvm
{
    public abstract class CrudViewModel<TModel, TKey, TCreateModel, TUpdateModel, TSelectedChangedEvent, TRefreshEvent> :
        ViewModelBase
        where TModel : DataViewObject<TKey>
        where TCreateModel : ValidityDvo
        where TUpdateModel : ValidityDvo
        where TSelectedChangedEvent : PubSubEvent<TModel>, new()
        where TRefreshEvent : PubSubEvent, new()
    {
        #region Properties

        public abstract string ModelName { get; set; }

        public TModel SelectedItem { get; set; }

        protected readonly IValidatorProvider ValidatorProvider;

        #endregion Properties

        #region Commands

        public virtual ICommand CreateCommand { get; set; }

        public virtual ICommand UpdateCommand { get; set; }

        public virtual ICommand DeleteCommand { get; set; }

        #endregion Commands

        #region Ctor

        public CrudViewModel(IContainerExtension container) : base(container)
        {
            ValidatorProvider = Container.Resolve<IValidatorProvider>();
        }

        #endregion Ctor

        #region Methods

        protected override void RegisterCommands()
        {
            CreateCommand = new RelayCommand(ExecuteCreate, CanCreate);
            UpdateCommand = new RelayCommand(ExecuteUpdate, CanUpdate);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanDelete);
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            EventAggregator.GetEvent<TSelectedChangedEvent>().Subscribe(x => { SelectedItem = x; });
        }

        protected virtual bool CanCreate()
        {
            return true;
        }

        protected async virtual void ExecuteCreate()
        {
            var model = await GetCreateModelAsync();
            //var provider = Container.Resolve<IValidatorProvider>();
            //var model = (TCreateModel)Activator.CreateInstance(typeof(TCreateModel), provider);
            model.ValidAsync();

            await ShowObjectDialog($"创建{ModelName}", model, async args =>
            {
                if (args.Result == ButtonResult.OK || args.Result == ButtonResult.Yes)
                {
                    var @object = (args.Parameters as PropertyGridDialogParameters).Object;

                    var result = await OnCreatedAsync(@object);

                    if (result)
                    {
                        OnCreateSuccess();
                        OnRefresh();
                    }
                }
            });
        }

        protected virtual void OnCreateSuccess()
        {
            Notifier.Success($"创建{ModelName}成功");
        }

        protected virtual bool CanUpdate()
        {
            return SelectedItem is not null;
        }

        protected async virtual void ExecuteUpdate()
        {
            var model = await GetUpdateModelAsync(SelectedItem.Id);
            model.ValidAsync();

            await ShowObjectDialog($"编辑{ModelName}", model, async args =>
            {
                if (args.Result == ButtonResult.OK || args.Result == ButtonResult.Yes)
                {
                    var @object = (args.Parameters as PropertyGridDialogParameters).Object;

                    var result = await OnUpdatedAsync(@object);

                    if (result)
                    {
                        OnUpdateSuccess();
                        OnRefresh();
                    }
                }
            });
        }

        protected virtual void OnUpdateSuccess()
        {
            Notifier.Success($"更新{ModelName}成功");
        }

        protected virtual bool CanDelete()
        {
            return SelectedItem is not null;
        }

        protected async virtual void ExecuteDelete()
        {
            await ShowConfirmDialogAsync(null, async dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK || dialogResult.Result == ButtonResult.Yes)
                {
                    var result = await OnDeletedAsync();
                    if (result)
                    {
                        OnDeleteSuccess();
                        OnRefresh();
                    }
                }
            });
        }

        protected virtual void OnDeleteSuccess()
        {
            Notifier.Success($"删除{ModelName}成功");
        }

        protected virtual void OnRefresh()
        {
            EventAggregator.GetEvent<TRefreshEvent>().Publish();
        }

        #region Methods - Abstract

        protected abstract Task<TCreateModel> GetCreateModelAsync();

        protected abstract Task<TUpdateModel> GetUpdateModelAsync(TKey id);

        protected abstract Task<bool> OnCreatedAsync(object @object);

        protected abstract Task<bool> OnUpdatedAsync(object @object);

        protected abstract Task<bool> OnDeletedAsync();

        #endregion Methods - Abstract

        #endregion Methods
    }
}