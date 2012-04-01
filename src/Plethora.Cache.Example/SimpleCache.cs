using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Plethora.Cache
{
    class SimpleCache : CacheBase<Foo, FooArg>
    {
        private readonly FooSource fooSource = new FooSource();

        public Foo GetFoo(int id)
        {
            List<FooArg> fooArgs = new List<FooArg> {new FooArg(id)};
            var foos = GetData(fooArgs, 5000);
            return foos.Single();
        }

        public void DropFooFromCache(int id)
        {
            List<FooArg> fooArgs = new List<FooArg> {new FooArg(id)};
            DropData(fooArgs);
        }

        #region Overrides of CacheBase<Foo,FooArg>

        protected override IEnumerable<Foo> GetDataFromSource(
            IEnumerable<FooArg> arguments, int millisecondsTimeout)
        {
            return arguments
                .Select(arg => fooSource.GetFoo(arg.Id))
                .ToList();
        }

        #endregion
    }

    class FooArg : IArgument<Foo, FooArg>
    {
        public readonly int Id;

        public FooArg(int id)
        {
            this.Id = id;
        }

        #region Implementation of IArgument<Foo,FooArg>

        public bool IsOverlapped(FooArg B, out IEnumerable<FooArg> notInB)
        {
            if (B.Id == this.Id)
            {
                notInB = null;
                return true;
            }
            else
            {
                notInB = new List<FooArg> { this };
                return false;
            }
        }

        public bool IsDataIncluded(Foo data)
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
            Thread.Sleep(3000);

            return new Foo(id, myFoos[id]);
        }
    }
}
