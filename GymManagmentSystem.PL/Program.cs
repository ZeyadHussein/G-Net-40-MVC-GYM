using GymManagmentSystem.BLL;
using GymManagmentSystem.BLL.Services.AttachmentService;
using GymManagmentSystem.BLL.Services.Calassess;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.DAL.dbcontext;
using GymManagmentSystem.DAL.Repositories.Classes;
using GymManagmentSystem.DAL.Repositories.Interfaces;
using GymManagmentSystem.PL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

#region Services

builder.Services.AddDbContext<GymDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(m =>
{
    m.AddProfile(new MappingProfiles());
});

builder.Services.AddScoped(typeof(IGenericRepository<>),
                           typeof(GenericRepository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPLanRepository, PlanRepository>();

builder.Services.AddScoped<ISessionRepository, SessionRepository>();

builder.Services.AddScoped<IAttachmentService,
                           AttachmentService>();

builder.Services.AddScoped<IMemberService,
                           MemberService>();

builder.Services.AddScoped<ISessionService,
                           SessionService>();

#endregion

var app = builder.Build();

await app.MigrateAndSeedDatabaseAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Home}/{action=Index}/{id?}");

app.Run();