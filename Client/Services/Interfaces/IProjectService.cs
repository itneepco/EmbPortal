using EmbPortal.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectResponse>> GetAllProjects();
    }
}
