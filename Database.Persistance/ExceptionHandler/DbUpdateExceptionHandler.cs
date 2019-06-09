using System;
using System.Text.RegularExpressions;
using Framework.Service.Translation;

namespace Database.Persistance.ExceptionHandler
{
    public static class DbUpdateExceptionHandler
    {
        public static object[] Process(Exception ex)
        {
            var values = new object[2];

            if (((ex.InnerException).InnerException).Message.Contains("UNIQUE"))
            {
                var message = ((System.Data.SqlClient.SqlException)(ex.InnerException.InnerException)).Errors[0].Message;
                var messageSplit = message.Split('\'');
                var propertyName = messageSplit[1].Split('_')[2];

                var duplicateValue = string.Format("'{0}'", Regex.Match(messageSplit[4], @"\((.*?)\)").Groups[1].ToString().Split(',')[0]);
                var returnValue = string.Format(SystemMessageLookup.GetMessage("UniqueConstraintErrorText"), propertyName, duplicateValue);

                values[0] = returnValue;
            }
            else if (((ex.InnerException).InnerException).Message.Contains("DELETE statement conflicted with the REFERENCE constraint"))
            {
                var message = ((System.Data.SqlClient.SqlException)(ex.InnerException.InnerException)).Errors[0].Message;
                var messageSplit = message.Split(',');

                var tablename = Regex.Match(messageSplit[1], "\"[^\"]*\"").Groups[0].Value.Replace("dbo.","");
                var returnValue = string.Format(SystemMessageLookup.GetMessage("RecordInUseConstraintErrorText"), tablename);

                values[0] = returnValue;
            }
            else if (((ex.InnerException).InnerException).Message.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint"))
            {
                var message = ((System.Data.SqlClient.SqlException)(ex.InnerException.InnerException)).Errors[0].Message;
                var messageSplit = message.Split(',');

                var tablename = Regex.Match(messageSplit[1], "\"[^\"]*\"").Groups[0].Value.Replace("dbo.", "");
                var returnValue = string.Format(SystemMessageLookup.GetMessage("RecordConstraintErrorText"), "INSERT",  tablename);

                values[0] = returnValue;
            }

            else if (((ex.InnerException).InnerException).Message.Contains("The UPDATE statement conflicted with the FOREIGN KEY constraint"))
            {
                var message = ((System.Data.SqlClient.SqlException)(ex.InnerException.InnerException)).Errors[0].Message;
                var messageSplit = message.Split(',');

                var tablename = Regex.Match(messageSplit[1], "\"[^\"]*\"").Groups[0].Value.Replace("dbo.", "");
                var returnValue = string.Format(SystemMessageLookup.GetMessage("RecordConstraintErrorText"), "UPDATE", tablename);

                values[0] = returnValue;
            }
            else
            {
                values[0] = ex.Message;
                values[1] = ex.InnerException.InnerException;
            }

            return values;
        }
    }
}