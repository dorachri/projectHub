using Microsoft.AspNetCore.SignalR;
using ProjectHub.Repositories;
using System.Threading.Tasks;

namespace ProjectHub.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IProjectRepository _projectRepository;

        public NotificationHub(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task SendNotification(string projectCode, string role)
        {
            var project = _projectRepository.GetProjectByCode(projectCode);

            switch (role)
            {
                case "Student":
                    await Clients.Users(project.Professor.UserId, project.Company.UserId).SendAsync("ReceiveNotification");
                    break;
                case "Professor":
                    await Clients.Users(project.Student.UserId, project.Company.UserId).SendAsync("ReceiveNotification");
                    break;
                case "Company":
                    await Clients.Users(project.Professor.UserId, project.Student.UserId).SendAsync("ReceiveNotification");
                    break;
            }
        }
    }
}
