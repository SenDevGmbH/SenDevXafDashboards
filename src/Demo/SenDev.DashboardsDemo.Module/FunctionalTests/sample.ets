;#DropDB SenDev.DashboardsDemoEasyTest

#Application SenDev.DashboardsDemo.Win
#Application SenDev.DashboardsDemo.Web
#IfDef SenDev.DashboardsDemo.Win

*Action About...
 
*Action OK

#EndIf

#IfDef SenDev.DashboardsDemo.Web
!Action Navigation(My Details)
#EndIf
