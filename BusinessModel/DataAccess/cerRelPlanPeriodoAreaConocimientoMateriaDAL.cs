using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    class cerRelPlanPeriodoAreaConocimientoMateriaDAL
    {

        public List<cerRelPlanPeriodoAreaConocimientoMateria> GetLstMateriasByPlan(string idPlan,string docDecTipocertificado)
        {
            List<cerRelPlanPeriodoAreaConocimientoMateria> lstMaterias = new List<cerRelPlanPeriodoAreaConocimientoMateria>();

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                lstMaterias = (from mat in certificadosEntities.cerRelPlanPeriodoAreaConocimientoMateria
                               where mat.idPlan == idPlan && mat.docTipoId== docDecTipocertificado
                               select mat).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerRelPlanPeriodoAreaConocimientoMateria>();
            }

            return lstMaterias;
        }
        public List<cerRelPlanPeriodoAreaConocimientoMateria> GetLstMateriasByTipoDocumento(string insId, string tipoDocumento)
        {
            List<cerRelPlanPeriodoAreaConocimientoMateria> lstMaterias = new List<cerRelPlanPeriodoAreaConocimientoMateria>();

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                lstMaterias = (from materia in certificadosEntities.cerRelPlanPeriodoAreaConocimientoMateria
                               where materia.insId == insId && materia.docTipoId == tipoDocumento
                               select materia).OrderByDescending(x => x.idPlan).ThenBy(x => x.idPeriodo).ThenBy(x => x.orden).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerRelPlanPeriodoAreaConocimientoMateria>();
            }

            return lstMaterias;
        }

    }
}
