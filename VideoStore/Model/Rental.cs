
namespace Leobro.VideoStore.Model
{
    public class Rental
    {
        public int CustomerId { get; set; }
        public int CasetteId { get; set; }
        public VideoTitle.TitleType TitleType { get; private set; }
        public int RentalDays { get; private set; }
        public decimal Price { get; private set; }
        public int BonusPointsPayed { get; set; }
        public bool IsActive { get; set; }

        public Rental()
        { }

        public Rental(int customerId, int casetteId, RentalOptions options)
        {
            CustomerId = customerId;
            CasetteId = casetteId;
            
            TitleType = options.TitleType;
            RentalDays = options.RentalDays;
            Price = options.Price;
            BonusPointsPayed = options.BonusPointsPayed;
            IsActive = true;
        }
    }
}
