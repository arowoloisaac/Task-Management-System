using Project_Manager.DTO.NoteDto;
using Project_Manager.Model;

namespace Project_Manager.Service.NoteService
{
    public interface INoteService
    {
        Task CreateNote(CreateNoteDto noteDto, Guid projectId, Guid? issueId, string userId);

        Task DeleteNote(Guid noteId, string userId);

        Task<IEnumerable<NoteDto>> GetNotes(Guid projectId, Guid? issueId, string userId);

        Task<Note> GetNote(Guid noteId, string userId);

        Task<string> UpdateNote(Guid noteId, string content, string userId);
    }
}
