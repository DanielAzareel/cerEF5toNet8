using BusinessModel.Business;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using CertificadosEletronicosMS.Models;
using DGSyTI_WEB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DGSyTI_WEB.Controllers
{
    public class VerificacionController : Controller
    {
        // GET: Verificación
        public ActionResult Index(String id = "")
        {
           
            ViewResult oViewResult = new ViewResult();
            cerDocumento documento = new CertificadosBL().ObtenerDocumentoViewXML(id);
  

                //return PartialView("ModalDetalleDocumento", selladoViewModel);


            if (documento != null)
            {
                if (!String.IsNullOrEmpty(documento.docXMLRetorno))
                {
                    try
                    {
                        VerificacionViewModel verificacionVM = new VerificacionViewModel();
                        verificacionVM.datosCertificado = new CertificadosBL().ValidarDocumentoXML(documento.docXMLRetorno);
                        verificacionVM.idEstatusDocumento = documento.estDocumentoId;
                        verificacionVM.CerDocumento = new CertificadosBL().GetCerDocumento(id);
                        verificacionVM.listUACDocumento = new CertificadosBL().GetListUACDocumentoByIdDocumento(id);

                        oViewResult = View(verificacionVM);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = "Error al cargar la información, intente nuevamente.";
                        oViewResult = View();
                    }
                }
                else
                {
                    ViewBag.Error = "No existe documento con esta información.";
                    oViewResult = View();
                }
            }
            else
            {
                ViewBag.Error = "No existe documento con esta información.";
                oViewResult = View();
            }

            return oViewResult;
        }
    }
}