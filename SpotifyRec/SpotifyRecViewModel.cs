using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace SpotifyRec
{
    class SpotifyRecViewModel : INotifyPropertyChanged
    {
        private string enteredName;
        public string EnteredName
        {
            get { return enteredName; }
            set
            {
                enteredName = value;
                NotifyPropertyChanged("EnteredName");
            }
        }

        private Artist selectedArtist;
        public Artist SelectedArtist
        {
            get { return selectedArtist; }
            set
            {
                selectedArtist = value;
                NotifyPropertyChanged("SelectedArtist");
                ArtistDetails = SelectedArtist;
                if (SelectedArtist != null)
                {
                    GetRelatedArtists();
                    GetRecommendedArtists();
                    GetTopTracks(SelectedArtist.ID);
                }
            }
        }

        private Artist selectedRelatedOrRecommendedArtist;
        public Artist SelectedRelatedOrRecommendedArtist
        {
            get { return selectedRelatedOrRecommendedArtist; }
            set
            {
                selectedRelatedOrRecommendedArtist = value;
                NotifyPropertyChanged("SelectedRelatedOrRecommendedArtist");
                if (SelectedRelatedOrRecommendedArtist != null)
                {
                    ArtistDetails = SelectedRelatedOrRecommendedArtist;
                    GetTopTracks(SelectedRelatedOrRecommendedArtist.ID);
                }
            }
        }

        private Artist artistDetails;
        public Artist ArtistDetails
        {
            get { return artistDetails; }
            set
            {
                artistDetails = value;
                NotifyPropertyChanged("ArtistDetails");
            }
        }

        private Track selectedTrack;
        public Track SelectedTrack
        {
            get { return selectedTrack; }
            set
            {
                selectedTrack = value;
                NotifyPropertyChanged("SelectedTrack");
            }
        }

        public ObservableCollection<Artist> FoundArtists = new ObservableCollection<Artist>();
        public ObservableCollection<Artist> RelatedArtists = new ObservableCollection<Artist>();
        public ObservableCollection<Artist> RecommendedArtists = new ObservableCollection<Artist>();
        public ObservableCollection<Track> ArtistsTopTracks = new ObservableCollection<Track>();

        public RelayCommand SearchArtistCommand { get; private set; }
        public RelayCommand RefreshRecommendedArtistsCommand { get; private set; }
        public RelayCommand SearchSelectedArtistCommand { get; private set; }
        public RelayCommand OpenArtistCommand { get; private set; }

        private SpotifyRecService spotifyRecService = new SpotifyRecService();

        public SpotifyRecViewModel()
        {
            SearchArtistCommand = new RelayCommand(SearchArtist, CanSearchArtist);
            RefreshRecommendedArtistsCommand = new RelayCommand(RefreshRecommendedArtists, CanRefreshRecommendedArtists);
            SearchSelectedArtistCommand = new RelayCommand(SearchSelectedArtist, CanSearchSelectedArtist);
            OpenArtistCommand = new RelayCommand(OpenArtist, CanOpenArtist);
        }

        public async void SearchArtist(object parameter)
        {
            List<Artist> list = await spotifyRecService.SearchArtist(EnteredName);
            FoundArtists.Clear();
            RelatedArtists.Clear();
            RecommendedArtists.Clear();
            ArtistsTopTracks.Clear();

            foreach (Artist artist in list)
            {
                FoundArtists.Add(artist);
            }
        }

        public bool CanSearchArtist(object parameter)
        {
            if (EnteredName != null)
                return true;
            else
                return false;
        }

        public void RefreshRecommendedArtists(object parameter)
        {
            GetRecommendedArtists();
        }

        public bool CanRefreshRecommendedArtists(object parameter)
        {
            if (SelectedArtist != null)
                return true;
            else
                return false;
        }

        public void SearchSelectedArtist(object parameter)
        {
            EnteredName = ArtistDetails.Name;
            SelectedArtist = null;
            SearchArtist(null);
        }

        public bool CanSearchSelectedArtist(object parameter)
        {
            if (ArtistDetails != null)
                return true;
            else
                return false;
        }

        public void OpenArtist(object parameter)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = ArtistDetails.Uri,
                UseShellExecute = true
            });
        }

        public bool CanOpenArtist(object parameter)
        {
            if (ArtistDetails != null)
                return true;
            else
                return false;
        }

        public async void GetRelatedArtists()
        {
            List<Artist> list = await spotifyRecService.GetRelatedArtists(SelectedArtist.ID);
            RelatedArtists.Clear();

            foreach (Artist artist in list)
            {
                RelatedArtists.Add(artist);
            }
        }

        public async void GetRecommendedArtists()
        {
            List<Artist> list = await spotifyRecService.GetRecommendedArtists(SelectedArtist.ID);
            RecommendedArtists.Clear();

            foreach (Artist artist in list)
            {
                RecommendedArtists.Add(artist);
            }
        }

        private async void GetTopTracks(string iD)
        {
            List<Track> list = await spotifyRecService.GetTopTracks(iD);
            ArtistsTopTracks.Clear();

            foreach (Track track in list)
            {
                ArtistsTopTracks.Add(track);
            }
        }

        public void ReselectArtist()
        {
            if (ArtistDetails != null && ArtistDetails.ID != SelectedArtist.ID)
            {
                ArtistDetails = SelectedArtist;
                GetRelatedArtists();
                GetRecommendedArtists();
                GetTopTracks(SelectedArtist.ID);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
