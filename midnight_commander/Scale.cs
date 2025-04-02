using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander
{
    public class Scale
    {
        public static int LastHeight = Console.WindowHeight;
        public static int LastWidth = Console.WindowWidth;
        public const int MIN_HEIGHT = 10;
        public const int MIN_WIDTH = 80;
        public static void Check(Application app)
        {
            if (Console.WindowWidth != LastWidth || Console.WindowHeight != LastHeight) 
            {

                if (Console.WindowWidth < MIN_WIDTH)
                {
                    Console.WindowWidth = MIN_WIDTH;
                    LastWidth = MIN_WIDTH;
                }
                else { LastWidth = Console.WindowWidth; }
                if (Console.WindowHeight < MIN_HEIGHT)
                {
                    Console.WindowHeight = MIN_HEIGHT;
                    LastHeight = MIN_HEIGHT;
                }
                else { LastHeight = Console.WindowHeight; }

                if (LastHeight >= MIN_HEIGHT && LastWidth >= MIN_WIDTH) 
                {
                    Color.Change(ColorType.BLACK);
                    Console.Clear();
                    app.ReSize();
                }
            }
        }
    }
}
