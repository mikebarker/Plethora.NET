using System;
using System.Collections.Generic;
using Plethora.Collections;
using System.Threading;

namespace Plethora.Cache.Sample
{
    class PersonSource
    {
        private readonly KeyedCollection<long, Person> people = new KeyedCollection<long, Person>(p => p.Id)
        {
            new Person(0, "Bob"),
            new Person(1, "Fred"),
            new Person(2, "Amy"),
            new Person(3, "Jill"),
            new Person(4, "Jane"),
            new Person(5, "Katherine"),
        };

        public Person GetPerson(int id)
        {
            // Simulate the source taking some time return a result
            Thread.Sleep(3000);

            if (this.people.TryGetValue(id, out var person))
                return person;
            else
                return null;
        }
    }
}
