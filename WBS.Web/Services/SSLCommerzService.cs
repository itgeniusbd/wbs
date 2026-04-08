using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WBS.Web.Models;

namespace WBS.Web.Services
{
    public class SSLCommerzService : ISSLCommerzService
    {
        private readonly SSLCommerzSettings _settings;
        private readonly ILogger<SSLCommerzService> _logger;
        private readonly HttpClient _httpClient;

        public SSLCommerzService(
            IOptions<SSLCommerzSettings> settings,
            ILogger<SSLCommerzService> logger,
            HttpClient httpClient)
        {
            _settings = settings.Value;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<SSLCommerzResponse> InitiatePaymentAsync(SSLCommerzRequest request)
        {
            try
            {
                _logger.LogInformation("Initiating SSLCOMMERZ payment for transaction: {TransactionId}", request.tran_id);

                // Set store credentials
                request.store_id = _settings.StoreId;
                request.store_passwd = _settings.StorePassword;
                request.currency = _settings.Currency;

                // Ensure all required fields have valid values
                request.cus_add2 = string.IsNullOrEmpty(request.cus_add2) ? request.cus_add1 : request.cus_add2;
                request.ship_add2 = string.IsNullOrEmpty(request.ship_add2) ? request.ship_add1 : request.ship_add2;
                request.cus_fax = string.IsNullOrEmpty(request.cus_fax) ? request.cus_phone : request.cus_fax;

                // Convert request to form data
                var formData = new Dictionary<string, string>
                {
                    { "store_id", request.store_id },
                    { "store_passwd", request.store_passwd },
                    { "total_amount", request.total_amount.ToString("0.00") },
                    { "currency", request.currency },
                    { "tran_id", request.tran_id },
                    { "success_url", request.success_url },
                    { "fail_url", request.fail_url },
                    { "cancel_url", request.cancel_url },
                    { "ipn_url", request.ipn_url },
                    { "product_name", request.product_name },
                    { "product_category", request.product_category },
                    { "product_profile", request.product_profile },
                    { "cus_name", request.cus_name },
                    { "cus_email", request.cus_email },
                    { "cus_add1", request.cus_add1 },
                    { "cus_add2", request.cus_add2 },
                    { "cus_city", request.cus_city },
                    { "cus_state", request.cus_state },
                    { "cus_postcode", request.cus_postcode },
                    { "cus_country", request.cus_country },
                    { "cus_phone", request.cus_phone },
                    { "cus_fax", request.cus_fax },
                    { "shipping_method", request.shipping_method },  // Added shipping_method
                    { "ship_name", request.ship_name },
                    { "ship_add1", request.ship_add1 },
                    { "ship_add2", request.ship_add2 },
                    { "ship_city", request.ship_city },
                    { "ship_state", request.ship_state },
                    { "ship_postcode", request.ship_postcode },
                    { "ship_country", request.ship_country }
                };

                // Add optional fields only if they have values
                if (!string.IsNullOrWhiteSpace(request.value_a))
                    formData.Add("value_a", request.value_a);
                if (!string.IsNullOrWhiteSpace(request.value_b))
                    formData.Add("value_b", request.value_b);
                if (!string.IsNullOrWhiteSpace(request.value_c))
                    formData.Add("value_c", request.value_c);
                if (!string.IsNullOrWhiteSpace(request.value_d))
                    formData.Add("value_d", request.value_d);

                _logger.LogInformation("Sending payment request to SSLCOMMERZ API: {FormData}", 
                    string.Join(", ", formData.Select(kvp => $"{kvp.Key}={kvp.Value}")));

                var content = new FormUrlEncodedContent(formData);

                _logger.LogInformation("Sending payment request to SSLCOMMERZ API");
                var response = await _httpClient.PostAsync(_settings.SessionUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("SSLCOMMERZ API Response Status: {StatusCode}", response.StatusCode);
                _logger.LogDebug("SSLCOMMERZ API Response: {Response}", responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("SSLCOMMERZ API request failed with status: {StatusCode}, Response: {Response}", 
                        response.StatusCode, responseContent);
                    return new SSLCommerzResponse { status = "FAILED", failedreason = $"API request failed: {responseContent}" };
                }

                var sslResponse = JsonSerializer.Deserialize<SSLCommerzResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (sslResponse == null)
                {
                    _logger.LogError("Failed to deserialize SSLCOMMERZ response: {Response}", responseContent);
                    return new SSLCommerzResponse { status = "FAILED", failedreason = "Invalid response from payment gateway" };
                }

                _logger.LogInformation("Payment session created. Status: {Status}, GatewayURL: {GatewayURL}", 
                    sslResponse.status, sslResponse.GatewayPageURL);
                
                return sslResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating SSLCOMMERZ payment: {Message}", ex.Message);
                return new SSLCommerzResponse { status = "FAILED", failedreason = ex.Message };
            }
        }

        public async Task<SSLCommerzValidationResponse> ValidatePaymentAsync(string validationId)
        {
            try
            {
                _logger.LogInformation("Validating payment with validation ID: {ValidationId}", validationId);

                var formData = new Dictionary<string, string>
                {
                    { "val_id", validationId },
                    { "store_id", _settings.StoreId },
                    { "store_passwd", _settings.StorePassword }
                };

                var content = new FormUrlEncodedContent(formData);
                var response = await _httpClient.PostAsync(_settings.ValidationUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogDebug("Validation Response: {Response}", responseContent);

                var validationResponse = JsonSerializer.Deserialize<SSLCommerzValidationResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (validationResponse == null)
                {
                    _logger.LogError("Failed to deserialize validation response");
                    return new SSLCommerzValidationResponse { status = "FAILED", error = "Invalid response" };
                }

                _logger.LogInformation("Payment validation completed. Status: {Status}", validationResponse.status);
                return validationResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating payment: {Message}", ex.Message);
                return new SSLCommerzValidationResponse { status = "FAILED", error = ex.Message };
            }
        }

        public async Task<bool> ValidateIPNAsync(SSLCommerzIPNRequest ipnData)
        {
            try
            {
                _logger.LogInformation("Validating IPN for transaction: {TransactionId}", ipnData.tran_id);

                // Validate IPN by calling validation API
                var validationResponse = await ValidatePaymentAsync(ipnData.val_id);

                if (validationResponse.status == "VALID" || validationResponse.status == "VALIDATED")
                {
                    // Additional checks
                    if (validationResponse.tran_id == ipnData.tran_id &&
                        Math.Abs(validationResponse.amount - ipnData.amount) < 0.01m)
                    {
                        _logger.LogInformation("IPN validated successfully for transaction: {TransactionId}", ipnData.tran_id);
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning("IPN validation failed: Transaction ID or amount mismatch");
                        return false;
                    }
                }
                else
                {
                    _logger.LogWarning("IPN validation failed: Invalid status {Status}", validationResponse.status);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating IPN: {Message}", ex.Message);
                return false;
            }
        }
    }
}
