using Plum.Windows.Convertors;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Plum.Windows.Controls.Dialog
{
    /// <summary>
    /// GridDialog.xaml 的交互逻辑
    /// </summary>
    public partial class GridDialog : UserControl
    {
        public GridDialog()
        {
            InitializeComponent();
        }

        private void dg_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var pi = (e.PropertyDescriptor as System.ComponentModel.PropertyDescriptor).ComponentType
                .GetProperties().FirstOrDefault(x => x.Name == e.PropertyName);

            if (pi is not null)
            {
                var b = new Binding(pi.Name);
                if (pi.PropertyType.IsEnum)
                {
                    b.Converter = new EnumDescriptionConverter();
                }
                else
                {
                    b.Converter = new ObjectToSingleLineStringConverter();
                }

                var col = new MaterialDesignThemes.Wpf.DataGridTextColumn
                {
                    Header = pi.GetDisplayName(),
                    Binding = b
                };

                e.Column = col;
            }
        }
    }
}