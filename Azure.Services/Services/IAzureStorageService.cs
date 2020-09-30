using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Domain.ViewModels;

namespace Azure.Services.Services
{
    public interface IAzureStorageService
    {
        Task CopyAllFiles(ContainerCopyModel copy);
        Task<List<string>> GetContainerBlobItems(GetContainerModel containerData);
    }
}