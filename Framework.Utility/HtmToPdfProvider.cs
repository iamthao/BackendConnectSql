using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Text;

namespace Framework.Utility
{
    public class ExportPdfProvider : IDisposable
    {
        private readonly string _pathApplicationExport;
        private readonly string _destinationSaveFilePdf;

        private string _contentHtml;

        private string _pathHeaderTemp;
        private string _pathFooterTemp;

        private string _argument;
        private string _pageSize;
        private string _marginLeft;
        private string _marginRigth;
        private string _marginTop;
        private string _marginBottom;

        private string _header;
        private string _footer;

        private bool _landscape = false;

        public ExportPdfProvider(string destinationSaveFilePdf)
        {
            _pathApplicationExport = ConfigurationManager.AppSettings["ExportHtmlToPdfFile"];
            _destinationSaveFilePdf = destinationSaveFilePdf;
        }

        public string Export(string contentHtml, string headerHtml, string footerHtml, bool landscape = false)
        {
            _landscape = landscape;
            SetHeader(headerHtml);
            SetFooter(footerHtml);
            SetContent(contentHtml);
            SetArgumentForExport();
            WritePdf();
            return File.Exists(_destinationSaveFilePdf) ? _destinationSaveFilePdf : null;
        }

        #region Configuration Export

        /// <summary>
        /// Set page setup
        /// </summary>
        /// <param name="pageSize">Defaul "A4"</param>
        /// <param name="marginLeft">float type: margin Left page</param>
        /// <param name="marginRigth">float type: margin Rigth page</param>
        /// <param name="marginTop">float type: margin Top page</param>
        /// <param name="marginBottom">float type: margin Bottom page, if you set footer, set margin bottom > 20.0</param>
        public void ConfigPdfFile(string pageSize = "A4", float? marginLeft = null, float? marginRigth = null,
            float? marginTop = null, float? marginBottom = null)
        {
            _pageSize = string.Format(" --page-size {0} ", pageSize);
            _marginLeft = marginLeft == null ? "" : string.Format(" --margin-left {0}mm ", marginLeft);
            _marginRigth = marginRigth == null ? "" : string.Format(" --margin-right {0}mm ", marginRigth);
            _marginTop = marginTop == null ? "" : string.Format(" --margin-top {0}mm ", marginTop);
            _marginBottom = marginBottom == null ? "" : string.Format(" --margin-bottom {0}mm ", marginBottom);
        }

        private void SetArgumentForExport()
        {
            _argument = " --encoding utf-8 " + (_landscape ? " --orientation Landscape " : "") + _pageSize + _marginLeft + _marginRigth + _marginTop + _marginBottom + _header + _footer;
        }

        /// <summary>
        /// Write content Html footer to temp file and set configuration
        /// </summary>
        /// <param name="footerHtml"></param>
        public void SetFooter(string footerHtml)
        {
            _pathFooterTemp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString().Replace("-", "") + ".html");
            _footer = WriteHtml(_pathFooterTemp, footerHtml)
                ? " --footer-html " + _pathFooterTemp.Replace("\\", "/") + " "
                : "";
            //_footer = WriteHtml(_pathFooterTemp, footerHtml) ? " --header-html " + _pathFooterTemp.Replace("\\", "/") + " " : "";
        }

        /// <summary>
        /// Write content Html header to temp file and set configuration
        /// </summary>
        /// <param name="headerHtml"></param>
        public void SetHeader(string headerHtml)
        {
            if (!string.IsNullOrEmpty(headerHtml))
            {
                _pathHeaderTemp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString().Replace("-", "") + ".html");
                _header = WriteHtml(_pathHeaderTemp, headerHtml)
                    ? " --header-spacing 3 --header-html " + _pathHeaderTemp.Replace("\\", "/") + " "
                    : "";
            }
            else
            {
                _header = "";
            }

        }

        public void SetContent(string contentHtml)
        {
            _contentHtml = contentHtml;
        }

        private static bool WriteHtml(string path, string content)
        {
            try
            {
                using (var sw = new StreamWriter(path, false))
                {
                    sw.Write(content);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion

        private void WritePdf()
        {
            //var gc = new GlobalConfig()
            //    .SetPaperSize(PaperKind.A4)
            //    .SetPaperOrientation(false);

            ////var oc = new ObjectConfig()
            ////    .SetLoadImages(true).SetZoomFactor(1.0)
            ////    .SetPrintBackground(true)
            ////    .SetScreenMediaType(true)
            ////    .SetCreateExternalLinks(true)
            ////    .SetAllowLocalContent(true);

            //var pechkin = new SynchronizedPechkin(gc);

            //byte[] bytes = pechkin.Convert(_contentHtml);

            //File.WriteAllBytes(_destinationSaveFilePdf, bytes);


            var psi = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = _pathApplicationExport,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = _argument + "-q -n - " + _destinationSaveFilePdf
            };

            // run the conversion utility


            // note that we tell wkhtmltopdf to be quiet and not run scripts
            // NOTE: I couldn't figure out a way to get both stdin and stdout redirected so we have to write to a file and then clean up afterwardspsi.Arguments = _argument + "-q -n - " + _destinationSaveFilePdf;
            var p = Process.Start(psi);

            try
            {
                if (p != null)
                {
                    var stdin = new StreamWriter(p.StandardInput.BaseStream, Encoding.UTF8) { AutoFlush = true };
                    //var stdin = p.StandardInput;

                    stdin.Write(_contentHtml);

                    stdin.Close();
                }

                if (p != null && p.WaitForExit(30000))
                {

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (File.Exists(_pathFooterTemp))
                {
                    File.Delete(_pathFooterTemp);
                }
                if (p != null)
                {
                    p.Close();
                    p.Dispose();
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
