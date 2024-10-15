using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class CerCatEstatusSolicitudDAL
    {
        public List<cerCatEstatusSolicitud> GetListEstatusSolicitud()
        {
            List<cerCatEstatusSolicitud> listcerCatEstatusSolicitud = new List<cerCatEstatusSolicitud>();
            try
            {
                using (CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
                {
                    oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                    listcerCatEstatusSolicitud = (from estatusdocumento in oCertificadosMediaSuperiorEntities.cerCatEstatusSolicitud
                                                  select estatusdocumento).ToList();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listcerCatEstatusSolicitud;
        }
    }
}
