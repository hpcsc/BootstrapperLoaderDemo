# Demo application for Sharpenter.BootstrapperLoader library

## Run the application

- Build the solution (make sure your machine has .NET Core - Version 2.0.0)
- Update connection string in `BootstrapperLoaderDemo\appsettings.json` to correct SQL Server Instance name (I use SQL Server Instance `SQLExpress2014` in this example)
- Run `BootstrapperLoaderDemo`

If everything works correctly, it should show this home page:

![alt Home Page](https://raw.githubusercontent.com/hpcsc/BootstrapperLoaderDemo/master/images/HomePage.png)

## Project structure

- `BootstrapperLoaderDemo`: main UI project, contains a single `HomeController` that displays a list of books retrieved from database
- `BootstrapperLoaderDemo.Core`: contain domain model (`Book`) and repository interface (`IBookRepository`)
- `BootstrapperLoaderDemo.Repository`: contains implementation of book repository (`BookRepository`)

## Notes

- There's no project reference from `BootstrapperLoader` to `BootstrapperLoaderDemo.Repository`. Controllers work strictly with repository interfaces (from `Core` project)
- Since there's no project reference from `BootstrapperLoader` to `BootstrapperLoaderDemo.Repository`, Visual Studio will not copy `BootstrapperLoaderDemo.Repository.dll` and its dependencies to output directory of `BootstrapperLoader` during building. To work around this, I use `PostBuild` target in `BootstrapperLoaderDemo.csproj`:

```
<ItemGroup>
  <ItemsToCopy Include="./../BootstrapperLoaderDemo.Repository/bin/$(Configuration)/$(TargetFramework)/*.dll" />
</ItemGroup>
<Target Name="PostBuild" AfterTargets="PostBuildEvent">
  <Copy
      SourceFiles="@(ItemsToCopy)"
      DestinationFolder="./bin/$(Configuration)/$(TargetFramework)/temp/">
      <Output
          TaskParameter="CopiedFiles"
          ItemName="SuccessfullyCopiedFiles"/>
    </Copy>
    <Message Importance="High" Text="PostBuild Target successfully copied:%0a@(ItemsToCopy->'- %(fullpath)', '%0a')%0a -> %0a@(SuccessfullyCopiedFiles->'- %(fullpath)', '%0a')"/>
</Target>
```

- `BootstrapperLoaderDemo.Repository` is responsible for its own initialization (in `Bootstrapper` class):
  - It registers concrete implementation of repositories to their interface in `Bootstrapper.ConfigureContainer()`
  - It does database initialization (drop and create database, create tables, seeds data) in `Bootstrapper.ConfigureDevelopment()`
- `BootstrapperLoader` object is created and configured in `BootstrapperLoaderDemo.Startup` constructor:

```
_bootstrapperLoader = new LoaderBuilder()
                        //Look for all dlls starting with BootstrapperLoaderDemo
                        .Use(new FileSystemAssemblyProvider(PlatformServices.Default.Application.ApplicationBasePath, "BootstrapperLoaderDemo.*.dll"))
                        .ForClass()
                            //Inject Configuration object into Bootstrapper classes found in those dlls
                            .HasConstructorParameter(Configuration)
                            .Methods()
                                //Call Bootstrapper.ConfigureDevelopment() if it's development environment
                                .Call("ConfigureDevelopment").If(env.IsDevelopment)
                        .Build();
```

It triggers `ConfigureContainer()` in bootstrapper classes in `Startup.ConfigureServices()`, passing in `IServiceCollection` instance so that bootstrapper classes can register IoC mapping:

```
_bootstrapperLoader.Trigger("ConfigureContainer", services);
```

And trigger `ConfigureDevelopment()` in bootstrapper classes in `Startup.Configure()`, passing in IoC `GetService()`

