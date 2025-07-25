using DevFreela.Application.Commands.InsertCommentInProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace DevFreela.UnitTests.Application.Commands
{
    public class InsertCommentInProjectHandlerTests
    {
        #region Success
        [Fact]
        public async Task ProjectExists_InsertComment_Success_NSubstitute()
        {
            const int ID_PROJECT = 1;
            const int ID_USER = 2;
            const int COMMENT_ID = 123;

            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            repository.Exists(ID_PROJECT).Returns(true);
            repository.When(r => r.AddComment(Arg.Any<ProjectComment>()))
                      .Do(ci => ci.Arg<ProjectComment>().Id = COMMENT_ID);

            var handler = new InsertCommentInProjectHandler(repository);
            var command = new InsertCommentInProjectCommand("This is a comment", ID_PROJECT, ID_USER);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.Equal(COMMENT_ID, result.Data);
            await repository.Received(1).Exists(ID_PROJECT);
            await repository.Received(1).AddComment(Arg.Any<ProjectComment>());
        }
        #endregion

        #region Failure
        [Fact]
        public async Task ProjectNotExist_InsertComment_Failure_NSubstitute()
        {
            const int ID_PROJECT = 1;
            const int ID_USER = 2;

            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            repository.Exists(ID_PROJECT).Returns(false);

            var handler = new InsertCommentInProjectHandler(repository);
            var command = new InsertCommentInProjectCommand("Invalid comment", ID_PROJECT, ID_USER);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSucess);
            Assert.Equal(InsertCommentInProjectHandler.PROJECT_NOT_EXIST_MESSAGE, result.Message);
            await repository.Received(1).Exists(ID_PROJECT);
            await repository.DidNotReceive().AddComment(Arg.Any<ProjectComment>());
        }
        #endregion

        #region Exception
        [Fact]
        public async Task UnexpectedError_InsertComment_Exception_NSubstitute()
        {
            const int ID_PROJECT = 1;
            const int ID_USER = 2;

            // Arrange
            var repository = Substitute.For<IProjectRepository>();
            repository.Exists(ID_PROJECT).Returns(true);
            repository.AddComment(Arg.Any<ProjectComment>())
                      .Throws(new Exception("Unexpected failure"));

            var handler = new InsertCommentInProjectHandler(repository);
            var command = new InsertCommentInProjectCommand("Some comment", ID_PROJECT, ID_USER);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.Equal("Unexpected failure", exception.Message);
            await repository.Received(1).Exists(ID_PROJECT);
            await repository.Received(1).AddComment(Arg.Any<ProjectComment>());
        }
        #endregion
    }
}
