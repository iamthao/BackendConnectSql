﻿namespace Framework.DomainModel.DataTransferObject
{
    public enum FeedbackStatus
    {
        Success,
        Error,
        Critical, //user cannot do any action, should redirect user to error page
    }
}