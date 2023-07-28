namespace Plum.Windows.Settings
{
    public class CommonSettings
    {
        public string SavePath { get; set; }

        public CommonSettings()
        {
            SavePath = SystemPath.AppData;
        }
    }
}