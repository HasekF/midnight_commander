using System;
using System.Collections.Generic;
using System.Text;
using midnight_commander.PopUpWindows.Components;

namespace midnight_commander.PopUpWindows.PopUpTypes
{
    public class MkDir : PopUpTableConstructor
    {
        public MkDir(ComponentManager compManager, Table[] tables) : base(compManager, tables)
        {
        }
        public override void Construct()
        {
            CompManager.AddText("Create a new Directory");
            CompManager.AddText("Enter directory name:", false);
            CompManager.AddTextBox(45);
            CompManager.AddText(" ");
            CompManager.AddButtons(true, true, "OK", "Cancel");
        }
    }
}
