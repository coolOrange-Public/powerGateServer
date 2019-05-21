using System.Collections.Generic;
using System.Linq;
using Autofac;
using VaultServices.Entities.Base;
using VaultServices.Entities.File;
using VaultServices.Entities.File.FindStrategies;

namespace VaultServices
{
	public class Infrastructure : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<FileConversionContext>().PropertiesAutowired();
			builder.RegisterType<FindVaultFilesByIds>().Named<IQueryOperation<File>>("findFileStrategy");
			builder.RegisterType<FindVaultFilesByProperties>().Named<IQueryOperation<File>>("findFileStrategy");
			builder.Register(context => new FindFiles(context.ResolveNamed<IEnumerable<IQueryOperation<File>>>("findFileStrategy").Reverse())).As<IQueryOperation<File>>();
			builder.RegisterDecorator<IQueryOperation<File>>((c,inner) => new FindFiles(null), "findFileStrategy");
		}
	}
}