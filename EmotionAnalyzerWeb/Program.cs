using EmotionAnalyzerWeb.Components;
using EmotionAnalyzerWeb.Services;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();




builder.Services.AddHttpClient<EmotionApiService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]);
});


builder.Services.AddHttpClient("ApiWakeup", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]);
});



var app = builder.Build();




_ = Task.Run(async () =>
{
    try
    {
        using var scope = app.Services.CreateScope();

        var clientFactory =
            scope.ServiceProvider
            .GetRequiredService<IHttpClientFactory>();

        var client =
            clientFactory.CreateClient("ApiWakeup");


        await client.GetAsync("/");
    }
    catch
    {
    }
});



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();



app.Run();