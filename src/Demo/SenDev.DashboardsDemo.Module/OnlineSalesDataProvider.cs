using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using SenDev.DashboardsDemo.Module.BusinessObjects;

namespace SenDev.DashboardsDemo.Module
{
	public static class OnlineSalesDataProvider
	{
		public static IQueryable GetData(IObjectSpace objectSpace)
		{
			return objectSpace.GetObjectsQuery<OnlineSales>()
				.OrderBy(s => s.OnlineSalesKey)
				.Select(s => new
				{
					s.ProductKey.ProductName,
					s.ProductKey.ProductLabel,
					s.PromotionKey.PromotionName,
					s.PromotionKey.PromotionLabel,
					s.SalesAmount,
					s.SalesQuantity,
					s.StoreKey.StoreName,
					s.CurrencyKey.CurrencyName,
					s.CustomerKey.CompanyName,
					s.CustomerKey.CustomerLabel,
					s.CurrencyKey.CurrencyLabel,
					Date = s.DateKey.Datekey,
					s.ReturnAmount,
					s.ReturnQuantity,
					s.SalesOrderNumber,
					s.DiscountAmount,
					s.DiscountQuantity,
					s.TotalCost,
					s.UnitCost,
					s.UnitPrice,
				});
		}
	}
}
