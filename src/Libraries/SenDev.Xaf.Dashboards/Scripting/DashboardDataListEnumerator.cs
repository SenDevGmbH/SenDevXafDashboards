using System.Collections;

namespace SenDev.Xaf.Dashboards.Scripting
{

	public class DashboardDataListEnumerator : IEnumerator
	{
		private int currentIndex = -1;


		public DashboardDataListEnumerator(DashboardDataList owner)
		{
			Owner = owner;
		}


		private DashboardDataList Owner
		{
			get;
		}

		public object Current => Owner.GetElement(currentIndex);

		object IEnumerator.Current => Current;

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			currentIndex++;
			return currentIndex < Owner.Count;

		}

		public void Reset()
		{
			currentIndex = 0;
		}
	}
}
