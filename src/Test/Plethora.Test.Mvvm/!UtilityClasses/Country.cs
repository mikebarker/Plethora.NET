namespace Plethora.Test.Mvvm._UtilityClasses
{
    class Country
    {
        public static readonly Country UnitedKingdom = new Country("GB", "United Kingdom");
        public static readonly Country UnitedStatesOfAmerica = new Country("US", "United States of America");

        private readonly string code;
        private readonly string name;

        public Country(
            string code,
            string name)
        {
            this.code = code;
            this.name = name;
        }

        public string Code
        {
            get { return this.code; }
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
