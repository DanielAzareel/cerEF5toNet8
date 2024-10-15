using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerCatTipoUacDAL
    {
        public List<cerCatTipoUAC> GetLstTipoUacsByInsId(string insId)
        {
            List<cerCatTipoUAC> lsttipoUac = new List<cerCatTipoUAC>();

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                lsttipoUac = (from tipoUac in certificadosEntities.cerCatTipoUAC
                              where tipoUac.insId==insId
                            select tipoUac).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerCatTipoUAC>();
            }

            return lsttipoUac;
        }
    }
}
