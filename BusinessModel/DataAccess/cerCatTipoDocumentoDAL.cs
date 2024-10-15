using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerCatTipoDocumentoDAL
    {
        public List<cerCatTipoDocumento> CerCatTipoDocumentos()
        {
            try
            {
                return (from c in new CertificadosMediaSuperiorEntities().cerCatTipoDocumento  select c).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerCatTipoDocumento>();
            }
        }

        public List<cerCatTipoDocumento> GetListCerCatTipoDocumento()
        {
            List<cerCatTipoDocumento> listCerTipoDocumento = new List<cerCatTipoDocumento>();
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                listCerTipoDocumento = (from tipodocumento in oCertificadosMediaSuperiorEntities.cerCatTipoDocumento
                                        select tipodocumento).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return listCerTipoDocumento;
        }

        public cerCatTipoDocumento GetListCerCatTipoDocumentoById(string sIdTipoDocumento)
        {
            cerCatTipoDocumento CerTipoDocumento = new cerCatTipoDocumento();
            try
            {
                CertificadosMediaSuperiorEntities oCertificadosMediaSuperiorEntities = new CertificadosMediaSuperiorEntities();
                oCertificadosMediaSuperiorEntities.Configuration.LazyLoadingEnabled = false;

                CerTipoDocumento = (from tipodocumento in oCertificadosMediaSuperiorEntities.cerCatTipoDocumento
                                        where tipodocumento.docTipoId == sIdTipoDocumento
                                        select tipodocumento).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return CerTipoDocumento;
        }
    }
}
