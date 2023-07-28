namespace Plum.Object
{
    public class DataColumn
    {
        public string ColumnName { get; set; }

        public string AliasName
        {
            get { return _AliasName.IsNullOrWhiteSpace() ? ColumnName : _AliasName; }
            set { _AliasName = value; }
        }

        private string _AliasName;

        public eDataType ColumnType { get; set; }

        public int Size { get; set; }

        public int Precision { get; set; }

        public int Scale { get; set; }

        public bool PrimaryKey { get; set; }

        public bool Nullable { get; set; }

        public bool Auto { get; set; }

        public bool Enabled { get; set; }

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
            return $"{ColumnName}, {ColumnType}";
        }
    }
}