<Query Kind="Program">
  <Connection>
    <ID>59ce3865-da45-4116-a90c-bcb90bbfbb74</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <Server>(local)</Server>
    <Database>Contoso</Database>
    <DisplayName>Contoso</DisplayName>
    <DriverData>
      <EncryptSqlTraffic>True</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

//Take-Home Practice:
//Question 1: Strongly Typed Queries
void Main()
{
	GetInventorySummary(5, "Cell").Dump();
}

// You can define other methods, fields, classes and namespaces here
public List<InventorySummaryView> GetInventorySummary(int storeId, string categoryName)
{
	return Inventories
			.Where(x => x.StoreID == storeId &&
				x.Product.ProductSubcategory.ProductCategory.ProductCategoryName.Contains(categoryName))
			.Select(x => new InventorySummaryView {
				StoreID = x.StoreID,
				StoreName = x.Store.StoreName,
				ProductName = x.Product.ProductName,
				Reorder = x.SafetyStockQuantity > x.OnHandQuantity + x.OnOrderQuantity ?
							"Yes" : "No",
				CategoryName = x.Product.ProductSubcategory.ProductCategory.ProductCategoryName
			})
			.OrderBy(x => x.StoreID)
			.ToList();
}

public class InventorySummaryView
{
	public int StoreID { get; set; }
	public string StoreName { get; set; }
	public string ProductName { get; set; }
	public string Reorder { get; set; }
	public string CategoryName { get; set; }
}