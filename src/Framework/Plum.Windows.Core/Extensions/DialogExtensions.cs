using Prism.Services.Dialogs;

namespace Plum.Windows
{
    public static class DialogExtensions
    {
        public static void AddRange(this IDialogParameters @this, IDialogParameters parameters)
        {
            if (@this is null || parameters is null) return;
            foreach (var key in parameters.Keys)
            {
                var values = parameters.GetValues<object>(key);
                foreach (var value in values)
                {
                    @this.Add(key, value);
                }
            }
        }
    }
}