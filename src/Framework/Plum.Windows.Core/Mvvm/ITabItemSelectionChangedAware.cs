namespace Plum.Windows.Mvvm
{
    /// <summary>
    /// ITabItemSelectionChangedAware
    /// </summary>
    public interface ITabItemSelectionChangedAware
    {
        /// <summary>
        /// Called when [selected].
        /// </summary>
        void OnSelected();

        /// <summary>
        /// Called when [unselected].
        /// </summary>
        void OnUnselected();
    }
}