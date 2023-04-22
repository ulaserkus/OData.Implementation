#region OData Connected Service Extension
using Default;

var serviceRoot = "https://localhost:7115/odata/";
var context = new Container(new Uri(serviceRoot));
context.MergeOption = Microsoft.OData.Client.MergeOption.NoTracking;
var productsAll = context.Products
                         .GetAllPages();

var products = context.Products.AddQueryOption("$filter", "Id eq 2")
                               .AddQueryOption("$select", "Id,Name")
                               .ExecuteAsync()
                               .Result;

var productsWithExpand = context.Products
                                .AddQueryOption("$expand", "feature")
                                .AddQueryOption("$expand", "category")
                                .GetAllPages();

products.ToList().ForEach(p =>
{
    Console.WriteLine(p.Name);
});

Thread.Sleep(100);

Console.WriteLine("--------------------------------------------------------");

productsWithExpand.ToList().ForEach(p =>
{
    Console.WriteLine("Kategori : " + p?.Category?.Name + " Renk : " + p?.Feature?.Color);
});

Thread.Sleep(100);

Console.WriteLine("--------------------------------------------------------");

productsAll.ToList().ForEach(p =>
{
    Console.WriteLine(p.Name);
});

Console.ReadLine();
#endregion
