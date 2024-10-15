using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BusinessModel.Models.CertificadoML;

namespace BusinessModel.DataAccess
{
    public class CerDocumentoDAL
    {
        public List<cerDocumento> getDocumentosByCURP(string curp, string insId)
        {
            List<cerDocumento> listViewCerDocumento = new List<cerDocumento>();
            List<string> estatus = new List<string>() { "4" };
            try
            {
                CertificadosMediaSuperiorEntities oTEEntities = new CertificadosMediaSuperiorEntities();
                oTEEntities.Configuration.LazyLoadingEnabled = false;
                listViewCerDocumento = (from doc in oTEEntities.cerDocumento where estatus.Contains(doc.estDocumentoId.ToString()) && doc.cerConfiguracionInstitucion.insId == insId && doc.docAlumnoCurp == curp select doc).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return listViewCerDocumento;
        }
        public cerDocumento GetDatosDocumentoById(string docId) // //
        {
            cerDocumento datosDocumento = new cerDocumento();

            try
            {
                CertificadosMediaSuperiorEntities oTEEntities = new CertificadosMediaSuperiorEntities();
                oTEEntities.Configuration.LazyLoadingEnabled = false;

                datosDocumento = (from doc in oTEEntities.cerDocumento
                                  where doc.docId == docId
                                  select doc).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return datosDocumento;
        }
        public CertificadoML getDatosDocumentoById(criteriosBusquedaCertificadosML filtros, string docId)
        {
            CertificadoML datosDocumento = new CertificadoML();

            try
            {
                CertificadosMediaSuperiorEntities oTEEntities = new CertificadosMediaSuperiorEntities();
                oTEEntities.Configuration.LazyLoadingEnabled = false;

                datosDocumento.documento = (from doc in oTEEntities.cerDocumento
                                            where doc.docId == docId
                                            //&& filtros.listInstitucionAcceso.Contains(doc.insId)
                                            //&& filtros.listPlanAcceso.Contains(doc.docPlanId)
                                            select doc).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return datosDocumento;
        }
        public Boolean actualizaCorreo(CertificadoML documentos)
        {
            bool result = false;
            try
            {
                CertificadosMediaSuperiorEntities oTEEntities = new CertificadosMediaSuperiorEntities();
                oTEEntities.Configuration.LazyLoadingEnabled = false;

                using (oTEEntities)
                {
                    oTEEntities.Database.Connection.Open();

                    using (DbContextTransaction oTransactionScope = oTEEntities.Database.BeginTransaction())
                    {
                        cerDocumento documento = (from doc in oTEEntities.cerDocumento
                                                  where doc.docId == documentos.docId
                                                  select doc).FirstOrDefault();
                        documento.docCorreo = documentos.docCorreo;

                        oTEEntities.Entry(documento).State = EntityState.Modified;

                        oTEEntities.SaveChanges();
                        oTransactionScope.Commit();
                        result = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }
        public CertificadoML getDatosDocumentoPortalDescarga(string docId)
        {
            CertificadoML datosDocumento = new CertificadoML();

            try
            {
                CertificadosMediaSuperiorEntities oTEEntities = new CertificadosMediaSuperiorEntities();
                oTEEntities.Configuration.LazyLoadingEnabled = false;

                datosDocumento = (from doc in oTEEntities.cerDocumento
                                  where doc.docId == docId
                                  select new CertificadoML()
                                  {
                                      docCorreo = doc.docCorreo,
                                      docId = doc.docId,
                                      docPlanId = doc.docPlanId,
                                      docAlumnoCurp = doc.docAlumnoCurp
                                  }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return datosDocumento;

        }

        public List<cerDocumento> GetCerDocumentosByIds(List<string> Ids)
        {
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;
                return (from c in conn.cerDocumento where Ids.Contains(c.docId) select c).ToList();
            }
            catch (Exception ex)
            {
            }
            return new List<cerDocumento>();
        }
        public List<cerCompetenciasIEMS> GetCompetenciasIEMSByDocIds(List<string> Ids)
        {
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;
                return (from c in conn.cerCompetenciasIEMS where Ids.Contains(c.docId) select c).ToList();
            }
            catch (Exception ex)
            {
            }
            return new List<cerCompetenciasIEMS>();
        }
        public List<cerCompetenciaDocumento> GetCompetenciasDocumentoByIds(List<string> Ids)
        {
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;
                return (from c in conn.cerCompetenciaDocumento where Ids.Contains(c.docId) select c).ToList();
            }
            catch (Exception ex)
            {
            }
            return new List<cerCompetenciaDocumento>();
        }


        public List<cerDocumento> GetDocumentoCurp(string curp, List<int> estatus, string tokenDescarga, string insId)
        {
            try
            {
                using (CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities())
                {
                      conn.Configuration.LazyLoadingEnabled = false;
  
                    return (from c in conn.cerDocumento
                            where c.docAlumnoCurp == curp && estatus.Contains(c.estDocumentoId.Value) &&
    ((c.cerConfiguracionInstitucion.insCertificadosPublicos == true) || (c.insId == insId && c.cerConfiguracionInstitucion.insTokenSeguridadDescarga == tokenDescarga))
                            select c).ToList();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return new List<cerDocumento>();
            }
            
            
        }

        //OBTIENE REGISTRO DE [cerDocumento] PARA PORTAL DE VALIDACIÓN.
        public cerDocumento ObtenerDocumentoViewXML(String idDocumento)
        {
            cerDocumento oViewCerDocumento = new cerDocumento();

            try
            {
                CertificadosMediaSuperiorEntities oTEEntities = new CertificadosMediaSuperiorEntities();
                oTEEntities.Configuration.LazyLoadingEnabled = false;

                oViewCerDocumento = (from doc in oTEEntities.cerDocumento
                                     where (doc.estDocumentoId == 4 || doc.estDocumentoId == 6) // 6 = Cancelado por sep y 4 = Documento concluido.
                                     && doc.docId.Equals(idDocumento)
                                     select doc).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return oViewCerDocumento;
        }

        public Boolean EliminarDocumentoById(cerDocumento documento)
        {
            Boolean resultado = false;
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                documento = (from c in conn.cerDocumento where c.docId == documento.docId && c.insId == documento.insId select c).FirstOrDefault();
                conn.cerDocumento.Remove(documento);
                conn.SaveChanges();
                resultado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return resultado;
        }


        public cerDocumento GetDocumentoById(string docId)
        {
            cerDocumento datosDocumento = new cerDocumento();

            try
            {
                CertificadosMediaSuperiorEntities oTEEntities = new CertificadosMediaSuperiorEntities();

                datosDocumento = (from doc in oTEEntities.cerDocumento
                                  where doc.docId == docId
                                  select doc).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return datosDocumento;
        }


        public List<cerDocumento> ObtenerListaDocumentosBySolId(string sSolId)
        {
            List<cerDocumento> listCerDocumento = new List<cerDocumento>();
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                listCerDocumento = (from documentos in oCertificadosMediaSuperiorEntities.cerDocumento
                                    where documentos.solId == sSolId
                                    select documentos).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
            return listCerDocumento;
        }

        public List<ViewCerDocumento> ObtenerListaDocumentos(CriteriosBusquedaMonitoreoModel oCriterios, int iRegInicio, int iBloque)
        {
            List<ViewCerDocumento> listCerDocumento = new List<ViewCerDocumento>();
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                listCerDocumento = (from documentos in oCertificadosMediaSuperiorEntities.ViewCerDocumento
                                    where documentos.solId == oCriterios.sSolId
                                    && (oCriterios.lstEstatus.Count == 0 || oCriterios.lstEstatus.Contains(documentos.estDocumentoId.ToString()))
                                    && (oCriterios.lstPlan.Count == 0 || oCriterios.lstPlan.Contains(documentos.docPlanId))
                                    && (oCriterios.lstTipoCertificado.Count == 0 || oCriterios.lstTipoCertificado.Contains(documentos.docTipoId))
                                    && (oCriterios.lstCURP.Count == 0 || oCriterios.lstCURP.Contains(documentos.docAlumnoCurp))
                                    && (String.IsNullOrEmpty(oCriterios.sNombre) || (documentos.docAlumnoNombre + " " + documentos.docAlumnoPrimerApellido + " " + documentos.docAlumnoSegundoApellido).Contains(oCriterios.sNombre))
                                    && (oCriterios.lstFolio.Count == 0 || oCriterios.lstFolio.Contains(documentos.docAlumnoNumeroControl))
                                    && (String.IsNullOrEmpty(oCriterios.sIdInstitucion) || documentos.insId.Equals(oCriterios.sIdInstitucion))
                                    select documentos).OrderBy(x => x.docAlumnoPrimerApellido + " " + x.docAlumnoSegundoApellido + " "+ x.docAlumnoNombre).Skip(iRegInicio).Take(iBloque).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listCerDocumento;
        }

        public int ObtenerCountDocumentos(CriteriosBusquedaMonitoreoModel oCriterios)
        {
            int iResultado = 0;
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                iResultado = (from documentos in oCertificadosMediaSuperiorEntities.cerDocumento
                              where documentos.solId == oCriterios.sSolId
                              && (oCriterios.lstEstatus.Count == 0 || oCriterios.lstEstatus.Contains(documentos.estDocumentoId.ToString()))
                                    && (oCriterios.lstPlan.Count == 0 || oCriterios.lstPlan.Contains(documentos.docPlanId))
                                    && (oCriterios.lstTipoCertificado.Count == 0 || oCriterios.lstTipoCertificado.Contains(documentos.docTipoId))
                              && (oCriterios.lstCURP.Count == 0 || oCriterios.lstCURP.Contains(documentos.docAlumnoCurp))
                              && (String.IsNullOrEmpty(oCriterios.sNombre) || (documentos.docAlumnoNombre + " " + documentos.docAlumnoPrimerApellido + " " + documentos.docAlumnoSegundoApellido).Contains(oCriterios.sNombre))
                              && (oCriterios.lstFolio.Count == 0 || oCriterios.lstFolio.Contains(documentos.docAlumnoNumeroControl))
                              && (String.IsNullOrEmpty(oCriterios.sIdInstitucion) || documentos.insId.Equals(oCriterios.sIdInstitucion))
                              select documentos).Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
            return iResultado;
        }

        public string GetXMLRetornoByDocId(string sDocId)
        {
            string sXML = "";
            try
            {
                using (CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
                {
                    oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                    sXML = (from l in oCertificadosMediaSuperiorEntities.cerDocumento
                            where l.docId == sDocId
                            select l.docXMLRetorno).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sXML;
        }

        public string GetXMLEnvioByDocId(string sDocId)
        {
            string sXML = "";
            try
            {
                using (CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
                {
                    oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                    sXML = (from l in oCertificadosMediaSuperiorEntities.cerDocumento
                            where l.docId == sDocId
                            select l.docXMLEnvio).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sXML;
        }

        public cerDocumento GetDocumentoByDocId(string sDocId)
        {
            cerDocumento oCerDocumento = new cerDocumento();
            try
            {
                using (CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
                {
                    oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                    oCerDocumento = (from dcumento in oCertificadosMediaSuperiorEntities.cerDocumento
                                     where dcumento.docId == sDocId
                                     select dcumento).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return oCerDocumento;
        }

        public bool ActualizarCorreoByIdDocumento(string sIdDocumento, string sCorreo)
        {
            bool bBandera = false;
            try
            {
                using (CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
                {
                    oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                    var oDocumento = (from documento in oCertificadosMediaSuperiorEntities.cerDocumento
                                      where documento.docId == sIdDocumento
                                      select documento).FirstOrDefault();

                    oDocumento.docCorreo = sCorreo;

                    oCertificadosMediaSuperiorEntities.SaveChanges();

                    bBandera = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bBandera;
        }

        public bool CancelarDocumentoSEPByDocId(string sIdDocumento, string sObservaciones)
        {
            bool bBandera = false;
            try
            {
                using (CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
                {
                    oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                    var oDocumento = (from documento in oCertificadosMediaSuperiorEntities.cerDocumento
                                      where documento.docId == sIdDocumento
                                      select documento).FirstOrDefault();

                    oDocumento.docObservaciones = sObservaciones;
                    oDocumento.estDocumentoId = 6;
                    oCertificadosMediaSuperiorEntities.SaveChanges();

                    bBandera = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bBandera;
        }

        public string GetPlantillaByIdDocumento(string sIdDocumento)
        {
            string sPlantillaId = "";
            using (CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
            {
                oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                sPlantillaId = (from documento in oCertificadosMediaSuperiorEntities.cerDocumento
                                where documento.docId == sIdDocumento
                                select documento.planId).FirstOrDefault();

            }
            return sPlantillaId;
        }

        public List<(string sIdDocumento, string sCorreo)> GetCorreoByListIdDocumento(List<string> listIdDocumento)
        {
            List<(string sIdDocumento, string sCorreo)> lstCorreDocumento = new List<(string sIdDocumento, string sCorreo)>();
            try
            {
                using (CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
                {
                    oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                    lstCorreDocumento = (from correo in oCertificadosMediaSuperiorEntities.cerDocumento
                                         where listIdDocumento.Contains(correo.docId)
                                         select new { sIdDocumento = correo.docId, sCorreo = correo.docCorreo }).ToList().Select(x => (x.sIdDocumento, x.sCorreo)).ToList();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lstCorreDocumento;

        }
    }
}
