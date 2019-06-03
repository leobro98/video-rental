
namespace Leobro.VideoStore.Model
{
    public class ActiveRental
    {
        public int CustomerId { get; set; }
        public Casette RentedCasette { get; set; }
        public RentalOptions Terms { get; set; }
    }
}
