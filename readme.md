Common
===

A package that contains commonly used features and extensions.



## Installation

To use this package, simply reference it from your .NET project, which will pull the
specified version from the [nuget.org](https://www.nuget.org/packages/OwlDomain.Common/) source.

In C#, that would look like this:
```csproj
<ItemGroup>
  <PackageReference Include="OwlDomain.Common" Version="1.0.0" />
</ItemGroup>
```



## Usage

Currently, this package provides the following:
- [Extensions](src/Common/Locking/SemaphoreLock.cs) for the [SemaphoreSlim](https://learn.microsoft.com/dotnet/api/system.threading.semaphoreslim).
- [Extensions](src/Common/Locking/ReaderWriterLocks.cs) for the [ReaderWriterLockSlim](https://learn.microsoft.com/dotnet/api/system.threading.readerwriterlockslim).
- A [DisposableBase](src/Common/DisposableBase.cs) that follows the [disposable pattern](https://learn.microsoft.com/dotnet/standard/garbage-collection/implementing-dispose).
- An [ObservableBase](src/Common/Observable/ObservableBase.cs) that implements the [INotifyPropertyChanging](https://learn.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanging) and [INotifyPropertyChanged](https://learn.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanged) interfaces, along with helper methods and attributes for raising notifications about property changes.



## Contributions

Code contributions will not be accepted, however feel free to provide feedback / suggestions 
by creating a [new issue](https://github.com/Owl-Domain/Common/issues/new), or look at 
the [existing issues](https://github.com/Owl-Domain/Common/issues?q=) to see if your
concern / suggestion has already been raised.



## License

This project (the source, and the release files, e.t.c) is release under the [OwlDomain License](/license.md).
