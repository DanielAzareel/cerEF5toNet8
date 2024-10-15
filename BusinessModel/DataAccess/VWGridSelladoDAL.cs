using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class VWGridSelladoDAL
    {

        public List<VWGridSellado> GetVWGridSelladoPaginacion(string where, int inicio, int bloque)
        {

            try
            {

                return  (new CertificadosMediaSuperiorEntities().VWGridSellado.SqlQuery("select * from VWGridSellado where "+where + " order by docFechaRegistro desc")
                        ).ToList().Skip(inicio).Take(bloque).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return new List<VWGridSellado>();
            }
        }
        public int GetVWGridSelladoPaginacionCount(string where  , int inicio, int bloque)
        {

            try
            { 
                return new CertificadosMediaSuperiorEntities().Database.SqlQuery<int>("select count(1) from VWGridSellado where "+ where).First() ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }
    }
}
