
namespace Leobro.VideoStore.Model
{
    public class Casette
    {
        public int Id { get; }
        public VideoTitle Title { get; }

        public Casette(int id, VideoTitle title)
        {
            Id = id;
            Title = title;
        }
    }
}
