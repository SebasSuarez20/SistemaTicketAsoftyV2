

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
            services.AddTransient<IHubconnection, hubService>();

            //DbHandler(IRepository)
            services.AddTransient<IdbHandler<Users>, repositoryServices<Users>>();
            services.AddTransient<IdbHandler<ticketssupport>, repositoryServices<ticketssupport>>();
            services.AddTransient<IdbHandler<consecticketview>, repositoryServices<consecticketview>>();
            services.AddTransient<IdbHandler<TicketMapAndSupView>, repositoryServices<TicketMapAndSupView>>();
            services.AddTransient<IdbHandler<codeGeneric>, repositoryServices<codeGeneric>>();
            services.AddTransient<IdbHandler<ticketSupportViewChats>, repositoryServices<ticketSupportViewChats>>();
        }
    }
}
