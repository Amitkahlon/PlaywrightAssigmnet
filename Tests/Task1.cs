using Microsoft.Playwright.NUnit;
using PlaywrightAssignment.Pages;
using PlaywrightAssignment.Services;

namespace PlaywrightAssignment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class DebuggingSectionTests : PageTest
{
    [Test]
    public async Task UniqueWordCountShouldMatchBetweenUiAndApi()
    {
        var wikiPage = new PlaywrightWikiPage(Page);
        await wikiPage.GotoAsync();
        var uiText = await wikiPage.GetDebuggingSectionTextAsync();
        var uiUniqueWordCount = CountUniqueWords(uiText);

        await using var apiContext = await Playwright.APIRequest.NewContextAsync();
        var apiService = new WikiApiService(apiContext);
        var apiText = await apiService.GetDebugSectionTextAsync();
        var apiUniqueWordCount = CountUniqueWords(apiText);

        TestContext.WriteLine($"UI unique word count: {uiUniqueWordCount}");
        TestContext.WriteLine($"API unique word count: {apiUniqueWordCount}");

        Assert.That(apiUniqueWordCount, Is.EqualTo(uiUniqueWordCount));
    }

    private static int CountUniqueWords(string text) =>
        text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();
}
