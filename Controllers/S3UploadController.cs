using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AWS_S3.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class S3UploadController : ControllerBase
    {
        private readonly IAmazonS3 _amazonS3;
        private string _bucketName = "mycoreapibucket";

        public S3UploadController(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            await _amazonS3.PutObjectAsync(new PutObjectRequest
            {
                InputStream = file.OpenReadStream(),
                BucketName = _bucketName,
                Key = file.FileName
            });
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ListFiles()
        {
            var files = await _amazonS3.ListObjectsAsync(_bucketName, CancellationToken.None);
            return Ok(files);
        }
    }
}