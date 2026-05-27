using NUnit.Framework;

namespace PlaywrightAssigmnet;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void RunBeforeAllTests()
    {
        Environment.SetEnvironmentVariable("HEADED", "1");
        Environment.SetEnvironmentVariable("SLOWMO", "500");
    }
}
