using ServiciosWeb.CertificadoMediaSuperiorWS;
using System;
using System.Threading.Tasks;
using ClienteIEMS.Models;
using ClienteIEMS.Controllers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace BusinessModel.ServiceAccess
{
    public class SEPSA
    {
        public cargaResponse EnviarCarga(byte[] archivoBase64, string nombreArchivo, string usuario, string password)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            cargaResponse response = new cargaResponse();
            cargaRequest request = new cargaRequest
            {
                archivoBase64 = archivoBase64,
                autenticacion = new ClienteIEMS.Models.autenticacionType { password = password, usuario = usuario },
                nombreArchivo = nombreArchivo
            };
            try
            {
                HttpClient client = new HttpClient();
                if (client.BaseAddress == null)
                    client.BaseAddress = new Uri("https://iemssiged.seg.guanajuato.gob.mx/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var myContent = JsonConvert.SerializeObject(request);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage resp = client.PostAsync("certificados/carga/", byteContent).Result;

                if (resp.IsSuccessStatusCode)
                {
                    string resultado = resp.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<cargaResponse>(resultado);
                }
                else
                {
                    string resultado = resp.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(resultado);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public consultaResponse ConsultarEstatusCarga(string numeroLote, string usuario, string password)
        {
            consultaResponse response = new consultaResponse();
            consultaRequest request = new consultaRequest
            {
                numeroLote = numeroLote,
                autenticacion = new ClienteIEMS.Models.autenticacionType { password = password, usuario = usuario }
            };
            try
            {
                HttpClient client = new HttpClient();
                if (client.BaseAddress == null)
                    client.BaseAddress = new Uri("https://iemssiged.seg.guanajuato.gob.mx/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var myContent = JsonConvert.SerializeObject(request);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage resp = client.PostAsync("certificados/consulta/", byteContent).Result;

                if (resp.IsSuccessStatusCode)
                {
                    string resultado = resp.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<consultaResponse>(resultado);
                }
                else
                {
                    string resultado = resp.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(resultado);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public descargaResponse DescargaResultado(string numeroLote, string usuario, string password)
        {
            descargaResponse response = new descargaResponse();
            descargaRequest request = new descargaRequest
            {
                numeroLote = numeroLote,
                autenticacion = new ClienteIEMS.Models.autenticacionType { password = password, usuario = usuario }
            };
            try
            {
                HttpClient client = new HttpClient();
                if (client.BaseAddress == null)
                    client.BaseAddress = new Uri("https://iemssiged.seg.guanajuato.gob.mx/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var myContent = JsonConvert.SerializeObject(request);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage resp = client.PostAsync("certificados/descarga/", byteContent).Result;

                if (resp.IsSuccessStatusCode)
                {
                    string resultado = resp.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<descargaResponse>(resultado);
                }
                else
                {
                    string resultado = resp.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(resultado);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private ServiciosWeb.CertificadoMediaSuperiorWS.autenticacionType Autenticar(string usuario, string contrasenia)
        {
            return new ServiciosWeb.CertificadoMediaSuperiorWS.autenticacionType { password = contrasenia, usuario = usuario };
        }
        private ServiciosWeb.CertificadosIEMS.autenticacionType Autenticar2(string usuario, string contrasenia)
        {
            return new ServiciosWeb.CertificadosIEMS.autenticacionType { password = contrasenia, usuario = usuario };
        }
        public cancelarResponse CancelaCertificadoElectronico(string folioCertificado, string usuario, string password)
        {
            cancelarResponse response = new cancelarResponse();
            cancelarRequest request = new cancelarRequest
            {
                folioCertificado = folioCertificado,
                autenticacion = new ClienteIEMS.Models.autenticacionType { password = password, usuario = usuario }
            };
            try
            {
                HttpClient client = new HttpClient();
                if (client.BaseAddress == null)
                    client.BaseAddress = new Uri("https://iemssiged.seg.guanajuato.gob.mx/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var myContent = JsonConvert.SerializeObject(request);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage resp = client.PostAsync("certificados/cancelar/", byteContent).Result;

                if (resp.IsSuccessStatusCode)
                {
                    string resultado = resp.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<cancelarResponse>(resultado);
                }
                else
                {
                    string resultado = resp.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(resultado);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
