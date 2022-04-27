using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PhotoStorage.Models;
using System;
using Azure.Storage.Blobs;

namespace PhotoStorage.Functions
{
    public static class PhotoStorageUploader
    {
        [FunctionName("PhotoStorage")]
        public static async Task<IActionResult> Run([HttpTrigger("post", Route = "PhotoStorage/Upload")] HttpRequest req,
            [Blob("photo", FileAccess.ReadWrite, Connection = "PhotoStorageConnectionString")] BlobContainerClient blobContainer,
            ILogger log)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<PhotoUploadModel>(body);

            var newId = Guid.NewGuid();
            var blobName = $"{newId}.jpg";

            await blobContainer.CreateIfNotExistsAsync();
            var photoBytes = Convert.FromBase64String(request.Photo);
            await blobContainer.UploadBlobAsync(blobName,new BinaryData(photoBytes));

            log.LogInformation($"Successfuly uploaded {newId} a .jpg file");

            return new OkObjectResult(newId);
        }
    }
}
