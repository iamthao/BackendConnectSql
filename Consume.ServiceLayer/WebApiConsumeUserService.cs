using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Thinktecture.IdentityModel.Client;

namespace Consume.ServiceLayer
{
    public class WebApiConsumeUserService : WebApiConsumeMasterFileService<UserLoginForWebApiDto>, IWebApiConsumeUserService
    {

        public TokenStoreDto GetToken(FranchisseNameAndLicenseDto loginData)
        {
            var client = new OAuth2Client(new Uri(ConfigurationManager.AppSettings["WebApiUrl"] + "token"), ConfigurationManager.AppSettings["ClientId"], ConfigurationManager.AppSettings["SecretCode"]);
            var response = client.RequestResourceOwnerPasswordAsync(loginData.FranchiseeName, loginData.LicenseKey).Result;
            if (response.IsError)
            {
                return null;
            }
            return new TokenStoreDto
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken
            };
        }

        public ModuleForFranchiseeDto GetModuleForFranchisee(FranchisseNameAndLicenseDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<ModuleForFranchiseeDto>("GetModuleForFranchisee", franchiseeData,
                urlRewrite);
        }

        public ActiveDateLicenseKeyDto GetActiveDateLicenseKey(FranchisseNameAndLicenseDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<ActiveDateLicenseKeyDto>("GetActiveDateLicenseKey", franchiseeData,
                urlRewrite);
        }
        public  bool UpdateFranchiseeConfig(FranchiseeTernantDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<bool>("UpdateFranchiseeConfig", franchiseeData,
                urlRewrite);
        }

        public bool UpdateFranchiseeTenantCloseAccount(FranchiseeTernantCloseAccountDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<bool>("UpdateFranchiseeTenantCloseAccount", franchiseeData,
                urlRewrite);
        }

        public bool UpdateFranchiseeTenantCancelAccount(FranchisseNameAndLicenseDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<bool>("UpdateFranchiseeTenantCancelAccount", franchiseeData,
                urlRewrite);
        }
        public override string WebApiBaseUrl
        {
            get { return "User"; }
        }

        public bool IsExpireFranchisee(FranchisseNameAndLicenseDto franchisseNameAndLicenseDto)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<bool>("CheckIsExpire", franchisseNameAndLicenseDto,
                urlRewrite);
        }

        public FranchiseeTernantDto GetInfoFranchisee(FranchisseNameAndLicenseDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<FranchiseeTernantDto>("GetInfoFranchisee", franchiseeData,
                urlRewrite);
        }

        public bool AddPackageHistory(AddPackageHistoryDto packageHistoryInfo)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<bool>("AddPackageHistory", packageHistoryInfo,
                urlRewrite);
        }

        public bool FranchiseeTenantUpdatePayment(FranchiseeTenantUpdatePaymentDto franchiseeTenantUpdatePaymentDto)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<bool>("FranchiseeTenantUpdatePayment", franchiseeTenantUpdatePaymentDto,
                urlRewrite);
        }

        public PackageHistoryDto GetPackageCurrent(FranchisseNameAndLicenseDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<PackageHistoryDto>("GetPackageCurrent", franchiseeData,
                urlRewrite);
        }

        public List<PackageHistoryDto> GetListPackageChange(FranchisseNameAndLicenseDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<List<PackageHistoryDto>>("GetListPackageChange", franchiseeData,
                urlRewrite);
        }
        #region No Get Token
        public FranchiseeTernantCurrentPackageDto GetPackageCurrentId(FranchisseNameAndLicenseDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlRewrite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var task = client.PostAsJsonAsync("GetPackageCurrentId", franchiseeData).ContinueWith(x => x.Result);
                task.Wait();

                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    return response.Content.ReadAsAsync<FranchiseeTernantCurrentPackageDto>().Result;
                }
                
            }
            return null;
        }
        public int GetRequestCurrentId(FranchisseNameAndLicenseDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlRewrite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var task = client.PostAsJsonAsync("GetRequestCurrentId", franchiseeData).ContinueWith(x => x.Result);
                task.Wait();

                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    return response.Content.ReadAsAsync<int>().Result;
                }

            }
            return 0;
        }
        public bool UpdateFranchiseeTenantLicenceExtentsion(FranchisseNameAndLicenseDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlRewrite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var task = client.PostAsJsonAsync("UpdateFranchiseeTenantLicenceExtentsion", franchiseeData).ContinueWith(x => x.Result);
                task.Wait();

                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    return response.Content.ReadAsAsync<bool>().Result;
                }

            }
            return false;
        }
        public FranchiseeTernantDto GetInfoFranchiseeNoToken(FranchisseNameAndLicenseDto franchiseeData)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlRewrite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var task = client.PostAsJsonAsync("GetInfoFranchiseeNoToken", franchiseeData).ContinueWith(x => x.Result);
                task.Wait();

                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    return response.Content.ReadAsAsync<FranchiseeTernantDto>().Result;
                }

            }
            return null;
        }
        public PackageHistoryDto GetPackageCurrentNoToken(FranchisseNameAndLicenseDto franchiseeData)
        {
           var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlRewrite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var task = client.PostAsJsonAsync("GetPackageCurrentNoToken", franchiseeData).ContinueWith(x => x.Result);
                task.Wait();

                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    return response.Content.ReadAsAsync<PackageHistoryDto>().Result;
                }

            }
            return null;
        }
        public bool AddPackageHistoryNoToken(PackageHistoryDto packageHistoryInfo)
        {
            var urlRewrite = ConfigurationManager.AppSettings["WebApiUrl"] + "api/Common/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlRewrite);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var task = client.PostAsJsonAsync("AddPackageHistoryNoToken", packageHistoryInfo).ContinueWith(x => x.Result);
                task.Wait();

                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    return response.Content.ReadAsAsync<bool>().Result;
                }

            }
            return false;
        }
        #endregion
    }
}