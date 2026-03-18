# Traffic logging sample behavior

This example behavior users a [Message Inspector](https://learn.microsoft.com/en-us/dotnet/framework/wcf/samples/message-inspectors) to log http traffic recieved and sent by the [powerGateServer](https://doc.coolorange.com/projects/powergateserver/en/stable/) for debugging purposes.

## Installation
The resulting assembly (TrafficLogging.dll) needs to be placed in the [powerGateServer installation directory](https://doc.coolorange.com/projects/powergateserver/en/stable/installation/#install-locations) (`%programfiles%\coolOrange\powerGateServer`).

## Configuration 
To enable the behavior on a powerGateServer the powerGateServer.exe.config (found in the installation directory `%programfiles%\coolOrange\powerGateServer`) needs to be extended:
* The `<behaviorExtensions>` element needs to be extended to load the behavior:
  ```xml
  <add name="trafficLogger" type="TrafficLogging.TrafficLoggingBehaviorExtensionElement, TrafficLogging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"/>
  ```
* The `<behavior>` element needs to be extended to add the behavior to all services:
  ```xml
  <trafficLogger/>
  ```

Example:
```xml
<!-- ... -->
<behaviors>
    <serviceBehaviors>
        <behavior>
            <log4net/>
            <trafficLogger/>
        </behavior>
    </serviceBehaviors>
</behaviors>
<extensions>
    <behaviorExtensions>
        <add name="log4net" type="powerGateServer.Logging.Log4NetBehaviorExtensionElement, powerGateServer"/>
        <add name="trafficLogger" type="TrafficLogging.TrafficLoggingBehaviorExtensionElement, TrafficLogging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"/>
    </behaviorExtensions>
</extensions>
<!-- ... -->
```