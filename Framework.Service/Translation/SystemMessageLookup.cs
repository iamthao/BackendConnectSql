namespace Framework.Service.Translation
{
    public class SystemMessageLookup
    {
        public static string GetMessage(string resourceKey)
        {
            switch (resourceKey)
            {
                case "ChangePasswordError":
                    return "Change password attempt failed ";
                case "ConcurrencyExceptionMessageText":
                    return "The data has been modified by another user. Please to reload the latest data.";
                case "NotExitExceptionMessageText":
                    return "The data don't exist. Please to reload the latest data.";
                case "UniqueConstraintErrorText":
                    return "{0} cannot contain value {1} as it is already in use";
                case "RecordInUseConstraintErrorText":
                    return "Unable to delete this record because some {0} are using it";
                case "RecordConstraintErrorText":
                    return "Unable to {0} this record because {1} records was deleted or removed";
                case "ValidationErrorText":
                    return "The following data validation errors have occurred and will need to be addressed before you can save this data.";
                case "GeneralExceptionMessageText":
                    return "An unexpected exception has occurred in the code, please contact your system administrator and pass on the information contained within this message.";
                case "RestorePasswordPageTitle":
                    return "Restore password";
                case "SignInPageTitle":
                    return "Sign in";
                case "InvalidUserAndPasswordText":
                    return "Username or password is incorrect.";
                case "FieldInvalidText":
                    return "The {0} is invalid.";
                case "RequiredTextResourceKey":
                    return "The {0} field is required";
                case "OrderPhysicianIsNotAvailable":
                    return "The {0} is not available";
                case "EmailInvalid":
                    return "The {0} is not a valid e-mail address.";
                case "EmailInvalidText":
                    return "your email is not a valid e-mail address.";
                case "ExistsTextResourceKey":
                    return "The {0} field already existed in the system.";
                case "BussinessGenericErrorMessageKey":
                    return "Following business rules failed:";
                case "RestorePasswordSuccessText":
                    return "Restore password successful. Please check your email to get information.";
                case "NoText":
                    return "No";
                case "YesText":
                    return "Yes";
                case "CreateText":
                    return "Create";
                case "UpdateText":
                    return "Update";
                case "CannotCopyText":
                    return "Cannot copy because you choose more than 1 item.";
                case "UpdateSuccessText":
                    return "Update success";
                case "CreateSuccessText":
                    return "Create success";
                case "DeleteSuccessText":
                    return "Delete success";
                case "MaxLengthRequied":
                    return "The {0} field must be a string with a maximum length of {1}.";
                case "EmailValid":
                    return "The {0} field is not a valid e-mail address.";
                case "PhoneValid":
                    return "The {0} field is not a valid phone number.";
                case "DateTimeValid":
                    return "The {0} field is not a valid date time.";
                case "SubjectToSendEmailForCreateUser":
                    return "Information login to Quickspatch.";
                case "UnAuthorizedAccessText":
                    return "Attempted to perform an unauthorized operation on this function.";
                case "ItemExistsWithParentItem":
                    return "The {0} {1} already existed with the {2} in the system.";
                case "DuplicateItemText":
                    return "There is duplicate '{0}' in system.";
                case "ActiveText":
                    return "Are you sure you want to active this record?";
                case "InactiveText":
                    return "Are you sure you want to inactive this record?";
                case "DeleteChildText":
                    return "You cannot delete this item without deleting its data.";
                case "InvalidAccessToken":
                    return "The access token is invalid.";
                case "Unauthorized":
                    return "Unauthorized";
                case "ForbiddenAccess":
                    return "Forbidden access";
                case "ExpectationFailed":
                    return "Expectation failed";
                case "DirtyDialogMessageText":
                    return "To prevent information loss, please save the changes before closing.";
                case "MustGreaterThan":
                    return "{0} must be greater than {1}";
                case "MustGreaterThanOrEqualTo":
                    return "{0} must be greater than or equal to {1}";
                case "MustGreaterThanNow":
                    return "{0} must be greater than now";
                case "ItemIsNotFound":
                    return "This record is not found. Please check it again!";
                case "CanDeleteAppRole":
                    return "You cannot delete app role.";
                case "DoBOverCurrentDate":
                    return "Date of birth must be before current date ";
                case "LackOfPracticeLocation":
                    return "At least 1 Practice Location must be added when choosing Multiple Locations";
                case "LoginWithInacticeUser":
                    return "Your account is locked. Please contact with administration to get more information.";
                case "LoginWithCourierUser":
                    return "You are is courier so you cannot login this website.";
                case "LoginWithInacticePractice":
                    return "This practice is currently inactive.";
                case "InacticeCourier":
                    return "This courier is currently inactive.";
                case "NotSelectCourierYet":
                    return "No courier is selected";
                case "ChoosePractice":
                    return "You must choose practice before access this page.";
                case "PasswordNotMatch":
                    return "The password is not match.";
                case "CurrentPasswordInvalid":
                    return "The current password is invalid. Please try agian.";
                case "InvalidDate":
                    return "Invalid {0} format. Must be MM/dd/yyyy";
                case "ConnectionStringInvalid":
                    return "Connection string is invalid.";
                case "InvalidLicenseKey":
                    return "The license key is invalid.";
                case "MustDifferentResourceKey":
                    return "{0} must be different {1}";
                case "ItemIsDeleted":
                    return "This {0} was deleted.";
                case "ExistsUserKey":
                    return "There is one {0} existing in system has this full name and this email.";
                case "RequiredSelectValue":
                    return "The {0} must be selected";
                case "CannotDeleteYourself":
                    return "You cannot delete yourself";
                case "SystemEventLogon":
                    return "{0}: \"{1}\" logged on";
                case "SystemEventLogOut":
                    return "{0}: \"{1}\" logged out";
                case "StatusRequestIsCompleted":
                    return "Cannot cancel request because it is completed by courier";
                case "MaximumOfCourierExceeded":
                    return "Maximum number of courier exceeded";
                case "RequestCannotUpdate":
                    return "Can't update this request because the status is {0}";
                case "NumberUserAllow":
                    return "Maximum number of courier exceeded. Please upgrade your plan.";//"Your package does not have enough active editor slots to create a new user/driver. Please upgrade your package.";
                case "ValueIsInteger":
                    return "The {0} must be an integer."; 
                case "LengthRequestNo":
                    return "The maximum length of {0} is three characters.";
                case "DeleteLocationDefault":
                    return "You can not delete this location, because it is a default location.";
                case "OutRangeRequest":
                    return "The request \"{0}\" is conflict with range from  \"Arrival Window From\" to \"Arrival Window To\" of this request, let choose range other.";
                case "OutRangeSchedule":
                    return "The shedule \"{0}\" is conflict with range from  \"Scheduled Departure Time\" to \"Scheduled Arrival Time\" of this schedule, let choose range other.";
                case "CourierIsDeleted":
                    return "This {0} was deleted. Please refresh the page or try again.";
                case "DeleteCourierHasRequest":
                    return "Unable to delete this record because this courier has active requests.";
                case "DeleteLocationHasRequest":
                    return "Unable to delete this record because this location has active requests.";
                case "FromDifferentTo":
                    return "The From Default must be different from the To Default.";
                case "ToDefaultFrom":
                    return "The To Default must be different from the From Default.";
            }
            return resourceKey;
        }
    }
}