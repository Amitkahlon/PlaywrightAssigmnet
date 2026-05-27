using Microsoft.Playwright.NUnit;
using PlaywrightAssignment.Pages;

namespace PlaywrightAssignment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class MicrosoftDevToolsTests : PageTest
{
    private PlaywrightWikiPage _wikiPage = null!;

    [SetUp]
    public async Task SetUp()
    {
        _wikiPage = new PlaywrightWikiPage(Page);
        await _wikiPage.GotoAsync();
    }

    [Test]
    public async Task AllTechnologyNamesShouldBeLinks()
    {
        var items = await _wikiPage.GetMicrosoftDevToolsItemsAsync();

        Assert.Multiple(() =>
        {
            foreach (var (name, isLink) in items)
                Assert.That(isLink, Is.True, $"'{name}' is not a text link");
        });
    }
}
