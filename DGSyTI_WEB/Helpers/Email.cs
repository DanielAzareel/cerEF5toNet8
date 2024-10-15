using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using DGSyTI_WEB;


namespace Helpers
{
    public class Email
    {
        // Field to handle multiple calls to Dispose gracefully.
        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
        private string _direccionSMTP = null;
        public String direccionSMTP { get { return _direccionSMTP; } set { _direccionSMTP = value; } }
        /// <summary>
        /// 
        /// </summary>
        private string _UsuarioSMTP = null;
        public String UsuarioSMTP { get { return _UsuarioSMTP; } set { _UsuarioSMTP = value; } }
        /// <summary>
        /// 
        /// </summary>
        private string _PassSMTP = null;
        public String PassSMTP { get { return _PassSMTP; } set { _PassSMTP = value; } }
        /// <summary>
        /// 
        /// </summary>
        private Int32 _PuertoSMTP;
        public Int32 PuertoSMTP { get { return _PuertoSMTP; } set { _PuertoSMTP = value; } }
        /// <summary>
        /// 
        /// </summary>
        private bool _SSL = false;
        public bool SSL { get { return _SSL; } set { _SSL = value; } }
        /// <summary>
        /// 
        /// </summary>
        private string _Remitente_Mail = null;
        public String Remitente_Mail { get { return _Remitente_Mail; } set { _Remitente_Mail = value; } }

        private string _Coneccion = null;
        public String Coneccion { get { return _Coneccion; } set { _Coneccion = value; } }

        private bool _Servicio = false;
        public bool Servicio { get { return _Servicio; } set { _Servicio = value; } }

        private String _PathHTML = null;
        public String PathHTML { get { return _PathHTML; } set { _PathHTML = value; } }

        private String _Mensaje = null;
        public String Mensaje { get { return _Mensaje; } set { _Mensaje = value; } }

        private String _CC = null;
        public String CC { get { return _CC; } set { _CC = value; } }

        private String _TO = null;
        public String TO { get { return _TO; } set { _TO = value; } }

        private String _From = null;
        public String From { get { return _From; } set { _From = value; } }

        private String _BCC = null;
        public String BCC { get { return _BCC; } set { _BCC = value; } }

        private String _Asunto = null;
        public String Asunto { get { return _Asunto; } set { _Asunto = value; } }
        /// <summary>
        /// 
        /// </summary>
        private bool Resultado = false;
        /// <summary>
        /// 
        /// </summary>
        private Exception ResultadoExcepcion = null;
        /// <summary>
        /// Función que regresa true si se envio correctamente el correo
        /// y Regresa False en caso de que ocurra algun problema
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool leerResultado
        {
            get { return Resultado; }
        }

        /// <summary>
        /// Función que regresa el valor de la excepción en caso de algun error 
        /// al enviar el correo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Exception LeerExcepcion
        {
            get { return ResultadoExcepcion; }
        }


        private string GetHTMLFromURL(string URL)
        {
            System.Text.UTF8Encoding ASCII = new System.Text.UTF8Encoding(true);
            System.Net.WebClient netWeb = new System.Net.WebClient();
            string lsWeb = null;
            byte[] laWeb = null;
            try
            {
                laWeb = netWeb.DownloadData(URL);
                lsWeb = ASCII.GetString(laWeb);
            }
            catch (Exception ex)
            {
                ResultadoExcepcion = ex;
                return "";
            }
            return lsWeb;

        }

        private string GetApplicationPath()
        {
            string strPath = "";
            if (((HttpContext.Current.Request.Url != null)))
            {

                strPath = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().IndexOf(HttpContext.Current.Request.ApplicationPath.ToLower(), Convert.ToInt32((HttpContext.Current.Request.Url.AbsoluteUri.ToLower().IndexOf(HttpContext.Current.Request.Url.Authority.ToLower()) + HttpContext.Current.Request.Url.Authority.Length))) + HttpContext.Current.Request.ApplicationPath.Length));
            }
            strPath = strPath + "/";
            return strPath;
        }

        #region Funciones para datos con las propiedades y template HTML
        public void EnviarCorreoSMTP(string Remitente_Mensaje_From, string ArchivoAdjunto, string NuevoNombreArchivoAdjunto = "")
        {
            MailMessage mail = new MailMessage();
            string Remitente = "";
            string urlTemplate = null;
            StringBuilder Template = new StringBuilder();
            SmtpClient smtp = new SmtpClient();


            try
            {

                LeerDatosConfig();
                smtp.Host = direccionSMTP;
                urlTemplate = GetApplicationPath() + PathHTML;


                Template.Append(GetHTMLFromURL(urlTemplate));

                foreach (string men in Mensaje.Split(Convert.ToChar(0)))
                {
                    string[] cad = men.Split(Convert.ToChar(1));
                    if (cad.Length > 1)
                    {
                        Template.Replace(cad[0], cad[1]);
                    }
                }
                if (string.IsNullOrEmpty(Remitente_Mail))
                {
                    if (String.IsNullOrEmpty(From))
                        Remitente = UsuarioSMTP;
                    else
                        Remitente = From;
                }
                else
                {
                    Remitente = Remitente_Mail;
                }
                mail.From = new MailAddress(Remitente, Remitente_Mensaje_From);
                foreach (string Destino in TO.Split(Convert.ToChar((","))))
                {
                    if (!String.IsNullOrEmpty(Destino))
                        mail.To.Add(Destino);
                }
                if (!string.IsNullOrEmpty(BCC) & BCC != null)
                {
                    foreach (string destinoBCC in BCC.Split(Convert.ToChar(",")))
                    {
                        mail.Bcc.Add(destinoBCC);
                    }
                }
                if (!string.IsNullOrEmpty((CC)))
                {
                    foreach (string destinoCC in CC.Split(Convert.ToChar(",")))
                    {
                        mail.CC.Add(destinoCC);
                    }
                }
                mail.Subject = Asunto;
                mail.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(ArchivoAdjunto) & ArchivoAdjunto != null)
                {
                    Exception exc = null;
                    foreach (string ArchAdj in ArchivoAdjunto.Split(Convert.ToChar(",")))
                    {
                        Attachment ADJUNTO = CrearArchivoAdjunto(ArchivoAdjunto, NuevoNombreArchivoAdjunto, exc);
                        if (exc == null)
                        {
                            mail.Attachments.Add(ADJUNTO);
                        }
                    }
                }
                mail.Body = Template.ToString();
                smtp.Port = PuertoSMTP;
                smtp.Credentials = new NetworkCredential(UsuarioSMTP, PassSMTP);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = SSL;
                // mail.Priority = CType("masivo", MailPriority)
                smtp.Send(mail);

                Resultado = true;

            }
            catch (Exception ex)
            {
                Resultado = false;
                ResultadoExcepcion = ex;
            }
            finally
            {
                smtp = null;

                if ((mail != null))
                {
                    mail.Dispose();
                }


            }
        }

        #endregion

        #region "Funciones para envio Sin template HTML"


        public void EnviarCorreoSMTP(string Mensaje, string Asunto, string ArchivoAdjunto, string NuevoNombreArchivoAdjunto = "")
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();



            try
            {

                LeerDatosConfig();

                smtp.Host = _direccionSMTP;
                mail.From = new MailAddress(Remitente_Mail, "SEG");
                foreach (string Destino in TO.Split(Convert.ToChar((","))))
                {
                    if (Destino.Contains("@"))
                    {
                        mail.To.Add(Destino);
                    }
                }
                if (!string.IsNullOrEmpty(BCC) & BCC != null)
                {
                    foreach (string destinoBCC in BCC.Split(Convert.ToChar(",")))
                    {
                        mail.Bcc.Add(destinoBCC);
                    }
                }
                if (!string.IsNullOrEmpty(CC))
                {
                    foreach (string destinoCC in CC.Split(Convert.ToChar(",")))
                    {
                        mail.CC.Add(destinoCC);
                    }
                }
                mail.Subject = Asunto;

                mail.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(ArchivoAdjunto) & ArchivoAdjunto != null)
                {
                    Exception exc = null;
                    foreach (string ArchAdj in ArchivoAdjunto.Split(Convert.ToChar(",")))
                    {
                        Attachment ADJUNTO = CrearArchivoAdjunto(ArchivoAdjunto, exc);
                        if (exc == null)
                        {
                            mail.Attachments.Add(ADJUNTO);
                        }
                    }
                }
                mail.Body = Mensaje;
                smtp.Port = _PuertoSMTP;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_UsuarioSMTP, _PassSMTP);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = _SSL;
                smtp.Timeout = 60000;
                smtp.Send(mail);

          

                Resultado = true;


            }
            catch (Exception ex)
            {
                Resultado = false;

                Console.Write(ex.Message);

            }
            finally
            {
                smtp = null;
                if ((mail != null))
                {
                    mail.Dispose();
                }


            }
        }

        #endregion


        private Attachment CrearArchivoAdjunto(string path, string newname, Exception exc)
        {
            FileStream fstream = null;
            System.IO.MemoryStream memStream = null;
            StreamWriter StreamwW = null;
            byte[] Datos = null;
            Attachment adjunto = null;
            try
            {
                fstream = new FileStream(path, FileMode.Open);
                Datos = new byte[Convert.ToInt32((fstream.Length) - 1) + 1];
                fstream.Read(Datos, 0, Datos.Length);
                memStream = new MemoryStream(Datos);
                StreamwW = new StreamWriter(memStream);
                StreamwW.Flush();
                memStream.Position = 0;
                if (string.IsNullOrEmpty(newname))
                {
                    newname = System.IO.Path.GetFileName(path);
                }
                adjunto = new Attachment(memStream, newname);
            }
            catch (Exception ex)
            {
                exc = ex;
            }
            finally
            {
                fstream.Dispose();
                //memStream.Dispose()
                //StreamwW.Dispose()
                //Datos = Nothing
            }
            return adjunto;
        }

        private Attachment CrearArchivoAdjunto(byte[] File, string newname, Exception exc)
        {
            // Dim fstream As FileStream = Nothing
            System.IO.MemoryStream memStream = null;
            StreamWriter StreamwW = null;

            Attachment adjunto = null;
            try
            {
                memStream = new MemoryStream(File);
                StreamwW = new StreamWriter(memStream);
                StreamwW.Flush();
                memStream.Position = 0;
                if (string.IsNullOrEmpty(newname))
                {
                    newname = "Archivo1";
                }
                adjunto = new Attachment(memStream, newname);
            }
            catch (Exception ex)
            {
                exc = ex;
            }
            return adjunto;
        }

        private Attachment CrearArchivoAdjunto(string File, Exception exc)
        {
            Attachment adjunto = null;
            try
            {               
                adjunto = new Attachment(File);
            }
            catch (Exception ex)
            {
                exc = ex;
            }
            return adjunto;
        }
        private void LeerDatosConfig()
        {
            
            direccionSMTP = AppConfig.AppEnviroment.EmailHost;
            PuertoSMTP = Int32.Parse(AppConfig.AppEnviroment.EmailPort);
            UsuarioSMTP = AppConfig.AppEnviroment.EmailUser; 
            PassSMTP = AppConfig.AppEnviroment.EmailPass;
            SSL = AppConfig.AppEnviroment.EmailSSL;
            if (UsuarioSMTP.Contains("@"))
                Remitente_Mail = UsuarioSMTP;
            else
                Remitente_Mail = "SEG@" + UsuarioSMTP + ".gob.mx";
        }





        //Funciones para correo *************************************************************************************************
    }

}