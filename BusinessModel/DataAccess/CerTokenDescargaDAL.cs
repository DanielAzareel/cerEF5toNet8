using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class CerTokenDescargaDAL
    {
        public void inactivaTokens(string curp, string tokenDescarga)
        {
            try
            {
                CertificadosMediaSuperiorEntities oTEEntities = new CertificadosMediaSuperiorEntities();
                oTEEntities.Configuration.LazyLoadingEnabled = false;

                using (oTEEntities)
                {
                    oTEEntities.Database.Connection.Open();

                    using (DbContextTransaction oTransactionScope = oTEEntities.Database.BeginTransaction())
                    {
                        (from c in oTEEntities.cerTokenDescarga
                         join d in oTEEntities.cerDocumento on c.tokCURP equals d.docAlumnoCurp
                         where d.docAlumnoCurp == curp
                       && c.tokCodigo.ToLower() == tokenDescarga.ToLower()
                       && c.tokEstatus == true
                       && c.tokUsado == false
                         select c).ToList().ForEach(x => { x.tokfechaDescarga = DateTime.Now.Date; x.tokUsado = true; });

                        (from c in oTEEntities.cerTokenDescarga
                         join d in oTEEntities.cerDocumento on c.tokCURP equals d.docAlumnoCurp
                         where d.docAlumnoCurp == curp
                       && c.tokEstatus == true
                         select c).ToList().ForEach(x => x.tokEstatus = false);

                        oTEEntities.SaveChanges();
                        oTransactionScope.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public Boolean guardaTokenDescarga(cerTokenDescarga token)
        {
            bool result = false;
            try
            {
                CertificadosMediaSuperiorEntities oTEEntities = new CertificadosMediaSuperiorEntities();
                oTEEntities.Configuration.LazyLoadingEnabled = false;

                using (oTEEntities)
                {
                    oTEEntities.Database.Connection.Open();

                    using (DbContextTransaction oTransactionScope = oTEEntities.Database.BeginTransaction())
                    {
                        (from c in oTEEntities.cerTokenDescarga where c.tokCodigo == token.tokCodigo &&c.tokCURP==token.tokCURP && c.tokEstatus == true select c).ToList().ForEach(x => x.tokEstatus = false);

                        oTEEntities.cerTokenDescarga.Add(token);

                        oTEEntities.SaveChanges();
                        oTransactionScope.Commit();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }
        public bool verificaToken(string curp, string tokenDescarga)
        {

            bool result = false;
            try
            {
                CertificadosMediaSuperiorEntities oTEEntities = new CertificadosMediaSuperiorEntities();
                oTEEntities.Configuration.LazyLoadingEnabled = false;

                using (oTEEntities)
                {
                    oTEEntities.Database.Connection.Open();

                    using (DbContextTransaction oTransactionScope = oTEEntities.Database.BeginTransaction())
                    {
                        result = (from c in oTEEntities.cerTokenDescarga
                                  join d in oTEEntities.cerDocumento on c.tokCURP equals d.docAlumnoCurp
                                  where d.docAlumnoCurp == curp
                                && c.tokCodigo.ToLower() == tokenDescarga.ToLower()
                                && c.tokEstatus == true
                                && c.tokUsado == false
                                  select c).Any();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }
    }
}
