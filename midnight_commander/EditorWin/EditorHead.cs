using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander.EditorWin
{
    public class EditorHead
    {
        public void Draw(string path, int columnNumber, bool modify, int linesOutOfWindow, int lineNumber, int lineCount, int charsToSelect, int charsCount, string selectedChar, bool selectAcitive)// doplnit další proměnné 
        {
            Console.SetCursorPosition(0, 0);
            int width = Scale.LastWidth;
            string line = path;
            if (path == "") { line = "NewFile"; }
            else { line = path; }
            line += "   [";
            if (selectAcitive) { line += "B"; }
            else { line += "-"; }
            if (modify) { line += "M"; }
            else { line += "-"; }
            line += "]   ";
            line += columnNumber + " L:[  l+ " + linesOutOfWindow + "   " + lineNumber + "/" + lineCount + "] *(" + charsToSelect + "/" + charsCount + ")";
            if(selectedChar == "<EOF>")
            {
                line += "  " + selectedChar;
            }
            else
            {
                line += "  " + (int)Convert.ToChar(selectedChar) + "  ";
                line += "0X0" + String.Format("{0:X}", (int)Convert.ToChar(selectedChar));
            }


            if (Scale.LastWidth > line.Length) { line = line.PadRight(Scale.LastWidth); }
            else if (Scale.LastWidth < line.Length) { line = line.Substring(0,Scale.LastWidth); }
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(line);
        }
    }
}
