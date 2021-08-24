namespace SpotifyRec
{
    public class Track
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public int Popularity { get; set; }
        public float Acousticness { get; set; }
        public string AnalysisUrl { get; set; }
        public float Danceability { get; set; }
        public float Energy { get; set; }
        public float Instrumentalness { get; set; }
        public string Key { get; set; }
        public float Liveness { get; set; }
        public float Loudness { get; set; }
        public string Mode { get; set; }
        public float Speechiness { get; set; }
        public float Tempo { get; set; }
        public int TimeSignature { get; set; }
        public float Valence { get; set; }

        public Track(string iD, string name, string uri, int popularity, float acousticness, string analysisUrl, float danceability, float energy, float instrumentalness, string key,
            float liveliness, float loudnesss, string mode, float speechiness, int tempo, int timeSignature, float valence)
        {
            ID = iD;
            Name = name;
            Uri = uri;
            Popularity = popularity;
            Acousticness = acousticness;
            AnalysisUrl = analysisUrl;
            Danceability = danceability;
            Energy = energy;
            Instrumentalness = instrumentalness;
            Key = key;
            Liveness = liveliness;
            Loudness = loudnesss;
            Mode = mode;
            Speechiness = speechiness;
            Tempo = tempo;
            TimeSignature = timeSignature;
            Valence = valence;
        }
    }
}
