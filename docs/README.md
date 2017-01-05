# PortFinder
A small library to find open ports on a specified host.

As the title says, the only purpose of this is to find opened ports.

# Program Usage

### It can be used by two methods
#### Using arguments

 * ```Portfinder.exe host min max ```

 * Being 
  * **host** the target hostname *i.e google.com*
  * **min** the minimum port range *i.e 70*
  * **max** the port range limit *i.e 80*

* Or by simply running it
 *  ``` Portfinder.exe```

# Library Usage


First we need to instantiate an object, you can do this by explicitly typing the minimum and maximum values

```csharp
var finder = new PortFinderManager(hostname, minimum, maximum);
```

or by instantiating a **Range class** object 

```csharp
var finder = new PortFinderManager(hostname, new Range{Min = MINIMUM, Max = MAXIMUM});
```
    
then we hook up to some of the available events

##### PortSearched Event
###### Occurs whenever a port is sucessfully scanned.
```csharp
finder.PortSearched += delegate(int index, bool open) { }; //Occurs when a port where searched.
```

* Being
 * (int) Index: The port number
 * (bool) Open: If the port is or not open
 
##### Completed Event
###### Occurs when the search is done.

```csharp
finder.Completed += delegate(bool success) { };.
```

* Being
 * (bool) Success: Determines if the search occurred without any trouble.
 

and last but not least, **start to search**

```csharp
finder.FindOpenPorts();
```
