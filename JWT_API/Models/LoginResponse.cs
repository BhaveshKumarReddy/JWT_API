namespace JWT_API.Models
{
    public class LoginResponse
    {
        public string token { get; set; } = string.Empty;
        public string User_Id { get; set; } = string.Empty;
        public bool Role { get; set; }
    }
}
