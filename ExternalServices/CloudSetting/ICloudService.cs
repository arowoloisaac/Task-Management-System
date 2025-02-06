namespace Project_Manager.Service.Configuration.CloudSetting
{
    public interface ICloudService
    {
        Task<List<string>> GetObjects();
    }
}
