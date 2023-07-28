using System.ComponentModel;
using System.Windows;

namespace Plum.Windows.Authorizations
{
    public class AuthorizeSource : INotifyPropertyChanged
    {
        private readonly string _key;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Fields

        private BackgroundWorker bgw;
        private Visibility visibility = Visibility.Collapsed;

        #endregion Fields

        public AuthorizeSource(string key, FrameworkElement element = null)
        {
            _key = key;

            if (element != null)
            {
                element.Loaded += OnLoaded;
                element.Unloaded += OnUnloaded;
            }

            bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;

            bgw.DoWork += new DoWorkEventHandler((s, es) =>
            {
                visibility = AuthorizeManager.Instance.IsGranted(_key) ? Visibility.Visible : Visibility.Collapsed;
            });

            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((s, es) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));
            });
        }

        public Visibility Visibility => visibility;

        private void RaiseValue()
        {
            bgw.RunWorkerAsync();
        }

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            RaiseValue();
        }

        /// <summary>
        /// Called when [unloaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            RaiseValue();
        }

        public static implicit operator AuthorizeSource(string resourceKey) => new(resourceKey);
    }
}