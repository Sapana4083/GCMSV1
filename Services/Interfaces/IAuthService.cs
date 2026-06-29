namespace GCMS.WEB.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateUserAsync(
            string username,
            string password);
    }
}
