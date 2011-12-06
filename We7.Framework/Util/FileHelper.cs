using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
namespace We7.Framework.Util
{
    /// <summary>
    /// 文件操作公共类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool Exists(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("参数为空或NULL");

            return File.Exists(file);

        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filePath">文件完全限定路径</param>
        /// <param name="content">需要写入的内容</param>
        /// /// <param name="fileModel">如果文件存在则设置添加模式(默认为追加)</param>
        public static void WriteFile(string filePath, string content, FileMode fileModel)
        {
            try
            {
                //创建目录
                CreateDirectory(filePath);
                //判断文件是否存在
                if (File.Exists(filePath))
                {
                    using (StreamWriter sw = File.CreateText(filePath))
                    {
                        TextWriter tw = TextWriter.Synchronized(sw);
                        tw.Write(content);
                        tw.Close();
                    }
                }
                else
                {
                    FileStream fs = File.Open(filePath, fileModel, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(true));
                    sw.Flush();
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                //log
            }
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filePath">文件完全限定路径</param>
        /// <param name="content">需要写入的内容</param>
        /// <param name="fileModel">如果文件存在则设置添加模式(默认为追加)</param>
        /// <param name="encoding">编码</param>
        /// <returns>成功：空；失败：错误消息</returns>
        public static string WriteFileWithEncoding(string filePath, string content, FileMode fileModel, Encoding encoding)
        {
            try
            {
                //创建目录
                CreateDirectory(filePath);
                //判断文件是否存在
                if (File.Exists(filePath))
                {
                    using (StreamWriter sw = File.CreateText(filePath))
                    {
                        TextWriter tw = TextWriter.Synchronized(sw);
                        tw.Write(content);
                        tw.Close();
                    }
                }
                else
                {
                    FileStream fs = File.Open(filePath, fileModel, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, encoding);
                    sw.Flush();
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }

                return "";
            }
            catch (Exception ex)
            {
                //log

                return ex.Message;
            }
        }


        /// <summary>
        ///  写文件
        /// </summary>
        /// <param name="filePath">文件完全限定路径</param>
        /// <param name="content">需要写入的内容</param>
        public static void WriteFile(string filePath, string content)
        {
            WriteFile(filePath, content, FileMode.Append);
        }

        /// <summary>
        /// 向指定文件中添加内容。这个是扩展以前的方法，以UTF8的行式写内容，不会出现乱码。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <param name="append"></param>
        public static void WriteFileEx(string filePath, string content, bool append)
        {
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Directory.Exists)
                fi.Directory.Create();

            using (StreamWriter sw = new StreamWriter(filePath, append, Encoding.UTF8))
            {
                sw.Write(content);
                sw.Flush();
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath">文件默认完路径名</param>
        /// <param name="encoding">读取文件编码方式（默认为utf-8）</param>
        /// <returns></returns>
        public static string ReadFile(string filePath, Encoding encoding)
        {
            //检查文件是否存在
            if (File.Exists(filePath))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    using (StreamReader sr = new StreamReader(filePath, encoding))
                    {
                        string temp = string.Empty;
                        while ((temp = sr.ReadLine()) != null)
                        {
                            sb.Append(temp);
                        }
                    }

                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    //log
                    throw ex;
                }
            }
            else
            {
                throw new Exception("文件不存在！");
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath">文件默认完路径名</param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// 读取文本文件（带换行符,能读取被锁住的文件）
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ReadFileWithLine(string filePath, Encoding encoding)
        {
            //检查文件是否存在
            if (File.Exists(filePath))
            {
                StringBuilder sb = new StringBuilder();
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (StreamReader sr = new StreamReader(fs,encoding))
                {
                    string temp = string.Empty;
                    while ((temp = sr.ReadLine()) != null)
                    {
                        sb.AppendLine(temp);
                    }
                }

                return sb.ToString();
            }
            else
            {
                throw new Exception("文件不存在！");
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void DeleteFile(string filePath)
        {
            //判断文件是否存在
            if (File.Exists(filePath))
            {
                try
                {
                    //删除文件
                    File.Delete(filePath);
                }
                catch { }
            }
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFileName">源文件完全路径</param>
        /// <param name="destFileName">新文件路径</param>
        /// <param name="overwrite">是否覆盖(默认true)</param>
        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {

            File.Copy(sourceFileName, destFileName, overwrite);
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFileName">源文件完全路径</param>
        /// <param name="destFileName">新文件路径</param>
        public static void Copy(string sourceFileName, string destFileName)
        {
            Copy(sourceFileName, destFileName, true);
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="SourcePath">原始文件夹路径</param>
        /// <param name="DestinationPath">新文件夹路径</param>
        /// <param name="overwriteexisting">是否覆盖(默认true)</param>
        /// <returns></returns>
        public static bool CopyDirectory(string sourcePath, string destinationPath, bool overwriteexisting)
        {
            bool ret = false;
            try
            {
                sourcePath = sourcePath.EndsWith(@"\") ? sourcePath : sourcePath + @"\";
                destinationPath = destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";

                if (Directory.Exists(sourcePath))
                {
                    if (Directory.Exists(destinationPath) == false)
                        Directory.CreateDirectory(destinationPath);

                    foreach (string fls in Directory.GetFiles(sourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(destinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(sourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, destinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="SourcePath">原始文件夹路径</param>
        /// <param name="DestinationPath">新文件夹路径</param>
        public static bool CopyDirectory(string sourcePath, string destinationPath)
        {
            return CopyDirectory(sourcePath, destinationPath, true);
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="fullFilePath">文件完全限定路径</param>
        private static void CreateDirectory(string fullFilePath)
        {
            if (!Directory.Exists(fullFilePath.Substring(0, fullFilePath.LastIndexOf(@"\"))))
            {
                Directory.CreateDirectory(fullFilePath.Substring(0, fullFilePath.LastIndexOf(@"\")));
            }
        }

        /// <summary>
        /// 获取文件最近修改时间
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static string GetFileLastChange(string virtualPath)
        {
            string fileName = HttpContext.Current.Server.MapPath(virtualPath);
            System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
            return fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
