using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace midnight_commander.FileService
{
    public static class EditorFileService
    {
        public static List<string> ReadTextFromFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            List<string> text = new List<string>();
            while(!reader.EndOfStream)
            {
                text.Add(reader.ReadLine());
            }
            reader.Close();
            return text;
        }
        public static void WriteTextToFile(string path, List<string> text)
        {
            StreamWriter writer = new StreamWriter(path);
            foreach (var item in text)
            {
                writer.WriteLine(item);
            }
            writer.Close();
        }
        public static int GetCharCount(List<string> texts)
        {
            int lenght = 0;
            foreach (var item in texts)
            {
                lenght += item.Length;
            }
            return lenght;
        }
        public static int GetCharCountToSelect(List<string> texts, int selectedLine, int selectedChar)
        {
            int lenght = 0;
            for (int i = 0; i < texts.Count; i++)
            {
                if (i == selectedLine)
                {
                    lenght += texts[i].Substring(0, selectedChar).Length;
                    break;
                }
                else
                {
                    lenght += texts[i].Length;
                }
            }
            return lenght;
        }
        public static string GetSelectedChar(List<string> texts, int selectedLine, int selectedChar)
        {
            try
            {
                return texts[selectedLine].Substring(selectedChar, 1);
            }
            catch
            { return "<EOF>"; }
        }
    }
}
