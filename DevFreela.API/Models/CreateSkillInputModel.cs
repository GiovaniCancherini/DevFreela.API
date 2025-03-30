namespace DevFreela.API.Models
{
    public class CreateUserInputModel
    {
        public CreateUserInputModel(string fullName, string email, DateTime birthDate)
        {
            FullName = fullName;
            Email = email;
            BirthDate = birthDate;
        }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
