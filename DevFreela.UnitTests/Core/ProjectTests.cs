using DevFreela.Core.Entities;
using DevFreela.Core.Enums;

namespace DevFreela.UnitTests.Core
{
    public class ProjectTests
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
    }
}
