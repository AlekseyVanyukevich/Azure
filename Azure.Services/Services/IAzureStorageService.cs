using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Domain.ViewModels;
using Azure.Storage.Blobs.Models;

namespace Azure.Services.Services
{
    public interface IAzureStorageService
    {
        Task CopyAllFiles(ContainerCopyModel copy);
        Task<List<string>> GetContainerBlobItems(GetContainerModel containerData);
    }
}