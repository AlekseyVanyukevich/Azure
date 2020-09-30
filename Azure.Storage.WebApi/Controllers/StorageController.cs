using System;
using System.Net;
using System.Threading.Tasks;
using Azure.Domain.ViewModels;
using Azure.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Azure.Storage.WebApi.Controllers
{
    [ApiController]
    [Route("api/storage")]
    public class StorageController : ControllerBase
    {
        private readonly ILogger<StorageController> _logger;
        private readonly IAzureStorageService _azureStorageService;

        public StorageController(ILogger<StorageController> logger,
            IAzureStorageService azureStorageService)
        {
            _logger = logger;
            _azureStorageService = azureStorageService;
        }

        // [HttpGet("containers")]
        // public IActionResult GetStorageContainers([FromQuery] string connectionString)
        // {
        //     if (string.IsNullOrWhiteSpace(connectionString))
        //     {
        //         return BadRequest();
        //     }
        //     return Ok(_azureStorageService.GetAccountContainers(connectionString));
        // }

        [HttpPost]
        public async Task<IActionResult> CopyAllFiles([FromBody] ContainerCopyModel copy)
        {
            try
            {
                await _azureStorageService.CopyAllFiles(copy);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("containers")]
        public async Task<IActionResult> GetContainerBlobItems([FromQuery] GetContainerModel containerData)
        {
            try
            {
                var blobItems =  await _azureStorageService
                    .GetContainerBlobItems(containerData);
                return Ok(blobItems);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}