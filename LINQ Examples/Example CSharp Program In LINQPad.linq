<Query Kind="Program">
  <Connection>
    <ID>6b821400-0acf-4186-ba5f-2174b2f7f2fe</ID>
    <NamingServiceVersion>3</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>(local)</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <UseMicrosoftDataSqlClient>true</UseMicrosoftDataSqlClient>
    <EncryptTraffic>true</EncryptTraffic>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook-2025</Database>
    <MapXmlToString>false</MapXmlToString>
    <DriverData>
      <SkipCertificateCheck>true</SkipCertificateCheck>
    </DriverData>
  </Connection>
</Query>

void Main()
{
	int paramYear = 2000;
	GetAllAlbumByReleaseYear(paramYear).Dump();
}

// You can define other methods, fields, classes and namespaces here
List<Album> GetAllAlbumByReleaseYear(int paramYear)
{
    return Albums
			.Where(x => x.ReleaseYear == paramYear)
			.Select(x => x) // Optional in this case if we returning all the columns
			.ToList();
}