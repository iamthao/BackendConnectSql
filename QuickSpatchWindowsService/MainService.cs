using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Autofac;
using Framework.Service.Diagnostics;
using ServiceLayer.Interfaces;

namespace QuickSpatchWindowsService
{
    public partial class MainService : ServiceBase
    {
        private System.Threading.Timer _callRequestTimer;
        private System.Threading.Timer _courierStatusTimer;
        private System.Threading.Timer _requestStatusTimer;

        private void WaitConnnectDatabaseAllready()
        {
            using (ILifetimeScope scope = Program.Container.BeginLifetimeScope())
            {
                bool dbConnected = false;
                var scheduleService = Program.Container.Resolve<IScheduleService>();
                var diagnosticService = Program.Container.Resolve<IDiagnosticService>();

                while (!dbConnected)
                {
                    try
                    {
                        var data = scheduleService.ListAll().Count;
                        dbConnected = true;
                    }
                    catch (Exception ex)
                    {
                        diagnosticService.Error(ex);
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var diagnosticService = Program.Container.Resolve<IDiagnosticService>();
            Thread.Sleep(20000);
            diagnosticService.Info("Service start");
            try
            {
                WaitConnnectDatabaseAllready();
                var tsInterval = new TimeSpan(0, 0, 30);

                #region backgroundWorker call request
                _callRequestTimer = new System.Threading.Timer(new System.Threading.TimerCallback(Job.SendRequestJob.SendRequestJobThread), null, tsInterval, tsInterval);
                #endregion

                #region backgroundWorker courier status
                _courierStatusTimer = new System.Threading.Timer(new System.Threading.TimerCallback(Job.CourierStatusJob.CourierStatusThread), null, tsInterval, tsInterval);
                #endregion

                #region backgroundWorker request status
                _requestStatusTimer = new System.Threading.Timer(new System.Threading.TimerCallback(Job.RequestStatusJob.RequestStatusThread), null, tsInterval, tsInterval);
                #endregion
            }
            catch (Exception ex)
            {
                diagnosticService.Error(ex);
            }
            
        }

        protected override void OnStop()
        {
            var diagnosticService = Program.Container.Resolve<IDiagnosticService>();
            diagnosticService.Info("Service stop");

            if (_callRequestTimer != null)
            {
                _callRequestTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                _callRequestTimer.Dispose();
                _callRequestTimer = null;
            }

            if (_courierStatusTimer != null)
            {
                _courierStatusTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                _courierStatusTimer.Dispose();
                _courierStatusTimer = null;
            }
            if (_requestStatusTimer != null)
            {
                _requestStatusTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                _requestStatusTimer.Dispose();
                _requestStatusTimer = null;
            } 
            base.OnStop();
        }
    }
}
