using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RoosterTools
{
    public class FileOperations
    {
        public static string GetValidCSVPath(string path)
        {
            if (!File.Exists(path))
            {
                OpenFileDialog saveFileDialog = new OpenFileDialog();
                saveFileDialog.Title = "Choose a CSV File Location";
                saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;


                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = saveFileDialog.FileName;
                }
                else
                {
                    return null;
                }
            }
            return path;
        }

        public static string GetDirectory(string title)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = title;

            if (DialogResult.OK == folderDialog.ShowDialog())
            {
                return folderDialog.SelectedPath;
            }
            else
            {
                return null;
            }

        }

        public static void CopyFolderandContents(string originPath, string newDirectoryPath)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(originPath))
                {
                    string newDirName = Path.Combine(newDirectoryPath, d.Split(Path.DirectorySeparatorChar).Last());
                    DirectoryInfo newD = Directory.CreateDirectory(newDirName);
                    CopyFolderandContents(d, newD.FullName);
                }
                foreach (string file in Directory.GetFiles(originPath))
                {
                    string newFileName = Path.Combine(newDirectoryPath, file.Split(Path.DirectorySeparatorChar).Last());
                    File.Copy(file, newFileName);
                }
            }
            catch (System.Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }



        public static void DeleteAllBackupRevitFiles(string newDirectoryPath)
        {
            List<string> newFamilyFiles = FileOperations.GetFilePathsRecursively(newDirectoryPath, "*.rfa");
            string backupPattern = @"\.\d*\.";
            List<string> backupFamilyFiles = newFamilyFiles.Where(x => Regex.Match(x, backupPattern).Success).ToList();
            foreach (string filename in backupFamilyFiles)
            {
                File.Delete(filename);
            }
        }


        public static List<string> GetFilePathsRecursively(string directoryPath, string extension)
        {
            List<string> allPaths = new List<string>();

            List<string> subDirPaths = AddAllFilePaths(directoryPath, extension, allPaths);

            return allPaths;
        }

        //method is used to recursively look thorugh directories, adding found files to path
        internal static List<string> AddAllFilePaths(string directoryPath, string extension, List<string> allFilePaths)
        {
            foreach (string directory in Directory.GetDirectories(directoryPath))
            {
                AddAllFilePaths(directory, extension, allFilePaths);
            }
            foreach (string filePaths in Directory.GetFiles(directoryPath, extension))
            {
                allFilePaths.Add(filePaths);
            }
            return allFilePaths;
        }

        public static string[][] ReadCSVToArray(string path)
        {
            StreamReader reader = new StreamReader(File.OpenRead(path));
            List<string[]> rows = new List<string[]>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] row = line.Split(',');
                rows.Add(row);
            }

            return rows.ToArray();
        }
    }
}
