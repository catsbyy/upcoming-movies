using System;
using System.Windows;
using System.Windows.Input;
using UpcomingMovies.Core;
using UpcomingMovies.MVVM.View;
using UpcomingMovies.MVVM.ViewModel;
using UpcomingMovies.Resources;

namespace UpcomingMovies
{
    class MainViewModel : ObservableObjects
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand MovieDetailsViewCommand { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public DiscoveryViewModel DiscoveryVM { get; set; }
        public MovieDetailsViewModel MovieDetailsVM { get; set; }

        TmdbAPI tmdb = new TmdbAPI();

        public DiscoveryView discoveryView { get; set; }
        public HomeView homeView { get; set; }
        public MovieDetailsView movieDetailsView { get; set; }

        public string CurrentDate { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private bool isMovieDetailsButtonChecked { get; set; }
        public bool IsMovieDetailsButtonChecked
        {
            get { return isMovieDetailsButtonChecked; }
            set
            {
                isMovieDetailsButtonChecked = value;
                OnPropertyChanged();
            }
        }

        string movieDetailsVisibility { get; set; }
        public string MovieDetailsVisibility
        {
            get { return movieDetailsVisibility; }
            set
            {
                movieDetailsVisibility = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            try
            {
                CurrentDate = DateTime.Today.ToShortDateString();
                IsMovieDetailsButtonChecked = false;
                MovieDetailsVisibility = "Hidden";
                HomeVM = new HomeViewModel() { Model = this };
                DiscoveryVM = new DiscoveryViewModel() { Model = this };
                MovieDetailsVM = new MovieDetailsViewModel() { Model = this };

                discoveryView = new DiscoveryView() { DataContext = DiscoveryVM };
                homeView = new HomeView() { DataContext = HomeVM };
                movieDetailsView = new MovieDetailsView() { DataContext = tmdb.ExampleMovieDetails() };
                CurrentView = HomeVM;
            }
            catch
            {
                CustomMessageBox.Show(AllResources.InternetConnectionError, MessageBoxButton.OK);
            }
            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = homeView;
                MovieDetailsVisibility = "Hidden";
            });
            DiscoveryViewCommand = new RelayCommand(o =>
            {
                CurrentView = discoveryView;
                MovieDetailsVisibility = "Hidden";
            });
            MovieDetailsViewCommand = new RelayCommand(o =>
            {
                CurrentView = movieDetailsView;
            });  
        }

        public ICommand CloseApp
        {
            get
            {
                return new RelayCommand(o  =>
                {
                    if (CustomMessageBox.Show(AllResources.WantToClose, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Application.Current.Shutdown();
                    }
                });
            }
        }
        public ICommand MinimizeApp
        {
            get { return new RelayCommand(param => { CurWindowState = WindowState.Minimized; }); }
        }

        private WindowState _curWindowState;
        public WindowState CurWindowState
        {
            get { return _curWindowState; }
            set
            {
                _curWindowState = value;
                base.OnPropertyChanged("CurWindowState");
            }
        }


    }
}
