namespace Markdown
{
    public class MarkdownClient : IMarkdownClient
    {
        public MarkdownClient() 
        { 
        }
        public string ToHtml(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown);
        }
    }
}
