using System;
using System.Collections.Generic;
using System.Text;

namespace Gallery.AzureFunction
{
    public static class Configuration
    {
        public const string BlobContainer = "images";
        public const string TriggerContainer = "images-watermarked";
    }
}
