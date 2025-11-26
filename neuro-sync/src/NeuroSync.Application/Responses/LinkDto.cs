namespace NeuroSync.Application.Responses
{
    public class LinkDto
    {
        public string Rel { get; set; } = string.Empty;
        public string Href { get; set; } = string.Empty;
        public string Method { get; set; } = "GET";
    }
}
