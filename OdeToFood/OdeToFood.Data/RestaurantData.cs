using OdeToFood.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OdeToFood.Data
{
    public interface RestaurantData
    {
        IEnumerable<Restaurant> GetAllRestaurants();
    }

    public class InMemoryRestaurantData: RestaurantData
    {
        readonly List<Restaurant> restaurants;
        public InMemoryRestaurantData()
        {
            restaurants = new List<Restaurant>() {
                new Restaurant { Id = 1, Name = "Marko's Pizza", Location = "Zagreb", Cuisine = CuisineType.Mexican },
                new Restaurant { Id = 2, Name = "Duksa", Location = "Split", Cuisine = CuisineType.Italian },
                new Restaurant { Id = 3, Name = "Curry Bowl", Location = "Rijeka", Cuisine = CuisineType.Indian }
            };
        }

        public IEnumerable<Restaurant> GetAllRestaurants()
        {
            return
                from r in restaurants
                orderby r.Name
                select r;
        }
    }
}
