using Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Services
{
    public interface IBlobStorageService
    {
        Task<bool> UploadImageAsync(UploadViewModel upload, string userId);
        Task<GalleryViewModel> FetchImagesAsync();
    }
}
