using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class WSAutenticacionDAL
    {
        public bool boolAutenticacionDescarga(string token,string claveInsId)
        {
            try
            {
                 CertificadosMediaSuperiorEntities conn = new CertificadosMediaSuperiorEntities();
                conn.Configuration.LazyLoadingEnabled = false;
                int existe = 0;
                
                existe= (from sol in conn.cerConfiguracionInstitucion
                        where sol.insTokenSeguridadDescarga.Equals(token) && sol.insId.Equals(claveInsId)  //Verifica si hay un token registrado para Acceso
                            
                        select sol).Count();

                if (existe == 1)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        
    }
}
