using System;

namespace Scenes.MainGameWorld.Scripts
{
    public class Customer
    {
        private String name;
        private HouseTile residence;
        
        public Customer(String name, HouseTile residence)
        {
            this.name = name;
            this.residence = residence;
        }
        
        public String getName()
        {
            return name;
        }
    }
}