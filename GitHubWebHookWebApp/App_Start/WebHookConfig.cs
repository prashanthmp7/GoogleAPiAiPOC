using System.Web.Http;

namespace GitHubWebHookWebApp
{
    public static class WebHookConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.InitializeReceiveGitHubWebHooks();
        }
    }
}