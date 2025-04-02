using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.PopUpWindows.ComponentManagement
{
    public class DrawPopUp
    {
        public int StartTopPosition { get; set; }
        public int StartLeftPosition { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private ConsoleColor BackgroundColor;
        private bool RedDesign;
        public DrawPopUp()
        {
            BackgroundColor = ConsoleColor.Gray;
            RedDesign = false;
        }
        public void Draw(List<PopUpText> texts, bool redDesign)
        {
            if (redDesign) { BackgroundColor = ConsoleColor.Red; }
            RedDesign = redDesign;
            int lenght = GetLenght(texts);

            Width = lenght + 4;
            Height = 0;
            foreach (var item in texts)
            {
                if (item.NewLine) { Height++; }
            }
            ReSize();
            DrawBorder(BackgroundColor);
            texts = TextBorder(texts, lenght);
            Write(texts);
        }
        private void ReSize()
        {
            StartLeftPosition = (Scale.LastWidth / 2) - (Width / 2);
            StartTopPosition = (Scale.LastHeight / 2) - (Height / 2);
            BackgroundColor = ConsoleColor.Gray;
        }
        private void DrawBorder(ConsoleColor color)
        {
            Console.SetCursorPosition(StartLeftPosition, StartTopPosition);
            for (int i = 0; i < Height; i++)
            {
                Console.BackgroundColor = color;
                string line = "".PadRight(Width);
                Console.WriteLine(line);
                Console.SetCursorPosition(StartLeftPosition, Console.CursorTop);
            }
            Color.Change(ColorType.UNSELECTED);
        }
        private int GetLastPopUpText(List<PopUpText> lines, int indexOfFirstPupUp)
        {
            for (int i = indexOfFirstPupUp; i < lines.Count; i++)
            {
                if (lines[i].NewLine) { return i; }
            }
            return lines.Count - 1;
        }
        private List<PopUpText> TextBorder(List<PopUpText> lines, int lenght)
        {
            ConsoleColor back = ConsoleColor.Gray;
            ConsoleColor text = ConsoleColor.Black;
            if (RedDesign)
            {
                back = ConsoleColor.Red;
                text = ConsoleColor.Gray;
            }
            //horní řádek
            string line = "┌" + lines[0].Text.Trim().PadLeft(((Width - 2 - lines[0].Text.Length) / 2) + lines[0].Text.Length, '─').PadRight(Width - 2, '─') + "┐";
            lines.RemoveAt(0);
            lines.Insert(0, new PopUpText(line, true, text, back));

            //střed
            for (int i = 1; i < lines.Count; i++)
            {
                if (lines[i].NewLine)
                {
                    if (lines[i].Center) { lines[i].Text = lines[i].Text.PadLeft(((lenght - lines[i].Text.Length) / 2) + lines[i].Text.Length).PadRight(lenght); }
                    else { lines[i].Text = lines[i].Text.PadRight(lenght); }
                    if (lines[i].BackgroundColor != ConsoleColor.Gray && lines[i].BackgroundColor != ConsoleColor.Red)
                    {
                        lines.Insert(i, new PopUpText("│ ", false, text, back));
                        i++;
                        lines.Insert(i + 1, new PopUpText(" │", true, text, back));
                        lines[i].NewLine = false;
                        i++;
                    }
                    else { lines[i].Text = "│ " + lines[i].Text + " │"; }
                }
                else
                {
                    //lines.Insert(i, new PopUpText("│ ", false, text, back));
                    //int index = GetLastPopUpText(lines, i);
                    //lines[i].Text = lines[i].Text.PadRight((lenght - GetLenght(lines, i, index)) / 2);
                    //lines.Insert(index + 1, new PopUpText(" │", true, text, back));
                    //lines[index].NewLine = false;
                    //int temLenght = GetLenght(lines, i, index);
                    //lines[index + 1].Text = lines[index + 1].Text.PadLeft((lenght - temLenght) / 2);
                    //i = index + 1;

                    //lines.Insert(i, new PopUpText("│ ", false, text, back));
                    //int index = GetLastPopUpText(lines, i);
                    //int leftPad = lines[i + 1].Text.PadLeft((lenght - GetLenght(lines, i, index)) / 2).Length-lines[i+1].Text.Length;
                    //int rightPad = lines[index].Text.PadRight(lenght - GetLenght(lines, i, index)).Length-lines[index].Text.Length;
                    //lines[index].NewLine = false;
                    //lines.Insert(i, new PopUpText("".PadRight(leftPad), false, text, back));
                    //lines.Insert(index + 1, new PopUpText("".PadRight(rightPad), false, text, back));
                    //lines.Insert(i, new PopUpText(" │", true, text, back));
                    //i = index + 1;

                    lines.Insert(i, new PopUpText("│ ", false, text, back));
                    int index = GetLastPopUpText(lines, i);
                    int temLenght = GetLenght(lines, i, index);
                    lines.Insert(i+1,new PopUpText("".PadLeft(((lenght - temLenght) / 2)/* - lines[i + 1].Text.Length*/),false));
                    index = GetLastPopUpText(lines, i);
                    temLenght = GetLenght(lines, i, index);
                    lines.Insert(index+1, new PopUpText("".PadRight((lenght - temLenght) / 2), false));
                    temLenght = GetLenght(lines, i, index);
                    lines[index+1].NewLine = true;
                    lines.Insert(index + 1, new PopUpText(" │", true, text, back));
                    i = index + 3;

                    //lines.Insert(i, new PopUpText("│ ", false, text, back));
                    //int index = GetLastPopUpText(lines, i);
                    //lines[i + 1].Text = lines[i + 1].Text.PadLeft((lenght - GetLenght(lines, i, index)) / 2);
                    //lines[index].Text = lines[index].Text.PadRight(lenght - GetLenght(lines, i, index));
                    //lines[index].NewLine = false;
                    //lines.Insert(index + 1, new PopUpText(" │", true, text, back));
                    //i = index + 1;
                }
            }

            //dolní řádek
            lines.Add(new PopUpText("└" + "".PadRight(Width - 2, '─') + "┘", true, text, back));
            return lines;
        }
        private int GetLenght(List<PopUpText> lines)
        {
            if (!lines[lines.Count - 1].NewLine) { }
            int leght = 0;
            int i = -1;
            while (i < lines.Count - 1)
            {
                int tmpLenght = 0;
                do
                {
                    i++;
                    tmpLenght += lines[i].Text.Length;
                } while (!lines[i].NewLine);
                if (tmpLenght > leght) { leght = tmpLenght; }
            }
            return leght;
        }
        private int GetLenght(List<PopUpText> lines, int startIndex, int endIndex)
        {
            int tempLenght = 0;
            for (int a = startIndex + 1; a < endIndex; a++)
            {
                tempLenght += lines[a].Text.Length;
            }
            return tempLenght;
        }
        private void Write(List<PopUpText> texts)
        {
            Console.SetCursorPosition(StartLeftPosition, StartTopPosition);
            foreach (var item in texts)
            {
                Console.ForegroundColor = item.TextColor;
                Console.BackgroundColor = item.BackgroundColor;
                Console.Write(item.Text);
                if (item.NewLine)
                {
                    Console.WriteLine();
                    Console.SetCursorPosition(StartLeftPosition, Console.CursorTop);
                }
            }
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
        }
    }
}
