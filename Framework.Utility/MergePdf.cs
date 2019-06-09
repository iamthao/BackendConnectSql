using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;

namespace Framework.Utility
{
    public static class MergePdf
    {
        public static byte[] CombineMultiplePdfsByByte(this List<byte[]> byteFiles)
        {
            var document = new Document(PageSize.A4, 0, 0, 0, 0);
            var readerList = new List<PdfReader>();
            var pathOutFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
            byte[] result = null;
            try
            {
                foreach (var byteFile in byteFiles)
                {
                    var pdfReader = new PdfReader(byteFile);
                    readerList.Add(pdfReader);
                }

                //Define a new output document and its size, type

                //Create blank output pdf file and get the stream to write on it.

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pathOutFile, FileMode.Create));
                document.Open();

                foreach (PdfReader reader in readerList)
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        document.Add(Image.GetInstance(page));
                    }
                }
                document.Close();
                foreach (PdfReader reader in readerList)
                {
                    reader.Close();
                    reader.Dispose();
                }
                result = File.ReadAllBytes(pathOutFile);
            }
            catch (Exception ex)
            {
                //PathTempAppHelper.WriteLog("CombineMultiplePdfs: " + ex.Message);
                return null;
            }
            finally
            {
                File.Delete(pathOutFile);
            }
            return result;
        }

        public static void CombineMultiplePdfsByByteAndExport(this List<byte[]> byteFiles, string desPath)
        {
            var document = new Document(PageSize.A4, 0, 0, 0, 0);
            var readerList = new List<PdfReader>();
            var pathOutFile = desPath;
            try
            {
                readerList.AddRange(byteFiles.Select(byteFile => new PdfReader(byteFile)));

                //Define a new output document and its size, type

                //Create blank output pdf file and get the stream to write on it.

                var writer = PdfWriter.GetInstance(document, new FileStream(pathOutFile, FileMode.Create));
                document.Open();

                foreach (var reader in readerList)
                {
                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        var page = writer.GetImportedPage(reader, i);
                        document.Add(Image.GetInstance(page));
                    }
                }
                document.Close();
                foreach (var reader in readerList)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                //PathTempAppHelper.WriteLog("CombineMultiplePdfs: " + ex.Message);
                //return null;
            }

        }
    }

}
