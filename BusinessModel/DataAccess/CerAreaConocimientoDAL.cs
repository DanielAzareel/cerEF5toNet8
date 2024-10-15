using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
   public  class CerAreaConocimientoDAL
    {
        public List<cerAreaConocimiento> GetAreasConocimientoByInsId(string InsId)
        {
           
                List<cerAreaConocimiento> lstPlanes = new List<cerAreaConocimiento>();

                try
                {
                    CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                    certificadosEntities.Configuration.LazyLoadingEnabled = false;

                    lstPlanes = (from area in certificadosEntities.cerAreaConocimiento
                                 where (from c in certificadosEntities.cerRelPlanPeriodoAreaConocimientoMateria where c.insId == InsId select c.idAreaConocimiento).Contains(area.idAreaConocimiento)

                                 select area).ToList();
                }
                catch (Exception ex)
                {
                    return new List<cerAreaConocimiento>();
                }

                return lstPlanes;
          
        }
    }
}
