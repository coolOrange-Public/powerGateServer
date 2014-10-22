using powerGateServer.Addins;

namespace UserServices.ServiceDefinition
{
	public abstract class ReadonlyServiceMethod<T> : ServiceMethod<T>
	{
		public override void Update(T entity)
		{
		}

		public override void Create(T entity)
		{
		}

		public override void Delete(T entity)
		{
		}
	}
}