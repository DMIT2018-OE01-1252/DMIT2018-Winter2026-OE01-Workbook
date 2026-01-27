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
//Question 2: Strongly Typed Queries
void Main()
{
	GetInventorySummary(2, 10000).Dump();
}

// You can define other methods, fields, classes and namespaces here
public List<CustomerCollectionView> GetInventorySummary(int storeId, decimal totalAmount)
{
	return Invoices
			.Where(x => x.StoreID == storeId)
			.OrderBy(x => x.Customer.LastName)
			.Select(x => new CustomerCollectionView {
				InvoiceNo = x.InvoiceID,
				InvoiceDate = x.DateKey.ToString("M'/'d'/'yyyy"),
				Amount = x.TotalAmount,
				Name = $"{x.Customer.FirstName} {x.Customer.LastName}",
				StoreName = x.Store.StoreName,
				Manager = x.Store.Geography.CityName,
				Priority = x.TotalAmount > totalAmount ? "High Priority" : "Low Priority"
			})
			.ToList();
}

public class CustomerCollectionView
{
	public int InvoiceNo { get; set; }
	public string InvoiceDate { get; set; }
	public decimal Amount { get; set; }
	public string Name { get; set; }
	public string StoreName { get; set; }
	public string Manager { get; set; }
	public string Priority { get; set; }
}