using Microsoft.Playwright.NUnit;
using PlaywrightAssigmnet.Pages;

namespace PlaywrightAssigmnet.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class MicrosoftDevToolsTests : PageTest
{
    [Test]
    public async Task AllTechnologyNamesShouldBeLinks()
    {
        var wikiPage = new PlaywrightWikiPage(Page);
        await wikiPage.GotoAsync();

        var items = await wikiPage.GetMicrosoftDevToolsItemsAsync();

        Assert.Multiple(() =>
        {
            foreach (var (name, isLink) in items)
                Assert.That(isLink, Is.True, $"'{name}' is not a text link");
        });
    }
}
