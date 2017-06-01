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
            kernel.Bind<IОформление_заказаInterface>().To<Оформление_заказа>().InRequestScope();
            kernel.Bind<IdbInterface>().To<db>();
            kernel.Bind<IПоставщик_ценаInterface>().To<Поставщик_цена>();
            kernel.Bind<IТоварInterface>().To<Товар>();
            kernel.Bind<IСотрудникInterface>().To<Сотрудник>();
            kernel.Bind<IПоставщикInterface>().To<Поставщик>();
            kernel.Bind<IКлиентInterface>().To<CustomerEnt>();
            kernel.Bind<IЗаказInterface>().To<Заказ>();
            kernel.Bind<IДенежное_поступлениеInterface>().To<Денежное_поступление>();
            kernel.Bind<IОплата_заказаInterface>().To<Оплата_заказа>();
            kernel.Bind<ICustomerIndInterface>().To<CustomerInd>();
        }
    }
}