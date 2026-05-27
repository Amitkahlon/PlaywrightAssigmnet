using System.Text;
using Microsoft.Playwright;

namespace PlaywrightAssigmnet.Pages;


public class PlaywrightWikiPage
{
    private readonly IPage _page;

    private readonly ILocator _debuggingFeaturesHeader;

    public const string BaseURl = "https://en.wikipedia.org/wiki/Playwright_(software)";


    public PlaywrightWikiPage(IPage page)
    {
        _page = page;

        _debuggingFeaturesHeader = _page.Locator("#Debugging_features");
    }

    public async Task GotoAsync()
    {
        await _page.GotoAsync(BaseURl);
    }

    public async Task<IEnumerable<(string Name, bool IsLink)>> GetMicrosoftDevToolsItemsAsync()
    {
        var navbox = _page.Locator(".navbox", new() { HasText = "Microsoft development tools" });
        var items = await navbox.Locator("li").AllAsync();

        var result = new List<(string Name, bool IsLink)>();
        foreach (var item in items)
        {
            var name = (await item.InnerTextAsync()).Trim();
            var isLink = await item.Locator("a").CountAsync() > 0;
            result.Add((name, isLink));
        }

        return result;
    }

    public async Task SelectDarkColorAsync()
    {
        await _page.GetByRole(AriaRole.Radio, new() { Name = "Dark" }).ClickAsync();
    }

    public async Task<bool> IsDarkModeActiveAsync()
    {
        return await _page.Locator("html").EvaluateAsync<bool>(
            "el => el.classList.contains('skin-theme-clientpref-night')");
    }

    public async Task<string> GetDebuggingSectionTextAsync()
    {
        var headerText = await _debuggingFeaturesHeader.InnerTextAsync();
        var siblings = await _debuggingFeaturesHeader.Locator("xpath=../following-sibling::*").AllAsync();
        var sb = new StringBuilder();
        sb.AppendLine(headerText);

        foreach (var element in siblings)
        {
            var isSectionHeader = await element.EvaluateAsync<bool>("el => el.classList.contains('mw-heading')");
            if (isSectionHeader) break;
            var text = await element.EvaluateAsync<string>(@"el => {
                const clone = el.cloneNode(true);
                clone.querySelectorAll('.reference').forEach(r => r.remove());
                return clone.textContent;
            }");
            sb.AppendLine(text);
        }

        return sb.ToString().Trim();
    }



}