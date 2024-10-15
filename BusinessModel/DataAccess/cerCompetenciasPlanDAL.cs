using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerCompetenciasPlanDAL
    {
        public List<cerCompetenciasPlan> GetLstCompetenciasIEMS(string idPlan)
        {
            List<cerCompetenciasPlan> lstCompetencias = new List<cerCompetenciasPlan>();

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                lstCompetencias = (from com in certificadosEntities.cerCompetenciasPlan where com.idPlan==idPlan

                             select com).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerCompetenciasPlan>();
            }

            return lstCompetencias;
        }
    }
}
