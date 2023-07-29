using Plum.Attributes;
using System.ComponentModel;

namespace Plum.Object
{
    public class DataColumn
    {
        [DisplayName("实体属性名称")]
        public string PropertyName
        {
            get { return _PropertyName ?? ColumnName; }
            set { _PropertyName = value; }
        }

        private string _PropertyName;

        [DisplayName("数据库字段名称")]
        public string ColumnName { get; set; }

        [DisplayName("注释")]
        public string AliasName
        {
            get { return _AliasName.IsNullOrWhiteSpace() ? ColumnName : _AliasName; }
            set { _AliasName = value; }
        }

        private string _AliasName;

        [DisplayName("类型")]
        public eDataType ColumnType { get; set; }

        [DisplayName("长度")]
        public int Size { get; set; }

        [DisplayName("精度")]
        public int Precision { get; set; }

        [Enabled(false)]
        public int Scale { get; set; }

        [DisplayName("主键")]
        public bool PrimaryKey { get; set; }

        [DisplayName("可空")]
        public bool Nullable { get; set; }

        [DisplayName("自增")]
        public bool Auto { get; set; }

        [Enabled(false)]
        public bool Enabled { get; set; }

        [Enabled(false)]
        public bool Visible { get; set; }

        public DataColumn()
        {
            ColumnType = eDataType.String;
            Nullable = true;
            Enabled = true;
            Visible = true;
        }

        public override string ToString()
        {
            return AliasName.IsNullOrEmpty()
                ? ColumnName
                : ColumnName.Equals(AliasName)
                    ? ColumnName
                    : $"{AliasName}（{ColumnName}）";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return ((DataColumn)obj).ColumnName == ColumnName;
        }

        public override int GetHashCode()
        {
            return ColumnName.IsNullOrEmpty() ? 0 : ColumnName.GetHashCode();
        }
    }
}