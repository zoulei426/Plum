using Plum.Attributes;
using Plum.Object;
using Plum.Windows.DataDictionaries;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Plum.Windows.Controls
{
    public class PropertyDescriptorBuilderDataDictionary : PropertyDescriptorBuilder
    {
        public ObservableKeyValueList<string, string> Items { get; private set; }

        public override PropertyDescriptor Build(PropertyDescriptor defaultValue)
        {
            defaultValue.Designer.Dispatcher.Invoke(new Action(() =>
            {
                var pi = defaultValue.Object.GetType().GetProperty(defaultValue.Name);
                var dicCode = pi.GetAttribute<DataDictionaryAttribute>()?.Code;

                var dic = DataDictionaryManager.Instance.GetDictionaryByCode(dicCode);

                Items = new ObservableKeyValueList<string, string>();

                foreach (var item in dic)
                {
                    Items[item.Key] = item.Value;
                }

                var cb = new ComboBox();
                cb.SelectedValuePath = "Key";
                cb.DisplayMemberPath = "Value";
                cb.ItemsSource = Items;

                //var b = new Binding("Items");
                //b.Source = defaultValue;
                //cb.SetBinding(ComboBox.ItemsSourceProperty, b);

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