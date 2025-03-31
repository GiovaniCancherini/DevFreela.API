
using DevFreela.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevFreela.API.Models
{
    public class CreateProjectInputModel
    {
        public int IdClient { get; set; }

        public int IdFreeLancer { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal TotalCost { get; set; }

        public Project ToEntity()
            => new(Title, Description, IdClient, IdFreeLancer, TotalCost);
    }
}
