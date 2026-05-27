using Microsoft.Playwright.NUnit;
using PlaywrightAssignment.Pages;

namespace PlaywrightAssignment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class WikiColorTests : PageTest
{
    private PlaywrightWikiPage _wikiPage = null!;

    [SetUp]
    public async Task SetUp()
    {
        _wikiPage = new PlaywrightWikiPage(Page);
        await _wikiPage.GotoAsync();
    }

    [Test]
    public async Task SelectingDarkColorShouldApplyDarkTheme()
    {
        await _wikiPage.SelectDarkColorAsync();
        var isDark = await _wikiPage.IsDarkModeActiveAsync();
        Assert.That(isDark, Is.True, "Dark mode CSS class was not applied after selecting Dark color");
    }
}
