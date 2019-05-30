using System;
using ErpServices.Database;
using ErpServices.Services.DocumentInfoRecordService.Entities;

namespace ErpServices.Services.DocumentInfoRecordService
{
	public abstract class DirNavigationPropertyEntitySet<T> 
		: NavigationPropertyEntitySet<T,DocumentInfoRecordContext>
	{
		protected DirNavigationPropertyEntitySet(IEntityStores entityStores)
			:base(entityStores)
		{
		}

		protected override bool IsParentFor(DocumentInfoRecordContext parent, T entity)
		{
			return new DocumentInfoRecordContextComparer(entity)
				.Equals(parent);
		}
	}

	public class DocumentInfoRecordContextComparer : IEquatable<DocumentInfoRecordContext>
	{
		private readonly dynamic _source;

		public DocumentInfoRecordContextComparer(dynamic source)
		{
			_source = source;
		}

		public bool Equals(DocumentInfoRecordContext context)
		{
			return Equals(context.Documentversion, _source.Documentversion) &&
				Equals(context.Documenttype, _source.Documenttype) &&
				Equals(context.Documentpart, _source.Documentpart) &&
				Equals(context.Documentnumber, _source.Documentnumber);
		}
	}
}