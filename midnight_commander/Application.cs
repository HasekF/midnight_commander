using System;
using System.Collections.Generic;
using System.Text;
using midnight_commander.PopUpWindows;
using midnight_commander.EditorWin;

namespace midnight_commander
{
    public class Application
    {
        private AbstractAppWindow ActiveAppWindow;
        private TablesWindow TablesWindow { get; set; }
        public EditorWindow EditorWindow { get; set; }
        public Application()
        {
            TablesWindow = new TablesWindow();
            TablesWindow.OpenEditor += SwitchToEditor;
            EditorWindow = new EditorWindow(TablesWindow.Tables);
            EditorWindow.OpenTables += SwitchToTables;
            ActiveAppWindow = TablesWindow;
        }
        public void ReSize()
        {
            ActiveAppWindow.Regenerate();
        }
        public void HandleKey(ConsoleKeyInfo key)
        {
            ActiveAppWindow.HandleKey(key);
        }
        private void SwitchToEditor(string path)
        {
            Console.Clear();
            ActiveAppWindow = EditorWindow;
            EditorWindow.OpenEditor(path);
        }
        private void SwitchToTables()
        {
            ActiveAppWindow = TablesWindow;
            Console.CursorVisible = false;
            ActiveAppWindow.Regenerate();
        }
    }

}
