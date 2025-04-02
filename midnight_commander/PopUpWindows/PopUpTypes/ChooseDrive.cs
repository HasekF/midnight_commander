using midnight_commander.FileService;
using midnight_commander.PopUpWindows.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace midnight_commander.PopUpWindows.PopUpTypes
{
    public class ChooseDrive : PopUpTableConstructor
    {
        public ChooseDrive(ComponentManager compManager, Table[] tables) : base(compManager, tables)
        {
        }
        public override void Construct()
        {
            DriveInfo[] drives = FilesManager.GetDrives();
            string[] names = new string[drives.Length];
            for (int i = 0; i < drives.Length; i++)
            {
                names[i] = drives[i].Name;
            }
            CompManager.AddText("Choose drive");
            CompManager.AddText(" ");
            CompManager.AddButtons(false, true, names);
            CompManager.AddText(" ");
        }
    }
}
