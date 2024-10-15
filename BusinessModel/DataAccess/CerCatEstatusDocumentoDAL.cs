using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class CerCatEstatusDocumentoDAL
    {
        public List<cerCatEstatusDocumento> GetListEstatusDocumento()
        {
            List<cerCatEstatusDocumento> listcerCatEstatusDocumento = new List<cerCatEstatusDocumento>();
            try
            {
                using (CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities())
                {
                    oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                    listcerCatEstatusDocumento = (from estatusdocumento in oCertificadosMediaSuperiorEntities.cerCatEstatusDocumento
                                                  where estatusdocumento.estDocumentoId != 1
                                                  select estatusdocumento).ToList();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listcerCatEstatusDocumento;
        }
    }
}
