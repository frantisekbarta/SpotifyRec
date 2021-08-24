using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SpotifyRec
{
    public partial class SpotifyRecView : Window
    {
        SpotifyRecViewModel spotifyRecViewModel = new SpotifyRecViewModel();

        public SpotifyRecView()
        {
            InitializeComponent();
            DataContext = spotifyRecViewModel;
            Artists_ListBox.ItemsSource = spotifyRecViewModel.FoundArtists;
            RelatedArtists_ListBox.ItemsSource = spotifyRecViewModel.RelatedArtists;
            RecommendedArtists_ListBox.ItemsSource = spotifyRecViewModel.RecommendedArtists;
            ArtistsTopTracks_ListBox.ItemsSource = spotifyRecViewModel.ArtistsTopTracks;
            Search_TextBox.Focus();
        }

        private void Search_TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // disable repeat if enter key is held down:
            if (e.Key == Key.Return && e.IsRepeat)
                e.Handled = true;

            // scroll listbox to top:
            if (e.Key == Key.Return)
            {
                if (Artists_ListBox.Items.Count > 0)
                    Artists_ListBox.ScrollIntoView(Artists_ListBox.Items[0]);
            }
        }

        private void Artists_ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // scroll listbox to top:
            if (RelatedArtists_ListBox.Items.Count > 0)
                RelatedArtists_ListBox.ScrollIntoView(RelatedArtists_ListBox.Items[0]);
            if (RecommendedArtists_ListBox.Items.Count > 0)
                RecommendedArtists_ListBox.ScrollIntoView(RecommendedArtists_ListBox.Items[0]);
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            // scroll listbox to top:
            if (RecommendedArtists_ListBox.Items.Count > 0)
                RecommendedArtists_ListBox.ScrollIntoView(RecommendedArtists_ListBox.Items[0]);
        }

        private void Artists_ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // detect click on listbox item:
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                spotifyRecViewModel.ReselectArtist();
                // scroll listbox to top:
                if (RelatedArtists_ListBox.Items.Count > 0)
                    RelatedArtists_ListBox.ScrollIntoView(RelatedArtists_ListBox.Items[0]);
                if (RecommendedArtists_ListBox.Items.Count > 0)
                    RecommendedArtists_ListBox.ScrollIntoView(RecommendedArtists_ListBox.Items[0]);
            }
        }
    }
}
