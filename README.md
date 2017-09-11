# Demo application for Sharpenter.BootstrapperLoader library

## Run the application

- Build the solution using Visual Studio 2015/2017
- Make sure you have PostgreSQL database running. If you want to use Docker, here is the command to create a PostgreSQL container for testing purpose:

  ```
  docker run --name bootstrapper-loader-demo -p 5432:5432 -e POSTGRES_PASSWORD=mysecretpassword -d postgres
  ```

- Update connection string in `BootstrapperLoaderDemo\Web.config` to correct database connection string (The connection string provided in `Web.config` assumes we are using PostgreSQL database created from docker command above)
- Run `BootstrapperLoaderDemo`

If everything works correctly, it should show this home page:

![alt Home Page](https://raw.githubusercontent.com/hpcsc/BootstrapperLoaderDemo/master/images/HomePage.png)

## Project structure

- `BootstrapperLoaderDemo`: main UI project, contains a single `HomeController` that displays a list of books retrieved from database
- `BootstrapperLoaderDemo.Core`: contain domain model (`Book`) and repository interface (`IBookRepository`)
- `BootstrapperLoaderDemo.Repository`: contains implementation of book repository (`BookRepository`)

## Notes

- There's no project reference from `BootstrapperLoader` to `BootstrapperLoaderDemo.Repository`. Controllers work strictly with repository interfaces (from `Core` project)
- Since there's no project reference from `BootstrapperLoader` to `BootstrapperLoaderDemo.Repository`, Visual Studio will not copy `BootstrapperLoaderDemo.Repository.dll` and its dependencies to output directory of `BootstrapperLoader` during building. To work around this, I use `AfterBuild` target in `BootstrapperLoaderDemo.csproj`:

  ```
  <Target Name="AfterBuild">
    <ItemGroup>
      <SourceFiles Include="$(SolutionDir)\src\BootstrapperLoaderDemo.Repository\bin\$(Configuration)\**\*.*" />
    </ItemGroup>
    <Copy SourceFiles="@(SourceFiles)" DestinationFolder="$(OutputPath)\%(RecursiveDir)" ContinueOnError="true" SkipUnchangedFiles="true" />
  </Target>
  ```

- `BootstrapperLoaderDemo.Repository` is responsible for its own initialization (in `Bootstrapper` class):
  - It registers concrete implementation of repositories to their interface in `Bootstrapper.ConfigureContainer()`
  - It does database initialization (drop and create database, create tables, seeds data) in `Bootstrapper.ConfigureDevelopment()`
- `BootstrapperLoader` object is created and configured in `BootstrapperLoaderDemo.Startup` constructor:

  ```
  var bootstrapperLoader = new LoaderBuilder()
                                        .Use(new FileSystemAssemblyProvider(HttpRuntime.BinDirectory, "BootstrapperLoaderDemo.*.dll"))
                                        .ForClass()
                                            .HasConstructorParameter("DefaultConnection")
                                        .Methods()
                                            .Call("ConfigureDevelopment").If(() => HttpContext.Current.IsDebuggingEnabled)
                                        .Build();
  ```

It triggers `ConfigureContainer()` in bootstrapper classes in `Startup.ConfigureServices()`, passing in `IServiceCollection` instance so that bootstrapper classes can register IoC mapping:

  ```
  bootstrapperLoader.Trigger("ConfigureContainer", builder);
  ```

And trigger `ConfigureDevelopment()` in bootstrapper classes in `Startup.Configure()`, passing in IoC `GetService()`

