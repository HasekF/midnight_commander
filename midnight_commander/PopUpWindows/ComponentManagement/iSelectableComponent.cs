using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.Components
{
    public interface iSelectableComponent : iComponent
    {
        public bool Selected { get; set; }
        public void HandleKey(ConsoleKeyInfo key);
        public void YouAreSelected();
        public bool IsItTextBox { get; }
    }
}
