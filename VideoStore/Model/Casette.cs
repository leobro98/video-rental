
namespace Leobro.VideoStore.Model
{
    public class Casette
    {
        public enum CasetteStatus
        {
            Undefined,
            Rented,
            OnShelf
        }

        private int id;
        private VideoTitle title;

        public int Id { get { return id; } }
        public VideoTitle Title { get { return title; } }
        public CasetteStatus Status { get; set; }

        public Casette(int id, VideoTitle title)
        {
            this.id = id;
            this.title = title;
        }
    }
}
