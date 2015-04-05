namespace EverlastingStudent.Web
{
    using System.Web.Http.Dependencies;

    using Ninject;

    public class NinjectResolver : NinjectScope, IDependencyResolver,
    System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel kernel;
        public NinjectResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }
        public IDependencyScope BeginScope()
        {
            return new NinjectScope(this.kernel.BeginBlock());
        }
    }
}