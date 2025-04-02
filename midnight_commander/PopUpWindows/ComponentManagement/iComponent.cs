using midnight_commander.PopUpWindows;
using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander
{
    public interface iComponent
    {
        public List<PopUpText> Draw(List<PopUpText> texts);
        public bool RedDesign { get; set; }
    }
}
