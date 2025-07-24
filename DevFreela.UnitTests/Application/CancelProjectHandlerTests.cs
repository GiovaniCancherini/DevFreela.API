using DevFreela.Application.Commands.CancelProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace DevFreela.UnitTests.Application
{
    public class CancelProjectHandlerTests
    {
        private const string PROJECT_NOT_FOUND_MESSAGE = "Project not found.";
        private const string PROJECT_DELETED_MESSAGE = "Project is deleted.";

        #region Sucess
        [Fact]
        public async Task ProjectExists_Cancel_Success_NSubstitute()
        {
            const int ID = 1;

            // Arrange
            var project = new Project("Projeto 1", "Descricao", 1, 2, 1000m);

            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(ID).Returns(project);
            repository.Update(Arg.Any<Project>()).Returns(Task.CompletedTask);

            var handler = new CancelProjectHandler(repository);
            var command = new CancelProjectCommand(ID);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            await repository.Received(1).GetById(ID);
            await repository.Received(1).Update(project);
        }
        #endregion

        #region Failure
        [Fact]
        public async Task ProjectNotFound_Cancel_Failure_NSubstitute()
        {
            const int ID = 1;

            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(ID).Returns((Project?)null);

            var handler = new CancelProjectHandler(repository);
            var command = new CancelProjectCommand(ID);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal(PROJECT_NOT_FOUND_MESSAGE, result.Message);
            await repository.Received(1).GetById(ID);
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }

        [Fact]
        public async Task ProjectIsDeleted_Cancel_Failure_NSubstitute()
        {
            const int ID = 1;

            // Arrange
            var project = new Project("Projeto 2", "Descricao", 1, 2, 500m);
            project.SetAsDeleted();

            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(ID).Returns(project);

            var handler = new CancelProjectHandler(repository);
            var command = new CancelProjectCommand(ID);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal(PROJECT_DELETED_MESSAGE, result.Message);
            await repository.Received(1).GetById(ID);
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }
        #endregion

        #region Exception
        [Fact]
        public async Task UnexpectedError_Cancel_Exception_NSubstitute()
        {
            const int ID = 1;

            // Arrange
            var project = new Project("Projeto 3", "Descricao", 1, 2, 800m);
            var repository = Substitute.For<IProjectRepository>();

            repository.GetById(ID).Returns(project);
            repository.Update(Arg.Any<Project>()).Throws(new Exception("Unexpected error"));

            var handler = new CancelProjectHandler(repository);
            var command = new CancelProjectCommand(ID);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal("Unexpected error", exception.Message);

            await repository.Received(1).GetById(ID);
            await repository.Received(1).Update(project);
        }
        #endregion
    }
}
