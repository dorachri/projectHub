using Microsoft.EntityFrameworkCore;
using ProjectHub.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHub.Repositories
{
    public class StatusLogRepository : IStatusLogRepository
    {
        private readonly AppDbContext _appDbContext;

        public StatusLogRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddStatusLog(StatusLog statusLog)
        {
            _appDbContext.StatusLogs.Add(statusLog);
            _appDbContext.SaveChanges();
        }

        public void DeleteAllProjectsStatusLogs(int projectId)
        {
            var statusLogs = _appDbContext.StatusLogs.Where(log => log.ProjectId == projectId);

            foreach (var statusLog in statusLogs.ToList())
            {
                _appDbContext.StatusLogs.Remove(statusLog);
                _appDbContext.SaveChanges();
            }
        }

        public void DeleteStatusLog(int statusLogId)
        {
            StatusLog statusLog = _appDbContext.StatusLogs.FirstOrDefault(log => log.StatusLogId == statusLogId);
            _appDbContext.StatusLogs.Remove(statusLog);
            _appDbContext.SaveChanges();
        }

        public void EditStatusLog(StatusLog statusLog)
        {
            _appDbContext.StatusLogs.Update(statusLog);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<StatusLog> GetAllProjectStatusLogs(int projectId)
        {
            return _appDbContext.StatusLogs
                .Where(log => log.ProjectId == projectId)
                .Include(log => log.Project)
                .Include(log => log.Project.Student)
                .Include(log => log.Project.Professor)
                .Include(log => log.Project.Company);
        }

        public StatusLog GetStatusLogsById(int statusLogId)
        {
            return _appDbContext.StatusLogs.FirstOrDefault(log => log.StatusLogId == statusLogId);
        }
    }
}
