using Project_Manager.ExternalServices.EmailService;
using Project_Manager.Service.IssueService;
using Quartz;
using System.Text;

namespace Project_Manager.Service.BackgroundJobs
{
    public class SendReminderJobs : IJob
    {
        private readonly IEmailService _emailService;
        private readonly IIssueService _issueService;

        public SendReminderJobs(IEmailService emailService, IIssueService issueService)
        {
            _emailService = emailService;
            _issueService = issueService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var retrievedList = await _issueService.IssueDeadlineList();

            var groupIssueByUser = retrievedList.GroupBy(user => user.User);

            foreach (var userIssues in groupIssueByUser)
            {
                string email = userIssues.Key;

                var issueList = userIssues.Select(issue => $"- {issue.Name} (Deadline: {issue.EndDate})").ToList();

                StringBuilder emailBody = new StringBuilder();
                emailBody.AppendLine("Hello,");
                emailBody.AppendLine("You have the following issue(s) nearing their deadline:");
                emailBody.AppendLine();
                emailBody.AppendLine(string.Join("\n", issueList));
                emailBody.AppendLine();
                emailBody.AppendLine("Please take necessary action.");

                string subject = "Issue Reminder Deadline";
                await _emailService.SendEmail(subject, emailBody.ToString(), email);
            }
            await Task.CompletedTask;
        }
    }
}
