using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using midnight_commander.FileService;

namespace midnight_commander.EditorWin
{
    public class Editor
    {
        public List<string> TextLines { get; set; }
        public string Path { get; set; }
        private int Height;
        private int Width;
        public int SelectedLine { get; set; }
        public int SelectedChar { get; set; }
        public int DrawLeft { get; set; }
        public int DrawTop { get; set; }
        public bool Modify { get; set; }
        public Select Select { get; set; }
        public EditorFunction Functions { get; set; }
        public bool PopUpActive { get; set; }

        public Editor(string path)
        {
            PopUpActive = false;
            Path = path;
            TextLines = EditorFileService.ReadTextFromFile(path);
            SelectedLine = 0;
            SelectedChar = 0;
            DrawLeft = 0;
            DrawTop = 0;
            Modify = false;
            Select = new Select();
            Functions = new EditorFunction();
            Functions.ClosePopUp += ClosePopUp;
        }
        public void Regenerate()
        {
            Height = Scale.LastHeight-2;//-3 => horní a dolní lišta
            Width = Scale.LastWidth;
            Draw();
            if (PopUpActive)
                Functions.DrawPopUp();
        }
        public void Draw()
        {
            if(Select.EditingSelect)
            {
                if (SelectedChar > Select.DefChar || SelectedLine > Select.DefLine)
                {
                    Select.StopChar = SelectedChar;
                    Select.StopLine = SelectedLine;
                }
                else /*if(SelectedChar < Select.DefChar || SelectedLine < Select.DefLine)*/
                {
                    Select.StartChar = SelectedChar;
                    Select.StartLine = SelectedLine;
                }
            }


            while(SelectedChar >= DrawLeft + Width-1) { DrawLeft++; }
            while(SelectedChar < DrawLeft) { DrawLeft--; }

            while(SelectedLine > DrawTop + Height-1) { DrawTop++; }
            while(SelectedLine < DrawTop) { DrawTop--; }

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 1);
            SetToEditorColor();
            for (int i = DrawTop; i < DrawTop+Height; i++)
            {
                if(Select.Active)
                {
                    if(i == Select.StartLine)
                    {
                        Select.DrawSelect(TextLines,DrawLeft,SelectedLine, SelectedChar);
                        i = Select.StopLine;
                    }
                    else
                    {
                        string line = "";
                        if (i < TextLines.Count) { line = TextLines[i]; }
                        else { line = "".PadRight(Width); }
                        line = line.PadRight(Width + DrawLeft).Substring(DrawLeft, Width);
                        Console.Write(line.PadRight(Width));
                        Console.SetCursorPosition(0, Console.CursorTop + 1);
                    } 
                }
                else
                {
                    string line = "";
                    if (i < TextLines.Count) { line = TextLines[i]; }
                    else { line = "".PadRight(Width); }
                    line = line.PadRight(Width + DrawLeft).Substring(DrawLeft, Width);
                    Console.Write(line.PadRight(Width));
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
            }

            int a = Console.BufferHeight;
            int b = Console.BufferWidth;
            Console.SetCursorPosition(SelectedChar-DrawLeft, SelectedLine-DrawTop+1);
            Console.CursorVisible = true;

        }
        public void HandleKey(ConsoleKeyInfo key)
        {
            if (PopUpActive)
            {
                Functions.HandleKey(key);
            }
            else if (key.Key == ConsoleKey.DownArrow && SelectedLine < TextLines.Count - 1)
            {
                if (SelectedChar > TextLines[SelectedLine + 1].Length)
                {
                    SelectedChar = TextLines[SelectedLine + 1].Length;
                }
                SelectedLine++;
            }
            else if (key.Key == ConsoleKey.UpArrow && SelectedLine > 0)
            {
                if (SelectedChar > TextLines[SelectedLine - 1].Length)
                {
                    SelectedChar = TextLines[SelectedLine - 1].Length;
                }
                SelectedLine--;
            }
            else if (key.Key == ConsoleKey.LeftArrow)
            {
                if (SelectedChar == 0)
                {
                    if (SelectedLine > 0)
                    {
                        SelectedLine--;
                        SelectedChar = TextLines[SelectedLine].Length;
                    }
                }
                else { SelectedChar--; }
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (Select.Active && ((SelectedLine > Select.StartLine && SelectedLine < Select.StopLine) || (Select.StopLine == Select.StartLine && SelectedChar > Select.StartChar && SelectedChar < Select.StopChar)))
                {
                    Select.StopLine++;
                }
                if (SelectedChar == 0)
                {
                    TextLines.Insert(SelectedLine, "");
                    SelectedLine++;
                    Modify = true;
                }
                else
                {
                    TextLines.Insert(SelectedLine + 1, TextLines[SelectedLine].Substring(SelectedChar));
                    TextLines[SelectedLine] = TextLines[SelectedLine].Substring(0, SelectedChar);
                    SelectedLine++;
                    SelectedChar = 0;
                    Modify = true;
                }
                //Select.Active = false;
                //Select.EditingSelect = false;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (SelectedChar == 0)
                {
                    if (SelectedLine != 0)
                    {
                        if (Select.Active && ((SelectedLine > Select.StartLine && SelectedLine < Select.StopLine) || (Select.StopLine == Select.StartLine && SelectedChar > Select.StartChar && SelectedChar < Select.StopChar)))
                        {
                            Select.StopLine--;
                        }
                        SelectedChar = TextLines[SelectedLine - 1].Length;
                        TextLines[SelectedLine - 1] = TextLines[SelectedLine - 1] + TextLines[SelectedLine];
                        TextLines.RemoveAt(SelectedLine);
                        SelectedLine--;
                    }
                }
                else
                {//odstraň znak
                    if (Select.Active && SelectedLine == Select.StopLine && SelectedChar == Select.StopChar)
                    {
                        Select.StopChar--;
                    }
                    TextLines[SelectedLine] = TextLines[SelectedLine].Remove(SelectedChar - 1, 1);
                    SelectedChar--;
                    Modify = true;
                }
            }
            else if (key.Key == ConsoleKey.RightArrow && SelectedChar < TextLines[SelectedLine].Length)
            {
                if (SelectedChar == TextLines[SelectedLine].Length && SelectedLine < TextLines.Count)
                {
                    SelectedChar = 0;
                    DrawLeft = 0;
                    SelectedLine++;
                }
                else { SelectedChar++; }
            }
            else if (key.Key == ConsoleKey.F3)
            {
                if (Select.Active)
                {
                    if (Select.StopChar == Select.StartChar && Select.StopLine == Select.StartLine)
                    {
                        Select.Active = false;
                        Select.EditingSelect = false;
                    }
                    else
                    {
                        if (Select.EditingSelect)
                        {
                            Select.EditingSelect = false;
                        }
                        else
                        {
                            Select.Reset(SelectedLine, SelectedChar);
                            Select.EditingSelect = true;
                        }
                    }
                }
                else
                {
                    Select.Active = true;
                    Select.EditingSelect = true;
                    Select.Reset(SelectedLine, SelectedChar);
                }
            }
            else if (key.Key == ConsoleKey.F5)
            {
                Modify = true;
                TextLines = Functions.Copy(Select, TextLines, SelectedChar, SelectedLine);
                if (SelectedLine <= Select.StartLine)
                {
                    Select.Active = false;
                    Select.EditingSelect = false;
                }
            }
            else if (key.Key == ConsoleKey.F6)
            {
                if (Select.Active)
                {
                    Modify = true;
                    TextLines = Functions.Move(Select, TextLines, SelectedChar, SelectedLine);
                    Select.Active = false;
                }
            }
            else if (key.Key == ConsoleKey.F7)
            {
                PopUpActive = true;
                Functions.Search(Select, TextLines);
            }
            else if (key.Key == ConsoleKey.F8)
            {
                if (Select.Active)
                {
                    Modify = true;
                    TextLines = Functions.Delete(Select, TextLines);
                    Select.Active = false;
                    Select.EditingSelect = false;
                    SelectedChar = 0;
                }
            }
            else if(Char.GetUnicodeCategory(key.KeyChar) != UnicodeCategory.Control)
            {
                if(TextLines.Count == 0) { TextLines.Add(""); }
                TextLines[SelectedLine] = TextLines[SelectedLine].Insert(SelectedChar, Convert.ToString(key.KeyChar));
                SelectedChar++;
                Modify = true;
                //Select.Active = false;
                //Select.EditingSelect = false;
            }
            Draw();
            if(PopUpActive)
                Functions.DrawPopUp();
        }
        public void SaveEdit()
        {
            Modify = false;
            EditorFileService.WriteTextToFile(Path, TextLines);
        }
        public static void SetToEditorColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
        }
        public static void SetToSelectColor()
        {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        private void ClosePopUp()
        {
            PopUpActive = false;
            Draw();
        }
    }
}
