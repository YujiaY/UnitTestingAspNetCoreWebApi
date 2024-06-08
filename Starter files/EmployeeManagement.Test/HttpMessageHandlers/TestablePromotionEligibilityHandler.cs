using EmployeeManagement.Business;
using System.Text;
using System.Text.Json;

namespace EmployeeManagement.Test.HttpMessageHandlers
{
    public class TestablePromotionEligibilityHandler : HttpMessageHandler
    {
        private readonly bool _isEligibleForPromotion;
        private readonly JsonSerializerOptions _jsonSerializerOptions =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        public TestablePromotionEligibilityHandler(bool isEligibleForPromotion)
        {
            _isEligibleForPromotion = isEligibleForPromotion;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var promotionEligibility = new PromotionEligibility()
            {
                EligibleForPromotion = _isEligibleForPromotion
            };

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(
                   JsonSerializer.Serialize(promotionEligibility,
                    _jsonSerializerOptions),
                   Encoding.ASCII,
                   "application/json")
            };

            return Task.FromResult(response);
        }
    }
}
