using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InterviewEvaluationSystem.Startup))]
namespace InterviewEvaluationSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
