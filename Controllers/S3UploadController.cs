using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;

namespace AWS_S3.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class S3UploadController : ControllerBase
    {
        private readonly IAmazonS3 _amazonS3;

        public S3UploadController(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            await _amazonS3.PutObjectAsync(new PutObjectRequest
            {
                InputStream = System.IO.File.Open(@"D:\Downloads\FusionCharts.jpg", FileMode.Open, FileAccess.Read),
                BucketName = "mycoreapibucket",
                Key = "1234.jpg"
            });
            return Ok();
        }
    }
}