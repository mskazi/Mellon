using Mellon.Common.Services;
using Mellon.Services.Common.resources;
using Mellon.Services.Common.resources.Couriers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VoucherTaxydromiki;

namespace Mellon.Services.External.CourierProviders
{
    public class GenikiTaxydromikiCourierService : ICourierService
    {
        private readonly string serviceUrl;
        private readonly string servicePass;
        private readonly string serviceKey;
        private readonly string serviceUserName;


        public GenikiTaxydromikiCourierService(IConfiguration configuration, ILogger<GenikiTaxydromikiCourierService> logger)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            serviceUrl = configuration["Endpoints:COURIERS:GENIKI:url"] ?? throw new Exception("COURIERS GENIKI Service endpoint not found in configuration.");
            serviceUserName = configuration["Endpoints:COURIERS:GENIKI:userName"] ?? throw new Exception("COURIERS GENIKI userName found in configuration.");
            servicePass = configuration["Endpoints:COURIERS:GENIKI:password"] ?? throw new Exception("COURIERS GENIKI password found in configuration.");
            serviceKey = configuration["Endpoints:COURIERS:GENIKI:key"] ?? throw new Exception("COURIERS GENIKI key not found in configuration.");
        }

        public CourierMode CourierMode => CourierMode.GenikiTaxydromiki;

        public void Print()
        {
            throw new NotImplementedException();
        }

        public async Task<CourierTrackResource> Track(VoucherDetails voucherDetails)
        {
            var client = await this.Authenticate();

            var request = new TrackAndTraceRequest()
            {
                voucherNo = voucherDetails.CarrierVoucherNo,
                authKey = this.serviceKey,
                language = "el"
            };

            TrackAndTraceResponse response = await client.TrackAndTraceAsync(request);
            if (response.TrackAndTraceResult.Result != 0)
            {
                throw new BadRequestException("Did not receive a valid response from GenTax WS."); ;
            }
            this.Disponse(client);
            var courierTrackResource = new CourierTrackResource()
            {
                DeliveryDate = response.TrackAndTraceResult.DeliveryDate,
                DeliveredAt = response.TrackAndTraceResult.DeliveredAt,
                Status = response.TrackAndTraceResult.Status,
                Details = response.TrackAndTraceResult.Checkpoints.Select(i => new CourierTrackDetailsResource
                {
                    StatusDate = i.StatusDate,
                    Position = i.Shop,
                    Status = i.Status,
                })
            };
            return courierTrackResource;
        }

        private void Disponse(JobServicesSoapClient client)
        {
            client.Close();
        }


        private async Task<JobServicesSoapClient> Authenticate()
        {
            var endpoint = JobServicesSoapClient.EndpointConfiguration.JobServicesSoap;
            var client = new JobServicesSoapClient(endpoint, serviceUrl);
            AuthenticateResponse authResult = await client.AuthenticateAsync(new AuthenticateRequest(this.serviceUserName, this.servicePass, this.serviceKey));
            if (authResult.AuthenticateResult.Result != 0)
            {
                throw new Exception("COURIERS GENIKI Could not authenticate."); ;
            }

            return client;
        }
    }
}
