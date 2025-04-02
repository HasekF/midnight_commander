using midnight_commander.PopUpWindows.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.PopUpTypes
{
    public class EditorSearch : PopUpEditorConstructor
    {
        public EditorSearch(ComponentManager compManager) : base(compManager)
        {
        }
        public override void Construct()
        {
            CompManager.AddText("Search");
            CompManager.AddText("Enter search string:");
            CompManager.AddTextBox(45);
            CompManager.AddText(" ");
            CompManager.AddButtons(true, true, "OK", "Cancel");
        }
    }
}
