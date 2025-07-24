using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using NSubstitute;

namespace DevFreela.UnitTests.Application
{
    public class DeleteProjectHandlerTests
    {
        public const string PROJECT_NOT_FOUND_MESSAGE = "Project not found.";
        public const string PROJECT_DELETED_MESSAGE = "Project is deleted.";

        [Fact]
        public async Task ProjectExists_Delete_Sucess_NSubstitute()
        {
            const int ID = 1;

            // Arrange
            var project = new Project("New Project", "Project Description", 1, 2, 1000.00m);

            var repository = Substitute.For<IProjectRepository>();
            repository
                .GetById(Arg.Any<int>())
                .Returns(Task.FromResult((Project?)project));
            repository
               .Update(Arg.Any<Project>())
               .Returns(Task.CompletedTask);

            var handler = new DeleteProjectHandler(repository);

            var command = new DeleteProjectCommand(ID);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result.IsSucess);
            await repository.Received(1).GetById(1);
            await repository.Received(1).Update(Arg.Any<Project>());
        }

        [Fact]
        public async Task ProjectDoesNotExist_Delete_Failure_NSubstitute()
        {
            const int ID = 1;

            // Arrange
            var project = new Project("New Project", "Project Description", 1, 2, 1000.00m);

            var repository = Substitute.For<IProjectRepository>();
            repository
                .GetById(Arg.Any<int>())
                .Returns(Task.FromResult((Project?)null));
            repository
               .Update(Arg.Any<Project>())
               .Returns(Task.CompletedTask);

            var handler = new DeleteProjectHandler(repository);

            var command = new DeleteProjectCommand(ID);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal(PROJECT_NOT_FOUND_MESSAGE , result.Message);
            await repository.Received(1).GetById(Arg.Any<int>());
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }
    }
}
