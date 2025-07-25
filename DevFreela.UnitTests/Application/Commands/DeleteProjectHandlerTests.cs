using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace DevFreela.UnitTests.Application.Commands
{
    public class DeleteProjectHandlerTests
    {
        #region Sucess
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
        public async Task ProjectExists_Delete_Sucess_Moq()
        {
            const int ID = 1;

            // Arrange
            var project = new Project("New Project", "Project Description", 1, 2, 1000.00m);

            var repository = Mock.Of<IProjectRepository>(p => 
                p.GetById(It.IsAny<int>()) == Task.FromResult(project) && 
                p.Update(It.IsAny<Project>()) == Task.CompletedTask
                );

            var handler = new DeleteProjectHandler(repository);

            var command = new DeleteProjectCommand(ID);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.True(result.IsSucess);
            Mock.Get(repository).Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
            Mock.Get(repository).Verify(r => r.Update(It.IsAny<Project>()), Times.Once);
        }
        #endregion

        #region Failure
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
            Assert.Equal(DeleteProjectHandler.PROJECT_NOT_FOUND_MESSAGE, result.Message);
            await repository.Received(1).GetById(Arg.Any<int>());
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }

        [Fact]
        public async Task ProjectDoesNotExist_Delete_Failure_Moq()
        {
            const int ID = 1;

            // Arrange
            var project = new Project("New Project", "Project Description", 1, 2, 1000.00m);

            var repository = Mock.Of<IProjectRepository>(p =>
                p.GetById(It.IsAny<int>()) == Task.FromResult((Project?)null) &&
                p.Update(It.IsAny<Project>()) == Task.CompletedTask
                );

            var handler = new DeleteProjectHandler(repository);

            var command = new DeleteProjectCommand(ID);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal(DeleteProjectHandler.PROJECT_NOT_FOUND_MESSAGE, result.Message);
            Mock.Get(repository).Verify(r => r.GetById(ID), Times.Once);
            Mock.Get(repository).Verify(r => r.Update(It.IsAny<Project>()), Times.Never);
        }

        [Fact]
        public async Task ProjectIsAlreadyDeleted_Delete_Failure_NSubstitute()
        {
            const int ID = 1;

            // Arrange
            var project = new Project("New Project", "Project Description", 1, 2, 1000.00m);
            project.SetAsDeleted(); // simula projeto deletado

            var repository = Substitute.For<IProjectRepository>();
            repository
                .GetById(ID)
                .Returns(Task.FromResult((Project?)project));

            var handler = new DeleteProjectHandler(repository);
            var command = new DeleteProjectCommand(ID);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal(DeleteProjectHandler.PROJECT_DELETED_MESSAGE, result.Message);
            await repository.Received(1).GetById(ID);
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }
        #endregion

        #region Exception
        [Fact]
        public async Task RepositoryThrowsException_Delete_Exception_NSubstitute()
        {
            const int ID = 1;

            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            repository
                .GetById(ID)
                .ThrowsAsync(new Exception("Unexpected database error"));

            var handler = new DeleteProjectHandler(repository);
            var command = new DeleteProjectCommand(ID);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            await repository.Received(1).GetById(ID);
        }
        #endregion
    }
}
