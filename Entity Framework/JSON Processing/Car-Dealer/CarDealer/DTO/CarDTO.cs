using System.Collections.Generic;

namespace CarDealer.DTO
{
    public class CarDTO
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public int TravelledDistance { get; set; }

        public List<int> PartsId { get; set; }
    }
}
