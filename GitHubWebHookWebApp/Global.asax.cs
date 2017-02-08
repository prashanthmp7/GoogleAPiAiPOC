using System.Web.Http;

namespace GitHubWebHookWebApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(WebHookConfig.Register);
        }
    }
}
