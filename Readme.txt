SpotifyRec is a WPF application that retrieves selected data from the Spotify Web API via the SpotifyAPI Web wrapper by Jonas Dellinger. It focuses on artists – after searching an artist by name you can:

- see related artists (also shown under "Fans also like" in the Spotify app)
- get other artist recommendations (based on Spotify Web API Get recommendations function)
- see a list of top tracks (also shown under "Popular" in the Spotify app)
- see selected API only artist and track information (popularity, genre and audio features)

The list of recommended artists is obtained by calling the API Get recommendations function with the Artist as a seed. Get recommendations returns tracks – the list contains all artists in recommended tracks. It can be refreshed to get different results.

A selected artist's name from any of the lists can be used for a new search. The application doesn’t include a track player, but you can open a selected artist in your local Spotify app.
Some audio features are displayed in a different format than in the API response: Key directly shows tones according to Pitch class, Mode directly shows modality (Major or Minor) and Tempo is rounded.

