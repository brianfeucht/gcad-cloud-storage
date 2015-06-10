using Photo.Core.Interfaces;
using Photo.Core.Models;
using Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Photo.Controllers
{
    public class MemeController : Controller
    {
        public IMemeRepository MemeRepository { get; set; }
        // GET: Meme
        public async Task<ActionResult> Index(Guid id)
        {
            var completedMeme = await MemeRepository.GetCompletedMemeUri(id);

            return View(completedMeme);
        }

        // POST: Meme
        public async Task<ActionResult> Create(Meme meme, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.ContentLength > 0)
                {
                    var newMeme = new NewMeme()
                    {
                        Text = meme.Text
                    };

                    using (var reader = new System.IO.BinaryReader(photo.InputStream))
                    {
                       newMeme.Image = reader.ReadBytes(photo.ContentLength);
                    }

                    var pendingId = await MemeRepository.CreateNewMeme(newMeme);

                    return RedirectToAction("Index", pendingId);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}