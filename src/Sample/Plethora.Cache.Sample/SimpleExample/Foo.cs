using System.Collections.Generic;
using System.Threading;

namespace Plethora.Cache.Sample.SimpleExample
{
    struct FooArg : IArgument<Foo, FooArg>
    {
        public readonly int Id;

        public FooArg(int id)
        {
            this.Id = id;
        }

        #region Implementation of IArgument<Foo,FooArg>

        bool IArgument<Foo,FooArg>.IsOverlapped(FooArg B, out IEnumerable<FooArg> notInB)
        {
            notInB = null; //null (could be empty enumeration) in the case when true is returned (Ids match),
                           //and ignorred in the case where false is returned.

            return (B.Id == this.Id);
        }

        bool IArgument<Foo,FooArg>.IsDataIncluded(Foo data)
        {
            return (data.Id == this.Id);
        }
        #endregion
    }


    class Foo
    {
        public int Id;
        public long Value;

        public Foo(int id, long value)
        {
            this.Id = id;
            this.Value = value;
        }
    }

    class FooSource
    {
        private readonly Dictionary<int, long> myFoos;

        public FooSource()
        {
            myFoos = new Dictionary<int, long>
                         {
                             {0, 13},
                             {1, 54},
                             {2, 64},
                             {3, 72},
                             {4, 90},
                             {5, 07},
                         };
        }

        public Foo GetFoo(int id)
        {
            //Simulated the source taking some time return a result
            Thread.Sleep(3000);

            return new Foo(id, myFoos[id]);
        }
    }
}
