using DevFreela.Application.Commands.InsertUser;
using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevFreela.UnitTests.Application.Commands
{
    public class InsertUserHandlerTests
    {
        #region Success

        [Fact]
        public async Task GivenValidUser_InsertUser_Success_NSubstitute()
        {
            // Arrange
            const int expectedId = 1;

            var repository = Substitute.For<IUserRepository>();
            repository.Add(Arg.Any<User>())
                      .Returns(Task.FromResult(expectedId));

            var handler = new InsertUserHandler(repository);

            var command = new InsertUserCommand(
                "John Doe",
                "john.doe@example.com",
                new DateTime(1990, 1, 1)
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.Equal(expectedId, result.Data);
            await repository.Received(1).Add(Arg.Any<User>());
        }

        #endregion

        #region Failure

        // No failure branch in handler logic currently,
        // but if you add validation (ex: email format), tests would go here.

        #endregion

        #region Exception

        [Fact]
        public async Task GivenRepositoryThrowsException_InsertUser_ThrowsException_NSubstitute()
        {
            // Arrange
            var repository = Substitute.For<IUserRepository>();
            repository.Add(Arg.Any<User>())
                      .Returns<Task<int>>(x => throw new Exception("Database error"));

            var handler = new InsertUserHandler(repository);

            var command = new InsertUserCommand(
                "John Doe",
                "john.doe@example.com",
                new DateTime(1990, 1, 1)
            );

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal("Database error", exception.Message);
            await repository.Received(1).Add(Arg.Any<User>());
        }

        #endregion
    }
}
