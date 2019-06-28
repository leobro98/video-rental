using System;

namespace Leobro.VideoStore.Model
{
    /// <summary>
    /// Information about the film.
    /// </summary>
    public class VideoTitle
    {
        public enum TitleType
        {
            Undefined,
            New,
            Regular,
            Old
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Year { get; private set; }
        public TitleType Type { get; set; }

        public VideoTitle(String name, int year)
        {
            Name = name;
            Year = year;
            Type = TitleType.New;
        }

        public VideoTitle(String name, int year, TitleType type)
        {
            Name = name;
            Year = year;
            Type = type;
        }

        public VideoTitle(String name, int year, int id)
        {
            Id = id;
            Name = name;
            Year = year;
            Type = TitleType.New;
        }

        public VideoTitle(VideoTitle title, int id)
        {
            Name = title.Name;
            Year = title.Year;
            Type = title.Type;
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return IsEqual((VideoTitle) obj);
        }

        public bool Equals(VideoTitle title)
        {
            if (ReferenceEquals(null, title)) return false;
            if (ReferenceEquals(this, title)) return true;

            return IsEqual(title);
        }

        private bool IsEqual(VideoTitle title)
        {
            return String.Equals(title.Name, Name)
                && title.Year == Year;
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) ^ (ReferenceEquals(null, Name) ? 0 : Name.GetHashCode());
            hash = (hash * 7) ^ Year.GetHashCode();
            return hash;
        }
    }
}
