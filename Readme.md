# BCC-CA XML Signer Srever

This app is an example server of [BCC-CA Desktop Client](https://github.com/AbrarJahin/BCC-CA_XMLSigningClient) built on [ASP.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) with [PostGRE Database](https://www.postgresql.org/). We are using ASP.Net Core and PostGRE becuse both of them are open source and can be hosted in cheap ***linux server*** ([*Ubuntu*](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-3.1), [*CentOS*](https://www.vultr.com/docs/how-to-deploy-a-net-core-web-application-on-centos-7) etc.) with full functionality. If anyone like to use another DB, then here is the whole [list of supported databases](https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli#current-providers). For MySQL, along with [official connector library](https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core.html), other populer libraries can be used like [Pomelo.EntityFrameworkCore.MySql](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) for MySQL as well as [MariaDB](https://mariadb.org/).
This web app will be used as one of the server example for the signer  app. This app will also be used as XML Signature verification service. Most common commands and links regarding to ASP.Net Core is provided in [here](./Links-and-Commands.md).

In this web app,  provide [this](https://github.com/AbrarJahin/BCC-CA_XMLSigningClient#installation-and-deployment) mentioned APIs as well as XML verification service along with XML serialization and deserialization.

## APIs needed for enabling signature for web forms
In this web app, there are mainly 4 APIs implemented for enabling signature-

1. [Generate Download-Upload Token](#generate-download-upload-token)
2. [Download XML File](#download-xml-file-get-api)
3. [Upload XML File](#upload-xml-file-post-api)
4. [Verify XML File Signature](#verify-xml-file-signature)

Among this APIs, [Download XML File API](#download-xml-file-get-api) and [Upload XML File API](#upload-xml-file-post-api) is directly used by the [BCC-CA Desktop Signing Client](https://github.com/AbrarJahin/BCC-CA_XMLSigningClient#api-list-needed-in-the-server).
Along with those APIs, conversion of any form model to XML and XML to that form data is also provided in [here](./Library/Adapter.cs#L39) and [here](./Library/Adapter.cs#L52). Reading signature time from 

### Generate Download-Upload Token
This API is implemented in [here](./Controllers/api/XmlFilesController.cs#L114).

### Download XML File Get API
In this architecture of enabling digital signing to any web form, we are storing the web form as XML which is easily signed digitally. With this API, any XML file can be downloaded, but the XML file download URL can live for a small time with strong token validation so that the file security can be assured and no authinticated person or service can download the file and retrieve the data. To do that, we have created a API for generating download-upload token in [here](./Controllers/api/XmlFilesController.cs#L132). It is basically an API where only 

### Upload XML File POST API
This API is implemented in [here](./Controllers/api/XmlFilesController.cs#L68).

### Verify XML File Signature
This API is implemented in [here](./Controllers/api/XmlFilesController.cs#L35).
