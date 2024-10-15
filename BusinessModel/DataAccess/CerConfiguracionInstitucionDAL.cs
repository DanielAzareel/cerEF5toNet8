using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System.Data.Entity.Migrations;

namespace BusinessModel.DataAccess
{
    public class CerConfiguracionInstitucionDAL
    {
        public cerConfiguracionInstitucion ObtenerRegistroConfiguracionPorId(String IdInstitucion)
        {
            cerConfiguracionInstitucion configuracion = new cerConfiguracionInstitucion();

            try
            {
                CertificadosMediaSuperiorEntities certificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                certificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                configuracion = (from conf in certificadosMediaSuperiorEntities.cerConfiguracionInstitucion
                                 where conf.insId == IdInstitucion
                                 select conf).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return configuracion;
        }


        public bool AddOrUpdateInstitucion(cerConfiguracionInstitucion cerConfiguracionInstitucion)
        {
            bool registrado = false;

            try
            {
                using (CertificadosMediaSuperiorEntities certificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
                {
                    certificadosMediaSuperiorEntities.cerConfiguracionInstitucion.AddOrUpdate(cerConfiguracionInstitucion);

                    certificadosMediaSuperiorEntities.SaveChanges();
                    registrado = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return registrado;
        }

        // Guardar registro en cerConfiguracionInstitucion
        public bool AgregarConfigInstitucion(cerConfiguracionInstitucion cerConfiguracionInstitucion)
        {
            bool registrado = false;

            try
            {
                using (CertificadosMediaSuperiorEntities certificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
                {
                    cerConfiguracionInstitucion configConcentradora =
                        certificadosMediaSuperiorEntities.cerConfiguracionInstitucion.Add(cerConfiguracionInstitucion);

                    certificadosMediaSuperiorEntities.SaveChanges();
                    registrado = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return registrado;
        }

        // Actualizar registro en cerConfiguracionInstitucion
        public bool ActualizarConfigInstitucion(cerConfiguracionInstitucion cerConfiguracionInstitucion)
        {
            bool actualizo = false;
            try
            {
                CertificadosMediaSuperiorEntities certificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                certificadosMediaSuperiorEntities.Entry(cerConfiguracionInstitucion).State = System.Data.Entity.EntityState.Modified;
                certificadosMediaSuperiorEntities.SaveChanges();
                actualizo = true;

            }
            catch (Exception ex)
            {
                actualizo = false;
                throw ex;
            }

            return actualizo;
        }

        public List<cerConfiguracionInstitucion> GetInstituciones()
        {
            List<cerConfiguracionInstitucion> listConfiguracionInstitucion = new List<cerConfiguracionInstitucion>();
            try
            {
                using (CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities())
                {
                    certificadosEntities.Configuration.LazyLoadingEnabled = false;

                    listConfiguracionInstitucion = (from concentradora in certificadosEntities.cerConfiguracionInstitucion
                                                    select concentradora).ToList();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listConfiguracionInstitucion;
        }

        public cerConfiguracionInstitucion getInstitucionByDatosDescarga(string tokenDescarga, string folio)
        {
            try
            {
                return (from c in new CertificadosMediaSuperiorEntities().cerConfiguracionInstitucion where c.insId == folio && c.insTokenSeguridadDescarga == tokenDescarga select c).FirstOrDefault();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);

                return null;
            }
        }

        public List<ViewPlantillas> GetListViewPlantillas(string sIdInstitucion, string sIdTipoDocumento, string sIdPlan, int iPagina, int iBloque)
        {
            List<ViewPlantillas> viewPlantillas = new List<ViewPlantillas>();
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                viewPlantillas = (from plantillas in oCertificadosMediaSuperiorEntities.ViewPlantillas
                                  where (String.IsNullOrEmpty(sIdInstitucion) || plantillas.insId == sIdInstitucion) &&
                                  (String.IsNullOrEmpty(sIdTipoDocumento) || plantillas.docTipoId == sIdTipoDocumento) &&
                                  (String.IsNullOrEmpty(sIdPlan) || plantillas.idPlan == sIdPlan)
                                  select plantillas).OrderBy(x => x.planId).Skip(iPagina).Take(iBloque).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return viewPlantillas;
        }

        public int GetCountViewPlantillas(string sIdInstitucion, string sIdTipoDocumento, string sIdPlan)
        {
            int iResultado = 0;
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                iResultado = (from plantillas in oCertificadosMediaSuperiorEntities.ViewPlantillas
                              where (String.IsNullOrEmpty(sIdInstitucion) || plantillas.insId == sIdInstitucion) &&
                              (String.IsNullOrEmpty(sIdTipoDocumento) || plantillas.docTipoId == sIdTipoDocumento) &&
                              (String.IsNullOrEmpty(sIdPlan) || plantillas.idPlan == sIdPlan)
                              select plantillas).Count();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return iResultado;
        }
    }
}
