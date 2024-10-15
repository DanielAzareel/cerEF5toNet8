using System;
using System.IO;
using System.Web.Mvc;
using BusinessModel.Business; 
using DGSyTI_WEB.Controllers;

namespace CertificadosEletronicosMS.Controllers
{
    public class PlantillasCertificadosController : Controller
    {
        // GET: PlantillasCertificados
        public FileResult DescargarCertificado(string id)
        {
           
            string numeroControl = "";
            

            Stream archivo = new BusinessModel.Business.PlantillaCertificadosBL().GenerarCertificado(id, "", out numeroControl);

            if (archivo != null)
                return File(archivo, "application/pdf", numeroControl + ".pdf");
            return null;

        }

        [ValidarSesion]
        public FileResult DescargarCertificadoWithSession(string id, string idPlantilla = "")
        {
            string numeroControl = "";
           
            Stream archivo = new BusinessModel.Business.PlantillaCertificadosBL().GenerarCertificado(id, idPlantilla, out numeroControl);

            if (archivo != null)
                return File(archivo, "application/pdf", numeroControl + ".pdf");
            return null;

        }

    }

}