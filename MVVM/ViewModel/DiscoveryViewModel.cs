using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UpcomingMovies.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UpcomingMovies.MVVM.View;
using UpcomingMovies.Resources;

namespace UpcomingMovies.MVVM.ViewModel
{
    class DiscoveryViewModel : ObservableObjects
    {
        TmdbAPI tmdb = new TmdbAPI();
        PagingInfo paging_info;
        public DiscoveryViewModel()
        {
            tmdb.CallAPI();
            paging_info = tmdb.GetPagingInfo();

            _MoviesCollectionOnPage = tmdb.MoviesCollectionOnPage;
        }
        public MainViewModel Model { get; set; }

        private ObservableCollection<TheMovieDb> _MoviesCollectionOnPage = new ObservableCollection<TheMovieDb>();
        public ObservableCollection<TheMovieDb> MoviesCollectionOnPage
        {
            get
            {
                return _MoviesCollectionOnPage;
            }
            set
            {
                _MoviesCollectionOnPage = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand GoToNextPage
        {
            get {
                return new RelayCommand(o =>
              {
                  if (paging_info.CurrentPage != paging_info.TotalPages)
                  {
                      paging_info.CurrentPage++;
                      tmdb.GetMovies(paging_info.CurrentPage);
                  }
                  else
                  {
                      CustomMessageBox.Show(AllResources.NoMorePagesMessage, MessageBoxButton.OK);
                  }
              }); }
        }
        public ICommand GoToPreviousPage
        {
            get
            {
                return new RelayCommand(o =>
                {
                    if (paging_info.CurrentPage != paging_info.FirstPage)
                    {
                        paging_info.CurrentPage--;
                        tmdb.GetMovies(paging_info.CurrentPage);
                    }
                    else
                    {
                        CustomMessageBox.Show(AllResources.NoMorePagesMessage, MessageBoxButton.OK);
                    }
                });
            }
        }

        public ICommand GoToFirstPage
        {
            get
            {
                return new RelayCommand(o =>
                {
                    paging_info.CurrentPage = paging_info.FirstPage;
                    tmdb.GetMovies(paging_info.CurrentPage);
                });
            }
        }
        public ICommand GoToLastPage
        {
            get
            {
                return new RelayCommand(o =>
                {
                    paging_info.CurrentPage = paging_info.TotalPages;
                    tmdb.GetMovies(paging_info.CurrentPage);
                });
            }
        }
        public ICommand GoToMiddlePage
        {
            get
            {
                return new RelayCommand(o =>
                {
                    paging_info.CurrentPage = paging_info.MiddlePage;
                    tmdb.GetMovies(paging_info.CurrentPage);
                });
            }
        }
    }
}
