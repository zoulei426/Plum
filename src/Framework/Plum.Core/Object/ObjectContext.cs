using Plum.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Object
{
    public class ObjectContext : CDObject
    {
        #region Properties

        #region Properties - Map

        public PropertyContextCollection Columns { get; private set; }
        public PropertyContextCollection KeyLowerColumns { get; private set; }

        #endregion Properties - Map

        #region Properties - Element

        public Type ElementType { get; private set; }
        public string TableName { get; private set; }
        public string Schema { get; private set; }
        public bool IsClass { get; private set; }
        public bool IsAnonymousClass { get; private set; }
        public bool HasNoParameterConstructor { get; private set; }

        #endregion Properties - Element

        #endregion Properties

        #region Fields

        private static Dictionary<Type, ObjectContext> htOC = new Dictionary<Type, ObjectContext>();

        #endregion Fields

        #region Ctor

        private ObjectContext()
        {
        }

        #endregion Ctor

        #region Methods

        #region Methods - Static

        public static ObjectContext Create(Type elementType)
        {
            lock (htOC)
            {
                if (!htOC.ContainsKey(elementType))
                    new ObjectContext().Install(elementType);

                return (htOC[elementType] as ObjectContext);
            }
        }

        #endregion Methods - Static

        #region Methods - Install

        private void Install(Type elementType)
        {
            IsClass = !elementType.IsValueTypeOrString();

            ConstructorInfo ci = elementType.GetConstructor(new Type[0]);
            HasNoParameterConstructor = ci != null;

            IsAnonymousClass = elementType.Name.StartsWith("<>f__AnonymousType");

            var tableAttr = elementType.GetAttribute<TableAttribute>();
            if (tableAttr == null)
                tableAttr = new TableAttribute(elementType.Name);

            TableName = tableAttr.Name.IsNullOrEmpty() ?
                        elementType.Name : tableAttr.Name;
            Schema = tableAttr.Schema;

            Columns = new PropertyContextCollection();
            KeyLowerColumns = new PropertyContextCollection();

            if (IsClass)
                elementType.TraversalPropertiesInfo(
                    DataColumnHandler, BindingFlags.Instance | BindingFlags.Public);

            ElementType = elementType;

            htOC[elementType] = this;
        }

        private bool DataColumnHandler(PropertyInfo pi)
        {
            var dc = new DataColumn();

            var column = pi.GetCustomAttribute<ColumnAttribute>();
            dc.ColumnName = column is null ? pi.Name : column.Name;
            dc.ColumnType = DotNetTypeAttribute.GetType(pi.PropertyType);
            dc.AliasName = pi.GetDisplayName();

            //if (!dca.Enabled)
            //    return true;

            Columns[pi.Name] = new PropertyContext() { DataColumn = dc, PropertyInfo = pi };
            KeyLowerColumns[pi.Name.ToLower()] = new PropertyContext() { DataColumn = dc, PropertyInfo = pi };

            return true;
        }

        #endregion Methods - Install

        #region Methods - System

        public override object Clone()
        {
            ObjectContext map = base.Clone() as ObjectContext;
            map.Columns = Columns.Clone() as PropertyContextCollection;
            map.KeyLowerColumns = KeyLowerColumns.Clone() as PropertyContextCollection;

            return map;
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", ElementType.Name, TableName);
        }

        #endregion Methods - System

        #endregion Methods
    }
}