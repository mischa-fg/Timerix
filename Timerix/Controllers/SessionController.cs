﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timerix.Controllers;
using Timerix.Models;
using Timerix.Models.ViewModels;

namespace Timerix.Controllers
{
    [Route("session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSessionId()
        {
            return Content(HttpContext.Session.Id);
        }
        [HttpGet("reset")]
        public void SessionClear(String mid)
        {
            HttpContext.Session.Clear();
        }
        [HttpGet("mitId/{mid}")]
        public IActionResult SaveToSessionMitarbeiter(String mid)
        {
            HttpContext.Session.SetString("mit_id", mid);
            return Content($"{mid} save to session");
        }

        [HttpGet("mitId")]
        public IActionResult FetchFromSessionMitarbeiter()
        {
            string mit = HttpContext.Session.GetString("mit_id");
            return Content(mit);
        }
        [HttpPost("mitStandort")]
        public IActionResult SetObjectAsJson(MitarbeiterStandortViewModel value)
        {

            HttpContext.Session.SetString("mitarbeiterStandort", JsonConvert.SerializeObject(value));
            return Content($"MitarbeiterStandort save to session");
        }
        [HttpGet("mitStandort")]
        public MitarbeiterStandortViewModel GetObjectFromJson()
        {
            var value = HttpContext.Session.GetString("mitarbeiterStandort");
            //return Content(value);
            if (value == null) {
                return null;
            }
            var ausgabe =  JsonConvert.DeserializeObject<MitarbeiterStandortViewModel>(value);
            return ausgabe;
        }

        [HttpPost("zeiterfassungDaten")]
        public IActionResult SetZeiterfassungDaten(object value)
        {
            string s = value.ToString();
            //string s = JsonConvert.SerializeObject(value);
            HttpContext.Session.SetString("zeiterfassungDaten", s);
            return Content($"ZeiterfassungDaten save to session");
        }
        [HttpGet("zeiterfassungDaten")]
        public IActionResult GetZeiterfassungDaten()
        {
            string data = HttpContext.Session.GetString("zeiterfassungDaten");
            return Content(data);
        }

    }
}