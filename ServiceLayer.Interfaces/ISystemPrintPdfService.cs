using System;
using Framework.DomainModel.Entities.Common;

namespace ServiceLayer.Interfaces
{
    public interface ISystemPrintPdfService
    {
        string GetContentRequestReport(int courierId, DateTime fromDate, DateTime toDate, string displayName);
        string ExportDriverReport(string desPath, DriverReportQueryInfo queryInfo);
        string ExportDeliveryAgreementReport(string desPath, RequestQueryInfo queryInfo);
    }
}