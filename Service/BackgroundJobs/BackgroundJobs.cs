
namespace Project_Manager.Service.BackgroundJobs
{
    public class BackgroundJobs : IBackgroundJobs
    {
        public BackgroundJobs()
        {
            
        }

        public Task<bool> SendEmailReminderAsync()
        {
            throw new NotImplementedException();
        }
        //in here, will implement a class function that will prompt the send
        // to add users automatically as the details corresponds for both organization and group
    }
}
