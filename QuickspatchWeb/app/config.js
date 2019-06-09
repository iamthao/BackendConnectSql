﻿(function () {
    'use strict';

    var app = angular.module('app');

    // Configure Toastr
    toastr.options.timeOut = 4000;
    toastr.options.positionClass = 'toast-bottom-right';
    toastr.options.extendedTimeOut = 1000;

    // For use with the HotTowel-Angular-Breeze add-on that uses Breeze
    var remoteServiceName = 'breeze/Breeze';

    var events = {
        controllerActivateSuccess: 'controller.activateSuccess',
        spinnerToggle: 'spinner.toggle',
        controllerFormSaveDataEvent: 'controller.formSaveDataEvent',
        controllerFormCancelSaveDataEvent: 'controller.formCancelDataEvent',
    };

    var config = {
        appErrorPrefix: '[HT Error] ', //Configure the exceptionHandler decorator
        docTitle: 'Template: ',
        events: events,
        remoteServiceName: remoteServiceName,
        version: '1.0.0'
    };


  


    app.value('config', config);

    app.config(['$logProvider', function ($logProvider) {
        // turn debugging off/on (no info or warn)
        if ($logProvider.debugEnabled) {
            $logProvider.debugEnabled(true);
        }
    }]);

    //#region Configure the common services via commonConfig
    app.config(['commonConfigProvider', function (cfg) {
        cfg.config.controllerActivateSuccessEvent = config.events.controllerActivateSuccess;
        cfg.config.spinnerToggleEvent = config.events.spinnerToggle;
        cfg.config.controllerFormSaveDataEvent = config.events.controllerFormSaveDataEvent;
        cfg.config.controllerFormCancelSaveDataEvent = config.events.controllerFormCancelSaveDataEvent;
    }]);
    //#endregion
})();