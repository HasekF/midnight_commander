using midnight_commander.PopUpWindows.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.PopUpTypes
{
    public class Editor_QuitSave : PopUpTableConstructor
    {
        public Editor_QuitSave(ComponentManager compManager, Table[] tables) : base(compManager, tables)
        {
        }
        public override void Construct()
        {
            CompManager.RedDesign = false;
            CompManager.AddText("Close file");
            Table table = GetTable(true);
            CompManager.AddText("File " + table.ItemList[table.SelectedLine].Name + " was modified");
            CompManager.AddText("Save before close?");
            CompManager.AddButtons(false, true, "Yes", "No", "Cancel");
        }
    }
}
