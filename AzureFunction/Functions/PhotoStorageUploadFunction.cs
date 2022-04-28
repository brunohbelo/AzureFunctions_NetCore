using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PhotoStorageIsolated.Models;
using PhotoStorageIsolated.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace PhotoStorageIsolated.Functions
{
    public class PhotoStorageUploadFunction
    {
        private readonly IBlobStoragePhotoService _blobStoragePhotoSerivce;

        public PhotoStorageUploadFunction(IBlobStoragePhotoService blobStoragePhotoSerivce)
        {
            _blobStoragePhotoSerivce = blobStoragePhotoSerivce;
        }

        [Function("PhotoStorage")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger("post", Route = "PhotoStorage/Upload")] HttpRequestData req,
            FunctionContext context
        )
        {
            var logger = context.GetLogger("PhotoLogger");
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<PhotoUploadModel>(body);

            if(string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentNullException("Name must not be blank");
            }

            var photoBlobId = await _blobStoragePhotoSerivce.UploadNewPhoto(request.Photo);
            logger.LogInformation($"Successfuly uploaded {photoBlobId} a .jpg file");

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync(photoBlobId.ToString());
            return response;
        }
    }
}
