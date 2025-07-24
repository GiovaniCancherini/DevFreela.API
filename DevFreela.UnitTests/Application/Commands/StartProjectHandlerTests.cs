using DevFreela.Application.Commands.StartProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using DevFreela.Core.Repositories;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace DevFreela.UnitTests.Application.Commands
{
    public class StartProjectHandlerTests
    {
        private const int PROJECT_ID = 1;

        #region Success
        [Fact]
        public async Task ValidProject_Start_Success_NSubstitute()
        {
            // Arrange
            var project = new Project("Projeto Teste", "Descrição", 1, 2, 1500m);
            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(PROJECT_ID).Returns(project);

            var handler = new StartProjectHandler(repository);
            var command = new StartProjectCommand(PROJECT_ID);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.NotNull(result.Message);
            Assert.Equal(ProjectStatusEnum.InProgress, project.Status);
            Assert.NotNull(project.StartedAt);

            await repository.Received(1).GetById(PROJECT_ID);
            await repository.Received(1).Update(project);
        }
        #endregion

        #region Failure
        [Fact]
        public async Task ProjectNotFound_Start_Failure_NSubstitute()
        {
            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(PROJECT_ID).Returns((Project?)null);

            var handler = new StartProjectHandler(repository);
            var command = new StartProjectCommand(PROJECT_ID);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal("Project not found.", result.Message);

            await repository.Received(1).GetById(PROJECT_ID);
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }

        [Fact]
        public async Task ProjectIsDeleted_Start_Failure_NSubstitute()
        {
            // Arrange
            var project = new Project("Deletado", "Desc", 1, 2, 1000m);
            project.SetAsDeleted();

            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(PROJECT_ID).Returns(project);

            var handler = new StartProjectHandler(repository);
            var command = new StartProjectCommand(PROJECT_ID);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal("Project is deleted.", result.Message);

            await repository.Received(1).GetById(PROJECT_ID);
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }
        #endregion

        #region Exception
        [Fact]
        public async Task UnexpectedException_Start_Exception_NSubstitute()
        {
            // Arrange
            var project = new Project("Projeto", "Desc", 1, 2, 1000m);
            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(PROJECT_ID).Returns(project);
            repository.Update(Arg.Any<Project>()).Throws(new Exception("Erro no banco"));

            var handler = new StartProjectHandler(repository);
            var command = new StartProjectCommand(PROJECT_ID);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.Equal("Erro no banco", ex.Message);
            await repository.Received(1).GetById(PROJECT_ID);
            await repository.Received(1).Update(project);
        }
        #endregion
    }
}
