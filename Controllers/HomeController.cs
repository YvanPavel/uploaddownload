using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using uploaddownload.Models;

namespace uploaddownload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            List<FileStore> fs_list = new List<FileStore>();

            string connectionString = _configuration.GetConnectionString("StorageConnectionString");

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient;

            try
            {
                containerClient = blobServiceClient.CreateBlobContainer("meincontainer238");
            }
            catch (Exception)
            {
                containerClient = blobServiceClient.GetBlobContainerClient("meincontainer238");
            }

            // Auflisten der Blobdateien
            foreach (BlobItem blobItem in containerClient.GetBlobs())
            {
                var file = new FileStore
                {
                    FileName = blobItem.Name,
                    ContentType = blobItem.Properties.ContentType,
                    ContentLength = blobItem.Properties.ContentLength
                };

                fs_list.Add(file);
            }


            

            //containerClient.SetAccessPolicy(PublicAccessType.Blob);


            //// Löschen des Blobs
            //blobClient.Delete();

            //// Löschen des Containers
            //containerClient.Delete();

            return View(model: fs_list);
        }

        
        public IActionResult Upload(IFormFile dieDatei)
        {
            string connectionString = _configuration.GetConnectionString("StorageConnectionString");

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient;

            try
            {
                containerClient = blobServiceClient.CreateBlobContainer("meincontainer238");
            }
            catch (Exception)
            {
                containerClient = blobServiceClient.GetBlobContainerClient("meincontainer238");
            }

            BlobClient blobClient = containerClient.GetBlobClient(dieDatei.FileName);

            blobClient.Upload(dieDatei.OpenReadStream(), true);
            
            containerClient.SetAccessPolicy(PublicAccessType.Blob);

            return View();
        }

        //public IActionResult Download(string filename)
        //{
        //    string connectionString = _configuration.GetConnectionString("StorageConnectionString");

        //    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

        //    BlobContainerClient containerClient;

        //    try
        //    {
        //        containerClient = blobServiceClient.CreateBlobContainer("meincontainer238");
        //    }
        //    catch (Exception)
        //    {
        //        containerClient = blobServiceClient.GetBlobContainerClient("meincontainer238");
        //    }

        //    BlobClient blobClient = containerClient.GetBlobClient(dieDatei.FileName);


        //    BlobDownloadInfo blobDownload = blobClient.Download();
        //    using (FileStream fs = new FileStream(@"C:\Users\ITA5-TN04\Downloads\DeBlob2.jpg", FileMode.Create, FileAccess.Write))
        //    {
        //        blobDownload.Content.CopyTo(fs);
        //    }

        //    return File()
        //}




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
