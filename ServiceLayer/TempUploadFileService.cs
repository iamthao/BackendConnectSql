using System;
using System.IO;
using System.Web;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class TempUploadFileService : ITempUploadFileService
    {
        public string SaveFile(HttpPostedFileBase file)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = HttpContext.Current.Server.MapPath(FilePath);
            var filePath = Path.Combine(path, fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            file.SaveAs(filePath);
            return FilePath + fileName;
        }

        public void RemoveFile(string file)
        {
            if (!String.IsNullOrEmpty(file))
            {
                file = HttpContext.Current.Server.MapPath(file);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        public string FilePath { get; set; }
    }
}