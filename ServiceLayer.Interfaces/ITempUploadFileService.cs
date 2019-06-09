
using System.Web;

namespace ServiceLayer.Interfaces
{
    public interface ITempUploadFileService
    {
        string SaveFile(HttpPostedFileBase file);

        void RemoveFile(string file);

        string FilePath { get; set; }
    }
}