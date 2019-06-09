using System;
using System.Configuration.Install;
using System.IO;
using System.Windows.Forms;
using System.ServiceProcess;


namespace QuickSpatchManagementForm.Control
{
    public partial class ServiceControl : UserControl
    {
        public ServiceControl()
        {
            InitializeComponent();
        }

        public string ServicePath{ get; set;}
        public string ServiceName { get; set; }

        private void ServiceControl_Load(object sender, EventArgs e)
        {
            UpdateControlsByServiceState(GetServiceControl(ServiceName));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!CheckFolderExists())
            {
                MessageBox.Show("Service file not found", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DisableAllControls();
            Cursor.Current = Cursors.WaitCursor;
            Exception ex = null;
            if (StartService(ref ex))
            {
                ShowControlsWithServiceStart();
            }
            else
            {
                MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowControlsWithServiceStop();
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!CheckFolderExists())
            {
                MessageBox.Show("Service file not found", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DisableAllControls();
            Cursor.Current = Cursors.WaitCursor;
            Exception ex = null;
            if (StopService(ref ex))
            {
                ShowControlsWithServiceStop();
            }
            else
            {
                MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowControlsWithServiceStart();
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnRestartService_Click(object sender, EventArgs e)
        {
            if (!CheckFolderExists())
            {
                MessageBox.Show("Service file not found", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DisableAllControls();
            Cursor.Current = Cursors.WaitCursor;
            Exception ex = null;
            if (!RestartService(ref ex))
            {
                MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ShowControlsWithServiceStart();
            Cursor.Current = Cursors.Default;
        }

        private bool StartService(ref Exception exRef)
        {
            try
            {
                ServiceController serviceController = GetServiceControl(ServiceName);
                if (serviceController == null)
                {
                    InstallService(ServicePath);
                    serviceController = GetServiceControl(ServiceName);
                }
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 5, 0));
                return true;
            }
            catch (Exception ex)
            {
                exRef = ex;
                return false;
            }
        }

        private bool StopService(ref Exception exRef)
        {
            try
            {
                ServiceController checkController = GetServiceControl(ServiceName);
                if (checkController != null)
                {
                    ServiceController controller = new ServiceController(ServiceName);

                    if (checkController.Status != ServiceControllerStatus.Stopped)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 5, 0));
                    }
                }
                UninstallService(ServicePath);
                return true;
            }
            catch (Exception ex)
            {
                exRef = ex;
                return false;
            }
        }

        private bool RestartService(ref Exception exRef)
        {
            try
            {
                ServiceController serviceController = GetServiceControl(ServiceName);
                if (serviceController != null)
                {
                    if (serviceController.Status != ServiceControllerStatus.Stopped)
                    {
                        serviceController.Stop();
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 5, 0));
                    }
                    serviceController.Start();
                    serviceController.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 5, 0));
                }
                return true;
            }
            catch (Exception ex)
            {
                exRef = ex;
                return false;
            }
        }

        private void InstallService(string servicePath)
        {
            ManagedInstallerClass.InstallHelper(new string[] { servicePath });
        }

        private void UninstallService(string servicePath)
        {
            ManagedInstallerClass.InstallHelper(new string[] { "/u", servicePath });
        }

        private void DisableAllControls()
        {
            btnStart.Enabled = false;
            btnStop.Enabled = false;
            btnRestartService.Enabled = false;
        }

        private void ShowControlsWithServiceStop()
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnRestartService.Enabled = false;
        }

        private void ShowControlsWithServiceStart()
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnRestartService.Enabled = true;
        }

        private void UpdateControlsByServiceState(ServiceController controller)
        {
            if (controller == null)
            {
                ShowControlsWithServiceStop();
            }
            else
            {
                btnStop.Enabled = (controller.Status == ServiceControllerStatus.Running && controller.CanStop);
                btnRestartService.Enabled = (controller.Status == ServiceControllerStatus.Running && controller.CanStop);
                btnStart.Enabled = controller.Status == ServiceControllerStatus.Stopped;
            }
        }

        public ServiceController GetServiceControl(string strServiceName)
        {
            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController controller in services)
            {
                if (controller.ServiceName.Equals(strServiceName))
                {
                    return controller;
                }
            }
            return null;
        }

        private bool CheckFolderExists()
        {
            if (File.Exists(ServicePath)) return true;

            var folderDialog = new FolderBrowserDialog
            {
                Description = "Select folder contain QuickSpatchWindowsService.exe"
            };

            if (folderDialog.ShowDialog() != DialogResult.OK) return false;

            ServicePath = folderDialog.SelectedPath + @"\QuickSpatchWindowsService.exe";
            return true;
        }
    }
}
