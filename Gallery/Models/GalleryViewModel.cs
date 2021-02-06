using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Models
{
    public class GalleryViewModel
    {
        public List<ImageViewModel> images { get; set; }
    }

    public class ImageViewModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Path { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
