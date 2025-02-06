namespace Project_Manager.ExternalServices.CloudSetting
{
    public interface ICloudService
    {
        Task<List<string>> GetObjects();
    }
}
