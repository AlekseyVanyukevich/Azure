using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Domain.ViewModels;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.DataMovement;
using Microsoft.Extensions.Logging;

namespace Azure.Services.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly ILogger<AzureStorageService> _logger;

        public AzureStorageService(ILogger<AzureStorageService> logger)
        {
            _logger = logger;
        }

        public async Task CopyAllFiles(ContainerCopyModel copy)
        {
            var sourceContainer = await GetBlobContainer(copy.Source.StorageConnectionString, copy.Source.ContainerName);
            var destinationContainer = await GetBlobContainer(copy.Destination.StorageConnectionString, copy.Destination.ContainerName);
            if (!await sourceContainer.ExistsAsync() || !await destinationContainer.ExistsAsync())
            {
                throw new ArgumentException();
            }
            foreach (var blobItem in sourceContainer.ListBlobs(null, false))
            {
                var blobName = blobItem.Uri.Segments.Last();
                var blob = sourceContainer.GetBlobReference(blobName);
                var insertedBlob = destinationContainer.GetBlobReference(blobName);
                if (!await insertedBlob.ExistsAsync())
                {
                    await TransferManager.CopyAsync(blob, insertedBlob, CopyMethod.ServiceSideAsyncCopy, null, null );
                }
            }
        }

        public async Task<List<string>> GetContainerBlobItems(GetContainerModel containerData)
        {
            var container = await GetBlobContainer(containerData.StorageConnectionString, containerData.ContainerName);
            return container.ListBlobs(null, false)
                .Select(b => b.Uri.AbsoluteUri)
                .ToList();
        }


        private async Task<CloudBlobContainer> GetBlobContainer(string connectionString, string containerName)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            return container;
        }
    }
}