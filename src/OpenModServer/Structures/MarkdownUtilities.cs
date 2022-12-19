using Markdig;
using Microsoft.AspNetCore.Html;
using Westwind.AspNetCore.Markdown;

namespace OpenModServer.Structures;

public class MarkdownUtilities
{
    public static MarkdownParserMarkdig MarkdownEngine =
        new MarkdownParserMarkdig(false, false, markdown =>
        {
            markdown.DisableHtml();
        });

    public static HtmlString RenderMarkdownToHtml(string markdown) => new HtmlString(MarkdownEngine.Parse(markdown));
}