namespace Plum.Windows.Controls
{
    public class PropertyDescriptorBuilderOpenFileBpfAndExcel : PropertyDescriptorBuilderOpenFileBrowser
    {
        public PropertyDescriptorBuilderOpenFileBpfAndExcel() :
            base("BPF文件(*.bpf)|*.bpf|Excel文件(*.xls,*.xlsx)|*.xls;*.xlsx")
        {
        }
    }
}