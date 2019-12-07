namespace Lands.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using Xamarin.Forms; 
    public class LandsViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion
        //Todo Biding en la Page, se deben de poner Atributos y Propiedades
        #region Attributes
        //Se pinta en ListView
        private ObservableCollection<LandItemViewModel> lands;
        private bool isRefreshing;
        private string filter;
        #endregion
        
        #region Properties
        public ObservableCollection<LandItemViewModel> Lands
        {
            get { return this.lands;  }
            set { SetValue(ref this.lands, value);}
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                //Realizar la busqueda sin dar enter
                this.Search(); 
            }
        }
        #endregion

        #region Constructor
        public LandsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadLands();
        }
        #endregion

        #region Methods
        private async void LoadLands()
        {
            this.IsRefreshing = true;
            //Validar el estatus de conexion del Telefono
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess )
            {
                this.IsRefreshing = false;
               await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Aceptar");
                //Regresa a la pantalla anterior
                await Application.Current.MainPage.Navigation.PopAsync(); 
                return;
            }
            //Conectamos a la Api de paises
            var response = await this.apiService.GetList<Land>(
                "http://restcountries.eu",
                "/rest",
                "/v2/all");
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
            MainViewModel.GetInstance().LandsList = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<LandItemViewModel>(
               this.ToLoadItemViewModel());
            this.IsRefreshing = false;
        }
        //herda una lista de LandItemViewModel de la lista Original, Esto para no tocar el modelo Paises
        private IEnumerable<LandItemViewModel> ToLoadItemViewModel()
        {
            return MainViewModel.GetInstance().LandsList.Select(l => new LandItemViewModel
            {
                Alpha2Code = l.Alpha2Code,
                Alpha3Code = l.Alpha3Code,
                AltSpellings = l.AltSpellings,
                Area = l.Area,
                Borders = l.Borders,
                callingCodes = l.callingCodes,
                Capital = l.Capital,
                Cioc = l.Cioc,
                Currencies = l.Currencies,
                Demonym = l.Demonym,
                Flag = l.Flag,
                Gini = l.Gini,
                Languages = l.Languages,
                Latlng = l.Latlng,
                Name = l.Name,
                NativeName = l.NativeName,
                NumericCode = l.NumericCode,
                Population = l.Population,
                Region = l.Region,
                RegionalBlocs = l.RegionalBlocs,
                Subregion = l.Subregion,
                Timezones = l.Timezones,
                TopLevelDomain = l.TopLevelDomain,
                Translations = l.Translations,
            });
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadLands);
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Lands = new ObservableCollection<LandItemViewModel>(
               this.ToLoadItemViewModel());
            }
            else
            {
                this.Lands = new ObservableCollection<LandItemViewModel>(
                    this.ToLoadItemViewModel().Where(
                        l =>l.Name.ToLower().Contains(this.Filter.ToLower())
                        || l.Capital.ToLower().Contains(this.Filter.ToLower())));
            }
        }
        #endregion
    }
}
