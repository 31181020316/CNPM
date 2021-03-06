﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc.Html;

using System.Web.Mvc;
using TMDT2.Models;

using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Diagnostics;
using TMDT2.Helpers;

namespace TMDT2.Controllers
{
    [System.Web.Http.Authorize]
    public class VerifyPhoneController : Controller
    {

        public VerifyPhoneController() { }

        public InputModel Input { get; set; }

        public ActionResult VerifyPhone()
        {
            string url = "https://vnexpress.net/rss/giai-tri.rss";
            ViewBag.listItems = RSSHelper.read(url);
            return View();
        }

        [HttpPost]

        public async Task<ActionResult> OnPost(InputModel model)
        {

            string ur = "https://vnexpress.net/rss/giai-tri.rss";
            ViewBag.listItems = RSSHelper.read(ur);
            //if (ModelState.IsValid)
            //{

            var client = new HttpClient();

            var AuthyAPIKey = "E09FjK58eXC4oLAovzH9VBxFGzcAlobB";

            // Add authentication header
            client.DefaultRequestHeaders.Add("X-Authy-API-Key", AuthyAPIKey);


            var values = new Dictionary<string, string>
        {
            { "via", "sms" },
            {"phone_number",model.PhoneNumber },
            {"country_code",  model.DialingCode },

        };
            var content = new FormUrlEncodedContent(values);

            var url = $"https://api.authy.com/protected/json/phones/verification/start?api_key=" + AuthyAPIKey;

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("SecondView", "VerifyPhone");
            }
        }
        public ActionResult SecondView()
        {
            string url = "https://vnexpress.net/rss/giai-tri.rss";
            ViewBag.listItems = RSSHelper.read(url);
            return View();
        }
        public ActionResult Confirm(InputModel model)
        {
            string url = "https://vnexpress.net/rss/giai-tri.rss";
            ViewBag.listItems = RSSHelper.read(url);
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> ConfirmPhone(InputModel model)
        {
            string ur = "https://vnexpress.net/rss/giai-tri.rss";
            ViewBag.listItems = RSSHelper.read(ur);
            var client = new HttpClient();

            // Add authentication header
            var AuthyAPIKey = "E09FjK58eXC4oLAovzH9VBxFGzcAlobB";
            client.DefaultRequestHeaders.Add("X-Authy-API-Key", AuthyAPIKey);

            var phone_number = model.PhoneNumber;
            var country_code = model.DialingCode;
            var verification_code = model.VerificationCode;

            var url = $"https://api.authy.com/protected/json/phones/verification/check?" + "&phone_number=" + phone_number + "&country_code=" + country_code + "&verification_code=" + verification_code;

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }

    }
}