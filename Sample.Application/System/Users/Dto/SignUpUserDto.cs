namespace Sample.Application.System.Users.Dto
{
    public class SignUpUserDto
    {
        public string? Id { get; set; }
        public int Type { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? TaxCode { get; set; }
        public string? FullName { get; set; }
        public bool JobSeekerVerified { get; set; }
        public string? AttachmentFile { get; set; }
    }

    public enum OauthUserEnum
    {
        Google = 1, Facebook = 2
    }
}
