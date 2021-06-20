using System;
using System.Threading.Tasks;
using Claims_Api.Repositories;
using Claims_Api.Repositories.UnitOfWork;
using Claims_Api.Services.Claim;
using Claims_Api.Services.Queues;
using Moq;

namespace Tests.Helpers
{
    public static class ServiceProviderHelper
    {
        public static Mock<IClaimRepository> ClaimRepository { get; set; }

        public static IServiceProvider MockServiceProvider()
        {
            var unitOfWork = MockUnitOfWork();
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IUnitOfWork)))
                .Returns(unitOfWork.Object);
            var queueService = new Mock<IQueueService>();
            queueService.Setup(_ => _.PublishMessage(It.IsAny<string>())).Returns(Task.CompletedTask);
            serviceProvider.Setup(x => x.GetService(typeof(IQueueService))).Returns(queueService.Object);

            return serviceProvider.Object;
        }

        private static Mock<IUnitOfWork> MockUnitOfWork()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(_ => _.ClaimRepository).Returns(ClaimRepository.Object);
            return unitOfWork;
        }
        
        public static IServiceProvider MockServices()
        {
            var claimService = new Mock<IClaimService>();
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(claimService.Object);
            return serviceProvider.Object;
        }
    }
}