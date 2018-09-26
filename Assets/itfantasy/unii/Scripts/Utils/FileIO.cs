using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace itfantasy.unii.utils
{
    public class FileIO
    {

        public static string MD5(byte[] buffer)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(buffer);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static List<FileInfo> GetFileInfos(string dir)
        {
            if (dir.Trim() == "" || !Directory.Exists(dir))
            {
                return null;
            }
            List<FileInfo> fileInfoList = new List<FileInfo>();
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            FileInfo[] fileInfos = dirInfo.GetFiles();
            if (fileInfos != null && fileInfos.Length > 0)
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    fileInfoList.Add(fileInfo);
                }
            }
            return fileInfoList;
        }

        public static List<string> GetSubDirectory(string dir)
        {
            if (dir.Trim() == "" || !Directory.Exists(dir))
            {
                return null;
            }
            List<string> dirList = new List<string>();
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
            if (dirInfos != null && dirInfos.Length > 0)
            {
                foreach (DirectoryInfo childDirInfo in dirInfos)
                {
                    dirList.Add(childDirInfo.Name);
                }
            }
            return dirList;
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static void CreateFile(string path, byte[] content, int offset, int count)
        {
            string dir = GetDirectoryByPath(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Write(content, offset, count);
            fs.Flush();
            fs.Close();
        }

        public static void CreateFile(string path, byte[] content)
        {
            CreateFile(path, content, 0, content.Length);
        }

        public static void CreateFile(string path, string content)
        {
            byte[] contentBytes = Comm.Str2Bytes(content);
            CreateFile(path, contentBytes);
        }

        public static byte[] ReadFileRaw(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] content = new byte[fs.Length];
            fs.Read(content, 0, (int)fs.Length);
            fs.Close();
            return content;
        }

        public static string ReadFile(string path)
        {
            byte[] contentBytes = ReadFileRaw(path);
            return Comm.Bytes2Str(contentBytes);
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static string GetDirectoryByPath(string path)
        {
            string[] temps = path.Split('/');
            string dir = "";
            for (int i = 0; i < temps.Length - 1; i++)
            {
                dir += temps[i] + "/";
            }
            return dir;
        }

        public static string GetFileNameByPath(string path)
        {
            string[] temps = path.Split('/');
            return temps[temps.Length - 1];
        }

        public static void CopyFile(string surPath, string desPath)
        {
            if (File.Exists(surPath))
            {
                File.Copy(surPath, desPath);
            }
        }
    }
}
