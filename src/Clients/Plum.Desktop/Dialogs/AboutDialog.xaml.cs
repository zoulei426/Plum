using System.Windows.Controls;

namespace Plum.Dialogs
{
    /// <summary>
    /// AboutDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AboutDialog : UserControl
    {
        public AboutDialog()
        {
            InitializeComponent();
            var a = this.DataContext;
        }
    }
}