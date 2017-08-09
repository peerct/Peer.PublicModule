Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

' 有关程序集的常规信息通过下列特性集
' 控制。更改这些特性值可修改
' 与程序集关联的信息。

' 查看程序集特性的值

<Assembly: AssemblyTitle("TjLog")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("")> 
<Assembly: AssemblyProduct("TjLog")> 
<Assembly: AssemblyCopyright("Copyright ©  2013")> 
<Assembly: AssemblyTrademark("")> 

<Assembly: ComVisible(False)>

'如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
<Assembly: Guid("3b77abbe-fc60-4813-845d-24943eafd92e")> 

' 程序集的版本信息由下面四个值组成:
'
'      主版本
'      次版本
'      内部版本号
'      修订号
'
' 可以指定所有这些值，也可以使用“内部版本号”和“修订号”的默认值，
' 方法是按如下所示使用“*”:
' <Assembly: AssemblyVersion("1.0.*")> 

<Assembly: AssemblyVersion("1.0.0.0")> 
<Assembly: AssemblyFileVersion("1.0.0.0")> 
'指定log4net的配置文件的名字为：“Log4netSetFile.config”
<Assembly: log4net.Config.XmlConfiguratorAttribute(ConfigFile:="Log4netSetFile.config", Watch:=True)> 
