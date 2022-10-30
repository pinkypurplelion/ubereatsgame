namespace Scenes.MainGameWorld.Scripts
{
    /// <summary>
    /// Represents information about the customers within the game that live in houses and order food from stores.
    /// </summary>
    /// <author>Liam Angus</author>
    public class Customer
    {
        private readonly string _name;
        private readonly HouseTile _residence;

        /// <summary>
        /// Constructor for the Customer class.
        /// </summary>
        /// <param name="name">The customer's name</param>
        /// <param name="residence">The HouseTile object where the customer resides</param>
        public Customer(string name, HouseTile residence)
        {
            _name = name;
            _residence = residence;
        }

        /// <summary>
        /// Returns the customer's name.
        /// </summary>
        /// <returns>The customer's name in string format</returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Returns the customer's residence
        /// </summary>
        /// <returns>The HouseTile object where the customer resides</returns>
        public HouseTile GetResidence()
        {
            return _residence;
        }
    }
}