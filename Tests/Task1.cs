using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using PlaywrightAssignment.Pages;
using PlaywrightAssignment.Services;

namespace PlaywrightAssignment.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class DebuggingSectionTests : PageTest
{
    private PlaywrightWikiPage _wikiPage = null!;
    private WikiApiService _apiService = null!;
    private IAPIRequestContext _apiContext = null!;

    [SetUp]
    public async Task SetUp()
    {
        _wikiPage = new PlaywrightWikiPage(Page);
        await _wikiPage.GotoAsync();

        _apiContext = await Playwright.APIRequest.NewContextAsync();
        _apiService = new WikiApiService(_apiContext);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _apiContext.DisposeAsync();
    }

    [Test]
    public async Task UniqueWordCountShouldMatchBetweenUiAndApi()
    {
        var uiText = await _wikiPage.GetDebuggingSectionTextAsync();
        var uiUniqueWordCount = CountUniqueWords(uiText);

        var apiText = await _apiService.GetDebugSectionTextAsync();
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
