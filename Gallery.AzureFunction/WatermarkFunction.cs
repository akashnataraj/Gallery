using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Gallery.AzureFunction
{
    public static class WatermarkFunction
    {
        const string WaterMarkText = "(c) Copyright Image";

        [FunctionName("WatermarkFunction")]
        public static void Run([BlobTrigger("images/{name}")] Stream inputBlob,
                               [Blob("images-watermarked/{name}", FileAccess.Write)] Stream outputBlob,
                               string name,
                               ILogger log)
        {
            try
            {
                Watermark.WriteWatermark(WaterMarkText, inputBlob, outputBlob);
            }
            catch (Exception e)
            {
                log.LogError($"Watermaking failed {e.Message}");
            }
        }
    }
}
