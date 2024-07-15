namespace Markdown
{
    public interface IMarkdownClient
    {
        string ToHtml(string markdown);
    }
}
