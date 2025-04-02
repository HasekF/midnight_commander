using System;
using System.Collections.Generic;
using System.Text;
using midnight_commander.FileService;
using midnight_commander.PopUpWindows;

namespace midnight_commander.EditorWin
{
    public class EditorWindow : AbstractAppWindow
    {
        public Editor Editor { get; set; }
        public EditorHead EditorHead { get; set; }
        public event Action OpenTables;
        private Menu Menu;
        private PopUpWindow PopUpWindow;
        private Table[] Tables { get; set; }

        public EditorWindow(Table[] tables)
        {
            Menu = new Menu("Help", "Save", "Mark", "Replace", "Copy", "Move", "Search", "Delete", "PullDn", "Quit");
            EditorHead = new EditorHead();
            Tables = tables;
            PopUpWindow = new PopUpWindow();
            PopUpWindow.Done += DonePopUp;
        }
        public void OpenEditor(string path)
        {
            Editor = new Editor(path);

            Regenerate();
        }
        public override void Regenerate()
        {
            Editor.Regenerate();
            EditorHead.Draw(Editor.Path, Editor.SelectedChar, Editor.Modify, Editor.DrawTop, Editor.SelectedLine+1, Editor.TextLines.Count, EditorFileService.GetCharCountToSelect(Editor.TextLines, Editor.SelectedLine, Editor.SelectedChar), EditorFileService.GetCharCount(Editor.TextLines), EditorFileService.GetSelectedChar(Editor.TextLines, Editor.SelectedLine, Editor.SelectedChar), Editor.Select.Active);
            Menu.Regenerate();
            Console.SetCursorPosition(Editor.SelectedChar - Editor.DrawLeft, Editor.SelectedLine - Editor.DrawTop + 1);
        }
        public override void HandleKey(ConsoleKeyInfo key)
        {
            if(PopUpWindow.Active)
            {
                PopUpWindow.HandleKey(key);
            }
            else
            {
                if (key.Key == ConsoleKey.F2)
                {
                    Editor.SaveEdit();
                    Regenerate();
                }
                else if (key.Key == ConsoleKey.F10 || key.Key == ConsoleKey.Escape)
                {
                    if (Editor.Modify)
                    {
                        PopUpWindow.CreateTablePopUp(PopUpType.EDITOR_QUITSAVE, Tables);
                    }
                    else
                    {
                        OpenTables();
                    }

                }
                else
                {
                    Editor.HandleKey(key);
                    Regenerate();
                }
            }

        }
        public void DonePopUp(List<string> result)
        {
            PopUpType type = PopUpWindow.Type;
            if (type == PopUpType.EDITOR_QUITSAVE)
            {
                if(result[0] == "Yes") { Editor.SaveEdit(); OpenTables(); }
                else if (result[0] == "No") { OpenTables(); }
                else if (result[0] == "Cancel") { Regenerate(); }
            }
        }
    }
}
