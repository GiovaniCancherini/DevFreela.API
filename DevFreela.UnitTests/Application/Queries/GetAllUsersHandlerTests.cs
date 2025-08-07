using DevFreela.Application.Queries.GetAllUsers;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Queries
{
    public class GetAllUsersHandlerTests
    {
        #region Success

        [Fact]
        public async Task UsersExist_GetAllUsers_Success_Moq()
        {
            // Arrange
            var user = new User("User Name", "user@email.com", DateTime.Now, "123", "ADM");
            user.Id = 1;

            var repository = Mock.Of<IUserRepository>(r =>
                r.GetAll(It.IsAny<string?>()) == Task.FromResult(new List<User> { user })
            );

            var handler = new GetAllUsersHandler(repository);
            var query = new GetAllUsersQuery("search");

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.True(result.IsSucess);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
            Assert.Equal(user.FullName, result.Data[0].FullName);
            Mock.Get(repository).Verify(r => r.GetAll(It.IsAny<string?>()), Times.Once);
        }

        [Fact]
        public async Task NoUsers_GetAllUsers_ReturnsEmptyList_Moq()
        {
            // Arrange
            var repository = Mock.Of<IUserRepository>(r =>
                r.GetAll(It.IsAny<string?>()) == Task.FromResult(new List<User>())
            );

            var handler = new GetAllUsersHandler(repository);
            var query = new GetAllUsersQuery("search");

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.True(result.IsSucess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
            Mock.Get(repository).Verify(r => r.GetAll(It.IsAny<string?>()), Times.Once);
        }

        #endregion

        #region Failure

        [Fact]
        public async Task NullResult_GetAllUsers_ReturnsNullData_Moq()
        {
            // Arrange
            var repository = Mock.Of<IUserRepository>(r =>
                r.GetAll(It.IsAny<string?>()) == Task.FromResult((List<User>?)null)
            );

            var handler = new GetAllUsersHandler(repository);
            var query = new GetAllUsersQuery("search");

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            Assert.True(result.IsSucess);
            Assert.Null(result.Data);
            Mock.Get(repository).Verify(r => r.GetAll(It.IsAny<string?>()), Times.Once);
        }

        #endregion

        #region Exception

        [Fact]
        public async Task RepositoryThrowsException_GetAllUsers_Exception_Moq()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            mockRepository
                .Setup(r => r.GetAll(It.IsAny<string?>()))
                .ThrowsAsync(new Exception("DB is down"));

            var handler = new GetAllUsersHandler(mockRepository.Object);
            var query = new GetAllUsersQuery("search");

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, default));
            Assert.Equal("DB is down", ex.Message);

            mockRepository.Verify(r => r.GetAll(It.IsAny<string?>()), Times.Once);
        }

        #endregion
    }
}
