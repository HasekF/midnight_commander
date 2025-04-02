using midnight_commander.FileService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace midnight_commander
{
    public class Table
    {
        //stavební materiál =>   ─ │ ┌ ┐ └ ┘ ├ ┤ ┬ ┴ ┼ ═ ║
        public event Action<PopUpType> OpenPopUp;
        private int Height;
        private int Width;
        public bool Selected { get; set; }
        public string Path { get; set; }
        public bool FirstTable { get; set; }
        private int MaxItemsOnScreen;
        private int TableLenght;
        private int DefaulCursorPosition;
        public List<Item> ItemList { get; set; }
        private int SizeLenght;
        private int TimeLenght;
        public int SelectedLine { get; set; }
        private int DrawIndex;
        private int CursorTopPosition;
        
        public Table(bool firstTable, bool selected)
        {
            FirstTable = firstTable;
            ItemList = new List<Item>();
            SelectedLine = 0;
            Selected = selected;
            Path = @"C:\";

            Regenerate();
        }

        public void Regenerate()
        {
            DrawIndex = 0;
            SelectedLine = 0;
            this.Height = Scale.LastHeight;
            this.Width = Scale.LastWidth;
            this.TableLenght = (this.Width - this.Width%2)/2;
            if (!this.FirstTable) { this.DefaulCursorPosition = this.TableLenght; }
            else { this.DefaulCursorPosition = 0; }
            MaxItemsOnScreen = Scale.LastHeight - 6;
            ItemList = FilesManager.GetItemsFromDir(Path);
            Draw();
        } 
        public void Draw()
        {
            try
            {
                List<Item> DrawList;
                if (ItemList.Count <= MaxItemsOnScreen) { DrawList = ItemList; }
                else { DrawList = CreateDrawList(); }
                //DrawList = ItemList;

                CursorTopPosition = 0;
                SizeLenght = GetLenght(GetLenghtType.SIZE);
                TimeLenght = GetLenght(GetLenghtType.TIME);
                DrawHead(Selected);
                DrawColumnNames();
                for (int i = 0; i < DrawList.Count; i++)
                {
                    if (SelectedLine == i + DrawIndex && Selected){DrawItem(DrawList[i], true);}
                    else{ DrawItem(DrawList[i], false); }
                }
                if (DrawList.Count < MaxItemsOnScreen)
                {
                    for (int i = 0; i < MaxItemsOnScreen - DrawList.Count; i++)
                    {
                        DrawEmpyLine();
                    }
                }
                DrawClosure();
            }
            catch
            {
                // toto je pro případ extrémního a neustálého zmenšování okna
            }
        }
        public void DrawHead(bool selected)
        {
            Console.SetCursorPosition(DefaulCursorPosition, 0);
            Color.Change(ColorType.UNSELECTED);
            Console.Write("┌─");
            string path = Path;
            if (path.Length > TableLenght-5)
            {
                path = path.Remove(0, path.Length - TableLenght + 10);
                path = path.Insert(0, " ~ ");
                int i = path.Length;
            }
            if (selected) { Color.Change(ColorType.PATH); }
            Console.Write(path);
            Color.Change(ColorType.UNSELECTED);
            string line = "".PadRight(TableLenght-3-path.Length,'─')+ "┐";
            Console.Write(line);
            CursorTopPosition++;
            Console.SetCursorPosition(DefaulCursorPosition, CursorTopPosition);
        }
        public void DrawColumnNames()
        {
            Color.Change(ColorType.UNSELECTED);
            Console.Write("│");
            string line = "";
            line = line.PadRight((TableLenght - SizeLenght - TimeLenght) / 2 - 5) + "Name";// -5 => -4 odečítám znaky ze slova Name -1 za čáru na začátku
            line = line.PadRight(TableLenght - SizeLenght - TimeLenght-8);// -8 =>-7 odečítám čáry a mezery u velikosti a datumu změny -1 čára na začátku
            Color.Change(ColorType.HEAD);
            Console.Write(line);
            Color.Change(ColorType.UNSELECTED);
            Console.Write("│");
            string size = "";
            size = size.PadRight(SizeLenght / 2-1) + "Size";//-1 => otečítám mezeru u levé čáry
            size = size.PadRight(SizeLenght+2);//+2 => přičítám obě mezery u obou čar
            Color.Change(ColorType.HEAD);
            Console.Write(size);
            Color.Change(ColorType.UNSELECTED);
            Console.Write("│");
            string time = "";
            time = time.PadRight(TimeLenght / 2 - 4) + "Modify time";//-4 => -1 za čáru, -3 kvůli vycentrování textu
            time = time.PadRight(TimeLenght + 2);//+2 => přičítám obě mezery u obou čar 
            Color.Change(ColorType.HEAD);
            Console.Write(time);
            Color.Change(ColorType.UNSELECTED);
            Console.Write("│");
            CursorTopPosition++;
            Console.SetCursorPosition(DefaulCursorPosition, CursorTopPosition);
        }
        public void DrawItem(Item item, bool selected)
        {
            string size = item.Size;
            if (size.Length < SizeLenght) { size = size.PadRight(SizeLenght); }

            string time = item.ModifyTime;
            if (time.Length < TimeLenght) { time = time.PadRight(TimeLenght); }

            string line = "";
            if (item.Type == ItemTypes.DIRECOTRY || item.Type == ItemTypes.BACK_BUTTON) { line += @"/"; }
            else if (item.Type == ItemTypes.FILE) { line += " "; }
            line += item.Name;
            int correctLineLenght = TableLenght - time.Length - size.Length-8;
            if(line.Length < correctLineLenght) { line = line.PadRight(correctLineLenght); }
            else if (line.Length > correctLineLenght)
            {
                line = line.Remove(correctLineLenght / 2, line.Length - correctLineLenght + 3);
                line = line.Insert(line.Length / 2, " ~ ");
            }

            if(selected)
            {
                Color.Change(ColorType.SELECTED);
                Console.Write("│"+line+ "│ "+size+ " │ " + time + " │");
                Color.Change(ColorType.UNSELECTED);
            }
            else
            {
                Console.Write("│");
                Color.Change(GetColorType(item.Name));
                Console.Write(line);
                Color.Change(ColorType.UNSELECTED);
                Console.Write("│ ");
                Color.Change(GetColorType(item.Name));
                Console.Write(size);
                Color.Change(ColorType.UNSELECTED);
                Console.Write(" │ ");
                Color.Change(GetColorType(item.Name));
                Console.Write(time);
                Color.Change(ColorType.UNSELECTED);
                Console.Write(" │");
            }
            CursorTopPosition++;
            Console.SetCursorPosition(DefaulCursorPosition, CursorTopPosition);
        }
        public void DrawEmpyLine()
        {
            string line = "│".PadRight(TableLenght - SizeLenght - TimeLenght - 7) + "│";
            line = line.PadRight(TableLenght - TimeLenght - 4) + "│";
            line = line.PadRight(TableLenght - 1) + "│";
            Console.Write(line);
            CursorTopPosition++;
            Console.SetCursorPosition(DefaulCursorPosition, CursorTopPosition);
        }
        public void DrawClosure()
        {
            string line = "├" + "".PadRight(TableLenght - 2, '─') + "┤";
            Console.Write(line);
            CursorTopPosition++;
            Console.SetCursorPosition(DefaulCursorPosition, CursorTopPosition);

            line = "│";
            if (ItemList[SelectedLine].Type == ItemTypes.DIRECOTRY || ItemList[SelectedLine].Type == ItemTypes.BACK_BUTTON)
                line += "/";
            else { line += " "; }
            string name = ItemList[SelectedLine].Name;
            if (name.Length > TableLenght - 4)
            {
                name = name.Remove((TableLenght-3) / 2, name.Length - (TableLenght-3));
                name = name.Insert(name.Length / 2, " ~ ");
            }
            line += name;
            line = line.PadRight(TableLenght - 1) + "│";
            Console.Write(line);
            CursorTopPosition++;
            Console.SetCursorPosition(DefaulCursorPosition, CursorTopPosition);

            string capacity = FilesManager.GetCapacityInfo(Path);
            line = capacity + "─┘";
            line = "└" + line.PadLeft(TableLenght - 1, '─');
            Console.Write(line);
        }
        private List<Item> CreateDrawList()
        {
            List<Item> DrawList = new List<Item>();
            if (DrawIndex + MaxItemsOnScreen <= SelectedLine)
                DrawIndex++;
            if (DrawIndex > SelectedLine)
                DrawIndex--;
            for (int i = 0; i < MaxItemsOnScreen; i++)
            {
                DrawList.Add(ItemList[DrawIndex + i]);
            }
            return DrawList;
        }
        private int GetLenght(GetLenghtType type)
        {
            int lenght = 0;
            foreach (var item in ItemList)
            {
                if(type == GetLenghtType.SIZE) 
                { 
                    if(lenght < item.Size.Length) { lenght = item.Size.Length; }
                }
                else if (type == GetLenghtType.TIME) 
                {
                    if (lenght < item.ModifyTime.Length) { lenght = item.ModifyTime.Length; }
                }
            }
            return lenght;
        }
        private ColorType GetColorType(string fileName)
        {
            string[] array = fileName.Split('.');
            string type = array[array.Length - 1];
            if(type == "zip" || type == "ZIP") { return ColorType.ZIP_FILE; }
            if (type == "txt" || type == "TXT") { return ColorType.TXT_FILE; }
            if (type == "xml" || type == "XML") { return ColorType.XML_FILE; }
            if (type == "dat" || type == "DAT") { return ColorType.DAT_FILE; }
            if (type == "tmp" || type == "TMP") { return ColorType.TMP_FILE; }
            if (type == "exe" || type == "EXE") { return ColorType.EXE_FILE; }
            return ColorType.UNSELECTED;
        }

        public void HandleKey(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.UpArrow && this.SelectedLine != 0)
            { SelectedLine--; }
            else if (key.Key == ConsoleKey.DownArrow && this.SelectedLine != this.ItemList.Count - 1)
            { this.SelectedLine++; }
            if (key.Key == ConsoleKey.Enter && (ItemList[SelectedLine].Type == ItemTypes.DIRECOTRY || ItemList[SelectedLine].Type == ItemTypes.BACK_BUTTON))
            {
                if(ItemList[SelectedLine].Type == ItemTypes.BACK_BUTTON && Path == ItemList[SelectedLine].Path)
                {
                    this.OpenPopUp(PopUpType.DRIVE_CHOOSE);
                }
                else
                {
                    Path = ItemList[SelectedLine].Path;
                    Regenerate();
                }
            }
            else
            {
                Draw();
            }
        }
    }
    public enum GetLenghtType
    {
        SIZE,
        TIME,
    }
}
