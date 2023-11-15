using CourierGenikiTaxydromiki;
using Mellon.Common.Services;
using Mellon.Services.Common.resources;
using Mellon.Services.Common.resources.Couriers;
using Mellon.Services.Infrastracture.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.ServiceModel;
using System.Web;

namespace Mellon.Services.External.CourierProviders
{
    public class GenikiTaxydromikiCourierService : ICourierService
    {
        private readonly string serviceUrl;
        private readonly HttpClient httpClient;
        public GenikiTaxydromikiCourierService(HttpClient httpClient, IConfiguration configuration, ILogger<GenikiTaxydromikiCourierService> logger)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            serviceUrl = configuration["Endpoints:COURIERS:GENIKIURL"] ?? throw new Exception("COURIERS GENIKI Service endpoint not found in configuration.");
            this.httpClient = httpClient;

        }

        public CourierMode CourierMode => CourierMode.GenikiTaxydromiki;
        public ElectraProjectSetup ProjectSetup { get; set; }

        public async Task<CourierTrackResource> Track(VoucherDetails voucherDetails, CancellationToken cancellation)
        {
            var authenticateResult = await this.Authenticate();
            try
            {
                TrackAndTraceResult response = await authenticateResult.Client.TrackAndTraceAsync(authenticateResult.Key, voucherDetails.CarrierVoucherNo, "el");
                if (response.Result != 0)
                {
                    throw new BadRequestException("Did not receive a valid response from GenTax WS. Result code: " + response.Result + " .Geniki Error Description:: " + this.getResultError(response.Result));
                }
                var courierTrackResource = new CourierTrackResource()
                {
                    DeliveryDate = response.DeliveryDate,
                    DeliveredAt = response.DeliveredAt,
                    Status = response.Status,
                    Details = response.Checkpoints.Select(i => new CourierTrackDetailsResource
                    {
                        StatusDate = i.StatusDate,
                        Position = i.Shop,
                        Status = i.Status,
                    })
                };
                return courierTrackResource;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disponse(authenticateResult.Client);
            }

        }

        public async Task<Stream> Print(IEnumerable<string> vouchers, CancellationToken cancellation)
        {
            var authenticateResult = await this.Authenticate();
            try
            {
                var vouchersUrl = string.Join("&voucherNumbers=", vouchers);
                var encodeKey = HttpUtility.UrlEncode(authenticateResult.Key);
                var url = string.Format("{0}/GetVouchersPdf?authKey={1}&voucherNumbers={2}&Format=Flyer&extraInfoFormat=None", this.serviceUrl, encodeKey, vouchersUrl);
                var response = await httpClient.GetAsync(url, cancellation);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStreamAsync();
                }
                else
                {
                    throw new Exception($"Failed to download PDF from Geniki taxydromiki");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Disponse(authenticateResult.Client);

            }
        }

        public async Task<Boolean> Cancel(VoucherDetails voucherDetails, CancellationToken cancellation)
        {
            var authenticateResult = await this.Authenticate();
            try
            {
                long jobId = Convert.ToInt32(voucherDetails.CarrierJobId);
                int response = await authenticateResult.Client.CancelJobAsync(authenticateResult.Key, jobId, true);
                if (response != 0)
                {
                    throw new BadRequestException("Did not receive a valid Carrier Job Id for cancelling.VoucherNo: " + voucherDetails.CarrierVoucherNo + " was NOT cancelled. ERROR!: " + response + " .Result message from Geniki: " + this.getResultError(response));
                }

                var jobResult = await authenticateResult.Client.GetVoucherJobAsync(authenticateResult.Key, jobId);
                if (jobResult.Result != 13)
                {
                    throw new BadRequestException("VoucherNo: " + voucherDetails.CarrierVoucherNo + " was NOT cancelled. ERROR! " + response + " .Result message from Geniki: " + this.getResultError(response));
                }

                return true;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disponse(authenticateResult.Client);
            }
        }

        public async Task<CourierCreateResource> Create(Datum voucherData, CancellationToken cancellation)
        {
            var authenticateResult = await this.Authenticate();
            try
            {
                var services = new List<string>();
                switch (voucherData.ConditionCode)
                {
                    case "COD":
                        services.Add("AM");
                        break;
                    case "DRC":
                        services.Add("ΕΜ");
                        services.Add("ΧΠ");
                        break;
                    case "DRD":
                        services.Add("ΕΜ");
                        break;
                    case "DRE":
                    case "DRP":
                        services.Add("ΠΚ");

                        break;
                    case "DCC":
                        services.Add("ΧΠ");
                        break;
                    case "SPD":
                        services.Add("ΔΔ");

                        break;
                    case "DBA":
                        services.Add("ΤΝ");
                        break;
                }

                if (voucherData.DeliverSaturday == true)
                {
                    services.Add("5Σ");
                }



                switch (voucherData.VoucherScheduledDelivery)
                {
                    case 1:
                        services.Add("3Σ");
                        break;
                    case 2:
                        services.Add("1Σ");
                        break;

                }

                int items = Convert.ToInt32(voucherData.CarrierPackageItems);
                if (items == 0)
                {
                    items = 1;
                }
                var record = new Record()
                {
                    OrderId = voucherData.Id.ToString(),
                    Name = string.Format("{0}{1}", voucherData.VoucherName, string.IsNullOrWhiteSpace(voucherData.VoucherContact) ? "" : "," + voucherData.VoucherContact),
                    Address = voucherData.VoucherAddress,
                    City = voucherData.VoucherCity,
                    Telephone = string.Format("{0} {1}", voucherData.VoucherPhoneNo, voucherData.VoucherMobileNo),
                    Zip = voucherData.VoucherPostCode,
                    Destination = "",
                    Courier = "",
                    Pieces = items,
                    Weight = voucherData.CarrierPackageWeight,
                    Comments = voucherData.VoucherDescription,
                    Services = String.Join(", ", services.ToArray()),
                    CodAmount = voucherData.CodAmount.HasValue ? voucherData.CodAmount.Value : 0,
                    InsAmount = 0,
                    VoucherNo = "",
                    SubCode = "",
                    BelongsTo = "",
                    DeliverTo = "",
                    ReceivedDate = DateTime.Now
                };

                CreateJobResult createJobResult = await authenticateResult.Client.CreateJobAsync(authenticateResult.Key, record, JobType.Voucher);

                if (createJobResult.Result != 0)
                {
                    throw new BadRequestException("001. Error Creating Voucher. Result code: " + createJobResult.Result + " .Result message from Geniki:: " + this.getResultError(createJobResult.Result));
                }

                var courierTrackResource = new CourierCreateResource()
                {
                    VoucherNo = createJobResult.Voucher,
                    JobID = createJobResult.JobId,
                    SubVouchers = createJobResult.SubVouchers != null ? createJobResult.SubVouchers.Select(i => new CourierCreateSubVoucherResource
                    {
                        VoucherNo = i.VoucherNo,
                        BelongsTo = i.BelongsTo,
                    }) : Enumerable.Empty<CourierCreateSubVoucherResource>(),
                };
                return courierTrackResource;

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disponse(authenticateResult.Client);
            }
        }
        private void Disponse(JobServicesSoapClient client)
        {
            client.Close();
        }

        private async Task<AuthenticateGenikiResult> Authenticate()
        {
            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.AllowCookies = true;
            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
            binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            var endpoint = new EndpointAddress(this.serviceUrl);
            var client = new JobServicesSoapClient(binding, endpoint);
            client.ClientCredentials.UserName.UserName = "npmellon";//ProjectSetup.CarrierUsername;
            client.ClientCredentials.UserName.Password = "101294@"; //ProjectSetup.CarrierPassword;
            AuthenticateResult authResult = await client.AuthenticateAsync("npmellon", "101294@", "EBFBC455-0C34-4962-A29F-7164B54279E9");
            if (authResult.Result != 0)
            {
                throw new Exception("COURIERS GENIKI Could not authenticate."); ;
            }
            return new AuthenticateGenikiResult(authResult.Key, client);
        }

        private string getResultError(int error)
        {
            var message = "";
            switch (error)
            {
                case 1: message = "Authenticate"; break;
                case 2: message = "None right now"; break;
                case 3: message = "CreateJob if null record is passed TrackAndTrace if null or empty volucher number is passed"; break;
                case 4: message = "CancelJob if the job is closed"; break;
                case 5: message = "CreateJob if the server has reached the max voucher number. Should not happen (still many millions to go), but if it does you should really contact us "; break;
                case 6: message = "CreateJob if the server has reached the max voucher number. Should not happen (still many millions to go), but if it does you should really contact us "; break;
                case 700: message = "CreateJob if any of the required fields (Name, Address, City) is empty"; break;
                case 701: message = "CreateJob, if cod service set with no cod amount"; break;
                case 702: message = "CreateJob, if cod amount set with no cod service"; break;
                case 703: message = "CreateJob, if cod amount limit is exceeded "; break;
                case 704: message = "CreateJob, if insurance service set with no insurance amount"; break;
                case 705: message = "CreateJob, if insurance amount set with no insurance service"; break;
                case 706: message = "CreateJob, if received date is smaller than today"; break;
                case 8: message = "All functions, internal error"; break;
                case 9: message = "GetVoucherJob or CancelJob, when specified job does not exist TrackAndTrace, if specified voucher number is not found "; break;
                case 10: message = "GetVoucherJob, CancelJob, when the requesting user has not right to access the specified job TrackAndTrace, when the requesting user has no right to access the specified voucher"; break;
                case 11: message = "All functions except Authenticate, if the specified athorization key is not valid. This is normal, the authorization key can change from time to time."; break;
                case 12: message = "All functions, internal error"; break;
                case 13: message = "GetVoucherJob, CancelJob, if the job is canceled. For GetVoucherJob it should be treated as an error but rather as a status. The info of the job is still returned"; break;
                case 14:
                    message = "CreateJob, ClosePendingJobs, ClosePendingJobsByDate, when temporarily the operation can't be carried out. "; break;
                case 15: message = "All functions, if a limit for requests per some time has been specified for the calling user and this limit has been exceeded"; break;
            }
            return message;
        }
    }

    internal class AuthenticateGenikiResult
    {
        public AuthenticateGenikiResult(string Key, JobServicesSoapClient Client)
        {
            this.Key = Key;
            this.Client = Client;
        }

        public string Key { get; }

        public JobServicesSoapClient Client { get; }
    }
}
