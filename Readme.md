# BCC-CA XML SIgner Srever

This app is the server of BCC_CA XML Signer app. It will be used as server of signer app.

This app will also be used as user verification service.

## Migration Commands-

	- Add-Migration InitialMigration -OutputDir "Data/Migrations"	(Add migration to a specific folder)
	- Add-Migration <MigrationName>	(Add a new migration in normal time)
	- Remove-Migration			(Remove last migration)
	- Remove-Migration -Force	(Remove last migration forcefully)
	- Update-Database 0			(Remove all table)
	- Update-Database			(Create all table)
	- Script-migration
	- Update-Database –TargetMigration: <name of last good migration>	(Restore from a good migration)

Create New DB after dropping DB-

	- Drop-Database				(Drop The Database)
	- Remove-Migration			(Remove Existing Migration)


## Initial Project Builder-

	https://aspnetboilerplate.com/

## API Creation-

https://www.codingame.com/playgrounds/35462/creating-web-api-in-asp-net-core-2-0/part-1---web-api

## Linking with Keys-

https://github.com/armancse100/ASP.NetCore-MySQL-Login-CRUD/blob/master/InventoryManagement/Models/Product.cs

## Seed Data -

https://csharp-video-tutorials.blogspot.com/2019/05/entity-framework-core-seed-data.html

## SignalR for Socket-

https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio

Configure SSL with SignalR-

https://weblog.west-wind.com/posts/2013/sep/23/hosting-signalr-under-sslhttps

## WebRTC for Video Chat For Real Time User Verifire-

https://www.html5rocks.com/en/tutorials/webrtc/basics/

https://webrtc.github.io/samples/

Should store and verify data after all data is provided by the user.

## JS, CSS, HTML Minifire Config-

https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification

