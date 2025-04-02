using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.Components
{
    public class TextBox_Comp : iSelectableComponent
    {
        public bool Selected { get; set; }
        public bool RedDesign { get; set; }
        public string Text { get; set; }
        public int Width { get; set; }
        public event Action SwitchComponent;
        public bool IsItTextBox { get; }
        public TextBox_Comp(bool selected, bool redDesign, int width, string text = "")
        {
            Selected = selected;
            RedDesign = redDesign;
            Text = text;
            Width = width;
            IsItTextBox = true;
        }

        public List<PopUpText> Draw(List<PopUpText> texts)
        {
            if (RedDesign) { texts.Add(new PopUpText(Text.PadRight(Width), true, ConsoleColor.Black, ConsoleColor.Gray, false)); }
            else { texts.Add(new PopUpText(Text.PadRight(Width), true, ConsoleColor.Black, ConsoleColor.DarkCyan, false)); }
            return texts;
        }
        public void HandleKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Tab)
            {
                SwitchComponent();
            }
            else if (key.Key == ConsoleKey.Enter) { /* textBox nikdy nebude reagovat na Enter */}
            else if (key.Key == ConsoleKey.Backspace) { if (Text.Length > 0) { Text = Text.Substring(0, Text.Length - 1); }}
            else { Text += key.KeyChar; }
            //if (Convert.ToInt32(key) >= 48 && Convert.ToInt32(key) <= 90)
        }
        public void YouAreSelected()
        {
            Selected = true;
        }
    }
}
