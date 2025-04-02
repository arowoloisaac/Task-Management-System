using Project_Manager.Model;

namespace Project_Manager.ExternalServices.CloudSetting
{
    public interface ICloudService
    {
        Task<List<string>> GetObjects();

        Task<byte[]> GetObject(string objectName);

        Task<string> UploadObject(ObjectUpload model);
    }
}
