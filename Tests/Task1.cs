using Microsoft.Playwright.NUnit;
using PlaywrightAssigmnet.Pages;
using PlaywrightAssigmnet.Services;

namespace PlaywrightAssigmnet.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class WikiPageTests : PageTest
{
    [Test]
    public async Task Task1_Test()
    {
        var wikiPage = new PlaywrightWikiPage(Page);
        await wikiPage.GotoAsync();
        var uiText = await wikiPage.GetDebuggingSectionTextAsync();
        var uiUniqueWordCount = CountUniqueWords(uiText);

        await using var apiContext = await Playwright.APIRequest.NewContextAsync();
        var apiService = new WikiApiService(apiContext);
        var apiText = await apiService.GetDebugSectionTextAsync();
        var apiUniqueWordCount = CountUniqueWords(apiText);

        Console.WriteLine($"UI unique word count: {uiUniqueWordCount}");
        Console.WriteLine($"API unique word count: {apiUniqueWordCount}");

        Assert.That(apiUniqueWordCount, Is.EqualTo(uiUniqueWordCount));
    }

    private static int CountUniqueWords(string text) =>
        text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();
}
