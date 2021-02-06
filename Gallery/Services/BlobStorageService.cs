using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Gallery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GalleryDbContext _ef;
        public BlobStorageService(IConfiguration configuration, UserManager<IdentityUser> userManager, GalleryDbContext ef)
        {
            _configuration = configuration;
            _userManager = userManager;
            _ef = ef;
        }

        public async Task<GalleryViewModel> FetchImagesAsync()
        {
            GalleryViewModel galleryViewModel = new GalleryViewModel();
            galleryViewModel.images = new List<ImageViewModel>();
            try
            {
                BlobContainerClient client = new BlobContainerClient(
                    _configuration.GetValue<string>("Azure:StorageAccount:ConnectionString"),
                    _configuration.GetValue<string>("Azure:StorageAccount:TriggerContainer")
                );

                await foreach (var blob in client.GetBlobsAsync())
                {
                    string path = client.GetBlockBlobClient(blob.Name).Uri.AbsoluteUri.ToString();
                    string name = _ef.Images.Where(x => x.Path == path).Select(x => x.ImageName).FirstOrDefault();
                    galleryViewModel.images.Add(new ImageViewModel() { ImageName = name, Path = path });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return galleryViewModel;
        }
        public async Task<bool> UploadImageAsync(UploadViewModel upload, string userId)
        {
            try
            {
                string filename = upload.Image.FileName + Guid.NewGuid().ToString() + Path.GetExtension(upload.Image.FileName);
                // Create a URI to the blob
                Uri blobUri = new Uri("https://" +
                                      _configuration.GetValue<string>("Azure:StorageAccount:Name") +
                                      ".blob.core.windows.net/" +
                                      _configuration.GetValue<string>("Azure:StorageAccount:BlobContainer") +
                                      "/" + filename);

                // Create StorageSharedKeyCredentials object by reading
                // the values from the configuration (appsettings.json)
                StorageSharedKeyCredential storageCredentials = new StorageSharedKeyCredential(
                    _configuration.GetValue<string>("Azure:StorageAccount:Name"), _configuration.GetValue<string>("Azure:StorageAccount:Key"));

                // Create the blob client.
                BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

                // Upload the file
                using (Stream file = upload.Image.OpenReadStream())
                {
                    var res = await blobClient.UploadAsync(file);
                    await RecordImageUploadAsync(upload.Image.FileName, filename, userId);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task RecordImageUploadAsync(string fileName, string imageUri, string userId)
        {
            var img = new Images
            {
                ImageName = fileName,
                Path = imageUri,
                CreatedBy = userId
            };

            await _ef.Images.AddAsync(img);
            await _ef.SaveChangesAsync();
        }
    }
}
