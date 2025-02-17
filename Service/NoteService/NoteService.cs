using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.DTO.NoteDto;
using Project_Manager.Model;
using Project_Manager.Service.UserConfiguration;
using System.Runtime.InteropServices;

namespace Project_Manager.Service.NoteService
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserConfig _userConfig;

        public NoteService(ApplicationDbContext dbContext, IUserConfig userConfig)
        {
            _context = dbContext;   
            _userConfig = userConfig;
        }

        public async Task CreateNote(CreateNoteDto dto, Guid projectId, Guid? issueId, string userId)
        {
            var user = await _userConfig.GetUserById(userId);

            var retrieveProject = await _context.Projects.FindAsync(projectId);

            if (retrieveProject == null)
            {
                throw new Exception("This project does not exist");
            }
            else
            {
                Note note;
                if (issueId == null)
                {
                    note = new Note
                    {
                        Content = dto.Content,
                        ProjectId = projectId,
                        CreatedBy = user.Id,
                        Users = user
                    };
                    _context.Notes.Add(note);
                }
                else
                {
                    var retrieveIssue = await _context.Issues.Where(itm => itm.Id == issueId && itm.Project.Id == retrieveProject.Id).SingleOrDefaultAsync();

                    if (retrieveIssue == null)
                    {
                        throw new Exception("This issue does not exist");
                    }
                    else
                    {
                        note = new Note
                        {
                            Content = dto.Content,
                            ProjectId = projectId,
                            IssueId = issueId,
                            CreatedBy = user.Id,
                            Users = user
                        };
                        _context.Notes.Add(note);
                    }
                }
                await _context.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        public async Task DeleteNote( Guid noteId, string userId)
        {
            var user = await _userConfig.GetUserById(userId);

            var retrieveNote = await _context.Notes.Where(note => note.Id == noteId && note.CreatedBy == user.Id).SingleOrDefaultAsync();

            if (retrieveNote == null)
            {
                throw new Exception("Note doesn't exist");
            }

            _context.Remove(retrieveNote);

            await _context.SaveChangesAsync();
        }

        public async Task<Note> GetNote( Guid noteId, string userId)
        {
            var user = await _userConfig.GetUserById(userId);

            var retrieveNote = await _context.Notes.Where(note => note.Id == noteId && note.CreatedBy == user.Id).SingleOrDefaultAsync();

            if (retrieveNote == null)
            {
                throw new Exception("Note doesn't exist");
            }

            return retrieveNote;
        }

        public async Task<IEnumerable<NoteDto>> GetNotes(Guid projectId, Guid? issueId, string userId)
        {
            var user = await _userConfig.GetUserById(userId);

            var retrieveProject = await _context.Projects.FindAsync(projectId);

            if (retrieveProject == null)
            {
                throw new Exception("This project does not exist");
            }
            else
            {
                if (issueId == null)
                {
                    var retrieveNote = await _context.Notes.Where(item => item.ProjectId == projectId && item.IssueId == null).ToListAsync();

                    if(retrieveNote == null)
                    {
                        return Enumerable.Empty<NoteDto>();
                    }

                    var result = retrieveNote.Select(lst => new NoteDto
                    {
                        Content = lst.Content,
                        Id = lst.Id,
                        CreatedDate = lst.CreatedDate,
                        ModifiedDate = lst.ModifiedDate,
                    }).ToList();

                    await Task.CompletedTask;
                    return result;
                }
                else
                {
                    var retrieveIssue = await _context.Issues.FindAsync(issueId);

                    if (retrieveIssue == null)
                    {
                        throw new ArgumentException("The issue does not exist");
                    }

                    var retrieveNote = await _context.Notes.Where(item => item.ProjectId == projectId && item.IssueId == issueId).ToListAsync();

                    if (retrieveNote == null)
                    {
                        return Enumerable.Empty<NoteDto>();
                    }

                    var result = retrieveNote.Select(lst => new NoteDto
                    {
                        Content = lst.Content,
                        Id = lst.Id,
                        CreatedDate = lst.CreatedDate,
                        ModifiedDate = lst.ModifiedDate,
                    }).ToList();

                    await Task.CompletedTask;
                    return result;
                }
            }
        }

        public async Task<string> UpdateNote(Guid noteId, string content, string userId)
        {
            var user = await _userConfig.GetUserById(userId);

            var retrieveNote = await _context.Notes.Where(note => note.Id == noteId && note.CreatedBy == user.Id).SingleOrDefaultAsync();

            if (retrieveNote == null)
            {
                throw new Exception("Note doesn't exist");
            }

            retrieveNote.Content = content;

            await _context.SaveChangesAsync();

            await Task.CompletedTask;
            return "Successfully updated";
        }
    }
}
