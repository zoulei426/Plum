using Prism.Services.Dialogs;
using System.Collections.Generic;

namespace Plum.Windows.Params
{
    public class FileDialogParameters : DialogParameters
    {
        public string FileName
        {
            get
            {
                return GetValue<string>(nameof(FileName));
            }
            set
            {
                Add(nameof(FileName), value);
            }
        }

        public List<string> FileNames
        {
            get
            {
                return GetValue<List<string>>(nameof(FileNames));
            }
            set
            {
                Add(nameof(FileNames), value);
            }
        }

        public FileDialogParameters(string fileName)
        {
            FileName = fileName;
        }

        public FileDialogParameters(List<string> fileNames)
        {
            FileNames = fileNames;
        }
    }
}