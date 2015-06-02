using Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photo.Controllers
{
    public class MemeController : Controller
    {
        // GET: Meme
        public ActionResult Index(Guid Id)
        {
            return View();
        }

        // POST: Meme
        public ActionResult Create(Meme meme, HttpPostedFileBase upload)
        {
            return View();
        }
    }
}