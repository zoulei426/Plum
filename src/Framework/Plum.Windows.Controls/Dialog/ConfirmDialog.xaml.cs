using Plum.Windows.Mvvm;
using System.Windows.Controls;

namespace Plum.Windows.Controls.Dialog
{
    /// <summary>
    /// ConfirmDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmDialog : UserControl, IConfirmDialog
    {
        public ConfirmDialog()
        {
            InitializeComponent();
        }
    }
}