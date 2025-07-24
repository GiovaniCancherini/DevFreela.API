using DevFreela.Application.Commands.InsertSkill;
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
    public class InsertSkillHandlerTests
    {
        #region Success

        [Fact]
        public async Task GivenValidSkill_InsertSkill_Success_NSubstitute()
        {
            // Arrange
            const int expectedId = 1;

            var repository = Substitute.For<ISkillRepository>();
            repository.Add(Arg.Any<Skill>())
                      .Returns(Task.FromResult(expectedId));

            var handler = new InsertSkillHandler(repository);

            var command = new InsertSkillCommand("C#");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.Equal(expectedId, result.Data);
            await repository.Received(1).Add(Arg.Any<Skill>());
        }

        #endregion

        #region Failure

        // Currently no failure branch in handler,
        // add here if validation or business rules are introduced.

        #endregion

        #region Exception

        [Fact]
        public async Task GivenRepositoryThrowsException_InsertSkill_ThrowsException_NSubstitute()
        {
            // Arrange
            var repository = Substitute.For<ISkillRepository>();
            repository.Add(Arg.Any<Skill>())
                      .Returns<Task<int>>(x => throw new Exception("Database error"));

            var handler = new InsertSkillHandler(repository);

            var command = new InsertSkillCommand("C#");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal("Database error", exception.Message);
            await repository.Received(1).Add(Arg.Any<Skill>());
        }

        #endregion
    }
}
