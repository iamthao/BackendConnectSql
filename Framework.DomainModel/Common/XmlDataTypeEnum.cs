using System.ComponentModel;

namespace Framework.DomainModel.Common
{
    public enum InsuranceType
    {
        PrimaryInsurance = 1,
        SecondInsurance = 2,
    }

    public enum XmlDataTypeEnum
    {
        Gender = 1,
        Status = 2,
        StatusCourier = 3,
        EventMessage=4
    }

    public enum EventMessage
    {
        CourierLogin = 1,
        CourierLogout = 2,
        CourierOnline = 3,
        CourierOffline = 4,
        CourierAcceptedRequest = 5,
        CourierDeclinedRequest = 6,
        CourierStartedRequest = 7,
        CourierCompletedRequest = 8,
        RequestCreated = 9,
        RequestReassigned = 10,
        RequestCancelled = 11,
        RequestSent = 12,
        RequestAbandoned=13,
        RequestExpired = 14
    }

    public enum EventMessageParam
    {
        Courier = 1,
        Request = 2,
        FromCourier = 3,
        ToCourier = 4
    }

    public enum StatusRequest
    {
        Abandoned = 10,
        Cancelled = 20,
        Completed = 30,
        Declined = 40,
        Expired = 50,
        NotSent = 60,
        Received = 70,
        Sent = 80,
        Started = 90,
        Waiting = 100
    }

    public enum HistoryRequestActionType
    {
        Reassign = 1,
        Declined = 2,
        Completed = 3,
        Cancelled = 4,
        Expired = 5
    }
    public enum StatusCourier
    {
        Offline = 1,
        Online = 2,
        Idle = 3
    }

    public enum FilterOperator
    {
        contains = 0
    }

    public enum GoogleMapStatus
    {
        OK,
        NOT_FOUND,
        ZERO_RESULTS,
        MAX_WAYPOINTS_EXCEEDED,
        INVALID_REQUEST,
        OVER_QUERY_LIMIT,
        REQUEST_DENIED,
        UNKNOWN_ERROR
    }

    public enum ReportType
    {
        DeliveryAgreement = 1
    }

    public enum TemplateType
    {
        [Description("Single driver")]
        ReportOfDriver = 1,
        [Description("All Drivers' Report")]
        ReportOfAllDriver = 2,
    }
}
