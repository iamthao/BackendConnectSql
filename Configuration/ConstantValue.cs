using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigValues
{
    public class ConstantValue
    {
        public static DeploymentMode DeploymentMode
        {
            get
            {
                var deploymentMode = ConfigurationManager.AppSettings["DeploymentMode"];
                if (deploymentMode == "franchisee")
                {
                    return DeploymentMode.Franchisee;
                }
                return DeploymentMode.Camino;
            }
        }

        //public static string FranchiseeName
        //{
        //    get { return ConfigurationManager.AppSettings["FranchiseeName"]; }
        //}

        //public static string LicenseKey
        //{
        //    get { return ConfigurationManager.AppSettings["LicenseKey"]; }
        //}

        public static string ConnectionStringAdminDb
        {
            get { return "AdminDb"; }
        }

        public static string ConnectionStringFranchiseeDb
        {
            get { return "FranchiseeDb"; }
        }
    }
}
