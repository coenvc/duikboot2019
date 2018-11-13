using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using System.Web.Http;
using System.Configuration;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using Mollie.Api.Models.Payment.Response;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Razor;
using Duikboot.Web.ExtensionMethods;
using Duikboot.Web.Models;
using Mollie.Api.Models.Payment;
using RazorEngine;
using RazorEngine.Templating;

namespace Duikboot.Web.Controllers
{

    public class RegistrationController : Controller
    {
        private readonly IPaymentClient _paymentClient;
        //private PassengerRepository _passengerRepository;

        public RegistrationController()
        {
            this._paymentClient = new PaymentClient(ConfigurationManager.AppSettings["MollieTestKey"]);
            //this._passengerRepository = new PassengerRepository();
        }

        [System.Web.Http.HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Registration/Register.cshtml");
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Submit([FromBody]Duikboot.Web.Models.User user)
        {
            user = Extension.SetDays(user);

            user.Days = new Dictionary<string, int>();
            if (user.Zaterdag == true)
            {
                user.Days.Add("Zaterdag", 40);
            }
            if (user.Zondag == true)
            {
                user.Days.Add("Zondag", 50);
            }
            if (user.Maandag == true)
            {
                user.Days.Add("Maandag", 45);
            }
            if (user.Dinsdag == true)
            {
                user.Days.Add("Dinsdag", 30);
            }

            var amount = $"{user.Amount:0.00}".Replace(",", ".");

            //Create new payment 
            PaymentRequest paymentRequest = new PaymentRequest()
            {
                Amount = new Amount(Currency.EUR, amount),
                Description = $"Betaling carnaval 2019 met CV D'N Duikboot - {user.FirstName} {user.SurName}",
                RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + Url.Action("Complete", "Registration", null)
            };

            var paymentResponse = await _paymentClient.CreatePaymentAsync(paymentRequest);

            Session["paymentResponseId"] = paymentResponse.Id;
            Session["Meerijder"] = user;

            return Redirect(paymentResponse.Links.Checkout.Href);

        }

        [System.Web.Mvc.HttpGet]
        public async Task<ActionResult> Complete()
        {
            var paymentResponseId = Session["paymentResponseId"].ToString();
            PaymentResponse paymentResponse = await this._paymentClient.GetPaymentAsync(paymentResponseId);
            switch (paymentResponse.Status)
            {
                case (PaymentStatus.Paid):
                    Models.User meerijder = Session["Meerijder"] as Models.User;
                    // Save _user;
                    this.SendMail(meerijder);
                    return View("Complete");
                default:
                    return View("Failed");
            }
        }

        public ActionResult GetAvailability()
        {

            // TODO: FIX WHEN DATABASE IS CONNECTED
            var availability = new Dictionary<string, bool>
            {
                {"zaterdag", false},
                {"zondag", false},
                {"maandag", false},
                {"dinsdag", false}
            };

            //return Json(meerijderRepository.GetAvailableDates(), JsonRequestBehavior.AllowGet);
            return Json(availability, JsonRequestBehavior.AllowGet);
        }

        private void SendMail(User user)
        {
            var subscriptionTemplate = Server.MapPath("~/Templates/SubscriptionEmail.cshtml");
            if (System.IO.File.Exists(subscriptionTemplate))
            {
                var template = System.IO.File.ReadAllText(subscriptionTemplate);
                var result = Engine.Razor.RunCompile(template, Guid.NewGuid().ToString(), null, user);

                //Creating SmtpClient
                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Host = ConfigurationManager.AppSettings["SmtpClientHost"],
                    Port = int.Parse(ConfigurationManager.AppSettings["SmtpClientPort"]),
                    UseDefaultCredentials = false
                };

                // Setting the credentials
                var credentials =
                    new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"]
                        , ConfigurationManager.AppSettings["EmailPassword"]);

                smtpClient.Credentials = credentials;

                var mail = new MailMessage
                {
                    From = new MailAddress(ConfigurationManager.AppSettings["Email"], "CV D'n Duikboot")
                };

                //Creating the mail
                mail.To.Add(new MailAddress(user.Email));
                mail.Subject = "Inschrijving CV D'n Duikboot 2019";

                mail.IsBodyHtml = true;
                mail.Body = result;

                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                Console.WriteLine("File not found");
            }
        }
    }
}