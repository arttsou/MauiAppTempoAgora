using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "e7b1fe40983ad18df5a9a1df00b74ae4";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&appid={chave}&units=metric&lang=pt_br";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime time = new();
                    DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                    t = new()
                    {
                        lon = (double)rascunho["coord"]["lon"],
                        lat = (double)rascunho["coord"]["lat"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                        speed = (double)rascunho["wind"]["speed"],
                        description = (string)rascunho["weather"][0]["description"],
                        visibility = (int)rascunho["visibility"],
                        main = (string)rascunho["weather"][0]["main"]
                    }; // Fecha objeto do Tempo
                }//Fecha if do sucesso da resposta
                else
                {
                    switch (resp.StatusCode) 
                    { 
                        case HttpStatusCode.NotFound:
                            throw new Exception("Cidade não encontrada.");
                        case HttpStatusCode.Unauthorized:
                            throw new Exception("Chave de API inválida.");
                        case HttpStatusCode.RequestTimeout:
                            throw new Exception("Tempo de requisição esgotado.");
                        default:
                            throw new Exception($"Erro na requisição: {resp.ReasonPhrase}");
                    }
               
            }// fecha using do HttpClient

            return t;

        }
    }
}
