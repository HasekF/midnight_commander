using System;
using System.Collections.Generic;
using System.Text;

namespace midnight_commander
{
    public class Item
    {
        public ItemTypes Type { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string ModifyTime { get; set; }
        public string Path { get; set; }
        public Item(ItemTypes type, string fullName, string size, string modifyTime)
        {
            Type = type;
            Size = size;
            ModifyTime = modifyTime;
            SetNameAndPaht(fullName);
        }
        public void SetNameAndPaht(string fullName)
        {
            if(Type == ItemTypes.BACK_BUTTON)
            {
                Path = fullName;
                Name = "..";
            }
            else
            {
                string[] parts = fullName.Split("\\");
                Name = parts[parts.Length - 1];
                if (Type == ItemTypes.DIRECOTRY) { fullName += "\\"; }
                Path = fullName;
            }
        }
    }
    public enum ItemTypes
    {
        FILE,
        DIRECOTRY,
        BACK_BUTTON,
    }
}
