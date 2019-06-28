using System;

namespace Leobro.VideoStore.Model
{
    /// <summary>
    /// Data about the rental of a video casette.
    /// </summary>
    public class Rental
    {
        public Customer Customer { get; private set; }
        public Casette Casette { get; private set; }
        public int RentalDays { get; set; }
        public decimal Price { get; set; }
        public int BonusPointsPayed { get; set; }
        public bool IsActive { get; set; }

        public Rental(Customer customer, Casette casette)
        {
            Customer = customer ?? throw new ArgumentException("Customer may not be null", "customer");
            Casette = casette ?? throw new ArgumentException("Casette may not be null", "casette");
            
            IsActive = true;
        }
    }
}
