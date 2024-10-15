using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerCatPlantillaDAL
    {

        public List<cerCatPlantilla> GetLstPlantillas(string insId, string docTipoId)
        {
            List<cerCatPlantilla> lstPlantillas = new List<cerCatPlantilla>();



            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;



                lstPlantillas = (from plan in certificadosEntities.cerCatPlantilla
                                 where plan.insId == insId && plan.docTipoId == docTipoId
                                 select plan).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerCatPlantilla>();
            }



            return lstPlantillas;
        }

        public List<cerCatPlantilla> GetLstPlantillasByInstitucion(string insId, string sIdPlan, string sIdTipoDocumento, int pagina, int bloque)
        {
            List<cerCatPlantilla> lstPlantillas = new List<cerCatPlantilla>();
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                lstPlantillas = (from plantilla in certificadosEntities.cerCatPlantilla
                                 where plantilla.insId == insId
                                 && plantilla.idPlan == sIdPlan && plantilla.docTipoId == sIdTipoDocumento
                                 select new
                                 {
                                     docTipoId = plantilla.docTipoId,
                                     idPlan = plantilla.idPlan,
                                     insId = plantilla.insId,
                                     planEstatus = plantilla.planEstatus,
                                     planId = plantilla.planId,
                                     planIdAnterior = plantilla.planIdAnterior,
                                     planNombre = plantilla.planNombre,
                                     planNumHojas = plantilla.planNumHojas
                                 }).ToList().Select(x => new cerCatPlantilla
                                 {
                                     docTipoId = x.docTipoId,
                                     idPlan = x.idPlan,
                                     insId = x.insId,
                                     planEstatus = x.planEstatus,
                                     planId = x.planId,
                                     planIdAnterior = x.planIdAnterior,
                                     planNombre = x.planNombre,
                                     planNumHojas = x.planNumHojas
                                 }).OrderBy(x => x.planId).Skip(pagina).Take(bloque).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lstPlantillas;
        }

        public int GetCountPlantillasByInstitucion(string insId, string sIdPlan, string sIdTipoDocumento)
        {
            int iResultado = 0;
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                iResultado = (from plantilla in certificadosEntities.cerCatPlantilla
                              where plantilla.insId == insId
                              && plantilla.idPlan == sIdPlan && plantilla.docTipoId == sIdTipoDocumento
                              select plantilla).Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return iResultado;
        }

        public bool AsignarPlantilla(string sDocId, string sIdPlantilla)
        {
            bool bResultado = false;
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                var Documento = (from documento in certificadosEntities.cerDocumento
                                 where documento.docId == sDocId
                                 select documento).FirstOrDefault();

                Documento.planId = sIdPlantilla;
                certificadosEntities.SaveChanges();
                bResultado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return bResultado;
        }
        public string GetIdPlantByIdPlantilla(string idPlantilla)
        {
            string idPlan = "";
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                idPlan = (from plantilla in certificadosEntities.cerCatPlantilla
                          where plantilla.planId == idPlantilla
                          select plantilla.idPlan).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return idPlan;
        }
        public cerCatPlantilla GetPlantillasById(string idPlantilla)
        {
            cerCatPlantilla plantillaCer = new cerCatPlantilla();
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                plantillaCer = (from plantilla in certificadosEntities.cerCatPlantilla
                                where plantilla.planId == idPlantilla
                                select plantilla).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return plantillaCer;
        }


        public ViewPlantillas GetViewPlantillaById(string idPlantilla)
        {
            ViewPlantillas plantillaCer = new ViewPlantillas();
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;



                plantillaCer = (from c in certificadosEntities.ViewPlantillas
                                where c.planId == idPlantilla
                                select c
                                        ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return plantillaCer;
        }

        public bool AsignarPlantilla(string sIdPlantilla)
        {
            bool bResultado = false;
            try
            {
                CertificadosMediaSuperiorEntities certificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                certificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                cerCatPlantilla cerCatPlantilla = (from plantilla in certificadosMediaSuperiorEntities.cerCatPlantilla
                                                   where plantilla.planId == sIdPlantilla
                                                   select plantilla).FirstOrDefault();

                cerCatPlantilla cerCatPlantillaAnterior = (from plantilla in certificadosMediaSuperiorEntities.cerCatPlantilla
                                                           where plantilla.docTipoId == cerCatPlantilla.docTipoId && plantilla.idPlan == cerCatPlantilla.idPlan
                                                           && plantilla.planEstatus == true
                                                           && plantilla.insId == cerCatPlantilla.insId
                                                           select plantilla).FirstOrDefault();

                if (cerCatPlantillaAnterior != null)
                {
                    cerCatPlantillaAnterior.planEstatus = false;
                }

                cerCatPlantilla.planEstatus = true;

                certificadosMediaSuperiorEntities.SaveChanges();

                bResultado = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return bResultado;
        }

        public int ExisteNombrePlantilla(string sIdPlantilla, string sNombrePlantilla, string sIdInstitución)
        {
            int iCountExisteNombre = 0;
            try
            {
                CertificadosMediaSuperiorEntities certificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                certificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                if (String.IsNullOrEmpty(sIdPlantilla))
                {
                    iCountExisteNombre = (from plantilla in certificadosMediaSuperiorEntities.cerCatPlantilla
                                          where plantilla.planNombre == sNombrePlantilla && plantilla.insId == sIdInstitución
                                          select plantilla).Count();
                }
                else
                {
                    iCountExisteNombre = (from plantilla in certificadosMediaSuperiorEntities.cerCatPlantilla
                                          where plantilla.planId != sIdPlantilla && plantilla.planIdAnterior != sIdPlantilla && plantilla.planNombre == sNombrePlantilla && plantilla.insId == sIdInstitución
                                          select plantilla).Count();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return iCountExisteNombre;
        }

        public cerCatPlantilla GetPlantillaFileById(string sIdPlantilla)
        {
            cerCatPlantilla bArchivo = new cerCatPlantilla();
            try
            {
                CertificadosMediaSuperiorEntities certificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                certificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                bArchivo = (from plantilla in certificadosMediaSuperiorEntities.cerCatPlantilla
                            where plantilla.planId == sIdPlantilla
                            select plantilla).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return bArchivo;
        }

        public bool EliminarPlantillaById(string idPlantilla)
        {
            bool bResultado = false;
            try
            {
                CertificadosMediaSuperiorEntities certificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                certificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                cerCatPlantilla cerCatPlantilla = (from plantilla in certificadosMediaSuperiorEntities.cerCatPlantilla
                                                   where plantilla.planId == idPlantilla
                                                   select plantilla).FirstOrDefault();

                certificadosMediaSuperiorEntities.cerCatPlantilla.Remove(cerCatPlantilla);
                certificadosMediaSuperiorEntities.SaveChanges();

                bResultado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return bResultado;
        }

        public bool EditarPlantilla(cerCatPlantilla cerCatPlantilla, string sIdPlantillaAnterior)
        {
            bool bResultado = false;
            try
            {
                using (CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities())
                {
                    certificadosEntities.Database.ExecuteSqlCommand("UPDATE cerCatPlantilla SET planId={0} , planArchivo={1} ,planNombre={2},docTipoId={3},planNumHojas={4}, idPlan={5}, planIdAnterior={6} WHERE planId={6}",
                        cerCatPlantilla.planId, cerCatPlantilla.planArchivo, cerCatPlantilla.planNombre, cerCatPlantilla.docTipoId, cerCatPlantilla.planNumHojas, cerCatPlantilla.idPlan, sIdPlantillaAnterior);
                    certificadosEntities.SaveChanges();
                    bResultado = true;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return bResultado;
        }

        public bool AgregarPlantilla(cerCatPlantilla cerCatPlantilla)
        {
            bool bResultado = false;
            try
            {
                using (CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities())
                {
                    certificadosEntities.Configuration.LazyLoadingEnabled = false;

                    certificadosEntities.cerCatPlantilla.Add(cerCatPlantilla);
                    certificadosEntities.SaveChanges();
                    bResultado = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return bResultado;
        }
    }
}
