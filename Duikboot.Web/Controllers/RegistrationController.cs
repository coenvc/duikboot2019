using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using System.Web.Http;
using System.Configuration;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models;
using System;
using Mollie.Api.Models.Payment.Response;
using System.Threading.Tasks;
using Mollie.Api.Models.Payment;

namespace Duikboot.Web.Controllers
{

    public class RegistrationController :ApiController
    {
        private readonly IPaymentClient paymentClient;

        public RegistrationController()
        {
            this.paymentClient = new PaymentClient(ConfigurationManager.AppSettings["MollieTestKey"]);
        }

        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Submit([FromBody]Duikboot.Web.Models.User user)
        {
            //Create new payment 
            PaymentRequest paymentRequest = new PaymentRequest()
            {
                Amount = new Amount(Currency.EUR, "100"),
                Description = $"Betaling carnaval 2019 met CV D'N Duikboot - {user.FirstName} {user.SurName}",
                RedirectUrl = "https://www.pornhub.com"
            };

            var paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
            var secondPaymentResponse = await paymentClient.GetPaymentAsync(paymentResponse.Id);

            var test = secondPaymentResponse.Links.Checkout;

            switch (paymentResponse.Status)
            {
                case (PaymentStatus.Paid):
                    return Ok();

                default: return BadRequest(paymentResponse.Status.ToString());
            }

        }
    }
}