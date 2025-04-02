using midnight_commander.PopUpWindows.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.PopUpTypes
{
    public class PopUpEditorConstructor
    {
        public ComponentManager CompManager { get; set; }
        public PopUpEditorConstructor(ComponentManager compManager)
        {
            CompManager = compManager;
        }
        public virtual void Construct() { }
    }
}
