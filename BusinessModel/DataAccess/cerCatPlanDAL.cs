using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class CerCatPlanDAL
    {
      

        public List<cerCatPlan> GetLstPlanes()
        {
            List<cerCatPlan> lstPlanes = new List<cerCatPlan>();



            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;



                lstPlanes = (from plan in certificadosEntities.cerCatPlan



                             select plan).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerCatPlan>();
            }



            return lstPlanes;
        }


        public List<cerCatPlan> GetLstPlanesByIns(string InsId)
        {
            List<cerCatPlan> lstPlanes = new List<cerCatPlan>();

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                lstPlanes = (from plan in certificadosEntities.cerCatPlan where (from c in certificadosEntities.cerRelPlanPeriodoAreaConocimientoMateria where c.insId==InsId select c.idPlan).Contains(plan.idPlan)

                             select plan).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerCatPlan>();
            }

            return lstPlanes;
        }
    }
}

