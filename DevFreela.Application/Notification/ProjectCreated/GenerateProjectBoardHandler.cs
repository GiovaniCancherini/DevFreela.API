using MediatR;

namespace DevFreela.Application.Notification.ProjectCreated
{
    public class GenerateProjectBoardHandler : INotificationHandler<ProjectCreatedNotification>
    {
        public Task Handle(ProjectCreatedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Generating project board for: {notification.Id}");

            return Task.CompletedTask;
        }
    }
}
