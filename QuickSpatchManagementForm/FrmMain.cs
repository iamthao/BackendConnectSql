using System;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;
using Framework.Utility;

namespace QuickSpatchManagementForm
{
    public partial class FrmMain : Form
    {
        //readonly string _servicePath = AppDomain.CurrentDomain.BaseDirectory + @"\QuickSpatchWindowsService.exe";

        public FrmMain()
        {
            InitializeComponent();
            var serviceName = ConfigurationManager.AppSettings["ServiceName"];
            var servicePath = ConfigurationManager.AppSettings["ServicePath"];
            if (string.IsNullOrEmpty(serviceName) || string.IsNullOrEmpty(servicePath))
            {
                MessageBox.Show(@"Missing Configuration ServiceName and ServicePath", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            serviceControl.ServiceName = serviceName;
            serviceControl.ServicePath = servicePath;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
        }

        private void existToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void serviceControl_Load(object sender, EventArgs e)
        {

        }
    }
}
