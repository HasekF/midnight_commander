using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace midnight_commander.FileService
{
    public class FilesManager
    {
        public static Action<string> AccessDenied;

        public static List<Item> GetItemsFromDir(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            try
            {
                DirectoryInfo[] test = dir.GetDirectories();
            }
            catch
            {
                AccessDenied(Convert.ToString(dir.Parent));
                dir = new DirectoryInfo(Convert.ToString(dir.Parent));   
            }
            List<Item> DirecotriesAndFiles = new List<Item>();
            if(Convert.ToString(dir.Root) != Convert.ToString(dir.FullName))
                DirecotriesAndFiles.Add(new Item(ItemTypes.BACK_BUTTON, dir.Parent.FullName, "UP--DIR", Convert.ToString(dir.Parent.LastWriteTime)));
            else
                DirecotriesAndFiles.Add(new Item(ItemTypes.BACK_BUTTON, dir.FullName, "UP--DIR", Convert.ToString(dir.LastWriteTime)));
            foreach (var item in dir.GetDirectories())
            {
                string str = item.FullName;
                DirecotriesAndFiles.Add(new Item(ItemTypes.DIRECOTRY, item.FullName, "   ", Convert.ToString(item.LastWriteTime)));
            }
            foreach (var item in dir.GetFiles())
            {
                DirecotriesAndFiles.Add(new Item(ItemTypes.FILE, item.FullName, Convert.ToString(item.Length/1024), Convert.ToString(item.LastWriteTime)));
            }
            return DirecotriesAndFiles;
        }
        public static string GetCapacityInfo(string path)
        {
            DriveInfo drive = new DriveInfo(path);
            int totalSize = Convert.ToInt32(drive.TotalSize/ 1073741824);
            int freeSpace = Convert.ToInt32(drive.TotalFreeSpace / 1073741824);
            double percent = Convert.ToDouble(100*drive.TotalFreeSpace)/Convert.ToDouble(drive.TotalSize);
            string percentStr = Convert.ToString(Math.Round(percent, 2));
            string info = " " + Convert.ToString(freeSpace) + "G/" + Convert.ToString(totalSize) + "G (" + percentStr + "%) "; 
            return info;
        }
        public static DriveInfo[] GetDrives()
        {
            return DriveInfo.GetDrives();
        }
        public static void MakeDir(string path, string name)
        {
            Directory.CreateDirectory(path + name);
        }
        public static void Delete(string path, ItemTypes type)
        {
            if(type == ItemTypes.DIRECOTRY)
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                dir.Delete(true);
            }
            else if(type == ItemTypes.FILE)
            {
                FileInfo file = new FileInfo(path);
                file.Delete();
            }
        }
        public static void MoveDir(string sourcePath, string destPath, ItemTypes type)
        {
            //try
            //{
                if (type == ItemTypes.FILE)
                {
                    File.Move(sourcePath, destPath);
                }
                else if (type == ItemTypes.DIRECOTRY)
                {
                    Coping(sourcePath, destPath);
                    Directory.Delete(sourcePath, true);
                }
            //}
            //catch { }
        }
        public static void CopyFile(string sourcePath, string destPath, ItemTypes type)
        {
            //try
            //{
                if (type == ItemTypes.FILE)
                {
                    string[] parts = sourcePath.Split('\\');
                    File.Copy(sourcePath, destPath + parts[parts.Length - 1]);
                }
                else if (type == ItemTypes.DIRECOTRY)
                {
                    Coping(sourcePath, destPath);
                }
            //}
            //catch { }
        }
        private static void Coping(string source, string destination)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(source);
            destination = destination + sourceDir.Name + "\\";
            DirectoryInfo destinationDir = new DirectoryInfo(destination);
            if (!destinationDir.Exists) { Directory.CreateDirectory(destinationDir.FullName); }
            foreach (var item in sourceDir.GetFiles())
            {
                item.CopyTo(destination + item.Name);
            }
            foreach (var item in sourceDir.GetDirectories())
            {
                Coping(item.FullName, destination);
            }
        }
    }
}
