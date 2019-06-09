using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.DataTransferObject;

namespace Consume.ServiceLayer.Interface
{
    public interface IWebApiPaymentService
    {
        TokenStoreDto GetToken();
        PaymentTransactionItemDto GetListTransaction(TransactionDto transactionDto);
        GetPaymentInfoItemDto GetPaymentInfo(PaymentInfoApiDto paymentInfoDto);
        GetPaymentInfoItemDto CancelRequest(PaymentInfoApiDto paymentInfoDto);
        TransactionInfoDto GetTransactionDetail(TransactionDetailDto transactionDetailDto);
        SubscriptionPaymentDto GetSubscriptionPaymentStatus(PaymentInfoApiDto paymentInfoDto);
    }
}
