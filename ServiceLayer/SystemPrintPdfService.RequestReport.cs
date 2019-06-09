using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities.Common;
using Framework.Utility;

namespace ServiceLayer
{
    public partial class SystemPrintPdfService
    {
        public string GetContentRequestReport(int courierId, DateTime fromDate, DateTime toDate, string displayName)
        {
            var data = _requestRepository.GetListRequestForReport(courierId, fromDate, toDate);
            var listContent = "";

            double totalDistance = 0;

            if (data.Count > 0)
            {
                listContent +=
                    "<table width='100%' border-collapse: collapse; border: 1px solid #ddd;' cellspacing='0' cellpadding='0'>";
                listContent +=
                    " <tr ><th style='padding: 10px; border: 1px solid #ddd;background: #ccc;'>Request #</th>";
                if (courierId == 0)
                {
                    listContent +=
                        " <th style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;border-top: 1px solid #ddd;background: #ccc;'>Name</th>";
                }
                listContent +=
                    " <th style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;border-top: 1px solid #ddd;background: #ccc;'>From</th>";
                listContent +=
                    " <th style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;border-top: 1px solid #ddd;background: #ccc;'>To</th>";
                listContent +=
                    " <th style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;border-top: 1px solid #ddd;background: #ccc;'>Request Date</th>";
                listContent +=
                    " <th style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;border-top: 1px solid #ddd;background: #ccc;'>Estimate Time (hours)</th>";
                listContent +=
                    " <th style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;border-top: 1px solid #ddd;background: #ccc;'>Real Time (hours)</th>";
                listContent +=
                    " <th style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;border-top: 1px solid #ddd;background: #ccc;'>Estimate Distance (miles)</th>";
                listContent +=
                    " <th style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;border-top: 1px solid #ddd;background: #ccc;'>Real Distance (miles)</th>";
                listContent += "</tr>";

                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        totalDistance = totalDistance + item.ActualDistance.MetersToMiles(2);
                        listContent +=
                            " <tr ><td style='padding: 10px;border-left: 1px solid #ddd; border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;'>" +
                            item.RequestNo + "</td>";
                        if (courierId == 0)
                        {
                            listContent +=
                                "<td style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;'>" +
                                item.FullName + "</td>";
                        }

                        listContent +=
                            "<td style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;'>" +
                            item.LocationFrom + "</td>";
                        listContent +=
                            "<td style='padding: 10px;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;'>" +
                            item.LocationTo + "</td>";
                        listContent +=
                            "<td style='padding: 10px;text-align:right;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;' >" +
                           item.RequestDateFormat + "</td>";
                        listContent +=
                            "<td style='padding: 10px;text-align:right;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;'>" +
                            item.EstimateTime + "</td>";
                        listContent +=
                            "<td style='padding: 10px;text-align:right;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;'>" +
                            item.RealTime + "</td>";
                        listContent +=
                            "<td style='padding: 10px;text-align:right;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;'>" +
                            item.TextEstimateDistance + "</td>";
                        listContent +=
                            "<td style='padding: 10px;text-align:right;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;'>" +
                            item.TextActualDistance + "</td>";
                        listContent += "</tr>";
                    }
                }

                if (courierId == 0)
                {
                    listContent +=
                        "<tr ><th style='padding: 10px;text-align: right;border-left: 1px solid #ddd; border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;' colspan='8'>Total: </th>";
                }
                else
                {
                    listContent +=
                        "<tr ><th style='padding: 10px;text-align: right;border-left: 1px solid #ddd; border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;' colspan='7'>Total: </th>";
                }
                listContent +=
                    "<th style='padding: 10px; text-align:right;border-bottom: 1px solid #ddd;border-right: 1px solid #ddd;'>" +
                    String.Format("{0:0.00}", totalDistance) + " miles</th>";
                listContent += " </tr>";

                listContent += " </table>";
            }
            else
            {
                listContent += "There isn't request in list.";
            }
            //tao file logo temp from byte[]
            var pathDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileUpload");
            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }
            pathDir = Path.Combine(pathDir, "Temps");
            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }

            var franchisee = _franchiseeConfigurationRepository.GetFranchiseeConfiguration();
            //var logoFranchisee = new FileContentResult(franchisee.Logo, "image/jpeg");
            var fileName = Guid.NewGuid().ToString("N") + ".jpg";
            _pathTempFile = Path.Combine(pathDir, fileName);
            File.WriteAllBytes(_pathTempFile, franchisee.Logo);

            if (courierId == 0)
            {
                var reportTemplate1 = _templateRepository.FirstOrDefault(o => o.TemplateType == (int)TemplateType.ReportOfAllDriver);
                var html1 = TemplateHelpper.FormatTemplateWithContentTemplate(WebUtility.HtmlDecode(reportTemplate1.Content), new
                {
                    nameDisplay = displayName,
                    logo = fileName,
                    reportDate = DateTime.Now.ToShortDateString(),
                    fromDate = fromDate.ToShortDateString(),
                    toDate = toDate.ToShortDateString(),

                    listData = listContent

                });

                return html1;
            }

            //var displayNameCourier = 
            var courierInfo = _courierRepository.GetById(courierId);
            var reportTemplate = _templateRepository.FirstOrDefault(o => o.TemplateType == (int)TemplateType.ReportOfDriver);
            var html = TemplateHelpper.FormatTemplateWithContentTemplate(WebUtility.HtmlDecode(reportTemplate.Content), new
            {
                nameDisplay = displayName,
                logo = fileName,
                reportDate = DateTime.Now.ToShortDateString(),
                fromDate = fromDate.ToShortDateString(),
                toDate = toDate.ToShortDateString(),

                name = CaculatorHelper.GetFullName(courierInfo.User.FirstName, courierInfo.User.MiddleName, courierInfo.User.LastName),
                email = courierInfo.User.Email,
                homePhone = courierInfo.User.HomePhone.ApplyFormatPhone(),
                mobliePhone = courierInfo.User.MobilePhone.ApplyFormatPhone(),
                listData = listContent               
            });

            return html;

            //return "NO DATA";
        }

        public string ExportDriverReport(string desPath, DriverReportQueryInfo queryInfo)
        {
            var html = string.Format(htmlFormat, GetContentRequestReport(queryInfo.CourierId, queryInfo.FromDate, queryInfo.ToDate,queryInfo.DisplayName));
            var resultPath = _exportToPath(desPath, html);
            var resultByte = FileHelper.GetBytesFromFile(resultPath);

            var signeds = new List<byte[]>
            {
                resultByte
            };

            signeds.CombineMultiplePdfsByByteAndExport(resultPath);

            return resultPath;
        }

    }
}
