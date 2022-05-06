/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:WPF_MVVMLight_CRUD"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Extras.DynamicProxy;
using CommonServiceLocator;
using System.Reflection;
using WPF_MVVMLight_CRUD.DAL.Implement;
using WPF_MVVMLight_CRUD.DAL.Interface;
using WPF_MVVMLight_CRUD.Intercept;
using WPF_MVVMLight_CRUD.Services.Implement;
using WPF_MVVMLight_CRUD.Services.Interface;

namespace WPF_MVVMLight_CRUD.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static ContainerBuilder _container;
        private static IContainer container;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            //SimpleIoc.Default.Register<IDataAccessService, DataAccessService>();
            ////}

            //SimpleIoc.Default.Register<MainViewModel>();

            #region Autofac
            _container = new ContainerBuilder();

            Assembly assembly = Assembly.GetExecutingAssembly();

            // 註冊ViewModel
            _container.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("ViewModel"));

            // 註冊SQL連線工廠
            _container.RegisterType<SqlConnectionFactory>().As<IConnectionFactory>();

            // 註冊工作單元模式
            _container.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            // 註冊Interceptor
            _container.RegisterType(typeof(LoggingInterceptor));

            // 註冊Service
            _container.RegisterType<DataAccessService>().As<IDataAccessService>().EnableInterfaceInterceptors().InterceptedBy(typeof(LoggingInterceptor)); ;

            container = _container.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            #endregion
        }

        public MainViewModel Main => container.Resolve<MainViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}