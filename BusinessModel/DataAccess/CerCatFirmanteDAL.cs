using System;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace BusinessModel.DataAccess
{
    public class CerCatFirmanteDAL
    {
        public cerCatFirmante GetFirmanteActivoByInsId(string insId)
        {
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();

                return (from c in conn.cerCatFirmante where c.insId == insId && c.firPredeterminado == true select c).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine();
                return new cerCatFirmante();
            }

        }

        public bool AgregarEditarFirmante(cerCatFirmante oCerCatFirmante)
        {
            bool bResultado = false;
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;

                conn.cerCatFirmante.AddOrUpdate(oCerCatFirmante);
                conn.SaveChanges();
                bResultado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return bResultado;
        }

        public List<cerCatFirmante> GetListFirmantes(string sIdInstitucion, int iPagina, int iBloque)
        {
            List<cerCatFirmante> listcerCatFirmante = new List<cerCatFirmante>();
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;

                listcerCatFirmante = (from firmante in conn.cerCatFirmante
                                      where firmante.insId == sIdInstitucion
                                      select firmante).OrderBy(x => x.firCurp).Skip(iPagina).Take(iBloque).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return listcerCatFirmante;
        }

        public int GetCountFirmantes(string sIdInstitucion)
        {
            int iCantidadRegistros = 0;
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;

                iCantidadRegistros = (from firmante in conn.cerCatFirmante
                                      where firmante.insId == sIdInstitucion
                                      select firmante).Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return iCantidadRegistros;
        }

        public bool AsignarFirmante(string firId, string sIdInstitucion)
        {
            bool bResultado = false;
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;

                var cerfirmanteAnterior = (from firmante in conn.cerCatFirmante
                                           where firmante.insId == sIdInstitucion && firmante.firPredeterminado == true
                                           select firmante).FirstOrDefault();

                var cerfirmante = (from firmante in conn.cerCatFirmante
                                   where firmante.insId == sIdInstitucion && firmante.firId == firId
                                   select firmante).FirstOrDefault();

                if (cerfirmanteAnterior != null)
                {
                    cerfirmanteAnterior.firPredeterminado = false;
                }

                if (cerfirmante == null)
                {
                    return bResultado;
                }

                cerfirmante.firPredeterminado = true;
                conn.SaveChanges();

                bResultado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return bResultado;
        }

        public cerCatFirmante getFirmanteById(string firId, string sIdInstitucion)
        {
            cerCatFirmante cerCatFirmante = new cerCatFirmante();
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;

                cerCatFirmante = (from firmante in conn.cerCatFirmante
                                  where firmante.insId == sIdInstitucion && firmante.firId == firId
                                  select firmante).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return cerCatFirmante;
        }

        public bool EliminarFirmanteById(string firId, string sIdInstitucion)
        {
            bool bResultado = false;
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;

                var cerCatFirmante = (from firmante in conn.cerCatFirmante
                                      where firmante.insId == sIdInstitucion && firmante.firId == firId
                                      select firmante).FirstOrDefault();

                conn.cerCatFirmante.Remove(cerCatFirmante);
                conn.SaveChanges();

                bResultado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return bResultado;
        }

        public string GetCertificadoById(string firId, string insId)
        {
            string sArchivoBase64 = "";
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;


                sArchivoBase64 = (from firmante in conn.cerCatFirmante
                                  where firmante.firId == firId && firmante.insId == insId
                                  select firmante.firCertificadoPublico).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sArchivoBase64;
        }

        public cerCatFirmante GetFirmanteActivoSinArchivo(string insId)
        {
            cerCatFirmante oCerCatFirmante = new cerCatFirmante();
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;

                oCerCatFirmante = (from firmante in conn.cerCatFirmante
                                   where firmante.insId == insId && firmante.firPredeterminado == true
                                   select firmante).FirstOrDefault();

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return oCerCatFirmante;
        }

        public cerCatFirmante getFirmanteByNumeroCertificado(string firNumeroCertificado, string sIdInstitucion)
        {
            cerCatFirmante cerCatFirmante = new cerCatFirmante();
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;

                cerCatFirmante = (from firmante in conn.cerCatFirmante
                                  where firmante.insId == sIdInstitucion && firmante.firNumeroCertificado == firNumeroCertificado
                                  select firmante).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return cerCatFirmante;
        }

        public cerCatFirmante getFirmanteByNumeroCertificadoAndId(string firNumeroCertificado, string sIdInstitucion, string firId)
        {
            cerCatFirmante cerCatFirmante = new cerCatFirmante();
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;

                cerCatFirmante = (from firmante in conn.cerCatFirmante
                                  where firmante.insId == sIdInstitucion && firmante.firNumeroCertificado == firNumeroCertificado && firmante.firId != firId
                                  select firmante).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return cerCatFirmante;
        }
    }
}
