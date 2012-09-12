<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SGP.Components.Notifications" generation="1" functional="0" release="0" Id="e667bcdd-f7fd-45e3-a929-5f3e829ca8da" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SGP.Components.NotificationsGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="NotificationRole:ErrorQueueIdentifier" defaultValue="">
          <maps>
            <mapMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/MapNotificationRole:ErrorQueueIdentifier" />
          </maps>
        </aCS>
        <aCS name="NotificationRole:InputQueueIdentifier" defaultValue="">
          <maps>
            <mapMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/MapNotificationRole:InputQueueIdentifier" />
          </maps>
        </aCS>
        <aCS name="NotificationRole:Issuer" defaultValue="">
          <maps>
            <mapMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/MapNotificationRole:Issuer" />
          </maps>
        </aCS>
        <aCS name="NotificationRole:Key" defaultValue="">
          <maps>
            <mapMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/MapNotificationRole:Key" />
          </maps>
        </aCS>
        <aCS name="NotificationRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/MapNotificationRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="NotificationRole:ServiceBusNamespace" defaultValue="">
          <maps>
            <mapMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/MapNotificationRole:ServiceBusNamespace" />
          </maps>
        </aCS>
        <aCS name="NotificationRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/MapNotificationRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapNotificationRole:ErrorQueueIdentifier" kind="Identity">
          <setting>
            <aCSMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/NotificationRole/ErrorQueueIdentifier" />
          </setting>
        </map>
        <map name="MapNotificationRole:InputQueueIdentifier" kind="Identity">
          <setting>
            <aCSMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/NotificationRole/InputQueueIdentifier" />
          </setting>
        </map>
        <map name="MapNotificationRole:Issuer" kind="Identity">
          <setting>
            <aCSMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/NotificationRole/Issuer" />
          </setting>
        </map>
        <map name="MapNotificationRole:Key" kind="Identity">
          <setting>
            <aCSMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/NotificationRole/Key" />
          </setting>
        </map>
        <map name="MapNotificationRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/NotificationRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapNotificationRole:ServiceBusNamespace" kind="Identity">
          <setting>
            <aCSMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/NotificationRole/ServiceBusNamespace" />
          </setting>
        </map>
        <map name="MapNotificationRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/NotificationRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="NotificationRole" generation="1" functional="0" release="0" software="H:\Projects\git-remote\Test-Project\SGP.Components.Notifications\csx\Debug\roles\NotificationRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="ErrorQueueIdentifier" defaultValue="" />
              <aCS name="InputQueueIdentifier" defaultValue="" />
              <aCS name="Issuer" defaultValue="" />
              <aCS name="Key" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="ServiceBusNamespace" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;NotificationRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;NotificationRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/NotificationRoleInstances" />
            <sCSPolicyFaultDomainMoniker name="/SGP.Components.Notifications/SGP.Components.NotificationsGroup/NotificationRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="NotificationRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="NotificationRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>