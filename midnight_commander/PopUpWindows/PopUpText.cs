using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows
{
    public class PopUpText
    {
        public string Text { get; set; }
        public ConsoleColor TextColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public bool NewLine { get; set; }
        public bool Center { get; set; }
        public PopUpText(string text, bool newLine = true, ConsoleColor textColor = ConsoleColor.Black, ConsoleColor backgroundColor = ConsoleColor.Gray, bool center = true)
        {
            Text = text;
            TextColor = textColor;
            NewLine = newLine;
            TextColor = textColor;
            BackgroundColor = backgroundColor;
            Center = center;
        }
    }
}
