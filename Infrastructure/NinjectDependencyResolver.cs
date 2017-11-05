using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using D.Models;
using Ninject.Web.Common;
using D.Interfaces;
namespace D.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IOrderingInterface>().To<Ordering>().InRequestScope();
            kernel.Bind<IdbInterface>().To<db>();
            kernel.Bind<ISupplierPriceInterface>().To<SupplierPrice>();
            kernel.Bind<IProductInterface>().To<Product>();
            //kernel.Bind<IEmployeeInterface>().To<Employee>();
            kernel.Bind<ISupplierInterface>().To<Supplier>();
            //kernel.Bind<ICustomerEntInterface>().To<CustomerEnt>();
            //kernel.Bind<IOrderInterface>().To<Order>();
            kernel.Bind<IMoneyReceiptInterface>().To<MoneyReceipt>();
            kernel.Bind<IOrderPaymentInterface>().To<OrderPayment>();
            kernel.Bind<ICustomerIndInterface>().To<CustomerInd>();
        }
    }
}