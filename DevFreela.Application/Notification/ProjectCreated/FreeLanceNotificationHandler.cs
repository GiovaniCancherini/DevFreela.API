using MediatR;

namespace DevFreela.Application.Notification.ProjectCreated
{
    public class FreeLanceNotificationHandler : INotificationHandler<ProjectCreatedNotification>
    {
        public Task Handle(ProjectCreatedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Project Created: {notification.Id} - {notification.Title} - {notification.TotalCost}");

            return Task.CompletedTask;
        }
    }
}
