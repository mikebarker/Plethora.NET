using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Cache.Sample.SimpleExample
{
    class SimpleCache : CacheBase<Person, PersonArg>
    {
        private readonly PersonSource fooSource = new PersonSource();

        public async Task<Person> GetPersonAsync(int id)
        {
            List<PersonArg> fooArgs = new List<PersonArg> {new PersonArg(id)};
            var foos = await base.GetDataAsync(fooArgs).ConfigureAwait(false);
            return foos.SingleOrDefault();
        }

        public void ClearCache()
        {
            base.Clear();
        }

        #region Overrides of CacheBase<Person,PersonArg>

        protected override async Task<IEnumerable<Person>> GetDataFromSourceAsync(
            IEnumerable<PersonArg> arguments, CancellationToken cancellationToken = default)
        {
            return arguments
                .Select(arg => this.fooSource.GetPerson(arg.Id))
                .Where(foo => foo != null)
                .ToList();
        }

        #endregion
    }
}
