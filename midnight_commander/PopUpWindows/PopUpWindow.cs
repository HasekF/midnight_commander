using midnight_commander.PopUpWindows.Components;
using midnight_commander.PopUpWindows.PopUpTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace midnight_commander.PopUpWindows
{
    public class PopUpWindow
    {

        public ComponentManager CompManager { get; set; }
        public event Action<List<string>> Done;
        public PopUpTableConstructor TableConstructor { get; set; }
        public PopUpEditorConstructor EditorConstructor { get; set; }
        public PopUpType Type { get; set; }
        public bool Active { get; set; }

        public PopUpWindow()
        {
            Active = false;
            Type = PopUpType.NONE;
        }

        public void CreateTablePopUp(PopUpType type, Table[] tables)
        {
            PopUpSetUp(type);
            if (type == PopUpType.DRIVE_CHOOSE) { TableConstructor = new ChooseDrive(CompManager,tables); }
            else if (type == PopUpType.MAKE_DIR) { TableConstructor = new MkDir(CompManager,tables); }
            else if (type == PopUpType.DELETE) { TableConstructor = new Delete(CompManager,tables); }
            else if (type == PopUpType.MOVE) { TableConstructor = new Move(CompManager,tables); }
            else if (type == PopUpType.COPY) { TableConstructor = new Copy(CompManager,tables); }
            else if (type == PopUpType.EDITOR_QUITSAVE) { TableConstructor = new Editor_QuitSave(CompManager, tables); }
            TableConstructor.Construct();
            CompManager.Draw();
        }
        public void CreateEditorPopUp(PopUpType type)
        {
            PopUpSetUp(type);
            if (type == PopUpType.EDITOR_SEARCH) { EditorConstructor = new EditorSearch(CompManager); }
            //else if (type == PopUpType.EDITOR_SEARCH_NOT_FOUND) { EditorConstructor = new ChooseDrive(CompManager); }
            //else if (type == PopUpType.EDITOR_REPLACE_INPUT) { EditorConstructor = new ChooseDrive(CompManager); }
            //else if (type == PopUpType.EDITOR_SINGLE_REPLACE) { EditorConstructor = new ChooseDrive(CompManager); }
            EditorConstructor.Construct();
            CompManager.Draw();
        }
        private void PopUpSetUp(PopUpType type)
        {
            Console.CursorVisible = false;
            Active = true;
            Type = type;
            CompManager = new ComponentManager(false);
            CompManager.Done += JobIsDone;
        }
        public void Regenerate()
        {
            CompManager.Draw();
        }
        public void HandleKey(ConsoleKeyInfo key)
        {
            CompManager.HandelKey(key);
            if (Active) { CompManager.Draw(); }
        }
        private void JobIsDone(List<string> list)
        {
            Active = false;
            Done(list);
            Type = PopUpType.NONE;
        }
    }
}
