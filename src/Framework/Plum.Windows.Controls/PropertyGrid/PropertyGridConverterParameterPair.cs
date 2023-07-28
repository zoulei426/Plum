namespace Plum.Windows.Controls
{
    public class PropertyGridConverterParameterPair
    {
        #region Properties

        public PropertyGrid PropertyGrid { get; set; }
        public object ConverterParameter { get; set; }

        #endregion Properties

        #region Ctor

        public PropertyGridConverterParameterPair(PropertyGrid pg, object param)
        {
            PropertyGrid = pg;
            ConverterParameter = param;
        }

        #endregion Ctor
    }
}