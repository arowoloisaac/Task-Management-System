using Microsoft.EntityFrameworkCore;
using Project_Manager.Configuration;
using Project_Manager.Data;
using Project_Manager.DTO.OrganizationProjectDto;
using Project_Manager.DTO.ProjectDto;
using Project_Manager.Enum;
using Project_Manager.Model;
using Project_Manager.Service.UserConfiguration;

namespace Project_Manager.Service.OrganizationProjectService
{
    public class OrganizationProjectService : IOrganizationProjectService
    {
        private readonly ApplicationDbContext context;
        private readonly IUserConfig config;

        private const string admin = ApplicationRoleNames.OrganizationAdministrator;

        public OrganizationProjectService(ApplicationDbContext context, IUserConfig config)
        {
            this.context = context;
            this.config = config;
        }

        public Task<string> AssignProjectToGroup(Guid organizationId, Guid groupId, Guid projectId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreateProject(CreateDto dto, Guid organizationId, Guid? groupId, string userId)
        {

            Group? retrieveGroup = null;
            if (groupId.HasValue)
            {
                retrieveGroup = await context.Groups.FindAsync(groupId);
                
                if (retrieveGroup == null)
                {
                    throw new Exception("Group does not exist");
                }
            }

            var retrieveOrganization = await retrieveOrgization(organizationId); 

            var user = await config.GetUserById(userId);

            try
            {
                var validateProject = await context.Projects
                    .Where(dup => dup.Name == dto.Name && dup.Creator.UserName == user.UserName && dup.OrganizationId == organizationId).SingleOrDefaultAsync();

                if (validateProject != null)
                {
                    throw new Exception($"Project with {dto.Name} already exist in this organization");
                }
                else
                {
                    var createProject = new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = dto.Name,
                        Description = dto.Description,
                        CreatedBy = user.Id,
                        Creator = user,
                        CreatedTime = DateTime.UtcNow,
                        Complexity = dto.Complexity,
                        Progress = Progress.Todo,
                        Overview = dto.Overview,
                        Group = retrieveGroup,
                        OrganizationId = organizationId,
                    };
                    context.Projects.Add(createProject);

                    await context.SaveChangesAsync();

                    return "project created";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the project: Server Error: " + ex.Message);
            }
        }

        public async Task<string> DeleteProject(Guid projectId, Guid organizationId, string userId)
        {
            var user = await config.GetUserById(userId);

            var organization = await retrieveOrgization(organizationId);

            var findProjectById = await context.Projects
                .Where(project => project.Id == projectId && project.CreatedBy == user.Id && project.OrganizationId == organizationId).SingleOrDefaultAsync();

            if (findProjectById == null)
            {
                throw new Exception($"Project with id: {projectId} doesn not exist in this organization");
            }
            else
            {
                var removeCreatedIssues = await context.Issues.Where(issue => issue.Project == findProjectById).ToListAsync();

                if (removeCreatedIssues.Count <= 0)
                {
                    //continue
                }
                context.Issues.RemoveRange(removeCreatedIssues);
                context.Projects.Remove(findProjectById);

                await context.SaveChangesAsync();

                return "Project data removed";
            }
        }

        public Task<string> EditProject(Guid projectId, UpdateProjectDto dto, Guid organizationId, string userId)
        {
            
            throw new NotImplementedException();
        }

        //this for the group user
        public async Task<IEnumerable<GetProjectDto>> GetGroupProjects(Guid organizationId, Guid groupId, string userId)
        {
            var retrieveGroup = await retrieveOrgizationGroup(organizationId, groupId);
            
            var retrieveProject = await context.Projects
                .Where(pjt => pjt.OrganizationId == organizationId && pjt.Group == retrieveGroup).ToListAsync();

            if (retrieveProject == null)
            {
                return new List<GetProjectDto>();
            }

            var response = retrieveProject.Select(itm => new GetProjectDto
            {
                Id = itm.Id,
                Name = itm.Name,
                Overview = itm.Overview,
                Complexity = itm.Complexity,
            }).ToList();

            return response;
        }


        //since this will be passed from the frontend, the admin page wont require groupId while the group will (can use 2 diffeent endpoint for this)
        public async Task<GetOrganizationProjectDto> GetProjectById(Guid projectId, string userId, Guid organizationId, Guid? groupId)
        {
            var org = await retrieveOrgization(organizationId);
            var getPt = await retrieveProject(projectId);

            var query = context.Projects
                .Include(p => p.Group)
                .Where(p => p.OrganizationId == organizationId && p.Id == projectId);

            if (groupId.HasValue)
            {
                query = query.Where(p => p.Group.Id == groupId);
            }

            var project = await query.SingleOrDefaultAsync();

            if (project == null)
            {
                throw new InvalidOperationException($"Project with ID {projectId} does not exist or is not accessible.");
            }

            return new GetOrganizationProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                AssignedTo = project.Group?.Name ?? null,
                Overview = project.Overview,
                DateCreated = project.CreatedTime,
                Complexity = project.Complexity,
            };
            /*if (!groupId.HasValue)
            {
                var org = await retrieveOrgization(organizationId);

                var getPt = await retrieveProject(projectId);

                var project = await context.Projects
                    .Include(grp => grp.Group)
                    .Where(pjt => pjt.OrganizationId == organizationId && pjt.Id == projectId).SingleOrDefaultAsync();

                if (project == null)
                {
                    return new GetOrganizationProjectDto();
                }
                else
                {
                    return new GetOrganizationProjectDto
                    {
                        Id = project.Id,
                        Name = project.Name,
                        Description = project.Description,
                        AssignedTo = project.Group.Name ?? null,
                        Overview = project.Overview,
                        DateCreated = project.CreatedTime,
                        Complexity = project.Complexity,
                    };
                }
            }
            else
            {
                var orgGrp = await retrieveOrgizationGroup(organizationId, groupId.Value);

                var getPt = await retrieveProject(projectId);

                var project = await context.Projects
                    .Include(grp => grp.Group)
                    .Where(pjt => pjt.OrganizationId == organizationId && pjt.Id == projectId && pjt.Group.Id == groupId).SingleOrDefaultAsync();

                if (project == null)
                {
                    return new GetOrganizationProjectDto();
                }
                else
                {
                    return new GetOrganizationProjectDto
                    {
                        Id = project.Id,
                        Name = project.Name,
                        Description = project.Description,
                        Overview = project.Overview,
                        DateCreated = project.CreatedTime,
                        Complexity = project.Complexity,
                    };
                }
            }*/
        }

        //this is for the members of the organization
        public async Task<ProjectResponse> GetProjectPaginated(Progress? progress, Complexity? complexity, int? page, int itemPerPage, Guid organizationId, Guid groupId, string userId)
        {
            throw new NotImplementedException();
        }

        //for the admin
        public async Task<IEnumerable<GetProjectDto>> GetProjects(Progress? progress, Complexity? complexity, bool isAssigned, Guid organizationId, string userId) 
        {
            var org = await retrieveOrgization(organizationId);

            IQueryable<Project> query = context.Projects.Where(findProjects => findProjects.OrganizationId == organizationId);

            if (progress.HasValue)
            {
                query = query.Where(project => project.Progress == progress);
            }

            if (complexity.HasValue)
            {
                query = query.Where(project => project.Complexity == complexity); 
            }
            
            if (isAssigned == true)
            {
                query = query.Where(project => project.Group != null);
            }

            if (isAssigned != true)
            {
                query = query.Where(project => project.Group == null);
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
                    Description = project.Description,
                    Progress = project.Progress,
                    Complexity = project.Complexity,
                    Overview = project.Overview
                }).ToList();

                return projects;
            }
        }


        public Task<string> UpdateProject(Guid projectId, string? name, string? description, Progress? progress, Complexity? complexity, string userId)
        {
            throw new NotImplementedException();
        }


        private async Task<Organization> retrieveOrgization(Guid organizationId)
        {
            var org = await context.Organizations.FindAsync(organizationId);

            if (org == null)
            {
                throw new Exception("Organization does not exist");
            }
            return org;
        }

        private async Task<Group> retrieveOrgizationGroup(Guid organizationId, Guid groupId)
        {
            var org = await retrieveOrgization(organizationId);

            var retrieveGroup = await context.Groups.Where(og => og.Id == groupId && og.OrganizationId == org.Id).FirstOrDefaultAsync();

            if (retrieveGroup == null)
            {
                throw new Exception("Group does not exist in the organization");
            }

            return retrieveGroup;
        }

        private async Task<Project> retrieveProject(Guid projectId)
        {
            var project = await context.Projects.FindAsync(projectId);

            if (project == null)
            {
                throw new Exception("Project does not exist");
            }
            return project;
        }

        public Task<string> UnassignProjectToGroup(Guid organizationId, Guid groupId, Guid projectId)
        {
            throw new NotImplementedException();
        }
    }
}
