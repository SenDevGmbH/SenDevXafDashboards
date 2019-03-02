using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace SenDev.DashboardsDemo.Module.BusinessObjects
{

	public partial class Machine
	{
		public Machine(Session session) : base(session) { }
		public override void AfterConstruction()
		{
			base.AfterConstruction();
		}
	}

}
