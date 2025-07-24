using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace DevFreela.UnitTests.Application.Commands
{
    public class UpdateProjectHandlerTests
    {
        public const string PROJECT_NOT_FOUND_MESSAGE = "Project not found.";
        public const string PROJECT_DELETED_MESSAGE = "Project is deleted.";

        private const int PROJECT_ID = 1;

        #region Success
        [Fact]
        public async Task ValidProjectAndValidData_Update_Sucess_NSubstitute()
        {
            // Arrange
            var project = new Project("Old Title", "Old Description", 1, 2, 500);
            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(PROJECT_ID).Returns(project);

            var command = new UpdateProjectCommand(PROJECT_ID, "New Title", "New Description", 1000);
            var handler = new UpdateProjectHandler(repository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.NotNull(result.Message);

            await repository.Received(1).GetById(PROJECT_ID);
            await repository.Received(1).Update(project);
        }
        #endregion

        #region Failure
        [Fact]
        public async Task TitleTooLong_Update_Failure_NSubstitute()
        {
            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            var longTitle = new string('A', 51);
            var command = new UpdateProjectCommand(PROJECT_ID, longTitle, "Description", 1000);

            var handler = new UpdateProjectHandler(repository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal("50 characters maximum for the title.", result.Message);

            await repository.DidNotReceive().GetById(Arg.Any<int>());
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }

        [Fact]
        public async Task DescriptionTooLong_Update_Failure_NSubstitute()
        {
            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            var longDescription = new string('B', 201);
            var command = new UpdateProjectCommand(PROJECT_ID, "Title", longDescription, 1000);

            var handler = new UpdateProjectHandler(repository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal("200 characters maximum for description.", result.Message);

            await repository.DidNotReceive().GetById(Arg.Any<int>());
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }

        [Fact]
        public async Task ProjectNotFound_Update_Failure_NSubstitute()
        {
            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(PROJECT_ID).Returns((Project?)null);

            var command = new UpdateProjectCommand(PROJECT_ID, "Title", "Description", 1000);
            var handler = new UpdateProjectHandler(repository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal(PROJECT_NOT_FOUND_MESSAGE, result.Message);

            await repository.Received(1).GetById(PROJECT_ID);
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }

        [Fact]
        public async Task ProjectIsDeleted_Update_Failure_NSubstitute()
        {
            // Arrange
            var project = new Project("Title", "Desc", 1, 2, 1000);
            project.SetAsDeleted();

            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(PROJECT_ID).Returns(project);

            var command = new UpdateProjectCommand(PROJECT_ID, "Title", "Description", 1000);
            var handler = new UpdateProjectHandler(repository);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal(PROJECT_DELETED_MESSAGE, result.Message);

            await repository.Received(1).GetById(PROJECT_ID);
            await repository.DidNotReceive().Update(Arg.Any<Project>());
        }
        #endregion

        #region Exception
        [Fact]
        public async Task UnexpectedException_Update_Exception_NSubstitute()
        {
            // Arrange
            var project = new Project("Title", "Description", 1, 2, 1000);
            var repository = Substitute.For<IProjectRepository>();
            repository.GetById(PROJECT_ID).Returns(project);
            repository.Update(Arg.Any<Project>()).Throws(new Exception("Database error"));

            var command = new UpdateProjectCommand(PROJECT_ID, "New Title", "New Desc", 1500);
            var handler = new UpdateProjectHandler(repository);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.Equal("Database error", ex.Message);

            await repository.Received(1).GetById(PROJECT_ID);
            await repository.Received(1).Update(project);
        }
        #endregion
    }
}
