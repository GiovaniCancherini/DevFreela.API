using DevFreela.Application.Commands.InsertSkillsInUser;
using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevFreela.UnitTests.Application.Commands
{
    public class InsertSkillsInUserHandlerTests
    {
        #region Success

        [Fact]
        public async Task GivenValidSkillIds_InsertSkillsInUser_Success_NSubstitute()
        {
            // Arrange
            var userId = 1;
            var skillIds = new List<int> { 1, 2, 3 };

            var repository = Substitute.For<IUserRepository>();
            repository
                .AddSkills(Arg.Any<List<UserSkill>>())
                .Returns(Task.CompletedTask);

            var handler = new InsertSkillsInUserHandler(repository);

            var command = new InsertSkillsInUserCommand(skillIds.ToArray(), userId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.Equal(skillIds.Count, result.Data);
            await repository.Received(1).AddSkills(Arg.Any<List<UserSkill>>());
        }

        #endregion

        #region Failure

        // Currently the handler does not return failure states.
        // Add tests here if business rules or validations are added.

        #endregion

        #region Exception

        [Fact]
        public async Task GivenRepositoryThrowsException_InsertSkillsInUser_ThrowsException_NSubstitute()
        {
            // Arrange
            var userId = 1;
            var skillIds = new List<int> { 1, 2 };

            var repository = Substitute.For<IUserRepository>();
            repository
                .AddSkills(Arg.Any<List<UserSkill>>())
                .Returns<Task>(x => throw new Exception("Database error"));

            var handler = new InsertSkillsInUserHandler(repository);

            var command = new InsertSkillsInUserCommand(skillIds.ToArray(), userId);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal("Database error", exception.Message);
            await repository.Received(1).AddSkills(Arg.Any<List<UserSkill>>());
        }

        #endregion
    }
}
