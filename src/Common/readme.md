Common
===

A package that contains commonly used features and extensions.



## Usage

Currently, this package provides the following:
- [Extensions](Locking/SemaphoreLock.cs) for the [SemaphoreSlim](https://learn.microsoft.com/dotnet/api/system.threading.semaphoreslim).
- [Extensions](Locking/ReaderWriterLocks.cs) for the [ReaderWriterLockSlim](https://learn.microsoft.com/dotnet/api/system.threading.readerwriterlockslim).
- A [DisposableBase](DisposableBase.cs) that follows the [disposable pattern](https://learn.microsoft.com/dotnet/standard/garbage-collection/implementing-dispose).
- An [ObservableBase](Observable/ObservableBase.cs) that implements the [INotifyPropertyChanging](https://learn.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanging) and [INotifyPropertyChanged](https://learn.microsoft.com/dotnet/api/system.componentmodel.inotifypropertychanged) interfaces, along with helper methods and attributes for raising notifications about property changes.



## License

This project (the source, and the release files, e.t.c) is release under the [OwlDomain License](/license.md).
