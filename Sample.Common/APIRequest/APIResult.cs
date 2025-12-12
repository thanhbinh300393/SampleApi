namespace Sample.Common.APIRequest
{
    public class APIResult
    {
        public APIResult()
        {
        }

        public object? Data { get; set; }
        public string SystemMessage { get; set; }
        public string UserFriendlyMessage { get; set; }
        public string Html { get; set; }
        public string Title { get; set; }
    }
}
