using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using MediatR;

namespace DevFreela.Application.Commands.InsertProject
{
    public class InsertProjectCommand : IRequest<ResultViewModel<int>>
    {
        public InsertProjectCommand(int idClient, int idFreeLancer, string title, string description, decimal totalCost)
        {
            IdClient = idClient;
            IdFreeLancer = idFreeLancer;
            Title = title;
            Description = description;
            TotalCost = totalCost;
        }

        public int IdClient { get; set; }
        public int IdFreeLancer { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }

        public Project ToEntity()
            => new(Title, Description, IdClient, IdFreeLancer, TotalCost);
    }
}
