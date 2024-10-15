using BusinessModel.DataAccess;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using static BusinessModel.Models.CertificadoML;


namespace BusinessModel.Business
{
    public class CertificadosBL
    {
        
        public CertificadoML GetListadoCertificadosDescripcionesByCURP(string curp, string insId)
        {
            CertificadoML certificado = new CertificadoML();
            certificado.cerDocumentos = new CerDocumentoDAL().getDocumentosByCURP(curp, insId);
                           var planes= new CerCatPlanDAL().GetLstPlanesByIns(insId);
            var areasConocimientos = new CerAreaConocimientoDAL().GetAreasConocimientoByInsId(insId);
            certificado.cerDocumentos.ForEach(c => c.docPlanId = (from x in planes where x.idPlan == c.docPlanId select x.planDescripcion).FirstOrDefault());
            certificado.cerDocumentos.ForEach(c => c.docAreaEspecializacion = (from x in areasConocimientos where x.idAreaConocimiento == c.docAreaEspecializacion select x.areaConocimientoDescripcion).FirstOrDefault());
            return certificado;
        }
        public string[] verificaTokenDescarga(string idDocumento, string tokenDescarga)
        {
            string[] result = new string[2];
            if (tokenDescarga != null && tokenDescarga != "")
            {
                result[0] = new CerTokenDescargaDAL().verificaToken(idDocumento, tokenDescarga).ToString();
                result[1] = result[0] == "True" ? idDocumento : "El código ingresado no es válido, por favor intente nuevamente o solicite un nuevo código.";
            }
            else
            {
                result[0] = "False";
                result[1] = "Debe ingresar un código de descarga.";
            }

            return result;
        }
        // Generar pdf del certificado, regresa un stream del documento
        public Stream GenerarPDF(string docId)
        {

            // Obtener cerDocumento y el id de plantilla que le corresponde
            cerDocumento documento = new CerDocumentoDAL().GetDatosDocumentoById(docId);
            string planId = documento.planId;

            // Obtener nombre de plantilla de la bd y ruta de la carpeta content para verificar existencia del archivo


            string planNombre = documento.planId + ".docx";
            string rutaCarpetaContent = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaPlantillas"]);
            string rutaPlantilla = Path.Combine(rutaCarpetaContent, documento.planId, planNombre);
            if (!Directory.Exists(rutaCarpetaContent)) Directory.CreateDirectory(rutaCarpetaContent);
            if (!File.Exists(rutaPlantilla))
            {
                cerCatPlantilla plantilla = ObtenerCatPlantillaById(planId);
                //PlantillaBL.extraerFilePlantilla(plantilla.planArchivo, plantilla.planId);
            }

            bool certificadoGenerado = false;

            // Si no se generó el título mandamos pdf con mensaje
            if (!certificadoGenerado)
            {
                string rutaTituloNoDisponible = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaTituloNoDisponible"]);
                return new FileStream(rutaTituloNoDisponible, FileMode.Open);
            }

            return null;
        }
        public cerCatPlantilla ObtenerCatPlantillaById(string planId)
        {
            return null;//new CerCatPlantillaDAL().ObtenerCatPlantillaById(planId);
        }
        public String[] enviaCorreoProfesionista(CertificadoML certificado, criteriosBusquedaCertificadosML filtros, string usrLogin, bool sinObservaciones = true)
        {
            String[] result = new String[2];
            List<BitacoraML> listBitacora = new List<BitacoraML>();
            certificado.docAlumnoCurp = usrLogin;
            string cuerpoMensaje = crearCuerpoMensaje(certificado);
            string asunto = cuerpoMensaje == "" ? "" : "Descarga de certificado electrónico de Preparatoria Abierta y a Distancia";

            result[1] = (result[0] = verificarExisteCertificado(certificado, filtros).ToString()) == "True" ?

                    (result[0] = EnvioCorreo(certificado.docCorreo, "", asunto, cuerpoMensaje, "").ToString()) == "True" ?

                         (sinObservaciones || actualizaCorreo(certificado)) ?
                         "Se envió el correo y actualizó el registro de certificado." : "Se envió el correo."
                    : "No fue posible enviar el correo."
                : "No cuenta con acceso a este registro de certificado.";

            listBitacora.Add(new BitacoraML()
            {
                bitId = Guid.NewGuid().ToString(),
                bitFecha = DateTime.Now,
                
                bitUsuario = usrLogin,
                bitDescripcion = "docId: " + certificado.docId + " correo: " + certificado.docCorreo + " mensaje: " + result[1],
                bitExitoso = Convert.ToBoolean(result[0]),
                accId = "enviarCorreo"
            });

            DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
            StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

            return result;
        }
        public String crearCuerpoMensaje(CertificadoML certificado)
        {
            string rutaPortal = ConfigurationManager.AppSettings["rutaPortalDescarga"];
            string rutaAvisoprivacidad = ConfigurationManager.AppSettings["rutaAvisoprivacidad"];
            string rutaPaginaSEG = ConfigurationManager.AppSettings["rutaPaginaSEG"];
            string codigo = obtenerToken(certificado);
            codigo = codigo == "" ? obtenerToken(certificado) : codigo;

            codigo = HttpUtility.HtmlEncode(codigo);

            return codigo == "" ? "" : "<div style='font-size: 16px;font-family: Arial;'>" +
                        "<p>Para descargar tu certificado electrónico ingresa con tu CURP en el siguiente portal de la Secretaría de Educación de Guanajuato, <a href=&quot" + rutaPortal + "&quot>" + rutaPortal+"</a>, selecciona el módulo <b>Certificados de Preparatoria Abierta y a Distancia</b> y captura el siguiente código de seguridad: " +
                        "</p>" +
                "<div>" +
                "<div style='font-size: 21.5px;font-family: Times New Roman,Arial;font-weight: bold;text-align:center'>" +
                    codigo +
                "</div>" +
                "<div style='font-size: 12px;font-family: Arial;text-align:center'>" +
                "Este es un mensaje automático y no es necesario responder." +
                "</div>" +
                "<div style='font-style: italic;font-size: 10.5px;font-family: Arial;text-align:justify'>" +
                    "<p>Aviso de Privacidad <br>" +
                    "Versión Simplificada <br>" +
                    "La Secretaría de Educación del Estado de Guanajuato (en adelante Secretaría), de conformidad con lo establecido en los artículos:" +
                        " 6, apartado A, fracción II, y 16 párrafo segundo de la Constitución Política para los Estados Unidos Mexicanos; 14 inciso B, fracciones" +
                        " II y III, de la Constitución Política para el Estado de Guanajuato; 13 fracción III, 15, 25, de la Ley Orgánica del Poder Ejecutivo para el" +
                        " Estado de Guanajuato y 13, 35, 36, 37, 38, 96, 97, 98, 99, 100 y 101 de la Ley de Protección de Datos Personales en Posesión de Sujetos " +
                        "Obligados para el Estado de Guanajuato, le informa que la protección de sus datos personales es un derecho humano vinculado a la protección" +
                        " de su privacidad.<br> Cabe señalar que los datos personales, se refieren a cualquier información concerniente a una persona física identificada " +
                        "o identificable y los datos personales sensibles, son aquellos que afecten a la esfera más íntima de su titular, o cuya utilización indebida" +
                        " pueda dar origen a discriminación o conlleve un riesgo grave para éste.<br>" +
                    "Sus datos personales de conformidad con las funciones propias de esta Secretaría pueden ser utilizados para las siguientes finalidades:<br>" +
                        "*Para la prestación del servicio educativo: Procesos de inscripciones, revalidaciones, certificaciones, autenticaciones, titulaciones, " +
                        "tramitación y expedición de cédulas profesionales, incorporaciones de instituciones particulares para impartir educación, así como diversos apoyos a" +
                        " través de programas.</p>" +
                    "<p>El Aviso de Privacidad Integral puede ser consultado en la página Institucional de esta Secretaría ( <a href=&quot" + rutaPaginaSEG + "&quot>" + rutaPaginaSEG + "</a>) o bien, de manera" +
                        " directa en la siguiente liga electrónica: <a href=&quot" + rutaAvisoprivacidad + "&quot>" + rutaAvisoprivacidad + "</a> </p>" +
                "</div>";
        }
        public String obtenerToken(CertificadoML documento)
        {
            //documento = new CertificadoML();
            string codigo = generaCodigo();
            cerTokenDescarga token = new cerTokenDescarga
           
            {
                tokCodigo = codigo,
                tokfechaCreacion = DateTime.Now,
                tokfechaDescarga = null,
                tokEstatus = true,
                tokUsado = false,
                tokCURP = documento.docAlumnoCurp,
            };
            return new CerTokenDescargaDAL().guardaTokenDescarga(token) ? codigo : "";
        }
        public String generaCodigo()
        {
            var chars = Guid.NewGuid().ToString().Replace("-", "") + "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[10];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);

            return finalString;
        }
        public Boolean verificarExisteCertificado(CertificadoML documento, criteriosBusquedaCertificadosML filtro)
        {
            bool result = false;
            CertificadoML documentoML = new CerDocumentoDAL().getDatosDocumentoById(filtro, documento.docId);
            result = documentoML.documento.docId != null ? true : result;
            return result;
        }
        public bool EnvioCorreo(string sDestinatario, string CC, string Asunto, string Cuerpo, string Ruta)
        {

            ServiciosWeb.EnvioCorreo.WSCorreoSoapClient wSCorreo = new ServiciosWeb.EnvioCorreo.WSCorreoSoapClient();
            string Elemento = "48214";

            try
            {
                var ewSCorreo = wSCorreo.Enviarcorreo(sDestinatario, CC, Asunto, Cuerpo, Ruta, Elemento);
                return ewSCorreo[0].claveEstatus == 0 ? true : false;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return false;
            }
        }
        public Boolean actualizaCorreo(CertificadoML documento)
        {
            return new CerDocumentoDAL().actualizaCorreo(documento);
        }
        // INICIA ÀREA PARA PORTAL VERIFICACIÓN
        //OBTIENE REGISTRO DE [cerDocumento] PARA PORTAL DE VALIDACIÓN.
        public cerDocumento ObtenerDocumentoViewXML(String idDocumento) => new CerDocumentoDAL().ObtenerDocumentoViewXML(idDocumento);
        public Dec ValidarDocumentoXML(String docXML)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(typeof(Dec));
            XmlReader oXmlReader = XmlReader.Create(GetMemoryStreamXML(docXML));
            Dec oCertificadoElectronico = (Dec)oXmlSerializer.Deserialize(oXmlReader);

            return oCertificadoElectronico;
        }
          public MemoryStream GetMemoryStreamXML(string sXMLData)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(sXMLData));

            MemoryStream xmlStream = new MemoryStream();
            doc.Save(xmlStream);

            xmlStream.Flush();
            xmlStream.Position = 0;

            return xmlStream;
        }


        public cerDocumento GetCerDocumento(string id)
        {
           return new CerDocumentoDAL().GetDocumentoById(id);

        }

        public List<cerUACDocumento> GetListUACDocumentoByIdDocumento(string sIdDocumento)
        {
            return new cerUACDocumentoDAL().GetListUACDocumento(sIdDocumento);
        }
        public BusinessModel.Models.Parcial.Dec DescerializarDecParcial(String docXML)
        {
            XmlSerializer oXmlSerializer = new XmlSerializer(typeof(Models.Parcial.Dec));
            XmlReader oXmlReader = XmlReader.Create(GetMemoryStreamXML(docXML));
            Models.Parcial.Dec oCertificadoElectronico = (Models.Parcial.Dec)oXmlSerializer.Deserialize(oXmlReader);

            return oCertificadoElectronico;
        }
    }

}
