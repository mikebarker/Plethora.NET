using System.Collections.Generic;

namespace Plethora.Cache.Sample.SimpleExample
{
    struct PersonArg : IArgument<Person, PersonArg>
    {
        public readonly int Id;

        public PersonArg(int id)
        {
            this.Id = id;
        }

        #region Implementation of IArgument<Person,PersonArg>

        bool IArgument<Person, PersonArg>.IsOverlapped(PersonArg B, out IEnumerable<PersonArg> notInB)
        {
            notInB = null; //null (could be empty enumeration) in the case when true is returned (Ids match),
                           //and ignorred in the case where false is returned.

            return (B.Id == this.Id);
        }

        bool IArgument<Person, PersonArg>.IsDataIncluded(Person person)
        {
            return (person.Id == this.Id);
        }
        #endregion
    }

}
