using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using TuesPechkin;

namespace ServiceLayer
{
    public class SystemPrintPdfServiceBAckup //: ISystemPrintPdfService
    {
        //private string _url = string.Empty;
        //private readonly string _tempPath = Path.GetTempPath();
        //private string _pathTempSaveFile;
        //private string _headerContent;
        //private string _bodyContent;
        //private string _footerContent;

        #region footer content
        private readonly string _footerContentFormat = @"<!DOCTYPE html>
                                        <html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
                                        <head>
                                            <meta charset='utf-8' />
                                            <title></title>
                                            <style>
                                                html, body {
                                                    padding: 0;
                                                    margin: 0;
                                                }

                                                table {
                                                    padding: 5px;
                                                    font-family: Calibri;
                                                    font-size: 14px;
                                                }

                                                h2, h3, p {
                                                    margin: 5px 0;
                                                }
                                            </style>
                                            <script>
                                            function subst() {
                                            var vars={};
                                            var x=window.location.search.substring(1).split('&');
                                            for (var i in x) {var z=x[i].split('=',2);vars[z[0]] = unescape(z[1]);}
                                            var x=['frompage','topage','page','webpage','section','subsection','subsubsection'];
                                            for (var i in x) {
                                                var y = document.getElementsByClassName(x[i]);
                                                for (var j=0; j<y.length; ++j) y[j].textContent = vars[x[i]];
                                            }
                                            }
                                            </script>
                                        </head>
                                        <body onload='subst()'>
                                            {0}
                                        </body>
                                        </html>";
        #endregion

        //private const string HtmlFormat = "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" + "<head>" + "<link href='/Content/bootstrap.css' rel='stylesheet'>" + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">" + "</head>" + "<body>{0}</body></html>";

        //private readonly IRequestRepository _requestRepository;

        //public SystemPrintPdfService(IRequestRepository requestRepository)
        //{
        //    _requestRepository = requestRepository;
        //    var pathExportFile = ConfigurationManager.AppSettings["ExportHtmlToPdfFile"];
        //    if (!File.Exists(pathExportFile))
        //    {
        //        throw new Exception("wkhtmltopdf does not install");
        //    }
        //}

        //public string _exportToPath(string path, string htmlContent, string idFooter = "footer", string idHeader = null, bool isLandscape = false, float? marginLeft = 5, float? marginRigth = 5,
        //    float? marginTop = 5, float? marginBottom = 18)
        //{
        //    //_setUrl();

        //    _handleHeaderBodyFooter(path, htmlContent, idFooter, idHeader);

        //    _export(marginLeft: marginLeft, marginRigth: marginRigth, marginTop: marginTop, marginBottom: marginBottom);

        //    if (File.Exists(_pathTempSaveFile))
        //    {
        //        return _pathTempSaveFile;
        //    }
        //    return null;
        //}

        ////set url for file
        //private void _setUrl()
        //{
        //    var result = "file:///";
        //    result += AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
        //    _url = result;
        //}

        //private void _handleHeaderBodyFooter(string path, string htmlContent, string idFooter, string idHeader = null)
        //{
        //    _pathTempSaveFile = path;
        //    var htmlDocumentHelper = new HtmlDocumentHelper(htmlContent);

        //    _headerContent = null;

        //    if (!string.IsNullOrEmpty(_url))
        //    {
        //        htmlDocumentHelper.MergeUrlOnImage(_url);
        //    }

        //    if (!string.IsNullOrEmpty(idFooter))
        //    {
        //        _footerContent = htmlDocumentHelper.GetContentById(idFooter);
        //    }

        //    _bodyContent = htmlDocumentHelper.RemoveContentById(idFooter);
        //}

        //private void _export(bool isLandscape = false, float? marginLeft = 5, float? marginRigth = 5,
        //    float? marginTop = 5, float? marginBottom = 18)
        //{
        //    using (var pdf = new ExportPdfProvider(_pathTempSaveFile))
        //    {
        //        pdf.ConfigPdfFile("A4", marginLeft, marginRigth, marginTop, marginBottom);
        //        _pathTempSaveFile = pdf.Export(_bodyContent, _headerContent, _footerContent, isLandscape);
        //    }
        //}

        //private string _getHtmlDeliveryAgreementReport(RequestQueryInfo queryInfo)
        //{
        //    var data = _requestRepository.GetListDeliveryAgreementVo(queryInfo);

        //    var html = TemplateHelpper.FormatTemplateWithContentTemplate(TemplateHelpper.ReadContentFromFile(TemplateConfigFile.DeliveryAgreementReportTemplate, true), new
        //    {
        //        data.RequestNo,
        //        data.Signature,
        //        data.RequestAgreed,
        //        data.RequestFrom,
        //        data.RequestTo,
        //        data.RequestTimes,
        //        data.RequestDistance,
        //        data.RequestFromName,
        //        data.RequestToName
        //    });
        //    return string.Format(HtmlFormat, html);
        //}

        ////implement
        //public string ExportDeliveryAgreementReport(string desPath, RequestQueryInfo queryInfo)
        //{
        //    var html = _getHtmlDeliveryAgreementReport(queryInfo);
             
        //    return _exportToPath(desPath, html);
        //}
    }
}