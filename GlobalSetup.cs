using NUnit.Framework;

namespace PlaywrightAssignment;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void RunBeforeAllTests()
    {
        var isCi = Environment.GetEnvironmentVariable("CI") == "true";
        if (!isCi)
        {
            Environment.SetEnvironmentVariable("HEADED", "1");
            Environment.SetEnvironmentVariable("SLOWMO", "500");
        }
    }
}
