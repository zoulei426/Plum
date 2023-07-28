using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Plum.Windows.Controls
{
    /// <summary>
    /// 自定义文件打开选择器控件
    /// </summary>
    public class PropertyDescriptorBuilderOpenFileBrowser : PropertyDescriptorBuilder
    {
        #region Fields

        private readonly string _filter;

        #endregion Fields

        #region Methods

        #region Methods - Override

        /// <summary>
        /// 构建控件描述信息
        /// </summary>
        /// <param name="defaultValue">默认值</param>
        /// <returns>控件描述信息</returns>
        public override PropertyDescriptor Build(PropertyDescriptor defaultValue)
        {
            defaultValue.Designer.Dispatcher.Invoke(() =>
            {
                var grid = new Grid();
                var column1 = new ColumnDefinition();
                var column2 = new ColumnDefinition();
                column1.Width = new GridLength(1, GridUnitType.Star);
                column2.Width = GridLength.Auto;
                grid.ColumnDefinitions.Add(column1);
                grid.ColumnDefinitions.Add(column2);

                var button = new Button()
                {
                    Content = new PackIcon()
                    {
                        Kind = PackIconKind.FolderText,
                        Width = 25,
                        Height = 25
                    },
                    ToolTip = "选择文件",
                    Command = new DelegateCommand(() =>
                    {
                        var dlg = new OpenFileDialog()
                        {
                            Filter = _filter,
                            CheckFileExists = true,
                            CheckPathExists = true
                        };

                        if (dlg.ShowDialog().GetValueOrDefault())
                        {
                            defaultValue.Value = dlg.FileName;
                        }
                    }),
                    Width = 50,
                    Padding = new Thickness(0),
                    Margin = new Thickness(5, 0, 5, 0)
                };
                button.SetResourceReference(Button.StyleProperty, "MaterialDesignFlatButton");

                // 双向绑定结果值
                var binding = new Binding(nameof(PropertyDescriptor.Value))
                {
                    Source = defaultValue,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
                };

                var textBox = new TextBox();

                textBox.SetBinding(TextBox.TextProperty, binding);

                grid.Children.Add(textBox);
                grid.Children.Add(button);
                Grid.SetColumn(textBox, 0);
                Grid.SetColumn(button, 1);

                defaultValue.Designer = grid;
            });

            return defaultValue;
        }

        #endregion Methods - Override

        #endregion Methods

        #region Ctor

        public PropertyDescriptorBuilderOpenFileBrowser() : this("所有文件(*.*)|*.*")
        {
        }

        public PropertyDescriptorBuilderOpenFileBrowser(string filter)
        {
            _filter = filter;
        }

        #endregion Ctor
    }
}