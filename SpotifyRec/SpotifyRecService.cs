using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SpotifyRec
{
    // https://developer.spotify.com/documentation/web-api/reference/#endpoint-get-an-artists-related-artists
    // https://developer.spotify.com/documentation/web-api/reference/#endpoint-get-recommendations
    // https://developer.spotify.com/documentation/web-api/reference/#category-tracks
    // https://developer.spotify.com/documentation/web-api/reference/#endpoint-get-audio-features
    // https://developer.spotify.com/documentation/web-api/reference/#object-audiofeaturesobject
    // https://github.com/JohnnyCrazy/SpotifyAPI-NET
    // https://johnnycrazy.github.io/SpotifyAPI-NET/docs/getting_started

    class SpotifyRecService
    {
        SpotifyClientConfig spotifyClientConfig;
        SpotifyClient spotifyClient;

        public SpotifyRecService()
        {
            spotifyClientConfig = SpotifyClientConfig
              .CreateDefault()
              .WithAuthenticator(new ClientCredentialsAuthenticator("client_id", "client_secret"));
            spotifyClient = new SpotifyClient(spotifyClientConfig);
        }

        public async Task<List<Artist>> SearchArtist(string artistsName)
        {
            List<Artist> foundArtists = new List<Artist>();
            if (!string.IsNullOrEmpty(artistsName))
            {
                try
                {
                    SearchRequest searchRequest = new SearchRequest(SearchRequest.Types.Artist, artistsName);
                    SearchResponse searchResponse = await spotifyClient.Search.Item(searchRequest);

                    foundArtists = ProcessArtist(searchResponse.Artists.Items);
                }
                catch
                {
                    // in case service is not available:
                    MessageBox.Show("Service not available.", "SpotifyRec", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return foundArtists;
        }

        public async Task<List<Artist>> GetRelatedArtists(string artistsID)
        {
            List<Artist> relatedArtists = new List<Artist>();
            ArtistsRelatedArtistsResponse artistsRelatedArtistsResponse = await spotifyClient.Artists.GetRelatedArtists(artistsID);

            relatedArtists = ProcessArtist(artistsRelatedArtistsResponse.Artists);

            return relatedArtists;
        }

        public async Task<List<Track>> GetTopTracks(string artistsID)
        {
            List<Track> topTracks = new List<Track>();
            ArtistsTopTracksRequest artistsTopTracksRequest = new ArtistsTopTracksRequest("CZ");
            ArtistsTopTracksResponse artistsTopTracksResponse = await spotifyClient.Artists.GetTopTracks(artistsID, artistsTopTracksRequest);

            topTracks = await ProcessTrack(artistsTopTracksResponse.Tracks);

            return topTracks;
        }

        public async Task<List<Artist>> GetRecommendedArtists(string artistsID)
        {
            RecommendationsRequest recommendationsRequest = new RecommendationsRequest();
            recommendationsRequest.SeedArtists.Add(artistsID);
            RecommendationsResponse recommendationsResponse = await spotifyClient.Browse.GetRecommendations(recommendationsRequest);

            List<string> recommendedArtistsIDs = new List<string>();

            foreach (var track in recommendationsResponse.Tracks)
            {
                foreach (var artist in track.Artists)
                {
                    recommendedArtistsIDs.Add(new string(artist.Id));
                }
            }

            recommendedArtistsIDs = recommendedArtistsIDs.Distinct().ToList();
            recommendedArtistsIDs = recommendedArtistsIDs.GetRange(0, Math.Min(recommendedArtistsIDs.Count, 50)); // maximum 50 IDs for Get Multiple Artists

            List<Artist> recommendedArtists = new List<Artist>();
            try
            {
                ArtistsRequest artistsRequest = new ArtistsRequest(recommendedArtistsIDs);
                ArtistsResponse artistsResponse = await spotifyClient.Artists.GetSeveral(artistsRequest);

                recommendedArtists = ProcessArtist(artistsResponse.Artists);

                recommendedArtists.RemoveAll(i => i.ID == artistsID);
                recommendedArtists = recommendedArtists.OrderByDescending(p => p.Popularity).ToList();
            }
            catch
            {
                // in case no recommended artists are available
            }

            return recommendedArtists;
        }

        private List<Artist> ProcessArtist(List<FullArtist> fullArtists)
        {
            List<Artist> artists = new List<Artist>();

            foreach (var item in fullArtists)
            {
                string smallImageUrl = "";
                string largeImageUrl = "";
                List<int> imageSizes = new List<int>();

                foreach (var image in item.Images)
                {
                    imageSizes.Add(image.Width);
                }

                if (imageSizes.Count > 0)
                {
                    smallImageUrl = item.Images[imageSizes.IndexOf(imageSizes.Min())].Url;
                    largeImageUrl = item.Images[imageSizes.IndexOf(imageSizes.Max())].Url;
                }
                else
                {
                    smallImageUrl = "blank_image.png";
                    largeImageUrl = "blank_image.png";
                }

                string genres = "";
                foreach (string genre in item.Genres)
                {
                    genres += genre + ", ";
                }

                if (item.Genres.Count > 0)
                    genres = genres.Substring(0, genres.Length - 2);
                artists.Add(new Artist(item.Id, item.Name, item.Uri, smallImageUrl, largeImageUrl, item.Popularity, genres));
            }

            return artists;
        }

        private async Task<List<Track>> ProcessTrack(List<FullTrack> fullTracks)
        {
            List<Track> tracks = new List<Track>();
            List<string> iDs = new List<string>();

            foreach (var item in fullTracks)
            {
                tracks.Add(new Track(item.Id, item.Name, item.Uri, item.Popularity, 0, "", 0, 0, 0, "", 0, 0, "", 0, 0, 0, 0));
                iDs.Add(item.Id);
            }

            try
            {
                TracksAudioFeaturesRequest tracksAudioFeaturesRequest = new TracksAudioFeaturesRequest(iDs);
                TracksAudioFeaturesResponse tracksAudioFeaturesResponse = await spotifyClient.Tracks.GetSeveralAudioFeatures(tracksAudioFeaturesRequest);

                int index = 0;
                foreach (var item in tracksAudioFeaturesResponse.AudioFeatures)
                {
                    tracks[index].Acousticness = item.Acousticness;
                    tracks[index].AnalysisUrl = item.AnalysisUrl;
                    tracks[index].Danceability = item.Danceability;
                    tracks[index].Energy = item.Energy;
                    tracks[index].Instrumentalness = item.Instrumentalness;
                    if (item.Key == 0) tracks[index].Key = "C";
                    if (item.Key == 1) tracks[index].Key = "C♯, D♭";
                    if (item.Key == 2) tracks[index].Key = "D";
                    if (item.Key == 3) tracks[index].Key = "D♯, E♭";
                    if (item.Key == 4) tracks[index].Key = "E";
                    if (item.Key == 5) tracks[index].Key = "F";
                    if (item.Key == 6) tracks[index].Key = "F♯, G♭";
                    if (item.Key == 7) tracks[index].Key = "G";
                    if (item.Key == 8) tracks[index].Key = "G♯, A♭";
                    if (item.Key == 9) tracks[index].Key = "A";
                    if (item.Key == 10) tracks[index].Key = "A♯, B♭";
                    if (item.Key == 11) tracks[index].Key = "B";
                    tracks[index].Liveness = item.Liveness;
                    tracks[index].Loudness = item.Loudness;
                    if (item.Mode == 0) tracks[index].Mode = "Minor";
                    if (item.Mode == 1) tracks[index].Mode = "Major";
                    tracks[index].Speechiness = item.Speechiness;
                    tracks[index].Tempo = (int)item.Tempo;
                    tracks[index].TimeSignature = item.TimeSignature;
                    tracks[index].Valence = item.Valence;
                    index++;
                }
            }
            catch
            {
                // in case no top tracks are available
            }

            return tracks;
        }
    }
}
