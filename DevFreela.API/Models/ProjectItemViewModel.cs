namespace DevFreela.API.Models
{
    public class ProjectItemViewModel
    {
        public int IdClient { get; set; }
        public int IdFreeLancer { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }
    }
}
