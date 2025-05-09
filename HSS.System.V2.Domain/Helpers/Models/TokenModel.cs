namespace HSS.System.V2.Domain.Helpers.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationTime { get; set; }
        public DateTime TokenExpirationDate { get; set; }
    }
}
