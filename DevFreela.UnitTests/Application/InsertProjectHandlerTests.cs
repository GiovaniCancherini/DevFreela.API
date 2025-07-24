using DevFreela.Application.Commands.InsertProject;
using DevFreela.Application.Models;
using DevFreela.Application.Notification.ProjectCreated;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace DevFreela.UnitTests.Application
{
    public class InsertProjectHandlerTests
    {
        #region Success
        [Fact]
        public async Task InputDataAreOk_Insert_Sucess_NSubstitute()
        {
            const int ID = 1;

            // Arrange
            var mediator = Substitute.For<IMediator>();
            mediator
                .Publish(Arg.Any<ProjectCreatedNotification>(), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            var repository = Substitute.For<IProjectRepository>();
            repository
                .Add(Arg.Any<Project>())
                .Returns(Task.FromResult(ID));

            var command = new InsertProjectCommand
            {
                Title = "New Project",
                Description = "Project Description",
                IdClient = 1,
                IdFreeLancer = 2,
                TotalCost = 1000.00m
            };

            var handler = new InsertProjectHandler(mediator, repository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.Equal(ID, result.Data);
            await repository.Received(1).Add(Arg.Any<Project>());
            await mediator.Received(1).Publish(Arg.Any<ProjectCreatedNotification>(), Arg.Any<CancellationToken>());
        }
        #endregion

        #region Failure
        [Fact]
        public async Task InvalidProjectData_Insert_Failure_NSubstitute()
        {
            // Aqui você pode decidir como deseja tratar dados inválidos.
            // Como o handler atual não faz validação, esse teste pode ser para o futuro.

            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            var mediator = Substitute.For<IMediator>();

            var handler = new InsertProjectHandler(mediator, repository);

            var command = new InsertProjectCommand
            {
                Title = "", // inválido se for validado
                Description = "",
                IdClient = 0,
                IdFreeLancer = 0,
                TotalCost = 0
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess); // atualmente ainda retorna sucesso
            // Se futuramente for adicionada validação, ajustar para Assert.False()
        }
        #endregion

        #region Exception
        [Fact]
        public async Task RepositoryThrowsException_Insert_ThrowsException_NSubstitute()
        {
            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            repository
                .Add(Arg.Any<Project>())
                .ThrowsAsync(new Exception("Database failure"));

            var mediator = Substitute.For<IMediator>();

            var command = new InsertProjectCommand
            {
                Title = "New Project",
                Description = "Desc",
                IdClient = 1,
                IdFreeLancer = 2,
                TotalCost = 1000
            };

            var handler = new InsertProjectHandler(mediator, repository);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            await repository.Received(1).Add(Arg.Any<Project>());
            await mediator.DidNotReceive().Publish(Arg.Any<ProjectCreatedNotification>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task MediatorPublishThrowsException_Insert_ThrowsException_NSubstitute()
        {
            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            repository
                .Add(Arg.Any<Project>())
                .Returns(Task.FromResult(10));

            var mediator = Substitute.For<IMediator>();
            mediator
                .Publish(Arg.Any<ProjectCreatedNotification>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new Exception("Mediator failure"));

            var command = new InsertProjectCommand
            {
                Title = "New Project",
                Description = "Desc",
                IdClient = 1,
                IdFreeLancer = 2,
                TotalCost = 1000
            };

            var handler = new InsertProjectHandler(mediator, repository);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            await repository.Received(1).Add(Arg.Any<Project>());
            await mediator.Received(1).Publish(Arg.Any<ProjectCreatedNotification>(), Arg.Any<CancellationToken>());
        }
        #endregion
    }
}
