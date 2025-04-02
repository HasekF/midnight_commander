using midnight_commander.PopUpWindows.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.PopUpTypes
{
    public class Delete : PopUpTableConstructor
    {
        public Delete(ComponentManager compManager, Table[] tables) : base(compManager, tables)
        {
        }
        public override void Construct()
        {
            CompManager.RedDesign = true;
            CompManager.AddText("Delete");
            Table table = GetTable(true);
            CompManager.AddText("You want to remove:");
            CompManager.AddText(table.ItemList[table.SelectedLine].Name);
            CompManager.AddText(" ");
            CompManager.AddText("Are you sure?");
            CompManager.AddButtons(false, true, "No", "Yes");
        }
    }
}
