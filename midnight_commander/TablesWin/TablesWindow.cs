using midnight_commander.EditorWin;
using midnight_commander.FileService;
using midnight_commander.PopUpWindows;
using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander
{
    public class TablesWindow : AbstractAppWindow
    {
        public Table[] Tables { get; set; }
        public Menu Menu { get; set; }

        //private bool PopUp;
        //public PopUpType WindowType { get; set; }
        private PopUpWindow PopUpWindow;
        public event Action<string> OpenEditor;

        public TablesWindow()
        {
            Tables = new Table[2];
            Tables[0] = new Table(true, true);
            Tables[1] = new Table(false, false);
            Menu = new Menu("Help", "Menu", "View", "Edit", "Copy", "RenMov", "Mkdir", "Delete", "PullDn", "Quit");
            PopUpWindow = new PopUpWindow();
            foreach (var item in Tables)
            {
                item.OpenPopUp += CreatePopUp;
            }
            FilesManager.AccessDenied += SetParentPath;
            PopUpWindow.Done += PopUpDone;
        }
        private void CreatePopUp(PopUpType type)
        {
            PopUpWindow.CreateTablePopUp(type, Tables);
        }
        public override void HandleKey(ConsoleKeyInfo key)
        {
            if (PopUpWindow.Active)
            {
                if (key.Key == ConsoleKey.Escape)
                {
                    //PopUp = false;
                    this.Regenerate();
                }
                else
                {
                    PopUpWindow.HandleKey(key);
                }
            }
            else
            {
                if (key.Key == ConsoleKey.Tab)
                {
                    foreach (var item in Tables)
                    {
                        item.Selected = !item.Selected;
                        item.Draw();
                    }
                }
                else if (key.Key == ConsoleKey.F7)
                {
                    PopUpWindow.CreateTablePopUp(PopUpType.MAKE_DIR, Tables);
                }
                else if (key.Key == ConsoleKey.F8 || key.Key == ConsoleKey.F6 || key.Key == ConsoleKey.F5)
                {
                    Table table = GetTable();
                    if (table.ItemList[table.SelectedLine].Type != ItemTypes.BACK_BUTTON)
                    {
                        if (key.Key == ConsoleKey.F8) { PopUpWindow.CreateTablePopUp(PopUpType.DELETE, Tables); }
                        else if (key.Key == ConsoleKey.F6) { PopUpWindow.CreateTablePopUp(PopUpType.MOVE, Tables); }
                        else if (key.Key == ConsoleKey.F5) { PopUpWindow.CreateTablePopUp(PopUpType.COPY, Tables); }
                    }
                }
                else if(key.Key == ConsoleKey.F4)
                {
                    foreach (var item in Tables)
                    {
                        if (item.Selected)
                        {
                            OpenEditor(item.ItemList[item.SelectedLine].Path);
                            //OpenEditor(new EditorWindow(Tables));
                        }
                    }
                }
                else
                {
                    foreach (var item in Tables)
                    {
                        if (item.Selected) { item.HandleKey(key); }
                    }
                }
            }
        }
        public void PopUpDone(List<string> result)
        {
            PopUpType type = PopUpWindow.Type;
            if (type == PopUpType.DRIVE_CHOOSE)
            {
                foreach (var item in Tables)
                {
                    if (item.Selected) { item.Path = result[0]; }
                }
            }
            else if (type == PopUpType.MAKE_DIR)
            {
                if (result[0] == "OK")
                {
                    FilesManager.MakeDir(GetTable().Path, result[1]);
                }
            }
            else if (type == PopUpType.DELETE)
            {
                if (result[0] == "Yes")
                {
                    Table table = GetTable();
                    FilesManager.Delete(table.ItemList[table.SelectedLine].Path, table.ItemList[table.SelectedLine].Type);

                }
            }
            else if (type == PopUpType.MOVE)
            {
                if (result[0] == "OK")
                {
                    Table table = GetTable();
                    FilesManager.MoveDir(result[1], result[2], table.ItemList[table.SelectedLine].Type);
                }
            }
            else if (type == PopUpType.COPY)
            {
                if (result[0] == "OK")
                {
                    Table table = GetTable();
                    FilesManager.CopyFile(result[1], result[2], table.ItemList[table.SelectedLine].Type);
                }
            }
            Regenerate();
        }
        private Table GetTable()
        {
            foreach (var item in Tables)
            {
                if (item.Selected) return item;
            }
            return null;
        }
        public override void Regenerate()
        {
            Console.CursorVisible = false;
            Tables[0].Regenerate();
            Tables[1].Regenerate();
            Menu.Regenerate();
            if (PopUpWindow.Type != PopUpType.NONE && PopUpWindow.Active) { PopUpWindow.Regenerate(); }
        }
        public void SetParentPath(string parentPath)
        {//pokud je přístup zamítnut, FilesManager volá tuto metodu, aby tuto skutečnost zdělila aktivní tabulce(ukazatel cesty) - zde je volána z Aplication
            foreach (var item in Tables)
            {
                if (item.Selected) { item.Path = parentPath; }
            }
        }
    }
    public enum PopUpType
    {
        NONE,
        DRIVE_CHOOSE,
        MAKE_DIR,
        DELETE,
        COPY,
        MOVE,
        EDITOR_QUITSAVE,
        EDITOR_SEARCH,
        EDITOR_SEARCH_NOT_FOUND,
        EDITOR_REPLACE_INPUT,
        EDITOR_SINGLE_REPLACE,


    }
}
