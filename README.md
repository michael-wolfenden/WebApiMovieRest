WebApi Movie Rest
==

WebApi Movie Rest is a sample ASP.NET Web API application providing a restful interface for maintaining imdb movie records.

* Uses Entity Framework for persistence
* Uses Simple Injector for IOC
* Supports both json and xml media types specified either in the content-type header or at the end of the uri (uri.json or uri.xml)
* Responses follow the [JSON API ID style](http://jsonapi.org/format/#id-based-json-api)
* BDD style unit and integration tests
* Uses a request / response / handler style of architecture
* Uses SwaggerUI for documentation

Building & Running
==

Connection strings
--
There are two configuration files that need to be modified before running this application

* src\IntegrationTests\App.config - The connection string in this configuration file points to the database used for the integration tests. This database will be deleted / re-created each time the integration tests are run so make sure the database name is different from the database name used for the application (below).

* src\Web\Web.config - The connection string in this configuration file points to the database used for the application.

Building from a command prompt or powershell
--
```
> build.bat
```
Cleans and builds the application then runs the unit tests followed by the integration tests. The tests will generate reports in the "build" directory.

Building from Visual Studio
--
Simply open the solution file and press F5 (Debug -> Start Debugging). You can use your favourite Visual Studio test runner (i.e. resharper) to run the unit and integration tests.

Todo
==
* Implement Http Caching with [CacheCow](https://github.com/aliostad/CacheCow)
* Implement Http Compression
* Support paging and sorting

3rd Party Libraries
==

* [ASP.NET Web API](http://aspnetwebstack.codeplex.com/) - ASP.NET Web API is a framework that makes it easy to build HTTP services that reach a broad range of clients, including browsers and mobile devices. | [License](http://aspnetwebstack.codeplex.com/license)

* [CuttingEdge.Conditions](http://conditions.codeplex.com/) - CuttingEdge.Conditions is a library that helps developers to write pre- and postcondition validations in their C# 3.0 and VB.NET 9.0 code base. | [License](http://conditions.codeplex.com/license)

* [Entity Framework](http://entityframework.codeplex.com/) - Entity Framework (EF) is an object-relational mapper that enables .NET developers to work with relational data using domain-specific objects | [License](http://entityframework.codeplex.com/license)

* [Fluent Assertions](https://github.com/dennisdoomen/FluentAssertions/) - Fluent Assertions is a set of .NET extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style test | [License](https://raw.github.com/dennisdoomen/fluentassertions/master/LICENSE)

* [FluentValidation](https://github.com/JeremySkinner/FluentValidation/) - A small validation library for .NET that uses a fluent interface 
and lambda expressions for building validation rules. | [License](https://raw.github.com/JeremySkinner/FluentValidation/master/License.txt)

* [NSubstitute](https://github.com/nsubstitute/nsubstitute) - A friendly substitute for .NET mocking frameworks. | [License](https://raw.github.com/nsubstitute/NSubstitute/master/LICENSE.txt)

* [psake](https://github.com/psake/psake) - psake is a build automation tool written in PowerShell. | [License](https://raw.github.com/psake/psake/master/license.txt)

* [ServiceStack](https://github.com/ServiceStack/ServiceStack) - Service Stack is a high-performance .NET web services platform that simplifies the development of high-performance REST. | [License](https://raw.github.com/ServiceStack/ServiceStack/master/LICENSE)

* [Simple Injector](http://simpleinjector.codeplex.com/) - The Simple Injector is an easy-to-use Inversion of Control library for .NET and Silverlight. | [License](http://simpleinjector.codeplex.com/license)

* [Swagger-UI](https://github.com/wordnik/swagger-ui/) - Swagger UI is a dependency-free collection of HTML, Javascript, and CSS assets that dynamically generate beautiful documentation from a Swagger-compliant API. | [License](https://github.com/wordnik/swagger-ui#license)

* [xbehave.net](https://github.com/xbehave/xbehave.net/) - A BDD/TDD framework based on xUnit .net and inspired by Gherkin. | [License](https://raw.github.com/xbehave/xbehave.net/master/license.txt)

* [xUnit.net](http://xunit.codeplex.com/) - xUnit.net is a free, open source, community-focused unit testing tool for the .NET Framework. | [License](http://xunit.codeplex.com/license)

License
==
Licensed under the terms of the [New BSD License](http://opensource.org/licenses/BSD-3-Clause/)