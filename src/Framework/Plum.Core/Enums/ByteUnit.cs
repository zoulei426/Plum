using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Enums
{
    /// <summary>
    /// 字节单位
    /// </summary>
    [Description("字节单位")]
    public enum ByteUnit
    {
        /// <summary>
        /// B
        /// </summary>
        [Description("B")]
        B,

        /// <summary>
        /// KB
        /// </summary>
        [Description("KB")]
        KB,

        /// <summary>
        /// MB
        /// </summary>
        [Description("MB")]
        MB,

        /// <summary>
        /// GB
        /// </summary>
        [Description("GB")]
        GB,

        /// <summary>
        /// TB
        /// </summary>
        [Description("TB")]
        TB
    }
}