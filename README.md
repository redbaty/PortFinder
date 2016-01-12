# PortFinder
A small library to find open ports on a specified host.

As the title says, the only purpose of this, is to thing opened ports.

# Usage

First we need to instantiate an object.

    var finder = new PortFinderManager(hostname, minimum, maximum);
    
then we hook up to some of the available events

    finder.PortSearched += delegate { };
    finder.PortDone += delegate { };
    finder.PortFound += delegate { };

and last but not least, start the searching
  
    finder.Run();
    
# TODO

- Improve perfomance.
