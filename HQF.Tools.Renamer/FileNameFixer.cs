using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace HQF.Tools.Renamer
{
    public class FileNameFixer
    {
        public FileNameFixer()
        {
            StringToRemove = "/";
            StringReplacement = "_";
        }

        public void FixAll(string rootDirectory)
        {
            var directories = ShowAllFoldersUnder(rootDirectory, 2);
            foreach (var directory in directories)
            {
                IEnumerable<string> files = Directory.EnumerateFiles(directory);
                foreach (string file in files)
                {
                    try
                    {
                        FileInfo info = new FileInfo(file);
                        if (!info.IsReadOnly && !info.Attributes.HasFlag(FileAttributes.System))
                        {
                            string destFileName = GetNewFile(file);
                            info.MoveTo(destFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.Message);
                    }
                }
            }
        }

        private string GetNewFile(string file)
        {
            string nameWithoutExtension = Path.GetFileNameWithoutExtension(file);
            if (nameWithoutExtension != null && nameWithoutExtension.Length > 1)
            {
                return Path.Combine(Path.GetDirectoryName(file),
                    file.Replace(StringToRemove, StringReplacement));// + Path.GetExtension(file));
            }
            return file;
        }

        private static List<string> ShowAllFoldersUnder(string path, int indent)
        {
            var result = new List<string>();
            try
            {
                if ((File.GetAttributes(path) & FileAttributes.ReparsePoint)
                    != FileAttributes.ReparsePoint)
                {
                    foreach (string folder in Directory.GetDirectories(path))
                    {
                        Console.WriteLine(
                            "{0}{1}", new string(' ', indent), Path.GetFileName(folder));

                        result.Add(folder);

                        result.AddRange(ShowAllFoldersUnder(folder, indent + 2));
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public string StringToRemove { get; set; }

        public string StringReplacement { get; set; }
    }
}