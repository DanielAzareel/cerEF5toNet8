using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerCatMateriaDAL
    {
        public List<cerCatMateria> GetLstMaterias(string idPlan="")
        {
            List<cerCatMateria> lstMaterias = new List<cerCatMateria>();

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                lstMaterias = (from mat in certificadosEntities.cerCatMateria
                               where
                                (idPlan == "" || mat.idPlan==idPlan)
                               select mat).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerCatMateria>();
            }

            return lstMaterias;
        }
        public List<(string insId, string idPlan, string idAreaConocimiento,string creditos)> MateriasAreaEspecializacionByInsId(string insId )
        {
            List<(string insId, string idPlan, string idAreaConocimiento, string creditos)> result = new List<(string insId, string idPlan, string idAreaConocimiento, string creditos)>();
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;
                result = (from rel in certificadosEntities.cerRelPlanPeriodoAreaConocimientoMateria
                             join
                             mat in certificadosEntities.cerCatMateria on rel.idMateria equals mat.idMateria
                             where rel.insId == insId  
                             select new { insId=rel.insId, idPlan=rel.idPlan, idAreaConocimiento=rel.idAreaConocimiento, materiaCreditos=mat.materiaCreditos }
                             ).ToList().Select(x=>(x.insId,x.idPlan,x.idAreaConocimiento,x.materiaCreditos)).ToList();

            }
            catch (Exception ex)
            {
                
            }


            return result;
        }
    }
}
