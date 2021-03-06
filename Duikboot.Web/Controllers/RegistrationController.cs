﻿using Mollie.Api.Client;
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
using Duikboot.Web.Repositories;

namespace Duikboot.Web.Controllers
{

    public class RegistrationController : Controller
    {
        private readonly IPaymentClient _paymentClient;
        private UserRepository _userRepository;
        private readonly CapacityRepository _capacityRepository;
        
        public RegistrationController()
        {
            this._paymentClient = new PaymentClient(ConfigurationManager.AppSettings["MollieLiveKey"]);

            this._capacityRepository = new CapacityRepository();

            this._userRepository = new UserRepository();
        }

        [System.Web.Http.HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Registration/Register.cshtml");
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Submit([FromBody]Models.User user)
        {
            user = Extension.SetDays(user);

            var dates = _capacityRepository.GetAvailableDates();
            user.Days = new Dictionary<string, int>();

            if (user.Zaterdag == true)
            {
                if (!dates["zaterdag"])
                {
                    user.Days.Add("Zaterdag", 40);
                }
                else
                {
                    user.Zaterdag = false;
                }
            }
            if (user.Zondag == true)
            {
                if (!dates["zondag"])
                {
                    user.Days.Add("Zondag", 50);
                }
                else
                {
                    user.Zondag = false;
                }
            }
            if (user.Maandag == true)
            {
                if (!dates["maandag"])
                {
                    user.Days.Add("Maandag", 45);
                }
                else
                {
                    user.Maandag = false;
                }
            }
            if (user.Dinsdag == true)
            {
                if (!dates["dinsdag"])
                {
                    user.Days.Add("Dinsdag", 30);
                }
                else
                {
                    user.Dinsdag = false;
                }
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

                    this._userRepository.Add(meerijder);

                    if (meerijder == null) return View("Failed");

                    if(meerijder.Zaterdag == true)
                    {
                        this._capacityRepository.UpdateSpots("Zaterdag");
                    }

                    if (meerijder.Zondag == true)
                    {
                        this._capacityRepository.UpdateSpots("Zondag");
                    }

                    if (meerijder.Maandag == true)
                    {
                        this._capacityRepository.UpdateSpots("Maandag");
                    }

                    if (meerijder.Dinsdag == true)
                    {
                        this._capacityRepository.UpdateSpots("Dinsdag");
                    }

                    this.SendMail(meerijder);

                    return View("Complete");

                default:
                    return View("Failed");
            }
        }

        public ActionResult GetAvailability()
        {

            return Json(_capacityRepository.GetAvailableDates(), JsonRequestBehavior.AllowGet);
        }

        private void SendMail(Models.User user)
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