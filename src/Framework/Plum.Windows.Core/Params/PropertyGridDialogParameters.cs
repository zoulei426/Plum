using Prism.Services.Dialogs;

namespace Plum.Windows.Params
{
    public class PropertyGridDialogParameters : DialogParameters
    {
        public object Object
        {
            get
            {
                return GetValue<object>(nameof(Object));
            }
            set
            {
                Add(nameof(Object), value);
            }
        }

        public string Title
        {
            get
            {
                return GetValue<string>(nameof(Title));
            }
            set
            {
                Add(nameof(Title), value);
            }
        }

        public PropertyGridDialogParameters(object @object)
        {
            Object = @object;
        }
    }
}