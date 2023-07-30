using Plum.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Plum
{
    public static class DataColumnExtensions
    {
        public static List<DataColumn> GetDataColumns(this ObjectContext oc)
        {
            return oc.Columns.Select(x =>
            {
                var col = x.Value.DataColumn.ConvertTo<DataColumn>();
                col.PropertyName = x.Key;
                return col;
            }).ToList();
        }

        public static List<DataColumn> GetDataColumnsInclude(this ObjectContext oc, params string[] includedColumns)
        {
            return oc.GetDataColumns().Where(x => includedColumns.Contains(x.PropertyName)).ToList();
        }

        public static List<DataColumn> GetDataColumns(this Type type)
        {
            return ObjectContext.Create(type).GetDataColumns();
        }

        public static List<DataColumn> GetDataColumnsInclude(this Type type, params string[] includedColumns)
        {
            var result = new List<DataColumn>();
            var columnDic = type.GetDataColumns().ToDictionary(k => k.PropertyName, v => v);
            foreach (var includedColumn in includedColumns)
            {
                if (columnDic.ContainsKey(includedColumn))
                    result.Add(columnDic[includedColumn]);
            }
            return result;
        }

        public static List<DataColumn> GetDataColumnsInclude<T>(this Type type, params Expression<Func<T, object>>[] includedColumns)
        {
            var columnNames = includedColumns.Select(x => x.GetPropertyInfo().Name).ToArray();
            return GetDataColumnsInclude(type, columnNames);
        }

        public static List<DataColumn> GetDataColumnsExclude(this Type type, params string[] excludedColumns)
        {
            return type.GetDataColumns().Where(x => !excludedColumns.Contains(x.PropertyName)).ToList();
        }

        public static List<DataColumn> GetDataColumnsExclude<T>(this Type type, params Expression<Func<T, object>>[] excludedColumns)
        {
            var columnNames = excludedColumns.Select(x => x.GetPropertyInfo().Name).ToArray();
            return GetDataColumnsExclude(type, columnNames);
        }

        public static List<DataColumn> GetDataColumns(this Type type, Func<DataColumn, bool> filter)
        {
            return type.GetDataColumns().Where(filter).ToList();
        }

        public static string GetDataColumnName<T>(this Type type, Expression<Func<T, object>> column)
        {
            var propertyName = column.GetPropertyInfo().Name;
            return ObjectContext.Create(type).Columns
                .Where(x => x.Key.Equals(propertyName))
                .Select(x => x.Value.DataColumn.ColumnName)
                .FirstOrDefault();
        }

        public static DataColumn GetDataColumn(this Type type, string columnName)
        {
            return type.GetDataColumns()
                .Where(x => x.PropertyName.Equals(columnName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public static DataColumn GetDataColumn<T>(this Type type, Expression<Func<T, object>> column)
        {
            var propertyName = column.GetPropertyInfo().Name;
            return type.GetDataColumns()
                .Where(x => x.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public static DataColumn GetDataColumn<T>(this Type type, Expression<Func<T, string>> column)
        {
            var propertyName = column.GetPropertyInfo().Name;
            return type.GetDataColumns()
                .Where(x => x.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
    }
}