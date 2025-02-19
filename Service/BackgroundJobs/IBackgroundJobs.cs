namespace Project_Manager.Service.BackgroundJobs
{
    public interface IBackgroundJobs
    {
        Task<bool> SendEmailReminderAsync();
    }
}
