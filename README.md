# PortFinder
A small library to find open ports on a specified host.

As the title says, the only purpose of this is to find opened ports.

# Program Usage

    Portfinder.exe host min max

### Being 
* **host** the target hostname *i.e google.com*
* **min** the minimum port range *i.e 70*
* **max** the port range limit *i.e 80*

# API Usage

First we need to instantiate an object.
```csharp
var finder = new PortFinderManager(hostname, minimum, maximum);
```
    
then we hook up to some of the available events
```csharp
finder.PortSearched += delegate(int index, bool opened) { }; //Occurs when a port where searched.
finder.SearchDone += delegate { }; //Occurs when the search is complete.
```
and last but not least, start the searching
```csharp
finder.Run();
```

# TODO

- Improve perfomance.
