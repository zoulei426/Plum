using Plum.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Object
{
    public class PropertyContext : CDObject
    {
        #region Properties

        public PropertyInfo PropertyInfo { get; set; }
        public DataColumn DataColumn { get; set; }

        #endregion Properties

        #region Methods

        #region Methods - System

        public override void Dispose()
        {
            base.Dispose();

            PropertyInfo = null;
            DataColumn = null;
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", PropertyInfo.Name, DataColumn.ColumnName);
        }

        #endregion Methods - System

        #endregion Methods
    }

    public class PropertyContextCollection : CDDictionary<string, PropertyContext>
    {
    }
}