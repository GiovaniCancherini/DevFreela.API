using DevFreela.Core.Entities;
using DevFreela.Core.Enums;

namespace DevFreela.UnitTests.Core
{
    public class ProjectXUnitTests
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
            Assert.Equal(ProjectStatusEnum.InProgress, project.Status);
            Assert.NotNull(project.StartedAt);

            Assert.True(project.Status == ProjectStatusEnum.InProgress);
            Assert.False(project.StartedAt is null);
        }
        #endregion

        #region Failure

        #endregion

        #region Exception
        [Fact]
        public void ProjectIsInvalidState_Start_ThrowsException()
        {
            // Arrange
            var project = new Project("Projeto A", "Descricao Projeto", 1, 2, 1000.1m);
            project.Start();

            // Act
            Action? start = project.Start;

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(start);
            Assert.Equal(Project.INVALID_STATE_MESSAGE, exception.Message);
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
            Assert.Equal(ProjectStatusEnum.Cancelled, project.Status);
            Assert.NotNull(project.FinishedAt);
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
            Assert.Equal(ProjectStatusEnum.Created, project.Status);
            Assert.Null(project.FinishedAt);
        }
        #endregion

        #region Exception

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
            typeof(Project).GetProperty("Status").SetValue(project, initialStatus);

            // Act
            project.Complete();

            // Assert
            Assert.Equal(ProjectStatusEnum.Completed, project.Status);
            Assert.NotNull(project.CompletedAt);
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
            Assert.Equal(ProjectStatusEnum.Created, project.Status);
            Assert.Null(project.CompletedAt);
        }

        #endregion

        #region Exception

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
            Assert.Equal(ProjectStatusEnum.PaymentPending, project.Status);
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
            Assert.Equal(ProjectStatusEnum.Created, project.Status);
        }

        #endregion

        #region Exception

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
            Assert.Equal("Novo", project.Title);
            Assert.Equal("Nova desc", project.Description);
            Assert.Equal(999.9m, project.TotalCost);
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

            // Act + Assert
            var ex = Assert.Throws<ArgumentException>(() => project.Update(title, desc, cost));
            Assert.Equal("Invalid project update parameters.", ex.Message);
        }

        #endregion

        #region Exception

        #endregion

        #endregion
    }
}
