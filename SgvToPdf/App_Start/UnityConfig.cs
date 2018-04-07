using SgvToPdf.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace SgvToPdf
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IPdfService, PdfServiceItextSharp>();
            container.RegisterType<ISvgService, SgvNetService>();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}