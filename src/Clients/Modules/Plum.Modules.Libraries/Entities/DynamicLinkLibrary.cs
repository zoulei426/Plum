using Plum.Attributes;
using Plum.Object;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Modules.Libraries.Entities
{
    [AddINotifyPropertyChangedInterface]
    [Table("base_dynamic_link_library")]
    public class DynamicLinkLibrary : DataViewObject
    {
        [Key]
        [Column("dll_id")]
        [Description("库ID")]
        public long DllId { get; set; }

        [Enabled(false)]
        [Column("org_id")]
        [Description("机构ID")]
        public long OrgId { get; set; }

        [Column("dll_code")]
        [Description("库代码")]
        public string? DllCode { get; set; }

        [Column("dll_version")]
        [Description("库版本")]
        public string? DllVersion { get; set; }

        [Column("dll_unit_name")]
        [Description("程序集名称")]
        public string? DllUnitName { get; set; }

        [Column("dll_call_name")]
        [Description("调用名")]
        public string? DllCallName { get; set; }

        [Column("dll_desc")]
        [Description("功能说明")]
        public string? DllDesc { get; set; }

        [Column("dll_clz_name")]
        [Description("类名")]
        public string? DllClzName { get; set; }

        [Column("dll_clz_method")]
        [Description("类方法")]
        public string? DllClzMethods { get; set; }

        [Enabled(false)]
        [Column("dll_path")]
        [Description("路径")]
        public string? DllPath { get; set; }

        [Column("dll_display")]
        [Description("是否可见")]
        public int DllDisplay { get; set; }

        [Column("dll_active")]
        [Description("是否激活")]
        public int DllActive { get; set; }

        [Enabled(false)]
        [Column("dll_target_ip")]
        [Description("目标地址IP")]
        public string? DllTargetIp { get; set; }

        [Enabled(false)]
        [Column("dll_target_port")]
        [Description("目标地址端口")]
        public string? DllTargetPort { get; set; }

        [Enabled(false)]
        [Column("dll_target_path")]
        [Description("库文件路径")]
        public string? DllTargetPath { get; set; }
    }
}