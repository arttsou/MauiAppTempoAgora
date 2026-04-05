using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);
                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat}\n" +
                                         $"Longitude: {t.lon}\n" +
                                         $"Condição: {t.main} - {t.description}\n" +
                                         $"Nascer do Sol: {t.sunrise}\n" +
                                         $"Pôr do Sol: {t.sunset}\n" +
                                         $"Temp Máx: {t.temp_max}°C\n" +
                                         $"Velocidade do Vento: {t.speed} m/s\n" +
                                         $"Visibilidade: {t.visibility} m\n" +
                                         $"Temp Min: {t.temp_min}°C\n";


                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Não foi possível obter a previsão para a cidade informada. Verifique o nome e tente novamente.";    
                    }
                }
                else
                {
                    lbl_res.Text= "Por favor, insira o nome de uma cidade.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

        }
    }
}
