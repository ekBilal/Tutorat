﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
	[AllowAnonymous]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Contactez-nous.";

			return View();
		}
	}
}