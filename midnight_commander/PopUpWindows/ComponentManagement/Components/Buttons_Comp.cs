using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.Components
{
    public class Buttons_Comp : iSelectableComponent
    {
        public List<SingleButton> Buttons { get; set; }
        public bool Selected { get; set; }
        public bool Default { get; set; }
        public bool RedDesign { get; set; }
        public bool ShortCut { get; set; }
        public event Action SwitchComponent;
        public event Action<List<string>> Done;
        private int SelectedButton;
        public bool IsItTextBox { get; }
        public Buttons_Comp(bool selected, bool redDesign, bool def, bool shortCut)
        {
            SelectedButton = 0;
            Buttons = new List<SingleButton>();
            Selected = selected;
            RedDesign = redDesign;
            Default = def;
            ShortCut = shortCut;
            IsItTextBox = false;
        }

        public List<PopUpText> Draw(List<PopUpText> texts)
        {
            foreach (var item in Buttons)
            {
                texts = item.GetButtonTexts(texts);
                if (RedDesign) { texts.Add(new PopUpText(" ",false,ConsoleColor.Gray,ConsoleColor.Red));}
                else { texts.Add(new PopUpText(" ", false, ConsoleColor.Black, ConsoleColor.Gray)); }
            }
            texts.RemoveAt(texts.Count - 1);
            texts[texts.Count - 1].NewLine = true;
            return texts;
        }
        public void AddButton(string text)
        {
            if (Buttons.Count == 0) { Buttons.Add(new SingleButton(text, Selected, RedDesign, Default, ShortCut)); }
            else { Buttons.Add(new SingleButton(text, false, RedDesign, false, ShortCut)); }
        }
        public void HandleKey(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.Tab)
            {
                if (SelectedButton < Buttons.Count - 1) 
                { 
                    SelectedButton++; 
                    SwitchButton(); 
                }
                else 
                {
                    SelectedButton = -1; 
                    SwitchButton();
                    SwitchComponent(); 
                }
            }
            else if(key.Key == ConsoleKey.Enter)
            {
                if(Selected)
                {
                    List<string> list = new List<string>();
                    list.Add(Buttons[SelectedButton].Text);
                    Done(list);
                }
                else
                {
                    foreach (var item in Buttons)
                    {
                        if (item.Default) 
                        {
                            List<string> list = new List<string>();
                            list.Add(item.Text);
                            Done(list);
                        }
                    }
                }
            }
            else
            {
                if(ShortCut)
                {
                    for (int i = 0; i < Buttons.Count; i++)
                    {
                        if (key.KeyChar == Convert.ToChar(Buttons[i].Text.Substring(0, 1).ToLower()))
                        {
                            List<string> list = new List<string>();
                            list.Add(Buttons[i].Text);
                            Done(list);
                        }
                    }
                }
            }
        }
        public void YouAreSelected()
        {
            Selected = true;
            SelectedButton = 0;
            SwitchButton();
        }
        private void SwitchButton()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                if (SelectedButton == -1 || SelectedButton != i) { Buttons[i].Selected = false; }
                else 
                {
                    if (Selected) { Buttons[i].Selected = true; } 
                }
            }
        }
    }
}
