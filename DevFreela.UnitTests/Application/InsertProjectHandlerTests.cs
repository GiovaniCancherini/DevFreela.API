using DevFreela.Application.Commands.InsertProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using MediatR;
using NSubstitute;

namespace DevFreela.UnitTests.Application
{
    public class InsertProjectHandlerTests
    {
        [Fact]
        public async Task InputDataAreOk_Insert_Sucess_NSubstitute()
        {
            const int ID = 1;

            // Arrange
            var mediator = Substitute.For<IMediator>();
            mediator
                .Publish(Arg.Any<Mediator>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(1));

            var repository = Substitute.For<IProjectRepository>();
            repository
                .Add(Arg.Any<Project>())
                .Returns(Task.FromResult(ID));

            var command = new InsertProjectCommand
            {
                Title = "New Project",
                Description = "Project Description",
                IdClient = 1,
                IdFreeLancer = 2,
                TotalCost = 1000.00m
            };
            var handler = new InsertProjectHandler(mediator, repository);


            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSucess);
            Assert.Equal(ID, result.Data);
            await repository.Received(1).Add(Arg.Any<Project>());
        }
    }
}
