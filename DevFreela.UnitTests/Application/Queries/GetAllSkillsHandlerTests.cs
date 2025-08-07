using DevFreela.Application.Queries.GetAllSkills;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Queries
{
    public class GetAllSkillsHandlerTests
    {
        #region Success

        [Fact]
        public async Task SkillsExist_GetAllSkills_Success_Moq()
        {
            // Arrange
            var skill = new Skill("C#");
            skill.Id = 1;

            var repository = Mock.Of<ISkillRepository>(r =>
                r.GetAll(It.IsAny<string?>()) == Task.FromResult(new List<Skill> { skill })
            );

            var handler = new GetAllSkillsHandler(repository);
            var query = new GetAllSkillsQuery("search");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
            Assert.Equal(skill.Id, result.Data[0].Id);
            Assert.Equal(skill.Description, result.Data[0].Description);
            Mock.Get(repository).Verify(r => r.GetAll(It.IsAny<string?>()), Times.Once);
        }

        [Fact]
        public async Task NoSkills_GetAllSkills_ReturnsEmptyList_Moq()
        {
            // Arrange
            var repository = Mock.Of<ISkillRepository>(r =>
                r.GetAll(It.IsAny<string?>()) == Task.FromResult(new List<Skill>())
            );

            var handler = new GetAllSkillsHandler(repository);
            var query = new GetAllSkillsQuery("search");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Mock.Get(repository).Verify(r => r.GetAll(It.IsAny<string?>()), Times.Once);
        }

        #endregion

        #region Failure

        [Fact]
        public async Task NullResult_GetAllSkills_ReturnsNullData_Moq()
        {
            // Arrange
            var repository = Mock.Of<ISkillRepository>(r =>
                r.GetAll(It.IsAny<string?>()) == Task.FromResult((List<Skill>?)null)
            );

            var handler = new GetAllSkillsHandler(repository);
            var query = new GetAllSkillsQuery("search");

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.Null(result.Data);
            Mock.Get(repository).Verify(r => r.GetAll(It.IsAny<string?>()), Times.Once);
        }

        #endregion

        #region Exception

        [Fact]
        public async Task RepositoryThrowsException_GetAllSkills_Exception_Moq()
        {
            // Arrange
            var mockRepository = new Mock<ISkillRepository>();
            mockRepository
                .Setup(r => r.GetAll(It.IsAny<string?>()))
                .ThrowsAsync(new Exception("DB is down"));

            var handler = new GetAllSkillsHandler(mockRepository.Object);
            var query = new GetAllSkillsQuery("search");

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
            Assert.Equal("DB is down", ex.Message);

            mockRepository.Verify(r => r.GetAll(It.IsAny<string?>()), Times.Once);
        }

        #endregion
    }
}
