using DevFreela.API.Controllers;
using DevFreela.API.Enums;

namespace DevFreela.API.Entities
{
    public class Project : BaseEntity
    {
        protected Project() { }

        public Project(string tittle, string decription, int idClient, int idFreelancer, decimal totalCost)
            : base()
        {
            Tittle = tittle;
            Decription = decription;
            IdClient = idClient;
            IdFreelancer = idFreelancer;
            TotalCost = totalCost;

            Status = ProjectControllerEnum.Created;
            Comments = new List<ProjectComment>();
        }

        public string Tittle { get; private set; }
        public string Decription { get; private set; }
        public int IdClient { get; private set; }
        public User Client { get; private set; }
        public int IdFreelancer { get; private set; }
        public User Freelancer { get; private set; }
        public decimal TotalCost { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public ProjectControllerEnum Status { get; private set; }
        public List<ProjectComment> Comments { get; private set; }

        public void Start()
        {
            if (Status == ProjectControllerEnum.Created)
            {
                Status = ProjectControllerEnum.InProgress;
                StartedAt = DateTime.Now;
            }
        }

        public void Cancel()
        {
            if (Status == ProjectControllerEnum.InProgress)
            {
                Status = ProjectControllerEnum.Cancelled;
                FinishedAt = DateTime.Now;
            }
        }

        public void Complete()
        {
            if (Status == ProjectControllerEnum.PaymentPending ||
                Status == ProjectControllerEnum.InProgress)
            {
                Status = ProjectControllerEnum.Completed;
                CompletedAt = DateTime.Now;
            }
        }

        public void SetPaymentPending()
        {
            if (Status == ProjectControllerEnum.InProgress)
            {
                Status = ProjectControllerEnum.PaymentPending;
            }
        }
    }
}
