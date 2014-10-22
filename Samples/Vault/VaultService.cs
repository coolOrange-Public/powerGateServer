using System.Collections.Generic;
using Autodesk.Connectivity.WebServicesTools;
using powerGateServer.Addins;

namespace UserServices.Vault
{
	public class VaultService : IWebService
	{
		public string Name { get { return "Arcona6/VAULT_SRV"; } }
		public IEnumerable<IServiceMethod> Methods { get; private set; }

		public VaultService()
		{
			var credentials = new UserPasswordCredentials("localhost", "IKN-01", "Administrator", "");
			var webServiceMgr = new WebServiceManager(credentials);
			var vaultConnection = new VaultConnection(webServiceMgr);
			var entityConverter = new VaultEntityConverter(vaultConnection);
			Methods = new IServiceMethod[]
			{
				new DocumentService(vaultConnection, entityConverter),
				new PropertyService()
			};
		}
	}
}