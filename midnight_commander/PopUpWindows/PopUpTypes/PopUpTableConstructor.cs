using midnight_commander.PopUpWindows.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.PopUpTypes
{
    public class PopUpTableConstructor
    {
        public ComponentManager CompManager { get; set; }
        public Table[] Tables { get; set; }
        public PopUpTableConstructor(ComponentManager compManager, Table[] tables)
        {
            CompManager = compManager;
            Tables = tables;
        }
        public virtual void Construct() { }
        protected Table GetTable(bool selected)
        {
            foreach (var item in Tables)
            {
                if (item.Selected && selected) return item;
                else if (!item.Selected && !selected) return item;
            }
            return null;
        }


    }
}
