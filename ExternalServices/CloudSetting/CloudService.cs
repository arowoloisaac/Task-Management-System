
using Amazon.S3;
using Amazon.S3.Model;
using Azure.Core;
using Microsoft.Extensions.Options;
using Project_Manager.Configuration;

namespace Project_Manager.ExternalServices.CloudSetting
{
    public class CloudService : ICloudService
    {
        private readonly YandexCloudSetting _setting;
        private readonly IAmazonS3 _s3Client;

        public CloudService(IOptionsSnapshot<YandexCloudSetting> cloudSetting, IAmazonS3 s3)
        {
            _setting = cloudSetting.Value;
            _s3Client = s3;
        }

        public async Task<List<string>> GetObjects()
        {
            var objectsList = new List<string>();
            var objectCloud = new ListObjectsV2Request
            {
                BucketName = _setting.BucketName,
            };

            var response = await _s3Client.ListObjectsV2Async(objectCloud);

            //var respo = await _s3Client.ListObjectsAsync(objectCloud);


            foreach (var obj in response.S3Objects)
            {
                objectsList.Add(obj.ETag);
            }

            return objectsList;
        }
    }
}
