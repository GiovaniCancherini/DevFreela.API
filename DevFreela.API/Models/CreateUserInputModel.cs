namespace DevFreela.API.Models
{
    public class CreateSkillInputModel
    {
        public CreateSkillInputModel(string decription)
        {
            Decription = decription;
        }

        public string Decription { get; private set; }
    }
}
