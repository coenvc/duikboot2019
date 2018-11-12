﻿using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using System.Web.Http;
using System.Configuration;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models;
using System;
using System.Collections.Generic;
using Mollie.Api.Models.Payment.Response;
using System.Threading.Tasks;
using System.Web.Mvc;
using Duikboot.Web.ExtensionMethods;
using Duikboot.Web.Models;
using Mollie.Api.Models.Payment;

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
    }
}