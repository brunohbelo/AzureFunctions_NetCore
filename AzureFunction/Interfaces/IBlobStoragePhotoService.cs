using System;
using System.Threading.Tasks;

namespace PhotoStorageIsolated.Interfaces
{
    public interface IBlobStoragePhotoService
    {
        Task<Guid> UploadNewPhoto(string base64Photo);
    }
}
