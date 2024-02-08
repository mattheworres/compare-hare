using CompareHare.Api.MediatR;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.PaPower.RequestHandlers.DistributorsList
{
    public class DistributorsListHandler : ApiRequestHandlerBase, IRequestHandler<DistributorsListMessage, IActionResult>
    {
        private readonly HttpClient _httpClient;
        // private const string DISTRIBUTOR_URL = "https://www.papowerswitch.com/umbraco/Api/ShopApi/ZipSearch?zipcode={0}&servicetype=residential";
        private const string DISTRIBUTOR_URL = "http://localhost:8000/public/PA_Distributor_Response.json";
        public DistributorsListHandler()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Handle(DistributorsListMessage message, CancellationToken cancellationToken)
        {
            var url = string.Format(DISTRIBUTOR_URL, message.Model.ZipCode);
            var response = await _httpClient.GetStringAsync(url);

            if (response == null) {
                return BadRequest();
            }

            return response != null
                ? Ok(response)
                : BadRequest();
        }
    }
}
