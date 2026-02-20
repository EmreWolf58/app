using Hangfire.Dashboard;

public class HangfireDashboardAuthorizationFilter: IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Burada kullanıcı doğrulama ve yetkilendirme işlemlerini yapabilirsiniz.
        // Örneğin, sadece belirli bir rol veya kullanıcıya izin verebilirsiniz.
        var httpContext = context.GetHttpContext();
        // Örnek: Sadece "Admin" rolüne sahip kullanıcılar erişebilir.
        return httpContext.User?.Identity?.IsAuthenticated == true && httpContext.User.IsInRole("Admin");
    }

}