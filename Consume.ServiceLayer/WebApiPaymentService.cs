using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Consume.ServiceLayer.Interface;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Interfaces;
using Framework.Exceptions;
using Thinktecture.IdentityModel.Client;

namespace Consume.ServiceLayer
{
    public class WebApiPaymentService : IWebApiPaymentService
    {
        public TokenStoreDto GetToken()
        {          
            var client = new OAuth2Client(new Uri(ConfigurationManager.AppSettings["PaymentUrlApi"] + "token"),
                ConfigurationManager.AppSettings["PaymentClient"], ConfigurationManager.AppSettings["PaymentSecretCode"]);          
            var response = client.RequestResourceOwnerPasswordAsync(ConfigurationManager.AppSettings["ProductKey"], ConfigurationManager.AppSettings["SecretKey"]).Result;
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

        public PaymentTransactionItemDto GetListTransaction(TransactionDto transactionDto)
        {
            var objToken = GetToken();
            var urlRewrite = ConfigurationManager.AppSettings["PaymentUrlApi"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<PaymentTransactionItemDto>("GetListTransaction", transactionDto,
                urlRewrite, objToken.AccessToken);
        }

        public TransactionInfoDto GetTransactionDetail(TransactionDetailDto transactionDetailDto)
        {
            var objToken = GetToken();
            var urlRewrite = ConfigurationManager.AppSettings["PaymentUrlApi"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<TransactionInfoDto>("GetTransactionDetail", transactionDetailDto,
                urlRewrite, objToken.AccessToken);
        }

        public GetPaymentInfoItemDto GetPaymentInfo(PaymentInfoApiDto paymentInfoDto)
        {
            var objToken = GetToken();
            var urlRewrite = ConfigurationManager.AppSettings["PaymentUrlApi"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<GetPaymentInfoItemDto>("GetPaymentInfo", paymentInfoDto,
                urlRewrite, objToken.AccessToken);
        }

        public SubscriptionPaymentDto GetSubscriptionPaymentStatus(PaymentInfoApiDto paymentInfoDto)
        {
            var objToken = GetToken();
            var urlRewrite = ConfigurationManager.AppSettings["PaymentUrlApi"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<SubscriptionPaymentDto>("GetSubscriptionPaymentStatus", paymentInfoDto,
                urlRewrite, objToken.AccessToken);
        }

        public GetPaymentInfoItemDto CancelRequest(PaymentInfoApiDto paymentInfoDto)
        {
            var objToken = GetToken();
            var urlRewrite = ConfigurationManager.AppSettings["PaymentUrlApi"] + "api/Common/";
            return SendMessageToWebApiAndReturnValue<GetPaymentInfoItemDto>("CancelRequest", paymentInfoDto,
                urlRewrite, objToken.AccessToken);
        }

        public  string WebApiBaseUrl { get; set; }

        public string WebApiUrl
        {
            get { return ConfigurationManager.AppSettings["PaymentUrlApi"] + "api/" + WebApiBaseUrl + "/"; }
        }

        private void SetClientHeader(HttpClient client, string baseAddressRewrite = "", string token = "")
        {
            if (!string.IsNullOrEmpty(baseAddressRewrite))
            {
                client.BaseAddress = new Uri(baseAddressRewrite);
            }
            else
            {
                client.BaseAddress = new Uri(WebApiUrl);
            }
           client.SetBearerToken(token);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private  TResult SendMessageToWebApiAndReturnValue<TResult>(
            string method, object data = null, string baseAddressRewrite = "", string token = "")
        {
            using (var client = new HttpClient())
            {
                SetClientHeader(client, baseAddressRewrite, token);
                var task = client.PostAsJsonAsync(method, data).ContinueWith(x => x.Result);
                task.Wait();

                var response = task.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    return response.Content.ReadAsAsync<TResult>().Result;
                }
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    FeedbackViewModel dataReturn = response.Content.ReadAsAsync<FeedbackViewModel>().Result;
                    throw new WebApiErrorException(dataReturn);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnAuthorizedAccessException("Unauthorized");
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new UnAuthorizedAccessException("ForbiddenAccess");
                }
                if (response.StatusCode == HttpStatusCode.ExpectationFailed)
                {
                    throw new UnAuthorizedAccessException("ExpectationFailed");
                }
            }
            return default(TResult);
        }
    }
}
