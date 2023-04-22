using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using OData.Implementation.API.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOData();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapODataRoute("odata", "odata", GetEdmModel());
    endpoints.Select().Expand().OrderBy().MaxTop(null).Count().Filter();

});
app.Run();

IEdmModel GetEdmModel()
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<Category>("Categories");
    oDataBuilder.EntitySet<Product>("Products");
    #region Actions
    //.../odata/categories(1)/totalproductprice
    oDataBuilder.EntityType<Category>()
                .Action("TotalProductPrice")
                .Returns<decimal>();
    oDataBuilder.EntityType<Category>()
                .Collection
                .Action("TotalProductPriceAll")
                .Returns<decimal>();
    //odata/categories/totalproductprice
    oDataBuilder.EntityType<Category>()
                .Collection
                .Action("TotalProductPriceWithParam")
                .Returns<decimal>()
                .Parameter<int>("categoryId")
                .Required();

    var actionTotal = oDataBuilder.EntityType<Category>()
                                  .Collection
                                  .Action("total")
                                  .Returns<decimal>();
    actionTotal.Parameter<int>("a");
    actionTotal.Parameter<int>("b");
    actionTotal.Parameter<int>("c");

    oDataBuilder.EntityType<Product>()
                .Collection
                .Action("Login")
                .Returns<string>()
                .Parameter<Login>("UserLogin");
    #endregion

    #region Functions
    oDataBuilder.EntityType<Category>()
                .Collection
                .Function("CategoryCount")
                .Returns<int>();

    var multiply = oDataBuilder.EntityType<Product>()
             .Collection
             .Function("MultiplyFunction")
             .Returns<int>();

    multiply.Parameter<int>("a1");
    multiply.Parameter<int>("a2");
    multiply.Parameter<int>("a3");

    oDataBuilder.EntityType<Product>()
                .Function("KdvHesapla")
                .Returns<decimal>()
                .Parameter<decimal>("kdv");

    oDataBuilder.Function("GetKdv")
                .Returns<decimal>();
    #endregion
    return oDataBuilder.GetEdmModel();
}