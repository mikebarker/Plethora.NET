using System.Collections.Generic;
using System.Linq;

namespace Plethora.Cache.Sample.SimpleExample
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
}
