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
        private IMemeRepository memeRepository;

        public MemeController(IMemeRepository memeRepository)
        {
            this.memeRepository = memeRepository;
        }

        // GET: Meme
        public async Task<ActionResult> Index(Guid id)
        {
            var completedMeme = await memeRepository.GetCompletedMemeUri(id);

            if(completedMeme == null)
            {
                await Task.Delay(500);
                return RedirectToAction("Index", new { id = id });
            }

            return RedirectPermanent(completedMeme.ToString());
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

                    var pendingId = await memeRepository.CreateNewMeme(newMeme);

                    return RedirectToAction("Index", pendingId);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}