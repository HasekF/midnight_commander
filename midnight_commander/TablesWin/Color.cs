using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander
{
    public class Color
    {
        public static void Change(ColorType type)
        {
            if (type == ColorType.BLACK)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (type == ColorType.SELECTED)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Cyan;
            }
            else if (type == ColorType.UNSELECTED)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else if (type == ColorType.ZIP_FILE)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else if (type == ColorType.EXE_FILE)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else if (type == ColorType.TMP_FILE)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else if (type == ColorType.TXT_FILE)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else if (type == ColorType.DAT_FILE)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else if (type == ColorType.XML_FILE)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else if (type == ColorType.PATH)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else if (type == ColorType.HEAD)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else if (type == ColorType.MENU_NUMBER)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (type == ColorType.MENU_TEXT)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
        }
    }
    public enum ColorType
    {
        BLACK,
        SELECTED,
        UNSELECTED,
        ZIP_FILE,
        EXE_FILE,
        XML_FILE,
        TMP_FILE,
        DAT_FILE,
        TXT_FILE,
        PATH,
        HEAD,
        MENU_NUMBER,
        MENU_TEXT,

    }
}
