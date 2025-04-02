using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.EditorWin
{
    public class Select
    {
        public bool Active { get; set; }
        public bool EditingSelect { get; set; }
        public int StartLine { get; set; }
        public int StopLine { get; set; }
        public int StartChar { get; set; }
        public int StopChar { get; set; }
        public int DefLine { get; set; }
        public int DefChar { get; set; }
        public Select()
        {
            EditingSelect = false;
            Active = false;
        }

        public void DrawSelect(List<string> textLines, int drawLeft, int cursorLine, int cursorChar)
        {
            CheckPosition(cursorLine, cursorChar);
            for (int i = StartLine; i <= StopLine; i++)
            {
                if(i == StartLine)
                {
                    if (StartLine == StopLine)
                    {
                        if (StopChar - drawLeft >= Scale.LastWidth)
                            DrawAll(textLines[i], drawLeft);
                        else if (StopChar > drawLeft)
                        {
                            if (StartChar > drawLeft)
                            {
                                if (StopChar > drawLeft + Scale.LastWidth)
                                    DrawRight(textLines[i], drawLeft);
                                else
                                    DrawMiddle(textLines[i], drawLeft);
                            }
                            else
                                DrawLeft(textLines[i], drawLeft);
                        }
                        else
                            DrawAll(textLines[i], drawLeft, false);
                    }
                    else
                    {
                        if (StartChar > drawLeft)
                            DrawRight(textLines[i], drawLeft);
                        else
                            DrawAll(textLines[i], drawLeft);
                    }
                }
                else if (i == StopLine)
                {
                    if (StopChar > drawLeft + Scale.LastWidth || StopChar < drawLeft)
                        DrawAll(textLines[i], drawLeft);
                    else
                        DrawLeft(textLines[i], drawLeft);
                }
                else
                {
                    DrawAll(textLines[i], drawLeft);
                }
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            }
            Editor.SetToEditorColor();
        }
        public void CheckPosition(int cursorLine, int cursorChar)
        {
            if(!((StopChar == DefChar && StopLine == DefLine)||(StartLine == DefLine && StartChar == DefChar)))
            {
                if (StopChar == cursorChar && StopLine == cursorLine)
                {
                    StartChar = DefChar;
                    StartLine = DefLine;
                }
                else if (StartChar == cursorChar && StartLine == cursorLine)
                {
                    StopChar = DefChar;
                    StopLine = DefLine;
                }
            }
            if(StopLine < StartLine && (StopLine == StartLine && StopChar < StartChar))
            {
                int tempLine = StartLine;
                int tempChar = StartChar;
                StartChar = StopChar;
                StartLine = StopLine;
                StopChar = tempChar;
                StopLine = tempLine;
            }
        }
        private void DrawLeft(string line, int drawLeft)
        {
            Editor.SetToSelectColor();
            Console.Write(line.PadRight(Scale.LastWidth).Substring(drawLeft, StopChar-drawLeft));
            Editor.SetToEditorColor();
            Console.Write(line.PadRight(Scale.LastWidth).Substring(StopChar, Scale.LastWidth - (StopChar - drawLeft + 1)).TrimEnd());
            Editor.SetToEditorColor();
            Console.Write("".PadRight(Scale.LastWidth - line.Length));
        }
        private void DrawMiddle(string line, int drawLeft)
        {
            //Console.Clear();
            Editor.SetToEditorColor();
            Console.Write(line.PadRight(Scale.LastWidth).Substring(drawLeft, StartChar-drawLeft));
            Editor.SetToSelectColor();
            Console.Write(line.PadRight(Scale.LastWidth).Substring(StartChar, StopChar - StartChar));
            Editor.SetToEditorColor();
            Console.Write(line.PadRight(Scale.LastWidth).Substring(StopChar, Scale.LastWidth - (StopChar - drawLeft + 1)).TrimEnd());
            Editor.SetToEditorColor();
            Console.Write("".PadRight(Scale.LastWidth - line.Length));
            //bug s tím, když manuálně dojedu na konec nejdelšího řádku(nutnost k vyjímce => aby drawLeft byl větší než 0)
        }
        private void DrawRight(string line, int drawLeft)
        {
            Editor.SetToEditorColor();
            Console.Write(line.PadRight(Scale.LastWidth).Substring(drawLeft, StartChar - drawLeft));
            Editor.SetToSelectColor();
            Console.Write(line.PadRight(Scale.LastWidth).Substring(StartChar, Scale.LastWidth - (StartChar - drawLeft)).TrimEnd());
            Editor.SetToEditorColor();
            Console.Write("".PadRight(Scale.LastWidth - line.Length));
        }
        public void DrawAll(string line, int drawLeft, bool selected = true)
        {
            if (selected)
                Editor.SetToSelectColor();
            else
                Editor.SetToEditorColor();

                line = line.Substring(drawLeft);
            Console.Write(line.PadRight(Scale.LastWidth).Substring(0, Scale.LastWidth).TrimEnd());
            Editor.SetToEditorColor();
            Console.Write("".PadRight(Scale.LastWidth - line.Length));
        }
        public void Reset(int selectedLine, int selectedChar)
        {
            DefChar = selectedChar;
            DefLine = selectedLine;
            StopLine = DefLine;
            StartLine = DefLine;
            StartChar = DefChar;
            StopChar = DefChar;
        }
    }
}
