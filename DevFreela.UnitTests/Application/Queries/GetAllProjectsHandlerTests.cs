using DevFreela.Application.Models;
using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;
using NSubstitute.ExceptionExtensions;

namespace DevFreela.UnitTests.Application.Queries
{
    public class GetAllProjectHandlerTests
    {
        #region Sucess

        [Fact]
        public async Task ReturnOne_GetAllProjects_Sucess_Moq()
        {
            const int ID = 1;

            // Arrange
            var project = new Project("New Project", "Project Description", 1, 2, 1000.00m);
            project.Id = ID;

            var clientTest = new User("Client", "email@email.com", DateTime.Now, "123", "ADM");
            var freelancerTest = new User("Freelancer", "email@email.com", DateTime.Now, "123", "ADM");
            typeof(Project)
                .GetProperty("Client")!
                .SetValue(project, clientTest);

            typeof(Project)
                .GetProperty("Freelancer")!
                .SetValue(project, freelancerTest);


            var repository = Mock.Of<IProjectRepository>(p =>
                p.GetAll(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()) == Task.FromResult(new List<Project> { project })
                );

            var handler = new GetAllProjectsHandler(repository);

            var query = new GetAllProjectsQuery("", 1, 1);

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.True(result.IsSucess);
            Mock.Get(repository).Verify(r => r.GetAll(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task EmptyList_GetAllProjects_Success_Moq()
        {
            // Arrange
            var repository = Mock.Of<IProjectRepository>(r =>
                r.GetAll(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()) == Task.FromResult(new List<Project>())
                );


            var handler = new GetAllProjectsHandler(repository);
            var query = new GetAllProjectsQuery("any search", 1, 10);

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.True(result.IsSucess);
            Assert.Empty((result.Data as List<ProjectItemViewModel>) ?? new List<ProjectItemViewModel>());
            Mock.Get(repository).Verify(r => r.GetAll(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        #endregion

        #region Failure

        [Fact]
        public async Task NullResult_GetAllProjects_Failure_Moq()
        {
            // Arrange
            var repository = Mock.Of<IProjectRepository>(r =>
                r.GetAll(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()) == Task.FromResult((List<Project>?)null)
                );

            var handler = new GetAllProjectsHandler(repository);

            var query = new GetAllProjectsQuery("", 1, 1);

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.True(result.IsSucess); // Ainda é sucesso, mas .Data deve ser null
            Assert.Null(result.Data);
            Mock.Get(repository).Verify(r => r.GetAll(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        #endregion

        #region Exception
        [Fact]
        public async Task RepositoryThrowsException_GetAllProjects_Exception_Moq()
        {
            // Arrange
            var mockRepository = new Mock<IProjectRepository>();

            mockRepository
                .Setup(r => r.GetAll(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("DB is down"));

            var handler = new GetAllProjectsHandler(mockRepository.Object);
            var query = new GetAllProjectsQuery("", 1, 1);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, default));
            Assert.Equal("DB is down", ex.Message);

            // Verify
            mockRepository.Verify(r => r.GetAll(It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
        #endregion
    }
}
