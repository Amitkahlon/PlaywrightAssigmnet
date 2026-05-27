using Microsoft.Playwright.NUnit;
using PlaywrightAssigmnet.Pages;

namespace PlaywrightAssigmnet.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class WikiColorTests : PageTest
{
    [Test]
    public async Task SelectingDarkColorShouldApplyDarkTheme()
    {
        var wikiPage = new PlaywrightWikiPage(Page);
        await wikiPage.GotoAsync();
        await wikiPage.SelectDarkColorAsync();
        var isDark = await wikiPage.IsDarkModeActiveAsync();
        Assert.That(isDark, Is.True, "Dark mode CSS class was not applied after selecting Dark color");
    }
}
