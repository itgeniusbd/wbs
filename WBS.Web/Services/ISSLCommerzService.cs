using WBS.Web.Models;

namespace WBS.Web.Services
{
    public interface ISSLCommerzService
    {
        Task<SSLCommerzResponse> InitiatePaymentAsync(SSLCommerzRequest request);
        Task<SSLCommerzValidationResponse> ValidatePaymentAsync(string validationId);
        Task<bool> ValidateIPNAsync(SSLCommerzIPNRequest ipnData);
    }
}
