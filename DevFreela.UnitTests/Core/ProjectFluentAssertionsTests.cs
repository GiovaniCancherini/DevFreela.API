using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using FluentAssertions;

namespace DevFreela.UnitTests.Core
{
    public class ProjectFluentAssertionsTests
    {
        #region Start

        #region Sucess
        [Fact]
        public void ProjectIsCreated_Start_Sucess()
        {
            // Arrange
            var project = new Project("Projeto A", "Descricao Projeto", 1, 2, 1000.1m);

            // Act
            project.Start();

            // Assert
            project.Status.Should().Be(ProjectStatusEnum.InProgress);
            project.StartedAt.Should().NotBeNull();
        }
        #endregion

        #region Failure
        // Nenhum teste aqui ainda
        #endregion

        #region Exception
        [Fact]
        public void ProjectIsInvalidState_Start_ThrowsException()
        {
            // Arrange
            var project = new Project("Projeto A", "Descricao Projeto", 1, 2, 1000.1m);
            project.Start();

            // Act
            Action start = project.Start;

            // Assert
            start.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(Project.INVALID_STATE_MESSAGE);
        }
        #endregion

        #endregion

        #region Cancel

        #region Sucess
        [Fact]
        public void ProjectInProgress_Cancel_Success()
        {
            // Arrange
            var project = new Project("Projeto A", "Desc", 1, 2, 500);
            project.Start();

            // Act
            project.Cancel();

            // Assert
            project.Status.Should().Be(ProjectStatusEnum.Cancelled);
            project.FinishedAt.Should().NotBeNull();
        }
        #endregion

        #region Failure
        [Fact]
        public void ProjectNotInProgress_Cancel_NoChange()
        {
            // Arrange
            var project = new Project("Projeto A", "Desc", 1, 2, 500);

            // Act
            project.Cancel();

            // Assert
            project.Status.Should().Be(ProjectStatusEnum.Created);
            project.FinishedAt.Should().BeNull();
        }
        #endregion

        #region Exception
        // Nenhum teste aqui ainda
        #endregion

        #endregion

        #region Complete

        #region Sucess
        [Theory]
        [InlineData(ProjectStatusEnum.InProgress)]
        [InlineData(ProjectStatusEnum.PaymentPending)]
        public void ValidProject_Complete_Success(ProjectStatusEnum initialStatus)
        {
            // Arrange
            var project = new Project("Projeto A", "Desc", 1, 2, 500);
            typeof(Project).GetProperty("Status")!.SetValue(project, initialStatus);

            // Act
            project.Complete();

            // Assert
            project.Status.Should().Be(ProjectStatusEnum.Completed);
            project.CompletedAt.Should().NotBeNull();
        }
        #endregion

        #region Failure
        [Fact]
        public void InvalidStatusProject_Complete_NoChange()
        {
            // Arrange
            var project = new Project("Projeto A", "Desc", 1, 2, 500);

            // Act
            project.Complete();

            // Assert
            project.Status.Should().Be(ProjectStatusEnum.Created);
            project.CompletedAt.Should().BeNull();
        }
        #endregion

        #region Exception
        // Nenhum teste aqui ainda
        #endregion

        #endregion

        #region SetPaymentPending

        #region Sucess
        [Fact]
        public void ProjectInProgress_SetPaymentPending_Success()
        {
            // Arrange
            var project = new Project("Projeto A", "Desc", 1, 2, 500);
            project.Start();

            // Act
            project.SetPaymentPending();

            // Assert
            project.Status.Should().Be(ProjectStatusEnum.PaymentPending);
        }
        #endregion

        #region Failure
        [Fact]
        public void ProjectNotInProgress_SetPaymentPending_NoChange()
        {
            // Arrange
            var project = new Project("Projeto A", "Desc", 1, 2, 500);

            // Act
            project.SetPaymentPending();

            // Assert
            project.Status.Should().Be(ProjectStatusEnum.Created);
        }
        #endregion

        #region Exception
        // Nenhum teste aqui ainda
        #endregion

        #endregion

        #region Update

        #region Sucess
        [Fact]
        public void ValidParams_Update_Success()
        {
            // Arrange
            var project = new Project("A", "Desc", 1, 2, 500);

            // Act
            project.Update("Novo", "Nova desc", 999.9m);

            // Assert
            project.Title.Should().Be("Novo");
            project.Description.Should().Be("Nova desc");
            project.TotalCost.Should().Be(999.9m);
        }
        #endregion

        #region Failure
        [Theory]
        [InlineData("", "Desc", 500)]
        [InlineData("Title", "", 500)]
        [InlineData("Title", "Desc", -1)]
        public void InvalidParams_Update_ThrowsException(string title, string desc, decimal cost)
        {
            // Arrange
            var project = new Project("A", "Desc", 1, 2, 500);

            // Act
            Action update = () => project.Update(title, desc, cost);

            // Assert
            update.Should()
                .Throw<ArgumentException>()
                .WithMessage("Invalid project update parameters.");
        }
        #endregion

        #region Exception
        // Nenhum teste aqui ainda
        #endregion

        #endregion
    }
}
