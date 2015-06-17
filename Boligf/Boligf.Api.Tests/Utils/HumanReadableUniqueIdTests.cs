using Boligf.Api.Utils;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Boligf.Api.Tests.Utils
{
	public class HumanReadableUniqueIdTests
	{
		[Fact]
		public void Generate_WhenCalled_ResultIsNotNullOrEmptyAndAreSixCharactersLong()
		{
			var generator = new HumanReadableUniqueId();
			var result = generator.Generate();

			result.ShouldNotBeNullOrEmpty();
            result.Length.ShouldBe(6);
		}

		[Fact]
		public void Generate_WhenCalledMutipleTimes_ResultIsUnique()
		{
			var generator = new HumanReadableUniqueId();

			var resultId1 = generator.Generate();
			var resultId2 = generator.Generate();
			var resultId3 = generator.Generate();

			resultId3.ShouldNotBe(resultId1);
			resultId3.ShouldNotBe(resultId2);
		}

		[Fact]
		public void Factory_InstanceIsSet_UsesCurrentInstanceInsteadOfNew()
		{
			var humanReadableUniqueIdMock = Substitute.For<IHumanReadableUniqueId>();

			humanReadableUniqueIdMock.Generate().Returns("test1");

			HumanReadableUniqueId.Instance = humanReadableUniqueIdMock;
            HumanReadableUniqueId.NewUid().ShouldBe("test1");
		}

		[Fact]
		public void Factory_InstanceIsNotSet_UsesNewCreatedInstance()
		{
			HumanReadableUniqueId.Instance = null;
			HumanReadableUniqueId.NewUid().Length.ShouldBe(6);
		}
	}
}
