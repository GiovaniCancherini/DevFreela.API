namespace DevFreela.Infrastructure.Auth
{
    public interface IAuthService
    {
        string ComputeSha256Hash(string rawData);
        string GenerateJwtToken(string email, string role);
    }
}
