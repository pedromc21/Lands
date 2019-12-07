namespace Lands.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel 
    {
        #region Attributes
        private String email;
        private String pwd;
        private bool is_Running;
        private bool is_Enabled;
        #endregion
        #region Properties
        public string TxtEmail {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }
        public string TxtPwd {
            get { return this.pwd; }
            set { SetValue(ref this.pwd, value);}
        }
        public bool IsRunning {
            get { return this.is_Running; }
            set { SetValue(ref this.is_Running, value); }
        }
        public bool IsRemembered { get; set; }
        public bool IsEnabled {
            get { return this.is_Enabled; }
            set { SetValue(ref this.is_Enabled, value); }
        }
        #endregion
        #region Constructors
        //Es como el evento Load
        public LoginViewModel()
        {
            this.IsRemembered = true;
            this.IsEnabled = true;
            this.TxtEmail = "pedro";
            this.TxtPwd = "123";
        }
        #endregion
        #region Commands
        public ICommand LoginCommand
        {
            get { return new RelayCommand(Login); }
        }
        private async void Login()
        {
            if (String.IsNullOrEmpty(this.TxtEmail))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Falta colocar el Email",
                    "Aceptar");
                return;
            }
            if (String.IsNullOrEmpty(this.TxtPwd))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Falta colocar su contraseña",
                    "Aceptar");
                return;
            }
            this.IsRunning = true;
            this.IsEnabled = false;
            if (this.TxtEmail != "pedro" || this.TxtPwd !="123")
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                   "Error",
                   "Usuario incorrecto",
                   "Aceptar");
                this.pwd = string.Empty;
                return;
            }
            this.IsRunning = false;
            this.IsEnabled = true;
            //Limpiar Text
            this.TxtEmail = string.Empty;
            this.TxtPwd = string.Empty;
            //Lamado a otro View (Ventana)
            MainViewModel.GetInstance().Lands = new LandsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new LandsPage());
        }
        #endregion

    }
}
