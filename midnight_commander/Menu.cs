using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander
{
    public class Menu
    {
        public int StartTopPosition { get; set; }
        public int StartLeftPosition { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private string[] Choice;
        public Menu(params string[] choices)
        {
            Choice = choices;
            Regenerate();
        }
        public void Regenerate()
        {
            StartLeftPosition = 0;
            StartTopPosition = Scale.LastHeight - 1;
            Height = 1;
            Width = Scale.LastWidth;
            //DrawBorder(ConsoleColor.Black);
            Draw();
        }
        public void Draw()
        {
            Console.SetCursorPosition(StartLeftPosition, StartTopPosition);
            int choices = 11;
            int width = Width;
            int rest = 0;
            while (width <= 7 || choices == 11)
            {
                choices--;
                width = Width;
                rest = width % choices;
                width -= rest;
                width = width / choices;
            }
            for (int i = 1; i <= choices; i++)
            {
                int localWidth = width;
                if(rest > 0)
                {
                    rest--;
                    localWidth++;
                }
                Color.Change(ColorType.MENU_NUMBER);
                if (i < 10) { Console.Write(" " + i); }
                else { Console.Write(i); }
                Color.Change(ColorType.MENU_TEXT);
                string text = Choice[i - 1];
                if (text.Length > localWidth)
                {
                    text = text.Remove(2, 2);
                    text = text.Insert(2, "~");
                }
                text = text.PadRight(localWidth - 2);
                Console.Write(text);
            }
            Color.Change(ColorType.BLACK);
        }
        public void HandleKey(ConsoleKeyInfo key)
        {

        }
    }
}
