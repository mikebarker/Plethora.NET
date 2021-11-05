namespace Plethora.Cache.Sample
{
    /// <summary>
    /// A person is uniquely keyed on the person's ID (i.e. 1 key dimension).
    /// </summary>
    public class Person
    {
        public Person(long id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public long Id { get; }
        public string Name { get; }
    }
}
