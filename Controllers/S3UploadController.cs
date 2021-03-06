using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AWS_S3.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class S3UploadController : ControllerBase
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly string _bucketName = "mycoreapibucket";

        public S3UploadController(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            await _amazonS3.UploadObjectFromStreamAsync(_bucketName, $"photos/{file.FileName}", file.OpenReadStream(),
                null, CancellationToken.None);
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var response = await _amazonS3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _bucketName,
                InputStream = file.OpenReadStream(),
                Key = $"photos/{file.FileName}"
            }, CancellationToken.None);
            var versionId = response.VersionId;
            return Ok(versionId);
        }

        [HttpGet]
        public async Task<IActionResult> ListFiles()
        {
            var files = await _amazonS3.ListObjectsAsync(_bucketName, CancellationToken.None);
            return Ok(files);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var result = await _amazonS3.DeleteObjectAsync(_bucketName, fileName);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var result = await _amazonS3.GetObjectAsync(_bucketName, fileName, CancellationToken.None);
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
            return new FileStreamResult(result.ResponseStream, "application/octet-stream");
        }
    }
}
