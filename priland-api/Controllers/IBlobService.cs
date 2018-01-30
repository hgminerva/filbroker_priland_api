using priland_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace priland_api.Controllers
{
    public interface IBlobService
    {
        Task<List<BlobUploadModel>> UploadBlobs(HttpContent httpContent);
        Task<BlobDownloadModel> DownloadBlob(int blobId);
    }
}