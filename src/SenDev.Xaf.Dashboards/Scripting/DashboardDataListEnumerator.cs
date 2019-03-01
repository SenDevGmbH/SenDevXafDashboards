using System.Collections;
using System.Collections.Generic;

namespace SenDev.Xaf.Dashboards.Scripting
{

	public class DashboardDataListEnumerator<TQuery, TElement> : IEnumerator<TElement>
	{
		private int currentIndex = -1;


		public DashboardDataListEnumerator(DashboardDataList<TQuery, TElement> owner)
		{
			Owner = owner;
		}


		private DashboardDataList<TQuery, TElement> Owner
		{
			get;
		}

		public TElement Current => Owner.GetElement(currentIndex);

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
