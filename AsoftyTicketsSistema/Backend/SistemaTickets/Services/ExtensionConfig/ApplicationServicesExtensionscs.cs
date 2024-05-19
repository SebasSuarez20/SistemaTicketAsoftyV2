

using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication;
using SistemaTickets.Repository;
using SistemaTickets.Services;
using SistemaTickets.Model;
using SistemaTickets.Interface.IModel;
using SistemaTickets.Model.View;
using SistemaTickets.Interface;

namespace SistemaPoscloud.Services.ServicesConfig.ExtensionsConfig
{
    public static class ApplicationServicesExtensionscs
    {


        public static void AddAplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Servicios
            services.AddTransient<ILogin,loginService>();
            services.AddTransient<IticketsSupport,ticketssupportService>();
            services.AddTransient<IFile,fileService>();
            services.AddTransient<IHubconnection, HubService>();

            //DbHandler(IRepository)
            services.AddTransient<IdbHandler<Users>, RepositoryServices<Users>>();
            services.AddTransient<IdbHandler<ticketssupport>, RepositoryServices<ticketssupport>>();
            services.AddTransient<IdbHandler<consecticketview>, RepositoryServices<consecticketview>>();
            services.AddTransient<IdbHandler<TicketMapAndSupView>, RepositoryServices<TicketMapAndSupView>>();
            services.AddTransient<IdbHandler<CodeGeneric>, RepositoryServices<CodeGeneric>>();
        }
    }
}
