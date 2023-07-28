using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Plum.Windows.Controls
{
    /// <summary>
    /// 次级数据字典
    /// </summary>
    public class PropertyDescriptorBuilderSubDataDictionary : PropertyDescriptorBuilder
    {
        //public ObservableKeyValueList<string, string> Items { get; set; }

        public override PropertyDescriptor Build(PropertyDescriptor defaultValue)
        {
            defaultValue.Designer.Dispatcher.Invoke(new Action(() =>
            {
                var pi = defaultValue.Object.GetType().GetProperty(defaultValue.Name);

                //Items = new ObservableKeyValueList<string, string>();

                var cb = new ComboBox();
                cb.SelectedValuePath = "Key";
                cb.DisplayMemberPath = "Value";
                //cb.ItemsSource = Items;

                var b1 = new Binding("Value");
                b1.Source = defaultValue;
                b1.ValidatesOnExceptions = true;
                b1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

                var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
                if (pda != null && pda.Converter != null)
                {
                    b1.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                    b1.ConverterParameter = new PropertyGridConverterParameterPair(defaultValue.PropertyGrid, pda.ConverterParameter);
                }

                cb.SetBinding(ComboBox.SelectedValueProperty, b1);

                defaultValue.Designer = cb;
            }));

            return defaultValue;
        }
    }
}