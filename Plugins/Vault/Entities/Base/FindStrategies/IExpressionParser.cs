using powerGateServer.SDK;

namespace VaultServices.Entities.Base.FindStrategies
{
	public interface IExpressionParser<TFrom>
	{
		IExpression<T> ParseFor<T>(IExpression<TFrom> propertyExpression) where T : IBaseObject;
	}
}