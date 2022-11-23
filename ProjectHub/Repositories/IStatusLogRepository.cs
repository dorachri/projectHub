using ProjectHub.Models;
using System.Collections.Generic;

namespace ProjectHub.Repositories
{
    public interface IStatusLogRepository
    {
        IEnumerable<StatusLog> GetAllProjectStatusLogs(int projectId);

        void AddStatusLog(StatusLog statusLog);

        void EditStatusLog(StatusLog statusLog);

        void DeleteStatusLog(int statusLogId);

        void DeleteAllProjectsStatusLogs(int projectId);

        StatusLog GetStatusLogsById(int statusLogId);
    }
}
