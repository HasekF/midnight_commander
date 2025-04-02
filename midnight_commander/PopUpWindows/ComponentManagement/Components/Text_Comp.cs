using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.Components
{
    public class Text_Comp : iComponent
    {
        public string Text { get; set; }
        public bool RedDesign { get; set; }
        public bool Center { get; set; }
        public Text_Comp(string text,bool redDesign, bool center)
        {
            Text = text;
            RedDesign = redDesign;
            Center = center;
        }
        public List<PopUpText> Draw(List<PopUpText> texts)
        {
            string text = Text;
            if (RedDesign) { texts.Add(new PopUpText(Text, true,ConsoleColor.Gray,ConsoleColor.Red,Center));}
            else { texts.Add(new PopUpText(Text, true, ConsoleColor.Black, ConsoleColor.Gray,Center)); }
            return texts;
        }
    }
}
