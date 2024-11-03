using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.DTO.ProjectDto;
using Project_Manager.Enum;
using Project_Manager.Model;

namespace Project_Manager.Service.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectService(UserManager<User> userManager, ApplicationDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }


        public async Task<string> CreateProject(CreateDto dto, string mail)
        {
            var user = await FindUser(mail);
            
            try
            {
                var validateProject = await _context.Projects.Where(duplicateName => duplicateName.Name == dto.Name).SingleOrDefaultAsync();

                if (validateProject != null)
                {
                    throw new Exception($"Project with {dto.Name} already exist");
                }
                else
                {
                    //make sure start date can't be yesterday and also end date can't be less than start date
                    var createProject = new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = dto.Name,
                        Description = dto.Description,
                        CreatedBy = user.Id,
                        Creator = user,
                        CreatedTime = DateTime.UtcNow,
                        Complexity = dto.Complexity,
                        Progress = Progress.GroundLevel
                    };
                    _context.Projects.Add(createProject);

                    await _context.SaveChangesAsync();

                    return "project created";
                }
            } 
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the project: " + ex.InnerException?.Message);
            }
            
        }


        public async Task<string> DeleteProject(Guid projectId, string mail)
        {
            var user = await FindUser(mail);

            var findProjectById = await _context.Projects.Where(project => project.Id ==projectId && project.CreatedBy ==user.Id).SingleOrDefaultAsync();

            if(findProjectById == null)
            {
                throw new Exception($"Project with id: {projectId} not found");
            }
            else
            {
                var removeCreatedIssues = await _context.Issues.Where(issue => issue.Project == findProjectById).ToListAsync();

                if (removeCreatedIssues.Count <= 0)
                {
                    //continue
                }
                _context.Issues.RemoveRange(removeCreatedIssues);
                _context.Projects.Remove(findProjectById);

                await _context.SaveChangesAsync();

                return "Prject infos removed";
            }
        }


        public async Task<GetProjectDto> GetProjectById(Guid projectId, string mail)
        {
            var user = await FindUser(mail);

            var project = await _context.Projects.FindAsync(projectId); 

            if (project == null)
            {
                throw new Exception($"Project with id {projectId} doesn't exist");
            }

            else
            {
                return new GetProjectDto 
                {
                    Id = project.Id,
                    Name = project.Name,
                    Creator = project.Creator.Email,
                    DateCreated = project.CreatedTime
                };
            }
        }

        public async Task<IEnumerable<GetProjectDto>> GetProjects(Progress? progress, Complexity? complexity, string mail)
        {
            var user = await FindUser(mail);

            IQueryable<Project> query = _context.Projects.Where(findProjects => findProjects.CreatedBy == user.Id);

            if (progress.HasValue)
            {
                query = query.Where(project => project.Progress == progress);
            }

            if(complexity.HasValue)
            {
                query = query.Where(project => project.Complexity == complexity);
            }

            var projectList = await query.ToListAsync();

            if (projectList.Count == 0)
            {
                return new List<GetProjectDto>();
            }

            else
            {
                var projects = projectList.Select(project => new GetProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Creator = project.Creator.Email,
                    DateCreated = project.CreatedTime
                }).ToList();

                return projects;
            }
        }

        public async Task<string> UpdateProject(Guid projectId, string? Name, string? Description, Progress? progress, Complexity? complexity, string mail)
        {
            var user = await FindUser(mail);

            var findProject = await _context.Projects.FindAsync(projectId);

            if (findProject == null)
            {
                throw new Exception("Project doesn't exist");
            }
            else
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    findProject.Name = Name;
                }

                if (!string.IsNullOrEmpty(Description))
                {
                    findProject.Description = Description;
                }

                if (progress.HasValue)
                {
                    findProject.Progress = progress.Value;
                }
                if (complexity.HasValue)
                {
                    findProject.Complexity = complexity.Value;
                }

                findProject.UpdatedBy = user.Id;
                findProject.UpdatedTime = DateTime.Now;

                var updateResponse = _context.Projects.Update(findProject);

                if (updateResponse is null)
                {
                    throw new InvalidOperationException("invalid");
                }
                
                await _context.SaveChangesAsync();

                return "updated";
            }
        }


        private async Task<User> FindUser(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);

            if (user == null)
            {
                throw new Exception("Not found");
            }

            return user;
        }
    }
}
