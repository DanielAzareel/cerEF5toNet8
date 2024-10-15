using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerCargaDAL
    {
        public bool GuardarDatosCarga(cerCarga datasCarga)
        {
            bool guardado = false;
            try
            {
                CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.cerCarga.Add(datasCarga);
                conn.SaveChanges();
                guardado = true;
            }
            catch (Exception)
            {
                return false;
            }
            return guardado;
        }
        public List<cerAreaConocimiento> GetLstAreasConocimiento()
        {
            List<cerAreaConocimiento> lstareas = new List<cerAreaConocimiento>();

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                lstareas = (from area in certificadosEntities.cerAreaConocimiento

                             select area).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerAreaConocimiento>();
            }

            return lstareas;
        }
    }
}
