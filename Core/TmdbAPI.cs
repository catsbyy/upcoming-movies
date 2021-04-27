using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using UpcomingMovies.MVVM.Model;
using UpcomingMovies.Resources;

namespace UpcomingMovies
{
    class TmdbAPI
    {
        public ObservableCollection<TheMovieDb> MoviesCollectionOnPage = new ObservableCollection<TheMovieDb>();

        int total_pages;

        //метод, першочергово викликається для відображення першої сторінки з фільмами
        public void CallAPI()
        {
            string upcomingMoviesLink = AllResources.AllMoviesLinkTemplate + ResourceAPI.api_key + AllResources.SortForLink;

            HttpWebRequest apiRequest = WebRequest.Create(upcomingMoviesLink) as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            ResponseSearchMovies rootObject = JsonConvert.DeserializeObject<ResponseSearchMovies>(apiResponse);

            total_pages = rootObject.total_pages;

            GetMovies(1);
        }

        //метод, який формує рядок із правильним відображенням дати
        private string RightDateFormat(string date)
        {
            string new_date = "";
            if (date !="")
            {
                string[] subs = date.Split('-');
                new_date = subs[2] + "." + subs[1] + "." + subs[0];
            }
            return new_date;
        }

        //метод, надсилає запит та оброблює його, щоб відобразити всі фільми на конкретній сторінці 
        public void GetMovies(int page_number)
        {
            MoviesCollectionOnPage.Clear();

            string upcomingMoviesLink = AllResources.AllMoviesLinkTemplate + ResourceAPI.api_key + AllResources.PageForLink + page_number + AllResources.RegionForLink;

            HttpWebRequest apiRequest = WebRequest.Create(upcomingMoviesLink) as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            ResponseSearchMovies rootObject = JsonConvert.DeserializeObject<ResponseSearchMovies>(apiResponse);

            foreach (Result result in rootObject.results)
            {
                TheMovieDb movie = new TheMovieDb();
                movie.title = result.title;
                movie.poster_path = CreatePosterPath(result.poster_path);
                movie.release_date = RightDateFormat(result.release_date);
                movie.id = result.id;

                MoviesCollectionOnPage.Add(movie);
            }
        }

        //метод, який повертає значення про кількість сторінок та кількість фільмів
        public PagingInfo GetPagingInfo()
        {
            PagingInfo paging_info = new PagingInfo();
            paging_info.CurrentPage = 1;
            paging_info.TotalPages = total_pages;
            paging_info.FirstPage = 1;
            paging_info.MiddlePage = GetMiddlePage();
            return paging_info;
        }

        //метод повертає середню сторінку
        private int GetMiddlePage()
        {
            int middlePage;
            if (total_pages%2==0)
            {
                middlePage = total_pages / 2;
            }
            else
            {
                middlePage = (total_pages + 1) / 2;
            }
            return middlePage;
        }

        //метод повертає повну адресу постера
        private string CreatePosterPath(string poster_path)
        {
            string full_posterpath = "";
            if (poster_path != null)
            {
                full_posterpath = AllResources.ForImageURL + poster_path;
            }
            else
            {
                full_posterpath = AllResources.NoImagePath;
            }
            return full_posterpath;
        }

        //метод, який формує рядок з усіх компаній-виробників фільму
        private string GetAllCompanies(List<Company> production_companies)
        {
            string all_companies = "";
            if (production_companies.Count != 0)
            {
                foreach (Company company in production_companies)
                {
                    all_companies += company.name + ", ";
                }
                all_companies = all_companies.Substring(0, all_companies.Length - 2);
            }
            return all_companies;
        }

        //метод, який рахує кількість днів до або після релізу
        private string GetDaysLeft(string release_date)
        {
            string daysLeft = "";
            if (release_date!="")
            {
                if ((DateTime.Today) != DateTime.Parse(release_date))
                {
                    daysLeft = (DateTime.Parse(release_date) - DateTime.Today).ToString();
                    daysLeft = daysLeft.Substring(0, daysLeft.IndexOf('.'));
                }
                else
                {
                    daysLeft = AllResources.NoDaysLeft;
                }
                if (int.Parse(daysLeft) < 0)
                {
                    daysLeft = daysLeft.Remove(0, 1) + AllResources.ForDaysLeft;

                }
            }
            return daysLeft;
        }

        //метод, який формує рядок з усіх жанрів фільму
        private string GetAllGenres(List<Genre> genres)
        {
            string all_genres="";
            if (genres.Count != 0)
            {
                foreach (Genre genre in genres)
                {
                    all_genres += genre.name + ", ";
                }
                all_genres = all_genres.Substring(0, all_genres.Length - 2);
            }
            return all_genres;
        }

        //метод, який формує рядок з усіх країн фільму
        private string GetAllCountries(List<Country> countries)
        {
            string all_countries = "";
            if (countries.Count !=0)
            {
                foreach (Country country in countries)
                {
                    all_countries += country.name + ", ";
                }
                all_countries = all_countries.Substring(0, all_countries.Length - 2);
            }
            return all_countries;

        }

        //метод, який формує рядок з усіх мов у фільмі
        private string GetAllLanguages(List<Language> spoken_languages)
        {
            string all_languages = "";
            if (spoken_languages.Count != 0)
            {
                foreach (Language language in spoken_languages)
                {
                    all_languages += language.english_name + ", ";
                }
                all_languages = all_languages.Substring(0, all_languages.Length - 2);
            }
            return all_languages;
        }

        //метод, який заповнює екземпляр фільмів даними
        private TheMovieDb CreateMovie(TheMovieDb movie, TheMovieDb rootObject)
        {
            movie.genres = rootObject.genres;
            movie.all_genres = GetAllGenres(movie.genres);
            movie.title = rootObject.title;
            movie.original_title = rootObject.original_title;
            movie.overview = rootObject.overview;
            movie.poster_path = CreatePosterPath(rootObject.poster_path);
            movie.production_countries = rootObject.production_countries;

            movie.all_countries = GetAllCountries(movie.production_countries);
            movie.production_companies = rootObject.production_companies;
            movie.all_companies = GetAllCompanies(movie.production_companies);
            movie.release_date = RightDateFormat(rootObject.release_date);
            movie.days_left = GetDaysLeft(movie.release_date);
            movie.runtime = rootObject.runtime;
            movie.tagline = rootObject.tagline;
            movie.vote_average = rootObject.vote_average;
            movie.spoken_languages = rootObject.spoken_languages;
            movie.all_languages = GetAllLanguages(movie.spoken_languages);
            movie.status = rootObject.status;
            return movie;
        }

        //метод, який відправляє та оброблює новий запит для відображення детальної інформації фільму
        public ObservableCollection<TheMovieDb> GetMovieDetails(int movieId)
        {
            ObservableCollection<TheMovieDb> movieDetails = new ObservableCollection<TheMovieDb>();
            
            string movieLink = AllResources.MovieLinkTemplate + movieId + AllResources.ApiKeyForLink + ResourceAPI.api_key + AllResources.RegionForLink;
            HttpWebRequest apiRequest = WebRequest.Create(movieLink) as HttpWebRequest;

            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }

            TheMovieDb rootObject = JsonConvert.DeserializeObject<TheMovieDb>(apiResponse);
            TheMovieDb movie = new TheMovieDb();
            movie = CreateMovie(movie, rootObject);
            
            movieDetails.Add(movie);
            return movieDetails;
        }

        //порожня колекція
        public ObservableCollection<TheMovieDb> ExampleMovieDetails()
        {
            ObservableCollection<TheMovieDb> exampleMovieDetails = new ObservableCollection<TheMovieDb>();

            //exampleMovieDetails = GetMovieDetails(466272);

            return exampleMovieDetails;
        }
    }
}
