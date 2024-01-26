using System.Threading.Tasks;

namespace FootBallNet
{
    public abstract class Service : IService
    {
        public abstract Task InitializeServiceAsync();
        public abstract void ResetService();
        public abstract void DestroyService();
    }

    public abstract class Service<TConfig> : IService<TConfig> where TConfig : Configuration
    {
        public TConfig Configuration { get; }

        public Service()
        {
            Configuration = Engine.GetConfiguration<TConfig>();
        }

        public abstract Task InitializeServiceAsync();

        public abstract void ResetService();

        public abstract void DestroyService();
    }
}
