using Prism.Regions;

namespace Plum.Windows.Params
{
    public class SelectedItemNavigationParameters : NavigationParameters
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

        public SelectedItemNavigationParameters(object @object)
        {
            Object = @object;
        }
    }
}