using Claims_Api.Repositories;
using Moq;

namespace Tests.Helpers
{
    public static class MockRepositories
    {
        public static void Setup()
        {
            ServiceProviderHelper.ClaimRepository = new Mock<IClaimRepository>{CallBase = true};
        }
    }
}