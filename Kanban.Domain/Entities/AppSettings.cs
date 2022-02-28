namespace Kanban.Domain.Entities
{
    public class AppSettings
    {
        public string DefaultErrorMessage { get; set; }

        public string JwtKey { get; set; }

        public string JwtIssuer { get; set; }

        public string JwtAudience { get; set; }

        public int JwtTokenTimeInMinutes { get; set; } = 60;
    }
}
