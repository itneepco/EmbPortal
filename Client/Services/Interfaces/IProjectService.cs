using EmbPortal.Shared.Requests;
using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectResponse>> GetAllProjects();
        Task<IResult<int>> CreateProject(ProjectRequest request);
        Task<IResult> UpdateProject(int id, ProjectRequest request);
        Task<IResult> DeleteProject(int id);
    }
}
