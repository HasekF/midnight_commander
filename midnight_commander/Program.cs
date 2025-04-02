using midnight_commander.PopUpWindows;
using System;
using System.IO;

namespace midnight_commander
{
    class Program
    {
        static void Main(string[] args)
        {

            Color.Change(ColorType.UNSELECTED);
            Console.WindowHeight = 30;
            Console.WindowWidth = 120;
            Application app = new Application();
            app.ReSize();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    app.HandleKey(Console.ReadKey());
                }
                Scale.Check(app);
            }
        }
    }
}
