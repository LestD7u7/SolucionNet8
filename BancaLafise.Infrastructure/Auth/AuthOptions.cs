namespace BancaLafise.Infrastructure.Auth
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public string ExpirationMinutes { get; set; }
    }
}
