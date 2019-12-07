namespace Lands.ViewModels
{
    using System.Collections.Generic;
    using Models;
    public class MainViewModel
    {
        //Se agregan todas las clases que se crean en la carpeta ViewModels
        #region Properties
        //Se crea un Objeto local que cargue los paises para poder trabajar sobre el con la busqueda
        public List<Land> LandsList
        {
            get;
            set;
        }
        #endregion

        #region Viewmodels
        public LoginViewModel Login
        {
            get;
            set;
        }
        public LandsViewModel Lands
        {
            get;
            set;
        }
        public LandViewModel Land
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Login = new LoginViewModel();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
            //http://restcountries.eu/rest/v2/all
        }
        #endregion
    }
}
