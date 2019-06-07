
namespace Leobro.VideoStore.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public int BonusPoints { get; set; }

        public Customer(int id)
        {
            Id = id;
        }

        public Customer(int id, int bonusPoints)
        {
            this.Id = id;
            this.BonusPoints = bonusPoints;
        }
    }
}
