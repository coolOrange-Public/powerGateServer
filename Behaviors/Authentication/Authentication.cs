using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace Authentication
{
    public class GuidUserNameAuthentication : UserNamePasswordValidator
	{
		public override void Validate(string userName, string password)
		{
			if (!Regex.IsMatch(userName, @"^[a-zA-Z0-9-]{16,48}$"))
				throw new SecurityTokenException("Username is not a valid GUID");

		}
	}

    public class UsernamePasswordAuthentication : UserNamePasswordValidator
    {
	    public override void Validate(string userName, string password)
	    {
		    if (string.Equals("myUserName", userName) && string.Equals("myPassword", password))
			    throw new SecurityTokenException("Username password combination not valid!");

	    }
	}
}
