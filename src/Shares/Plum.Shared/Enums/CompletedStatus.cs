using System.ComponentModel;

namespace Plum.Enums
{
    public enum CompletedStatus
    {
        [Description("默认")]
        Default,

        [Description("待完成")]
        ToBeCompleted,

        [Description("已完成")]
        Completed
    }
}