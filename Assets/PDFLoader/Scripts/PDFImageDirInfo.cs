using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
namespace uPDFLoader
{
    /// <summary>
    /// PDFを画像化した際に作られるディレクトリと画像Arrayの情報
    /// </summary>
    public class PDFImageDirInfo
    {
        public string[] imageFileURIs;
        public int fileCount { get { return imageFileURIs.Length; } }
        public PDFImageDirInfo(string dirPath)
        {
            var files = Directory.GetFiles(dirPath, "*.jpg", SearchOption.AllDirectories);
            files = files.OrderBy(s=>s.Length).ToArray(); 
            imageFileURIs = files;
        }
    }
}