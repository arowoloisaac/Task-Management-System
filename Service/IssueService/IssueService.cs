using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.DTO.IssueDto;
using Project_Manager.DTO.TaskDto;
using Project_Manager.Enum;
using Project_Manager.Model;
using System;

namespace Project_Manager.Service.IssueService
{
    public class IssueService : IIssueService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public IssueService(UserManager<User> userManager, ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }


        public async Task<string> CreateIssues(Guid projectId, CreateIssue issueDto, string mail)
        {
            var user = await GetUser(mail);

            var checkProject = await ValidateProject(projectId);
          
            var validateIssueName = await _context.Issues
                .Where(issue => issue.Name == issueDto.Name && issue.Project.Id == projectId).SingleOrDefaultAsync();

            if (validateIssueName == null)
            {
                if (issueDto.StartDate <= issueDto.EndDate)
                {
                    int date = CheckDate(issueDto.StartDate, issueDto.EndDate);

                    if (date >= issueDto.EstimatedTimeInMinutes)
                    {
                        var newIssue = await _context.Issues.AddAsync(new Issue
                        {
                            Id = Guid.NewGuid(),
                            Name = issueDto.Name,
                            Description = issueDto.Description,
                            //AssignedUserTo = issueDto.AssignedTo ?? null,//String.IsNullOrEmpty(userrole.RoleId) ? (Guid?)null : new Guid(userrole.RoleId)
                            EstimatedTimeInMinutes = issueDto.EstimatedTimeInMinutes,
                            CreatedBy = user.Id,
                            CreatedDate = DateTime.Now,
                            StartDate = issueDto.StartDate,
                            EndDate = issueDto.EndDate,
                            Complexity = issueDto.Complexity,
                            Progress = Progress.GroundLevel,
                            IssueType = issueDto.IssueType,
                            Project = checkProject,
                            
                        });
                        await _context.SaveChangesAsync();
                        return "Issue successfully created";
                    }
                    else
                    {
                        throw new Exception("Estimated time cannot be greated than the dates between");
                    }
                }
                else
                {
                    throw new Exception("start date must be less than end date or equal");
                }
            }
            else
            {
                throw new Exception("you cannot create issue with same name");
            }
        }

        public async Task<string> CreateSubIssue(Guid projectId, CreateIssue issueDto, Guid parentIssueId, string mail)
        {
            var user = await GetUser(mail);

            var checkProject = await ValidateProject(projectId);

            var getParentIssue = await ValidateIssue(parentIssueId, user.Id);

            var validateIssueName = await _context.Issues
                .Where(issue => issue.Name == issueDto.Name && issue.Project.Id == projectId).SingleOrDefaultAsync();

            if (validateIssueName != null)
            {
                throw new Exception("Killer being");
            }
            else
            {
                DateOnly start = issueDto.StartDate;
                DateOnly end = issueDto.EndDate;
                if(getParentIssue.StartDate >= start && getParentIssue.EndDate >= end)
                {
                    if (start <= end)
                    {
                        int date = CheckDate(issueDto.StartDate, issueDto.EndDate);

                        if (date >= issueDto.EstimatedTimeInMinutes)
                        {
                            var newIssue = await _context.Issues.AddAsync(new Issue
                            {
                                Id = Guid.NewGuid(),
                                Name = issueDto.Name,
                                Description = issueDto.Description,
                                //AssignedTo = issueDto.AssignedTo,
                                EstimatedTimeInMinutes = issueDto.EstimatedTimeInMinutes,
                                CreatedBy = user.Id,
                                CreatedDate = DateTime.Now,
                                StartDate = issueDto.StartDate,
                                EndDate = issueDto.EndDate,
                                Complexity = issueDto.Complexity,
                                Progress = Progress.GroundLevel,
                                IssueType = issueDto.IssueType,
                                Project = getParentIssue.Project,
                                ParentIssue = getParentIssue
                            });

                            //this is to add the time of the sub issue to the parent issue
                            getParentIssue.EstimatedTimeInMinutes += issueDto.EstimatedTimeInMinutes;
                            _context.Issues.Update(getParentIssue);

                            await _context.SaveChangesAsync();
                            return "Issue successfully created";
                        }
                        else
                        {
                            throw new Exception("Estimated time cannot be greated than the dates between");
                        }
                    }
                    else
                    {
                        throw new Exception("start date must be less than end date or equal");
                    }
                }
                else
                {
                    throw new Exception($"Make sure the date suite in line with the parent issue");
                }
            }
        }

        public async Task<string> DeleteIssues(Guid issueId, bool isDeleteChildren, string mail)
        {
            try
            {
                var user = await GetUser(mail);

                var issue = await ValidateIssue(issueId, user.Id);

                var issueChildren = await _context.Issues
                        .Where(filter => filter.ParentIssue.Id == issueId)
                        .ToListAsync();

                if (isDeleteChildren)
                {
                    _context.Issues.RemoveRange(issueChildren);
                }
                else
                {
                    foreach(var child in issueChildren)
                    {
                        child.ParentIssue = null;
                    }
                }

                _context.Issues.Remove(issue);

                await _context.SaveChangesAsync();

                return "deleted successfully";
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<IEnumerable<RetrieveIssue>> GetIssues(IssueType? issueType, Complexity? complexity, Progress? progress, string mail)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateIssues(Guid issueId, string? Name, string? Description,
            Complexity? complexity, uint? EstimatedTimeInMinute, uint timeSpent, int issueLevel, string mail)
        {
            var user = await GetUser(mail);

            await ValidateIssueUpdate(issueId, Name, Description, complexity, EstimatedTimeInMinute, timeSpent, issueLevel, user.Id);

            return "Task successful";
        }

        public async Task<List<RetrieveIssue>> GetIssue(Guid projectId)
        {
            var list = await _context.Issues.Where(issues => issues.Project.Id == projectId).ToListAsync(); 

            if(list.Count == 0)
            {
                return new List<RetrieveIssue>();
            }

            else
            {
                var response = list.Select(isse => new RetrieveIssue
                {
                    id = isse.Id,
                }).ToList();

                return response;
            }
        }


        private async Task<string> ValidateIssueUpdate(Guid id, string? Name, string? Description,
            Complexity? complexity, uint? estimatedTimeInMinute, uint timeSpent, int issueLevel, Guid userId)
        {
            
            var getIssue = await ValidateIssue(id, userId);
            
            
            if (!string.IsNullOrEmpty(Name))
            {
                getIssue.Name = Name;
            }

            if (!string.IsNullOrEmpty(Description))
            {
                getIssue.Description = Description;
            }

            if (complexity.HasValue)
            {
                getIssue.Complexity = complexity.Value;
            }

            uint initializedTime = getIssue.TimeSpent + timeSpent;

            if (estimatedTimeInMinute.HasValue)
            {
                getIssue.EstimatedTimeInMinutes = estimatedTimeInMinute.Value;

                if (initializedTime > estimatedTimeInMinute)
                {
                    throw new Exception("Time spent can't be greater than estimated time");
                }
                else
                {
                    uint v = initializedTime > estimatedTimeInMinute ? throw new Exception("reduce time spent") : getIssue.TimeSpent = initializedTime;

                    if (issueLevel < 100 && issueLevel > 0 ||  v > 0)
                    {
                        getIssue.IssueLevel = issueLevel;
                        getIssue.Progress = Progress.InProcess;
                    }
                    else if (issueLevel == 100 || v == estimatedTimeInMinute)
                    {
                        getIssue.IssueLevel = 100;
                        getIssue.Progress = Progress.Finished;
                    }
                    else
                    {
                        if (getIssue.IssueLevel == 0)
                        {
                            //continue
                        }
                        else if (getIssue.IssueLevel < 0)
                        {
                            throw new InvalidOperationException("can't be less than zero");
                        }
                        else
                        {
                            throw new Exception("can't validate action");
                        }
                    }
                }
            }
            else
            {
                if (initializedTime > getIssue.EstimatedTimeInMinutes)
                {
                    throw new Exception("you can't be greater than the time spent");
                }
                else
                {
                    
                    uint v = initializedTime > estimatedTimeInMinute ? throw new Exception("reduce time spent") : getIssue.TimeSpent = initializedTime;
                    if (issueLevel < 100 && issueLevel > 0)
                    {
                        getIssue.IssueLevel = issueLevel;
                        getIssue.Progress = Progress.InProcess;
                    }
                    else if (issueLevel == 100)
                    {
                        getIssue.IssueLevel = 100;
                        getIssue.Progress = Progress.Finished;
                    }
                    else
                    {
                        if (getIssue.IssueLevel == 0)
                        {
                            //continue
                        }
                        else if (getIssue.IssueLevel < 0)
                        {
                            throw new InvalidOperationException("can't be less than zero");
                        }
                        else
                        {
                            throw new Exception("can't validate action");
                        }
                    }
                }
            }

            _context.Issues.Update(getIssue);

            await _context.SaveChangesAsync();
            return "successful";
            
            
        }

        private int CheckDate( DateOnly startDate, DateOnly endDate)
        {
            int checkDate = endDate.DayNumber - startDate.DayNumber;

            int daysDifference = (checkDate == 0 ? 1 : checkDate) * 1440;

            return daysDifference;
        }

        private async Task<User> GetUser(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);

            if(user == null)
            {
                throw new Exception($"user with mail - {mail} not found");
            }
            return user;
        }

        private async Task<Project> ValidateProject(Guid projectId)
        {
            var validateProject = await _context.Projects.FindAsync(projectId);

            if (validateProject == null)
            {
                throw new Exception("You need to created a project in other " +
                    "to have access to creating an issue");
            }
            return validateProject;
        }

        private async Task<Issue> ValidateIssue(Guid issueId, Guid userId)
        {
            var getIssue = await _context.Issues.Where(search => search.Id == issueId && search.CreatedBy == userId).SingleOrDefaultAsync();

            if (getIssue == null)
            {
                throw new Exception("Can not find this issue");
            }
            return getIssue;
        }

        //to check if the parent issue is the sub issue of itself
        private async Task<Issue> ParentIssue(Guid issueId, Guid userId)
        {
            var getIssue = await ValidateIssue(issueId, userId);

            if (getIssue.ParentIssue == getIssue)
            {
                throw new Exception("Issue can be a sub task of itself");
            }
            return getIssue;
        }
    }
}
