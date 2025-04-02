using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.Components
{
    public class SingleButton
    {
        public string Text { get; set; }
        public bool Selected { get; set; }
        public bool Default { get; set; }
        public bool RedDesign { get; set; }
        public bool ShortCut { get; set; }
        public SingleButton(string text, bool selected, bool redDesign, bool def, bool shortCut)
        {
            Text = text;
            Selected = selected;
            RedDesign = redDesign;
            Default = def;
            ShortCut = shortCut;
        }
        public List<PopUpText> GetButtonTexts (List<PopUpText> texts)
        {
            ConsoleColor back = ConsoleColor.Gray;
            ConsoleColor text = ConsoleColor.Black;
            ConsoleColor shortcut = ConsoleColor.Black;
            if (ShortCut) { shortcut = ConsoleColor.Blue; }
            if (RedDesign)
            {
                back = ConsoleColor.Red;
                text = ConsoleColor.Gray;
                shortcut = ConsoleColor.Gray;
                if (ShortCut) { shortcut = ConsoleColor.DarkYellow; }
                if (Selected) { back = ConsoleColor.Gray; text = ConsoleColor.Black; }
            }
            else
            {
                if (Selected) { back = ConsoleColor.DarkCyan; }
            }
            if(Default){texts.Add(new PopUpText("[< ", false,text,back));}
            else { texts.Add(new PopUpText("[  ", false, text, back)); }
            texts.Add(new PopUpText(Text.Substring(0, 1), false, shortcut, back));
            if (Default) { texts.Add(new PopUpText(Text.Substring(1) + " >]", false, text, back)); }
            else { texts.Add(new PopUpText(Text.Substring(1) + "  ]", false, text, back)); }

            return texts;
        }
    }
}
