using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UpcomingMovies.Core;
using UpcomingMovies.MVVM.ViewModel;

namespace UpcomingMovies.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для DiscoveryView.xaml
    /// </summary>
    public partial class DiscoveryView : UserControl
    {
        public DiscoveryView()
        {
            InitializeComponent();
        }

        ObservableCollection<TheMovieDb> movieDetails;
        public void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                StackPanel temp = (StackPanel)sender;
                TheMovieDb new_movie = (TheMovieDb)temp.DataContext;
                TmdbAPI tmdb = new TmdbAPI();
                movieDetails = tmdb.GetMovieDetails(new_movie.id);
                ((DiscoveryViewModel) DataContext).Model.movieDetailsView.DataContext = movieDetails;
                ((DiscoveryViewModel) DataContext).Model.CurrentView = ((DiscoveryViewModel)DataContext).Model.movieDetailsView;

                ((DiscoveryViewModel)DataContext).Model.IsMovieDetailsButtonChecked = true;
                ((DiscoveryViewModel)DataContext).Model.MovieDetailsVisibility = "Visible";
            }
        }
    }
}
