using Plum.Attributes;
using Plum.Object;
using Plum.Tasks;
using Plum.Windows.Convertors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Plum.Windows.Controls
{
    public class PropertyDescriptor : INotifyPropertyChanged, IDisposable
    {
        #region Properties

        public string Gallery { get; set; }
        public string Name { get; set; }

        public string AliasName
        {
            get { return _AliasName; }
            set { _AliasName = value; NotifyPropertyChanged("AliasName"); }
        }

        public string Description { get; set; }
        public string Watermask { get; set; }
        public int Length { get; set; }

        public bool Editable { get; set; }

        public bool Required
        {
            get { return _Required; }
            set { _Required = value; NotifyPropertyChanged("Required"); }
        }

        private bool _Required;

        public bool Nullable
        {
            get { return _Nullable; }
            set { _Nullable = value; NotifyPropertyChanged("Nullable"); }
        }

        private bool _Nullable;

        public eDataType Type { get; set; }

        public UIElement Designer
        {
            get { return _Designer; }
            set { _Designer = value; BindingExpression = null; }
        }

        private UIElement _Designer;
        public PropertyInfo PropertyInfo { get; private set; }
        public object Object { get; set; }
        public ImageSource Image { get; set; }
        public PropertyTrigger Trigger { get; private set; }
        public PropertyGrid PropertyGrid { get; internal set; }

        public BindingExpressionBase BindingExpression { get; set; }

        public ImageSource ImageState
        {
            get { return _ImageState; }
            set { _ImageState = value; NotifyPropertyChanged("ImageState"); }
        }

        public object DefaultValue
        {
            get { return _valueDefault; }
            set { _valueDefault = value; NotifyPropertyChanged("DefaultValue"); }
        }

        public object Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        public Visibility ImageVisibility
        {
            get { return IsBusy ? Visibility.Hidden : Visibility.Visible; }
        }

        public Visibility LoadingVisibility
        {
            get { return IsBusy ? Visibility.Visible : Visibility.Hidden; }
        }

        public bool IsBusy
        {
            get { return GetIsBusy(); ; }
            set { SetIsBusy(value); }
        }

        public Visibility Visibility
        {
            get { return _Visibility; }
            set { _Visibility = value; NotifyPropertyChanged("Visibility"); }
        }

        public eMessageGrade Grade
        {
            get { return _Grade; }
            set { _Grade = value; NotifyPropertyChanged("Grade"); }
        }

        private eMessageGrade _Grade;

        public string DescriptionState
        {
            get { return _DescriptionState; }
            set { _DescriptionState = value; NotifyPropertyChanged("DescriptionState"); }
        }

        private string _DescriptionState;

        #endregion Properties

        #region Fields

        private static Dictionary<Type, PropertyDescriptorCreatorHandlerAttribute> handlers = null;

        //private DetentionReporter reporter;
        //private AutoResetEvent are;
        //private Exception exSetValue = null;
        private TaskQueue tq = new TaskQueueDispatcher(Application.Current.Dispatcher);

        private object _value;
        private object _valueDefault;
        private ImageSource _ImageState;
        private int cntBusy;
        private Visibility _Visibility;
        private String _AliasName;

        #endregion Fields

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Ctor

        static PropertyDescriptor()
        {
            handlers = PropertyDescriptorCreatorHandlerAttribute.Create(typeof(PropertyDescriptor));
        }

        public PropertyDescriptor(object obj, string propertyName)
        {
            Editable = true;

            var pi = obj.GetType().GetProperty(propertyName);
            var dt = pi.PropertyType.IsNullableGeneric() ?
                DotNetTypeAttribute.GetType(pi.PropertyType.GetGenericTypeInNullable()) :
                DotNetTypeAttribute.GetType(pi.PropertyType);

            Object = obj;
            PropertyInfo = pi;
            Type = dt;
            Name = pi.Name;
            Nullable = pi.PropertyType.IsNullable();

            var ra = pi.GetAttribute<RequiredAttribute>();
            Nullable = Nullable ? ra == null : Nullable;
            Required = ra != null;

            var dta = pi.GetAttribute<ColumnAttribute>();
            //Nullable = Nullable && dta != null ? dta.Nullable : Nullable;

            //var wa = pi.GetAttribute<WatermaskLanguageAttribute>();
            //Watermask = wa == null ? string.Empty : wa.Name;

            //var ga = pi.GetAttribute<GalleryLanguageAttribute>();
            //Gallery = ga == null ? string.Empty : ga.Name;

            var da = pi.GetAttribute<DescriptionAttribute>();
            AliasName = da == null ? null : da.Description;
            var da1 = pi.GetAttribute<DisplayNameAttribute>();
            AliasName = AliasName.IsNullOrEmpty() && da1 != null ? da1.DisplayName : AliasName;
            var da2 = pi.GetAttribute<DisplayAttribute>();
            AliasName = AliasName.IsNullOrEmpty() && da2 != null ? da2.Name : AliasName;
            AliasName = AliasName.IsNullOrEmpty() ? pi.Name : AliasName;

            var pa = pi.GetAttribute<DescriptionAttribute>();
            Description = pa == null ? AliasName : pa.Description;

            var ppa = pi.GetAttribute<PropertyDescriptorAttribute>();
            Image = ppa != null && !ppa.UriImage16.IsNullOrEmpty() ?
                BitmapFrame.Create(new Uri(ppa.UriImage16)) : null;

            //var ia = pi.GetAttribute<ImageAttribute>();
            //Image = Image == null ? (ia == null || ia.ImageUri.IsNullOrBlank() ?
            //    Image : (BitmapFrame.Create(new Uri(ia.ImageUri)))) : Image;

            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (pda != null)
            {
                var t = pda.CreateTrigger();
                if (t != null)
                    Trigger = t;
            }

            if (Trigger == null)
                Trigger = new PropertyTrigger();

            _value = obj.GetPropertyValue(propertyName);

            var b = new Binding("Value");
            b.Source = this;

            var tb = new TextBlock();
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.SetBinding(TextBlock.TextProperty, b);
            tb.SetBinding(TextBlock.ToolTipProperty, b);

            Designer = tb;
        }

        internal void Install()
        {
            var nt = Object as INotifyPropertyChanged;
            if (nt == null)
                return;
            nt.PropertyChanged += nt_PropertyChanged;
        }

        internal void Uninstall()
        {
            var nt = Object as INotifyPropertyChanged;
            if (nt == null)
                return;
            nt.PropertyChanged -= nt_PropertyChanged;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Static

        public static PropertyDescriptor Create(PropertyGrid pg, object obj, string propertyName, bool editable, string propClass, ePropertyGridContainerType type)
        {
            var pi = obj.GetType().GetProperty(propertyName);

            var ea = pi.GetAttribute<EnabledAttribute>();
            if (ea != null && !ea.Value)
                return null;

            var eai = pi.GetAttribute<EnabledIfAttribute>();
            if (eai != null)
            {
                var pv = obj.GetPropertyValue(eai.PropertyName)?.ToString();
                if (pv != eai.Value?.ToString())
                    return null;
            }

            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (propClass != "*" && pda.Class != propClass)
                return null;

            if ((!editable || (pda != null && !pda.Editable)) && (
                type == ePropertyGridContainerType.Details ||
                type == ePropertyGridContainerType.DetailsWithoutScrollViewer))
                return new PropertyDescriptorReadOnlyNoFrame(pg, obj, propertyName);

            if (!editable || (pda != null && !pda.Editable))
                return new PropertyDescriptorReadOnly(pg, obj, propertyName);

            if (pi.PropertyType.IsEnum)
                return new PropertyDescriptorEnum(pg, obj, propertyName);

            if (!handlers.ContainsKey(pi.PropertyType))
                return new PropertyDescriptor(obj, propertyName);

            return handlers[pi.PropertyType].Method.Invoke(
                null, new object[] { pg, obj, propertyName }) as PropertyDescriptor;
        }

        [PropertyDescriptorCreatorHandler(typeof(string))]
        private static PropertyDescriptor CreateString(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorString(pg, obj, propertyName);
        }

        [PropertyDescriptorCreatorHandler(typeof(int))]
        [PropertyDescriptorCreatorHandler(typeof(int?))]
        [PropertyDescriptorCreatorHandler(typeof(uint))]
        [PropertyDescriptorCreatorHandler(typeof(uint?))]
        private static PropertyDescriptor CreateInt32(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorInteger(pg, obj, propertyName);
        }

        [PropertyDescriptorCreatorHandler(typeof(short))]
        [PropertyDescriptorCreatorHandler(typeof(short?))]
        [PropertyDescriptorCreatorHandler(typeof(ushort))]
        [PropertyDescriptorCreatorHandler(typeof(ushort?))]
        private static PropertyDescriptor CreateInt16(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorShort(pg, obj, propertyName);
        }

        [PropertyDescriptorCreatorHandler(typeof(long))]
        [PropertyDescriptorCreatorHandler(typeof(long?))]
        [PropertyDescriptorCreatorHandler(typeof(ulong))]
        [PropertyDescriptorCreatorHandler(typeof(ulong?))]
        private static PropertyDescriptor CreateInt64(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorLong(pg, obj, propertyName);
        }

        [PropertyDescriptorCreatorHandler(typeof(byte))]
        [PropertyDescriptorCreatorHandler(typeof(byte?))]
        private static PropertyDescriptor CreateInt8(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorByte(pg, obj, propertyName);
        }

        [PropertyDescriptorCreatorHandler(typeof(float))]
        [PropertyDescriptorCreatorHandler(typeof(float?))]
        [PropertyDescriptorCreatorHandler(typeof(Single))]
        [PropertyDescriptorCreatorHandler(typeof(Single?))]
        private static PropertyDescriptor CreateFloat(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorFloat(pg, obj, propertyName);
        }

        [PropertyDescriptorCreatorHandler(typeof(double))]
        [PropertyDescriptorCreatorHandler(typeof(double?))]
        [PropertyDescriptorCreatorHandler(typeof(decimal))]
        [PropertyDescriptorCreatorHandler(typeof(decimal?))]
        private static PropertyDescriptor CreateDouble(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorDouble(pg, obj, propertyName);
        }

        [PropertyDescriptorCreatorHandler(typeof(bool))]
        [PropertyDescriptorCreatorHandler(typeof(bool?))]
        private static PropertyDescriptor CreateBoolean(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorBoolean(pg, obj, propertyName);
        }

        [PropertyDescriptorCreatorHandler(typeof(Guid))]
        [PropertyDescriptorCreatorHandler(typeof(Guid?))]
        private static PropertyDescriptor CreateGuid(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorGuid(pg, obj, propertyName);
        }

        [PropertyDescriptorCreatorHandler(typeof(DateTime))]
        [PropertyDescriptorCreatorHandler(typeof(DateTime?))]
        private static PropertyDescriptor CreateDateTime(PropertyGrid pg, object obj, string propertyName)
        {
            return new PropertyDescriptorDateTime(pg, obj, propertyName);
        }

        //[PropertyDescriptorCreatorHandler(typeof(Color))]
        //[PropertyDescriptorCreatorHandler(typeof(Color?))]
        //private static PropertyDescriptor CreateColor(PropertyGrid pg, object obj, string propertyName)
        //{
        //    return new PropertyDescriptorColor(pg, obj, propertyName);
        //}

        //[PropertyDescriptorCreatorHandler(typeof(Brush))]
        //private static PropertyDescriptor CreateSolidBrush(PropertyGrid pg, object obj, string propertyName)
        //{
        //    return new PropertyDescriptorSolidBrush(pg, obj, propertyName);
        //}

        #endregion Methods - Static

        #region Methods - Internal

        internal void PostPropertyValueChangedAsync(string propertyName, bool install)
        {
            if (Trigger == null)
                return;

            try
            {
                PropertyGrid.Dispatcher.Invoke(new Action(() => { IsBusy = true; }));

                if (install)
                    Trigger.PostPropertyValueInstalled(this, propertyName);
                else
                    Trigger.PostPropertyValueChanged(this, propertyName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                PropertyGrid.Dispatcher.Invoke(new Action(() => { IsBusy = false; }));
            }
        }

        #endregion Methods - Internal

        #region Methods - Protected

        protected void NotifyPropertyChanged(string propertyName)
        {
            var evt = PropertyChanged;
            if (evt == null)
                return;

            evt(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods - Protected

        #region Methods - Events

        private void nt_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!PropertyGrid.PropertyDescriptors.Any(c => c.Name == e.PropertyName))
                return;

            var broadcast = false;

            if (Application.Current.Dispatcher.CheckAccess())
                broadcast = PropertyGrid.BroadcastPropertyChanged;
            else
                Application.Current.Dispatcher.Invoke(
                    new Action(() => broadcast = PropertyGrid.BroadcastPropertyChanged));

            if (broadcast)
                tq.Do(go => { PostPropertyValueChangedAsync(e.PropertyName, false); });

            if (e.PropertyName != Name)
                return;

            var val = Object.GetPropertyValue(e.PropertyName);
            //if (object.Equals(val, Value))
            //    return;

            _value = val;
            NotifyPropertyChanged("Value");
        }

        #endregion Methods - Events

        #region Methods - Private

        public void Dispose()
        {
        }

        private void SetValue(object value)
        {
            _value = value;
            NotifyPropertyChanged("Value");

            Object.SetPropertyValue(Name, value);

            var info = Object as IDataErrorInfo;
            if (info != null)
            {
                var piGet = info.GetType().GetMethod("get_Item", BindingFlags.Instance | BindingFlags.Public);
                var error = piGet.Invoke(Object, new object[] { Name }) as string;
                if (!error.IsNullOrEmpty())
                    throw new Exception(error);
            }
        }

        private void SetIsBusy(bool value)
        {
            lock (this)
                cntBusy += value ? 1 : -1;

            NotifyPropertyChanged("IsBusy");
            NotifyPropertyChanged("ImageVisibility");
            NotifyPropertyChanged("LoadingVisibility");
        }

        private bool GetIsBusy()
        {
            lock (this)
                return cntBusy > 0;
        }

        #endregion Methods - Private

        #endregion Methods
    }

    public class PropertyDescriptorReadOnly : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorReadOnly(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            int maxLength = 0;
            var pi = obj.GetType().GetProperty(propertyName);
            var lengthAttr = pi.GetAttribute<StringLengthAttribute>();
            if (lengthAttr != null)
                maxLength = lengthAttr.MaximumLength;
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();

            Editable = false;

            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;

            if (pi.PropertyType.IsEnum)
            {
                b.Converter = new EnumDescriptionConverter();
            }
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            var tb = new TextBox();
            tb.IsReadOnly = true;
            if (maxLength > 0)
            {
                tb.MaxLength = maxLength;
            }
            tb.VerticalContentAlignment = VerticalAlignment.Center;
            tb.SetBinding(TextBox.TextProperty, b);
            tb.SetBinding(TextBox.ToolTipProperty, b);

            Designer = tb;
            BindingExpression = tb.GetBindingExpression(TextBox.TextProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorReadOnlyNoFrame : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorReadOnlyNoFrame(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            int maxLength = int.MaxValue;
            var pi = obj.GetType().GetProperty(propertyName);
            var lengthAttr = pi.GetAttribute<StringLengthAttribute>();
            if (lengthAttr != null)
                maxLength = maxLength > lengthAttr.MaximumLength ? lengthAttr.MaximumLength : maxLength;
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();

            Editable = false;

            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            var tb = new TextBox();
            tb.IsReadOnly = true;
            tb.MaxLength = maxLength;
            tb.Background = Brushes.Transparent;
            tb.BorderThickness = new Thickness(0);

            if (pda != null && pda.Height > 30)
            {
                tb.VerticalContentAlignment = VerticalAlignment.Stretch;
                tb.AcceptsReturn = true;
            }

            tb.SetBinding(TextBox.TextProperty, b);
            tb.SetBinding(TextBox.ToolTipProperty, b);

            Designer = tb;
            BindingExpression = tb.GetBindingExpression(TextBox.TextProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorString : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorString(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            int maxLength = 0;
            var pi = obj.GetType().GetProperty(propertyName);

            var lengthAttr = pi.GetAttribute<StringLengthAttribute>();
            if (lengthAttr != null)
                maxLength = lengthAttr.MaximumLength;
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();

            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.ValidatesOnDataErrors = true;
            b.UpdateSourceTrigger = pda == null ? UpdateSourceTrigger.LostFocus : pda.UpdateSourceTrigger;
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            var tb = new TextBox();
            if (maxLength > 0)
            {
                tb.MaxLength = maxLength;
            }
            tb.VerticalContentAlignment = VerticalAlignment.Center;
            //HintAssist.SetHelperText(tb, "测试");
            //tb.SetResourceReference(TextBox.StyleProperty, "MaterialDesignTextBox");

            if (pda != null && pda.Height > 30)
            {
                tb.VerticalContentAlignment = VerticalAlignment.Stretch;
                tb.AcceptsReturn = true;
            }

            tb.SetBinding(TextBox.TextProperty, b);
            tb.SetBinding(TextBox.ToolTipProperty, b);

            Designer = tb;
            BindingExpression = tb.GetBindingExpression(TextBox.TextProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorGuid : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorGuid(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            //var b = new Binding("Value");
            //b.Source = this;
            //b.Converter = new GuidToStringConverter();
            //b.Mode = BindingMode.TwoWay;
            //b.ValidatesOnExceptions = true;
            //b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;

            //var pi = obj.GetType().GetProperty(propertyName);
            //var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            //if (pda != null && pda.Converter != null)
            //{
            //    b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
            //    b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            //}

            //var designer = new GuidEditorTextBox();
            //designer.SetBinding(TextBox.TextProperty, b);
            //designer.SetBinding(TextBox.ToolTipProperty, b);

            //Designer = designer;
            //BindingExpression = designer.GetBindingExpression(TextBox.TextProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorEnum : PropertyDescriptor
    {
        #region Properties

        public ObservableKeyValueList<object, string> Items { get; private set; }

        #endregion Properties

        #region Ctor

        public PropertyDescriptorEnum(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            var pi = obj.GetType().GetProperty(propertyName);
            var enumType = pi.PropertyType;

            Items = new ObservableKeyValueList<object, string>();
            var fields = enumType.GetFields();
            foreach (var field in fields)
            {
                if (!field.FieldType.IsEnum)
                    continue;

                Items[field.GetValue(null)] = field.GetValue(null)?.GetDisplayName();
            }

            var cb = new ComboBox();
            //cb.Padding = new Thickness(6, 4, 6, 5);
            //cb.BorderThickness = new Thickness(1);
            cb.SelectedValuePath = "Key";
            cb.DisplayMemberPath = "Value";

            var b = new Binding("Items");
            b.Source = this;
            cb.SetBinding(ComboBox.ItemsSourceProperty, b);

            var b1 = new Binding("Value");
            b1.Source = this;
            b1.ValidatesOnExceptions = true;
            b1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (pda != null && pda.Converter != null)
            {
                b1.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b1.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            cb.SetBinding(ComboBox.SelectedValueProperty, b1);

            Designer = cb;
            BindingExpression = cb.GetBindingExpression(ComboBox.SelectedValueProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorBoolean : PropertyDescriptor
    {
        public ObservableKeyValueList<bool, string> Items { get; private set; }

        #region Ctor

        public PropertyDescriptorBoolean(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            var pi = obj.GetType().GetProperty(propertyName);

            Items = new ObservableKeyValueList<bool, string>();
            Items.Add(new KeyValue<bool, string>(true, "是"));
            Items.Add(new KeyValue<bool, string>(false, "否"));

            var cb = new ComboBox();
            cb.SelectedValuePath = "Key";
            cb.DisplayMemberPath = "Value";

            var b = new Binding("Items");
            b.Source = this;
            cb.SetBinding(ComboBox.ItemsSourceProperty, b);

            var b1 = new Binding("Value");
            b1.Source = this;
            b1.ValidatesOnExceptions = true;
            b1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (pda != null && pda.Converter != null)
            {
                b1.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b1.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            cb.SetBinding(ComboBox.SelectedValueProperty, b1);

            Designer = cb;
            BindingExpression = cb.GetBindingExpression(ComboBox.SelectedValueProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorInteger : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorInteger(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            var tb = new TextBox();
            //tb.Padding = new Thickness(5);
            tb.PreviewTextInput += (sender, e) => { e.Handled = !e.Text.IsNumeric(); };
            //tb.SetResourceReference(TextBox.StyleProperty, "MaterialDesignOutlinedTextBox");

            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.ValidatesOnDataErrors = true;
            b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            b.Converter = new SuperTypeConverter() { ConvertBackType = typeof(int?) };

            var pi = obj.GetType().GetProperty(propertyName);
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            tb.SetBinding(TextBox.TextProperty, b);

            Designer = tb;
            BindingExpression = tb.GetBindingExpression(TextBox.TextProperty);

            //var attrColumn = pi.GetAttribute<RangeAttribute>();
            //if (attrColumn != null)
            //{
            //    tb.Maximum = new DotNetTypeConverter().To<double>(attrColumn.Maximum);
            //    tb.Minimum = new DotNetTypeConverter().To<double>(attrColumn.Minimum);
            //}
        }

        #endregion Ctor
    }

    public class PropertyDescriptorShort : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorShort(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            //var cb = new NumericUpDown();
            //cb.BorderThickness = new Thickness(1);
            //cb.TextAlignment = TextAlignment.Left;

            //var b = new Binding("Value");
            //b.Source = this;
            //b.Mode = BindingMode.TwoWay;
            //b.ValidatesOnExceptions = true;
            //b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            //b.Converter = new SuperTypeConverter() { ConvertBackType = typeof(short?) };

            //var pi = obj.GetType().GetProperty(propertyName);
            //var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            //if (pda != null && pda.Converter != null)
            //{
            //    b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
            //    b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            //}

            //cb.SetBinding(NumericUpDown.ValueProperty, b);

            //Designer = cb;
            //BindingExpression = cb.GetBindingExpression(NumericUpDown.ValueProperty);

            //var attrColumn = pi.GetAttribute<RangeAttribute>();
            //if (attrColumn != null)
            //{
            //    cb.Maximum = new DotNetTypeConverter().To<double>(attrColumn.Maximum);
            //    cb.Minimum = new DotNetTypeConverter().To<double>(attrColumn.Minimum);
            //}
        }

        #endregion Ctor
    }

    public class PropertyDescriptorByte : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorByte(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            //var cb = new NumericUpDown();
            //cb.BorderThickness = new Thickness(1);
            //cb.TextAlignment = TextAlignment.Left;

            //var b = new Binding("Value");
            //b.Source = this;
            //b.Mode = BindingMode.TwoWay;
            //b.ValidatesOnExceptions = true;
            //b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            //b.Converter = new SuperTypeConverter() { ConvertBackType = typeof(byte?) };

            //var pi = obj.GetType().GetProperty(propertyName);
            //var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            //if (pda != null && pda.Converter != null)
            //{
            //    b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
            //    b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            //}

            //cb.SetBinding(NumericUpDown.ValueProperty, b);

            //Designer = cb;
            //BindingExpression = cb.GetBindingExpression(NumericUpDown.ValueProperty);

            //var attrColumn = pi.GetAttribute<RangeAttribute>();
            //if (attrColumn != null)
            //{
            //    cb.Maximum = new DotNetTypeConverter().To<double>(attrColumn.Maximum);
            //    cb.Minimum = new DotNetTypeConverter().To<double>(attrColumn.Minimum);
            //}
        }

        #endregion Ctor
    }

    public class PropertyDescriptorLong : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorLong(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            var tb = new TextBox();
            //tb.Padding = new Thickness(5);
            tb.PreviewTextInput += (sender, e) => { e.Handled = !e.Text.IsNumeric(); };
            //tb.SetResourceReference(TextBox.StyleProperty, "MaterialDesignOutlinedTextBox");

            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.ValidatesOnDataErrors = true;
            b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            b.Converter = new SuperTypeConverter() { ConvertBackType = typeof(long?) };

            var pi = obj.GetType().GetProperty(propertyName);
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            tb.SetBinding(TextBox.TextProperty, b);

            Designer = tb;
            BindingExpression = tb.GetBindingExpression(TextBox.TextProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorFloat : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorFloat(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            var tb = new TextBox();
            //tb.Padding = new Thickness(5);
            tb.PreviewTextInput += (sender, e) => { e.Handled = !e.Text.IsNumeric(); };
            //tb.SetResourceReference(TextBox.StyleProperty, "MaterialDesignOutlinedTextBox");

            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.ValidatesOnDataErrors = true;
            b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            b.Converter = new SuperTypeConverter() { ConvertBackType = typeof(float?) };

            var pi = obj.GetType().GetProperty(propertyName);
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            tb.SetBinding(TextBox.TextProperty, b);

            Designer = tb;
            BindingExpression = tb.GetBindingExpression(TextBox.TextProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorDouble : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorDouble(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            var tb = new TextBox();
            //tb.Padding = new Thickness(5);
            tb.PreviewTextInput += (sender, e) => { e.Handled = !e.Text.IsNumeric(); };
            //tb.SetResourceReference(TextBox.StyleProperty, "MaterialDesignOutlinedTextBox");

            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.ValidatesOnDataErrors = true;
            b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            b.Converter = new SuperTypeConverter() { ConvertBackType = typeof(double?) };

            var pi = obj.GetType().GetProperty(propertyName);
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            tb.SetBinding(TextBox.TextProperty, b);

            Designer = tb;
            BindingExpression = tb.GetBindingExpression(TextBox.TextProperty);
        }

        #endregion Ctor
    }

    public class PropertyDescriptorDateTime : PropertyDescriptor
    {
        #region Ctor

        public PropertyDescriptorDateTime(PropertyGrid pg, object obj, string propertyName)
            : base(obj, propertyName)
        {
            var dp = new DatePicker();
            dp.Language = XmlLanguage.GetLanguage("zh-CN");
            var b = new Binding("Value");
            b.Source = this;
            b.Mode = BindingMode.TwoWay;
            b.ValidatesOnExceptions = true;
            b.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;

            var pi = obj.GetType().GetProperty(propertyName);
            var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            if (pda != null && pda.Converter != null)
            {
                b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
                b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            }

            dp.SetBinding(DatePicker.SelectedDateProperty, b);

            Designer = dp;

            //var cb = new DateTimeUpDown();
            //cb.BorderThickness = new Thickness(1);

            //var b = new Binding("Value");
            //b.Source = this;
            //b.Mode = BindingMode.TwoWay;
            //b.ValidatesOnExceptions = true;
            //b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            //var pi = obj.GetType().GetProperty(propertyName);
            //var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
            //if (pda != null && pda.Converter != null)
            //{
            //    b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
            //    b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
            //}

            //cb.SetBinding(DateTimeUpDown.ValueProperty, b);

            //Designer = cb;
            //BindingExpression = cb.GetBindingExpression(DateTimeUpDown.ValueProperty);

            //var attrColumn = pi.GetAttribute<RangeAttribute>();
            //if (attrColumn != null)
            //{
            //    cb.Maximum = new DotNetTypeConverter().To<DateTime?>(attrColumn.Maximum);
            //    cb.Minimum = new DotNetTypeConverter().To<DateTime?>(attrColumn.Minimum);
            //}
        }

        #endregion Ctor
    }

    //public class PropertyDescriptorColor : PropertyDescriptor
    //{
    //    #region Ctor

    //    public PropertyDescriptorColor(PropertyGrid pg, object obj, string propertyName)
    //        : base(obj, propertyName)
    //    {
    //        var cb = new ColorPicker();
    //        cb.BorderThickness = new Thickness(1);

    //        var b = new Binding("Value");
    //        b.Source = this;
    //        b.Mode = BindingMode.TwoWay;
    //        b.ValidatesOnExceptions = true;
    //        b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

    //        var pi = obj.GetType().GetProperty(propertyName);
    //        var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
    //        if (pda != null && pda.Converter != null)
    //        {
    //            b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
    //            b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
    //        }

    //        cb.SetBinding(ColorPicker.SelectedColorProperty, b);

    //        Designer = cb;
    //        BindingExpression = cb.GetBindingExpression(ColorPicker.SelectedColorProperty);
    //    }

    //    #endregion Ctor
    //}

    //public class PropertyDescriptorSolidBrush : PropertyDescriptor
    //{
    //    #region Ctor

    //    public PropertyDescriptorSolidBrush(PropertyGrid pg, object obj, string propertyName)
    //        : base(obj, propertyName)
    //    {
    //        var cb = new ColorPicker();
    //        cb.BorderThickness = new Thickness(1);

    //        var b = new Binding("Value");
    //        b.Source = this;
    //        b.Converter = new SolidBrushToColorConverter();
    //        b.Mode = BindingMode.TwoWay;
    //        b.ValidatesOnExceptions = true;
    //        b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

    //        var pi = obj.GetType().GetProperty(propertyName);
    //        var pda = pi.GetAttribute<PropertyDescriptorAttribute>();
    //        if (pda != null && pda.Converter != null)
    //        {
    //            b.Converter = Activator.CreateInstance(pda.Converter) as IValueConverter;
    //            b.ConverterParameter = new PropertyGridConverterParameterPair(pg, pda.ConverterParameter);
    //        }

    //        cb.SetBinding(ColorPicker.SelectedColorProperty, b);

    //        Designer = cb;
    //        BindingExpression = cb.GetBindingExpression(ColorPicker.SelectedColorProperty);
    //    }

    //    #endregion Ctor
    //}
}