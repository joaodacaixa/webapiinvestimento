namespace webapiinvestimento.DTO
{
    public class AuthDtos
    {
        public class LoginRequest
        {
            public string? Usuario { get; set; }
            public string? Senha { get; set; }
        }

        public class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
            public DateTime ExpiraEm { get; set; }
        }
    }
}
