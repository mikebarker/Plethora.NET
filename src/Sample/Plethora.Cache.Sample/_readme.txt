
SIMPLE EXAMPLE
==============
The SimpleCache example shows an example of a cache where a single ID field uniquely identifies the
underlying data. In this case the Foo class has an Id property, which uniquely identifies it.

The FooArg struct is the implementation of IArgument used by the SimpleCache. It indicates which Foo the user
requested via the Id property. This is matched to the Id of the Foo class required.
The FooArg implementation of IArgument.IsOverlapped(...) simply returns true if Ids match, and false if they
do not. The out notInB parameter is set to null (could also be an empty set) in the case of true; and
ignorred in the case of false.


COMPLEX EXAMPLE
===============
The complex example show a cache in which the key-space is covered by two properties, ID and date.

For more information on the complexity of the example, see the PriceArg.IsOverlapped method definition.
