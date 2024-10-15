using ClienteIEMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClienteIEMS.Controllers
{
    public class ClienteController
    {
        static HttpClient client = new HttpClient();
        //Cambia la direccion del Servidor y el puerto en produccion
        static string servidor = "https://iemssiged.seg.guanajuato.gob.mx/wsIEMSdev/";

        public static async Task<cargaResponse> cargaCertificado(cargaRequest request)
        {
            try
            {
                client.BaseAddress = new Uri(servidor);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                cargaResponse respuesta = new cargaResponse();
                string json = JsonConvert.SerializeObject(request);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("certificados/carga", httpContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                respuesta = JsonConvert.DeserializeObject<cargaResponse>(responseContent);
                return respuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
                throw;
            }
        }
        public static async Task<consultaResponse> consultaCertificado(consultaRequest request)
        {
            client.BaseAddress = new Uri(servidor);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            consultaResponse respuesta = new consultaResponse();
            string json = JsonConvert.SerializeObject(request);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("certificados/consulta", httpContent);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            respuesta = JsonConvert.DeserializeObject<consultaResponse>(responseContent);
            return respuesta;
        }
        public static async Task<descargaResponse> descargaCertificado(descargaRequest request)
        {
            client.BaseAddress = new Uri(servidor);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            descargaResponse respuesta = new descargaResponse();
            string json = JsonConvert.SerializeObject(request);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("certificados/descarga", httpContent);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            respuesta = JsonConvert.DeserializeObject<descargaResponse>(responseContent);
            return respuesta;
        }
        public static async Task<cancelarResponse> cancelarCertificado(cancelarRequest request)
        {
            client.BaseAddress = new Uri(servidor);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            cancelarResponse respuesta = new cancelarResponse();
            string json = JsonConvert.SerializeObject(request);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("certificados/consulta", httpContent);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            respuesta = JsonConvert.DeserializeObject<cancelarResponse>(responseContent);
            return respuesta;
        }

    }
}
