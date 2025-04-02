using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander
{
    public abstract class AbstractAppWindow
    {

        public abstract void HandleKey(ConsoleKeyInfo key);
        public abstract void Regenerate();
    }
}
