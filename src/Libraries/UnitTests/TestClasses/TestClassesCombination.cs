namespace UnitTests
{
	public partial class DashboardDataListTests
	{
		class TestClassesCombination
		{
			public TestClassesCombination(TestClassWithName revenueType, TestClassWithNameAndNumber taskType)
			{
				RevenueType = revenueType;
				TaskType = taskType;
			}

			public TestClassWithName RevenueType
			{
				get;
			}
			public TestClassWithNameAndNumber TaskType
			{
				get;
			}
		}

	}
}
