using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerSolicitudDAL
    {
        public cerSolicitud ObtenerRegistroSolicitud(String solId)
        {
            cerSolicitud solicitud = new cerSolicitud();

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                solicitud = (from sol in certificadosEntities.cerSolicitud
                             where sol.solId == solId
                             select sol).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return solicitud;
        }

        public bool AgregarActualizarSolicitud(cerSolicitud solicitud)
        {
            try
            {
                using (CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities())
                {
                    certificadosEntities.cerSolicitud.AddOrUpdate(solicitud);
                    certificadosEntities.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;

                throw;
            }


        }

        public List<ViewSolicitud> ObtenerRegistrosSolicitud(CriteriosBusquedaMonitoreoModel oCriterios, int iPagina, int iBloque)
        {
            List<ViewSolicitud> listViewSolicitud = new List<ViewSolicitud>();
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                listViewSolicitud = (from sol in certificadosEntities.ViewSolicitud
                                     join documentos in certificadosEntities.ViewCerDocumento on sol.solId equals documentos.solId
                                     where

                                    (oCriterios.lstEstatus.Count == 0 || oCriterios.lstEstatus.Contains(sol.estSolicitudId.ToString()))
                                    && (oCriterios.lstPlan.Count == 0 || oCriterios.lstPlan.Contains(documentos.docPlanId))
                                    && (oCriterios.lstTipoCertificado.Count == 0 || oCriterios.lstTipoCertificado.Contains(documentos.docTipoId))
                                    && (String.IsNullOrEmpty(oCriterios.sNombre) || (documentos.docAlumnoNombre + " " + documentos.docAlumnoPrimerApellido + " " + documentos.docAlumnoSegundoApellido).Contains(oCriterios.sNombre))
                                    && (oCriterios.lstFolio.Count == 0 || oCriterios.lstFolio.Contains(documentos.docAlumnoNumeroControl))
                                    && (oCriterios.lstCURP.Count == 0 || oCriterios.lstCURP.Contains(documentos.docAlumnoCurp))
                                     && (String.IsNullOrEmpty(oCriterios.sIdInstitucion) || sol.insId.Equals(oCriterios.sIdInstitucion))
                                     select sol).Distinct().OrderByDescending(x => x.solFechaSellado).Skip(iPagina).Take(iBloque).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return listViewSolicitud;
        }

        public int ObtenerCountSolicitud(CriteriosBusquedaMonitoreoModel oCriterios)
        {
            int iResultado = 0;
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                iResultado = (from sol in certificadosEntities.ViewSolicitud
                              join documentos in certificadosEntities.ViewCerDocumento on sol.solId equals documentos.solId
                              where (oCriterios.lstEstatus.Count == 0 || oCriterios.lstEstatus.Contains(sol.estSolicitudId.ToString()))
                                    && (oCriterios.lstPlan.Count == 0 || oCriterios.lstPlan.Contains(documentos.docPlanId))
                                    && (oCriterios.lstTipoCertificado.Count == 0 || oCriterios.lstTipoCertificado.Contains(documentos.docTipoId))
                             && (oCriterios.lstCURP.Count == 0 || oCriterios.lstCURP.Contains(documentos.docAlumnoCurp))
                             && (String.IsNullOrEmpty(oCriterios.sNombre) || (documentos.docAlumnoNombre + " " + documentos.docAlumnoPrimerApellido + " " + documentos.docAlumnoSegundoApellido).Contains(oCriterios.sNombre))
                             && (String.IsNullOrEmpty(oCriterios.sIdInstitucion) || sol.insId.Equals(oCriterios.sIdInstitucion))
                             && (oCriterios.lstFolio.Count == 0 || oCriterios.lstFolio.Contains(documentos.docAlumnoNumeroControl))
                              select sol).Distinct().Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return iResultado;
        }

        public ViewSolicitud GetSolicitudById(string sIdsolicitud)
        {
            ViewSolicitud oViewSolicitud = new ViewSolicitud();
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                oViewSolicitud = (from sol in certificadosEntities.ViewSolicitud
                                  where sol.solId == sIdsolicitud
                                  select sol).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return oViewSolicitud;
        }

        public byte[] GetFileResultadoSEP(string sSolId)
        {
            byte[] bArchivo = new byte[0];
            try
            {
                using (CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities())
                {
                    certificadosEntities.Configuration.LazyLoadingEnabled = false;
                    bArchivo = (from solicitud in certificadosEntities.cerSolicitud
                                where solicitud.solId == sSolId
                                select solicitud.solArchivoResultado).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bArchivo;
        }

        public byte[] GetFileRetornoSEP(string sSolId)
        {
            byte[] bArchivo = new byte[0];
            try
            {
                using (CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities())
                {
                    certificadosEntities.Configuration.LazyLoadingEnabled = false;
                    bArchivo = (from solicitud in certificadosEntities.cerSolicitud
                                where solicitud.solId == sSolId
                                select solicitud.solArchivoRetorno).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bArchivo;
        }

        public byte[] GetFileEnvioSEP(string sSolId)
        {
            byte[] bArchivo = new byte[0];
            try
            {
                using (CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities())
                {
                    certificadosEntities.Configuration.LazyLoadingEnabled = false;
                    bArchivo = (from solicitud in certificadosEntities.cerSolicitud
                                where solicitud.solId == sSolId
                                select solicitud.solArchivoEnvio).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bArchivo;
        }
    }
}
