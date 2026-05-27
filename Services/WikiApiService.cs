using System.Text.Json;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.Playwright;

namespace PlaywrightAssigmnet.Services;

public class WikiApiService
{
    private readonly IAPIRequestContext _request;
    private const string ApiBase = "https://en.wikipedia.org/w/api.php";
    private const string PageName = "Playwright_(software)";
    private const string DebuggingSectionAnchor = "Debugging_features";

    public WikiApiService(IAPIRequestContext request)
    {
        _request = request;
    }

    public async Task<string> GetDebugSectionHtmlAsync()
    {
        int sectionIndex = await GetSectionIndexAsync();

        string url = $"{ApiBase}?action=parse&page={PageName}&format=json&prop=text&section={sectionIndex}";
        var response = await _request.GetAsync(url);
        var body = await response.JsonAsync();

        if (body is null)
            throw new Exception("Response body is null");

        return body.Value
                   .GetProperty("parse")
                   .GetProperty("text")
                   .GetProperty("*")
                   .GetString() ?? string.Empty;
    }

    public async Task<string> GetDebugSectionTextAsync()
    {
        var html = await GetDebugSectionHtmlAsync();

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var root = doc.DocumentNode;

        var nodesToRemove = root.SelectNodes("//*[contains(@class,'mw-editsection') or contains(@class,'reference') or contains(@class,'reflist') or contains(@class,'references')]");
        foreach (var node in nodesToRemove ?? Enumerable.Empty<HtmlNode>())
            node.Remove();

        var rawText = System.Net.WebUtility.HtmlDecode(root.InnerText);
        return Regex.Replace(rawText, @"\n{3,}", "\n\n").Trim();
    }

    private async Task<int> GetSectionIndexAsync()
    {
        var url = $"{ApiBase}?action=parse&page={PageName}&format=json&prop=sections";
        var response = await _request.GetAsync(url);
        var body = await response.JsonAsync();

        if (body is null)
            throw new Exception("Response body is null");

        var targetSection = body.Value
            .GetProperty("parse")
            .GetProperty("sections")
            .EnumerateArray()
            .FirstOrDefault(s => s.GetProperty("anchor").GetString() == DebuggingSectionAnchor);

        if (targetSection.ValueKind == JsonValueKind.Undefined)
            throw new Exception("Could not find debugging section");

        return int.Parse(targetSection.GetProperty("index").GetString()!);
    }
}
