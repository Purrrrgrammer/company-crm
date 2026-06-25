using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CompanyCRM.Data;
using CompanyCRM.Data.Repositories;
using CompanyCRM.MVVM.ViewModels;
using CompanyCRM.Services;
using ContractorEditWindow = CompanyCRM.MVVM.Views.ContractorEditWindow;
using EmployeeEditWindow = CompanyCRM.MVVM.Views.EmployeeEditWindow;
using MainWindow = CompanyCRM.MVVM.Views.MainWindow;
using OrderEditWindow = CompanyCRM.MVVM.Views.OrderEditWindow;

namespace CompanyCRM.Infrastructure
{
    public static class Bootstrapper
    {
        private static IWindsorContainer _container;

        public static void Initialize()
        {
            _container = new WindsorContainer();
            RegisterComponents();
        }

        private static void RegisterComponents()
        {
            _container.Register(
                Component.For<INHibernateHelper>()
                    .ImplementedBy<NHibernateHelper>()
                    .LifestyleSingleton()
                );

            _container.Register(
                Component.For<IEmployeeRepository>()
                    .ImplementedBy<EmployeeRepository>()
                    .LifestyleTransient()
                );
            
            _container.Register(
                Component.For<IContractorRepository>()
                    .ImplementedBy<ContractorRepository>()
                    .LifestyleTransient()
            );

            _container.Register(
                Component.For<IOrderRepository>()
                    .ImplementedBy<OrderRepository>()
                    .LifestyleTransient()
                );

            _container.Register(
                Component.For<IEmployeeService>()
                    .ImplementedBy<EmployeeService>()
                    .LifestyleTransient()
            );
            
            _container.Register(
                Component.For<IOrderService>()
                    .ImplementedBy<OrderService>()
                    .LifestyleTransient()
                );
            
            _container.Register(
                Component.For<IContractorService>()
                    .ImplementedBy<ContractorService>()
                    .LifestyleTransient()
            );
            
            _container.Register(
                Component.For<MainViewModel>()
                    .LifestyleTransient()
            );
            
            _container.Register(
                Component.For<IEditViewModel>()
                    .ImplementedBy<EmployeeEditViewModel>()
                    .Named("EmployeeEdit")
                    .LifestyleTransient()
            );

            _container.Register(
                Component.For<IEditViewModel>()
                    .ImplementedBy<ContractorEditViewModel>()
                    .Named("ContractorEdit")
                    .LifestyleTransient()
            );

            _container.Register(
                Component.For<IEditViewModel>()
                    .ImplementedBy<OrderEditViewModel>()
                    .Named("OrderEdit")
                    .LifestyleTransient()
            );
            
            _container.Register(
                Component.For<EmployeeEditWindow>()
                    .LifestyleTransient()
            );
            
            _container.Register(
                Component.For<ContractorEditViewModel>()
                    .LifestyleTransient()
            );

            _container.Register(
                Component.For<OrderEditViewModel>()
                    .LifestyleTransient()
            );

            _container.Register(
                Component.For<ContractorEditWindow>()
                    .LifestyleTransient()
            );
            
            _container.Register(
                Component.For<OrderEditWindow>()
                    .LifestyleTransient()
            );
            
            _container.Register(
                Component.For<MainWindow>()
                    .LifestyleTransient()
                );
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
        
        public static T Resolve<T>(string name)
        {
            return _container.Resolve<T>(name);
        }
        
        public static T Resolve<T>(string name, object arguments)
        {
            var args = new Arguments();
            if (arguments != null)
            {
                var properties = arguments.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    args.Add(prop.Name, prop.GetValue(arguments));
                }
            }
            return _container.Resolve<T>(name, args);
        }
        
        public static void Dispose()
        {
            _container?.Dispose();
        }
    }
}