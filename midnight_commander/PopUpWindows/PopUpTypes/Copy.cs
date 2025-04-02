using midnight_commander.PopUpWindows.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.PopUpTypes
{
    public class Copy : PopUpTableConstructor
    {
        public Copy(ComponentManager compManager, Table[] tables) : base(compManager, tables)
        {
        }
        public override void Construct()
        {
            Table table = GetTable(true);
            CompManager.AddText("Copy");
            CompManager.AddText("Source:");
            CompManager.AddTextBox(45, table.ItemList[table.SelectedLine].Path);
            CompManager.AddText(" ");
            CompManager.AddText("Destination:");
            table = GetTable(false);
            CompManager.AddTextBox(45, table.Path);
            CompManager.AddText(" ");
            CompManager.AddButtons(true, true, "OK", "Cancel");
        }
    }
}
