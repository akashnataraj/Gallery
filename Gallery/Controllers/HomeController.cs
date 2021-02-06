using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Gallery.Models;
using Gallery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Gallery.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IBlobStorageService _blobStorage;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(IConfiguration configuration, IBlobStorageService blobStorage, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _blobStorage = blobStorage;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Gallery()
        {
            GalleryViewModel galleryViewModel = await _blobStorage.FetchImagesAsync();
            return View(galleryViewModel);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(UploadViewModel upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if ((upload.Image.Length / (1024 * 1024)) > _configuration.GetValue<int>("Image:MaxSize")) //check if image size does not exceed limit
                    {
                        ModelState.AddModelError("customError", _configuration.GetValue<string>("Message:ExceededSizeLimit"));
                        return View();
                    }

                    if (_configuration.GetValue<string>("Image:AllowedFormats").Split(',').ToList() //Fetch all allowed formats
                        .Where(x => x == System.IO.Path.GetExtension(upload.Image.FileName)).Count() == 0) //check if uploaded file is image
                    {
                        ModelState.AddModelError("customError", _configuration.GetValue<string>("Message:InvalidImageExtension"));
                        return View();
                    }
                    var res = await _blobStorage.UploadImageAsync(upload, _userManager.GetUserId(User));
                    if(res == true)
                    {
                        ModelState.AddModelError("uploadSuccess", _configuration.GetValue<string>("Message:UploadSuccessfull"));
                    }
                    else
                    {
                        ModelState.AddModelError("customError", _configuration.GetValue<string>("Message:UploadUnsuccessfull"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
    }
}