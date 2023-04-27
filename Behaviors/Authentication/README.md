# Authentication sample behavior

The example behavior provides two sample implementations of a [`UserNamePasswordValidator`](https://learn.microsoft.com/en-us/dotnet/api/system.identitymodel.selectors.usernamepasswordvalidator?view=netframework-4.8.1) to configure basic authentication for [powerGateServer](https://doc.coolorange.com/projects/powergateserver/en/stable/) services.  
The `GuidUserNameAuthentication` allows only users with a username matching the specifid pattern to authenticate. The password is ignored in this validator.  
The `UsernamePasswordAuthentication` validator only allows a user to authenticate with a specific username and password ('myUserName' and 'myPassword' in this case). This sample could be extended to use a database to look up user credentials.


## Installation
The resulting assembly (Authentication.dll) needs to be placed in the [powerGateServer installation directory](https://doc.coolorange.com/projects/powergateserver/en/stable/installation/#install-locations) (`%programfiles%\coolOrange\powerGateServer`).

## Configuration
To configure basic authentication on a powerGateServer the powerGateServer.exe.config (found in the installation directory `%programfiles%\coolOrange\powerGateServer`) needs to be extended:
* The `<binding>` element needs to be extended with:
  ```xml
   <security mode="TransportCredentialOnly">
        <transport clientCredentialType="Basic"/>
   </security>
   ```
* The `<behavior>` element needs to be extended to configure the userNameAuthentication and the custom validator. If a custom implementation is used, the `customUserNamePasswordValidatorType` attribute value needs to be replaced with qualified type name to the validator that should be used.
  ```xml
  <serviceCredentials>
      <userNameAuthentication userNamePasswordValidationMode="Custom" customUserNamePasswordValidatorType="Authentication.GuidUserNameAuthentication, Authentication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"/>
  </serviceCredentials>
  ```

Example using the `GuidUserNameAuthentication`:
```xml
<!-- ... -->
<bindings>
    <webHttpBinding>
        <binding maxReceivedMessageSize="2147483647" maxBufferPoolSize="2147483647" maxBufferSize="2147483647">
            <readerQuotas maxDepth="2147483647" maxStringContentLength="2147483647" maxArrayLength="2147483647" maxBytesPerRead="2147483647" maxNameTableCharCount="2147483647"/>
            <security mode="TransportCredentialOnly">
                <transport clientCredentialType="Basic"/>
            </security>
        </binding>
    </webHttpBinding>
</bindings>
<behaviors>
    <serviceBehaviors>
        <behavior>
            <log4net/>
            <serviceCredentials>
                <userNameAuthentication userNamePasswordValidationMode="Custom" customUserNamePasswordValidatorType="Authentication.GuidUserNameAuthentication, Authentication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"/>
            </serviceCredentials>
        </behavior>
    </serviceBehaviors>
</behaviors>
<!-- ... -->
```

## Links
[MS documentation on custom username password validators](https://learn.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-use-a-custom-user-name-and-password-validator)