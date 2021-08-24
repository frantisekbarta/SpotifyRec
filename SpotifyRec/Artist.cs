namespace SpotifyRec
{
    public class Artist
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string SmallImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
        public int Popularity { get; set; }
        public string Genres { get; set; }

        public Artist(string iD, string name, string uri, string smallImageUrl, string largeImageUrl, int popularity, string genres)
        {
            ID = iD;
            Name = name;
            Uri = uri;
            SmallImageUrl = smallImageUrl;
            LargeImageUrl = largeImageUrl;
            Popularity = popularity;
            Genres = genres;
        }
    }
}
