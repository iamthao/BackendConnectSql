(function () {
    'use strict';

    var app = angular.module('app');
    var messageLanguage = {
        fillInUserAndPass: 'Please fill in username and password',
        createUser: 'Create user',
        createUserSuccess: 'Create user successfully',
        enterCurrentPass: 'Enter current password',
        enterNewPass: 'Enter new password',
        passIsNotMatch: '- Confirm new password does not match with New password',
        changePassSuccess: 'Your password has been changed successfully',
        updateProfile: 'Update profile',
        updateProfileSuccess: 'Update profile successfully',
        updateUser: 'Update user',
        updateUserSuccess: 'Update user successfully',
        updateFranchiseeConfigSuccess: 'Update franchisee configuration successfully',
        listUser: 'Show list user',
        userProfile: 'User profile',
        createUserRole: 'Create user role.',
        createUserRoleSuccess: 'Create user role successfully.',
        updateUserRole: 'Update user role',
        updateUserRoleSuccess: 'Update user role successfully',
        listUserRole: "Show list user role",
        deleteUserRoleSuccess: "Delete user role successfully",
        resetPasswordUser: "Reset password",
        resetPasswordUserSuccess: "Reset password successfully",
        comparePassword: "- New Password does not match the Confirm New Password field.",
        currentpasswordrequired: "- The Current Password field is required.",
        currentpasswordinvalid: "- The Current Password field is invalid. Please try agian.",
        passwordrequired: "- The New Password field is required.",
        rePasswordrequired: "- The Confirm New Password field is required.",
        passwordLenght: "-The New Password must be more than 6 characters.",
        rePasswordLenght: "- Confirm New Password must be more than 6 characters.",
        changePasswordProfile: "Change password",
        editProfile: "Edit profile",
        editAvatar: "Edit avatar",
        //Module
        listModule: 'Show list module',
        createModule: 'Create module',
        createModuleSuccess: 'Create module successfully',
        updateModule: 'Update module',
        updateModuleSuccess: 'Update module successfully',

        //Franchisee
        listFranchisee: 'Show list franchisee',
        createFranchisee: 'Create franchisee',
        createFranchiseeSuccess: 'Create franchisee successfully',
        updateFranchisee: 'Update franchisee',
        updateFranchiseeSuccess: 'Update franchisee successfully',

        //Franchisee module
        updateFranchiseeModule: 'Setup franchisee module',
        updateFranchiseeModuleSuccess: 'Setup franchisee module successfully',

        //Set module operation
        updateModuleOperation: 'Setup franchisee module operation',
        updateFranchiseeModuleOperationSuccess: 'Setup franchisee module operation successfully',

        //Franchisee
        listCustomer: 'Show list customer',
        createCustomer: 'Create customer',
        createCustomerSuccess: 'Create customer successfully',
        updateCustomer: 'Update customer',
        updateCustomerSuccess: 'Update customer successfully',

        //location
        listLocation: 'Show list location',
        createLocation: 'Create location',
        createLocationSuccess: 'Create location successfully',
        updateLocation: 'Update location',
        updateLocationSuccess: 'Update location successfully.',

        //Courier
        //listCourier: 'Show list ' + $rootScope.CourierDisplayName,
        //createCourier: 'Create ' + $rootScope.CourierDisplayName,
        //createCourierSuccess: 'Create ' + $rootScope.CourierDisplayName + ' successfully',
        //updateCourier: 'Update ' + $rootScope.CourierDisplayName,
        //updateCourierSuccess: 'Update ' + $rootScope.CourierDisplayName +' successfully',


        //Holding request
        createHoldingRequestSuccess: 'Create holding request successfully',
        updateHoldingRequestSuccess: 'Update holding request successfully',
        //Request

        createRequestSuccess: 'Create request successfully',
        updateRequestSuccess: 'Update request successfully',

        //reassign Courier Success
        reassignCourierSuccess: 'Re-assign courier successfully',

        //reassign Courier Fail
        reassignCourierFail: 'Re-assign courier fail',

        //reassign Courier Fail
        UnableDetermineCourier: 'Unable to determine the position of courier',

        //tracking
        showTracking: 'Show tracking',

        //schedule
        createSchedule: 'Create route',
        createScheduleSuccess: 'Create route successfully',
        updateSchedule: 'Update route',
        updateScheduleSuccess: 'Update route successfully',

        //close Success
        closeAcountSuccess: 'Close Account successfully',
        closeAccountError: 'The error occurred in the system, please press F5 to try again or contact the system administrator',
        cancelSuccess: 'Reopen Account successfully',
        cancelAccountError: 'The error occurred in the system, please press F5 to try again or contact the system administrator',
        messReopenAcountPackage: 'You must reopen account to change package.',
        messReopenAcount: 'You must reopen account to change information.',
        rePasswordCloseAccountRequired: "The Confirm Password field is required.",
        incorrectPasswordCloseAccount: "The Password is incorrect.",

        //Report
        driverReportError: 'The Driver field is required.',
        fromReportError: 'The From Date field is required.',
        fromReportErrorNaN: 'The From Date field is invalid.',
        toReportError: 'The To Date field is required.',
        toReportErrorNaN: 'The To Date field is invalid.',
        toGreaterThanFrom: 'The To Date must be greater than From Date.',

        //Change Package Billing    
        sameCurrentPackage: 'You are using this package. Please choose another package.',
        noExistTransaction: 'Current transaction has not completed.',
        messNotApplied: 'Your choose package less than current package, so you can not change until new package is applied.',
        messPackageNextApply: 'This package will be appiled when current package expire.',
        closeAccountRequesrCanceled: 'Cuurent transaction was canceled because you can not reopen account.',

        //contact
        createContactSuccess: 'Create contact successfully.',
        updateContactSuccess: 'Update contact successfully.',

        //locationdefault   
        compareLocationDefault: 'The From must be different from the To.',

        //Template
        createTemplateSuccess: 'Create template successfully.',
        updateTemplateSuccess: 'Update template successfully.',

        //SystemConfiguration
        createSystemConfigurationSuccess: 'Create configuration successfully.',
        updateTSystemConfigurationSuccess: 'Update configuration successfully.',

        //contact us
        fullNameRequired: 'The Full Name field is required.',
        emailRequired: 'The Email field is required.',
        emailInvalid: 'The Email field is not a valid e-mail address.',
        subjectRequired: 'The Subject field is required.',
        contentRequired: 'The Content field is required.',
        sendMessageSuccessfully: 'Your message was sent successfully.'
    };
    app.value('messageLanguage', messageLanguage);
})();