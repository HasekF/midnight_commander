using midnight_commander.PopUpWindows.ComponentManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.Components
{

    public class ComponentManager
    {
        public bool RedDesign { get; set; }
        public List<iComponent> Components { get; set; }
        public List<iSelectableComponent> Selectables { get; set; }
        public int SelectedComponent { get; set; }
        public event Action<List<string>> Done;
        private bool DefaultButtonExist;
        private List<TextBox_Comp> TextBoxList;
        private DrawPopUp DrawPopUp;
        public ComponentManager(bool redDesing)
        {
            Components = new List<iComponent>();
            Selectables = new List<iSelectableComponent>();
            TextBoxList = new List<TextBox_Comp>();
            DrawPopUp = new DrawPopUp();
            RedDesign = redDesing;
            SelectedComponent = 0;
            DefaultButtonExist = false;
        }

        public void Draw ()
        {
            List<PopUpText> texts = new List<PopUpText>();
            foreach (var item in Components)
            {
                texts = item.Draw(texts);
            }
            DrawPopUp.Draw(texts, RedDesign);
        }
        public void HandelKey(ConsoleKeyInfo key)
        {
            if(key.Key == ConsoleKey.Enter && DefaultButtonExist)
            {
                foreach (var item in Selectables)
                {
                    item.HandleKey(key);
                }
            }
            Selectables[SelectedComponent].HandleKey(key);
        }
        public void AddButtons(bool Default, bool shortCut, params string[] buttonsNames)
        {
            if (Default) { DefaultButtonExist = true; }
            Buttons_Comp buttons;
            if (Selectables.Count == 0) { buttons = new Buttons_Comp(true, RedDesign,Default, shortCut); }
            else { buttons = new Buttons_Comp(false, RedDesign,Default, shortCut); }
            buttons.SwitchComponent += SwitchComponent;
            buttons.Done += JobIsDone;
            foreach (var item in buttonsNames)
            {
                buttons.AddButton(item);
            }
            Components.Add(buttons);
            Selectables.Add(buttons);
        }
        public void AddText(string text, bool center = true)
        {
            Text_Comp text_Comp = new Text_Comp(text,RedDesign, center);
            Components.Add(text_Comp);
        }
        public void AddTextBox(int width, string text = "")
        {
            TextBox_Comp textBox;
            if (Selectables.Count == 0) { textBox = new TextBox_Comp(true,RedDesign,width, text); }
            else { textBox = new TextBox_Comp(false, RedDesign, width, text); }
            textBox.SwitchComponent += SwitchComponent;
            TextBoxList.Add(textBox);
            Components.Add(textBox);
            Selectables.Add(textBox);
        }
        private void SwitchComponent()
        {
            if (Selectables.Count > 1)
            {
                SelectedComponent++;
                if (SelectedComponent >= Selectables.Count)
                {
                    SelectedComponent = 0;
                    Selectables[Selectables.Count - 1].Selected = false;
                }
                else { Selectables[SelectedComponent - 1].Selected = false; }
                Selectables[SelectedComponent].Selected = true;
                Selectables[SelectedComponent].YouAreSelected();
            }
            else if(Selectables.Count == 1)
            {
                Selectables[0].YouAreSelected();
            }
        }
        private void JobIsDone(List<string> result)
        {
            foreach (var item in TextBoxList)
            {
                if (item.IsItTextBox) { result.Add(item.Text); }
            }
            Done(result);
        }
    }
}
