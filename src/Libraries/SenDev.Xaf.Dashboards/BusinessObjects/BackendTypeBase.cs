using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SenDev.Xaf.Dashboards.DataExtract;

namespace SenDev.Xaf.Dashboards.BusinessObjects
{
	/// <summary>
	/// Persistent base class for data extract backend types.
	/// Create a subclass and override <see cref="CreateBackend"/> to implement a custom backend.
	/// Instances are seeded in the database updater and selected via a UI lookup.
	/// </summary>
	[ImageName("BO_Unknown")]
	[NavigationItem("Reports")]
	[ModelDefault(nameof(IModelClass.Caption), "Data Extract Backend Type")]
	public abstract class BackendTypeBase : BaseObject
	{
		public BackendTypeBase(Session session) : base(session) { }

		private string name;

		[ModelDefault(nameof(IModelMember.Caption), "Name")]
		public string Name
		{
			get => name;
			set => SetPropertyValue(nameof(Name), ref name, value);
		}

		/// <summary>
		/// Creates the <see cref="IDataExtractBackend"/> that handles data creation,
		/// connection configuration, and dashboard patching for this backend type.
		/// </summary>
		public abstract IDataExtractBackend CreateBackend();
	}
}
