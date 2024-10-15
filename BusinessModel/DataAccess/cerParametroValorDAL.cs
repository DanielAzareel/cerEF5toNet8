using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class cerParametroValorDAL
    {

        public List<cerParametroValor> GetLstParametrosByIdDocumento(string insId, string docTipoId)
        {
            List<cerParametroValor> lstParametros = new List<cerParametroValor>();

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                lstParametros = (from par in certificadosEntities.cerParametroValor
                                 where par.insId == insId && par.docTipoId == docTipoId
                                 select par).ToList();
            }
            catch (Exception ex)
            {
                return new List<cerParametroValor>();
            }

            return lstParametros;
        }

        public List<cerParametroValor> GetLstParametros(string insId, string docTipoId, int iPagina, int iBloque)
        {
            List<cerParametroValor> lstParametros = new List<cerParametroValor>();

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                lstParametros = (from par in certificadosEntities.cerParametroValor
                                 where (String.IsNullOrEmpty(insId) || par.insId == insId)
                                 && (String.IsNullOrEmpty(docTipoId) || par.docTipoId == docTipoId)
                                 select par).OrderBy(x => x.parId).Skip(iPagina).Take(iBloque).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<cerParametroValor>();
            }

            return lstParametros;
        }

        public int GetCountParametros(string insId, string docTipoId)
        {
            int iResultado = 0;

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                iResultado = (from par in certificadosEntities.cerParametroValor
                              where (String.IsNullOrEmpty(insId) || par.insId == insId)
                              && (String.IsNullOrEmpty(docTipoId) || par.docTipoId == docTipoId)
                              select par).Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return iResultado;
        }

        public bool EditarParametro(string parId, string docTipoId, string parValor)
        {
            bool bResultado = false;

            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                certificadosEntities.Configuration.LazyLoadingEnabled = false;

                var oParametroValor = (from parametro in certificadosEntities.cerParametroValor
                                       where parametro.parId == parId && parametro.docTipoId == docTipoId
                                       select parametro).FirstOrDefault();

                oParametroValor.parValor = parValor;

                certificadosEntities.SaveChanges();

                bResultado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return bResultado;
        }
    }
}
