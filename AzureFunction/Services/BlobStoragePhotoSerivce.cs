using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using PhotoStorageIsolated.Interfaces;
using System;
using System.Threading.Tasks;

namespace PhotoStorageIsolated.Services
{
    public class BlobStoragePhotoSerivce : IBlobStoragePhotoService
    {
        private readonly BlobContainerClient _blobContainer;

        public BlobStoragePhotoSerivce(IConfiguration configuration)
        {
            var connection = configuration.GetValue<string>("BlobStorageConnectionString");
            var containerName = configuration.GetValue<string>("BlobContainerName");
            _blobContainer = new BlobContainerClient(connection, containerName);
        }

        public async Task<Guid> UploadNewPhoto(string base64Photo)
        {
            var newId = Guid.NewGuid();
            var blobName = $"{newId}.jpg";

            await _blobContainer.CreateIfNotExistsAsync();
            var photoBytes = Convert.FromBase64String(base64Photo);
            await _blobContainer.UploadBlobAsync(blobName, new BinaryData(photoBytes));

            return newId;
        }
    }
}
