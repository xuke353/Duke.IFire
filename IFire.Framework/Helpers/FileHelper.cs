using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFire.Framework.Helpers {

    public class FileHelper {

        #region 创建文本文件

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void CreateFile(string path, string content) {
            if (!Directory.Exists(Path.GetDirectoryName(path))) {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            using var sw = new StreamWriter(path, false, Encoding.UTF8);
            sw.Write(content);
        }

        #endregion 创建文本文件

        #region GetContentType

        public static string GetContentType(string path) {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            var contentType = types[ext];
            if (string.IsNullOrEmpty(contentType)) {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        #endregion GetContentType

        #region GetMimeTypes

        public static Dictionary<string, string> GetMimeTypes() {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        #endregion GetMimeTypes

        public static void CreateDirectory(string directory) {
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
        }

        public static void DeleteDirectory(string filePath) {
            try {
                if (Directory.Exists(filePath)) //如果存在这个文件夹删除之
                {
                    foreach (string d in Directory.GetFileSystemEntries(filePath)) {
                        if (File.Exists(d))
                            File.Delete(d); //直接删除其中的文件
                        else
                            DeleteDirectory(d); //递归删除子文件夹
                    }
                    Directory.Delete(filePath, true); //删除已空文件夹
                }
            } catch (Exception) {
                throw;
            }
        }
    }
}
