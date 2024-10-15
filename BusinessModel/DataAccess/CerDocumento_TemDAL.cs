using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.DataAccess
{
    public class CerDocumento_TemDAL
    {
        public bool AddRegistrosArchivo(List<cerDocumento_Tem> listArchivoCarga)
        {
            bool bBandera = false;
            try
            {
                using (CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities())
                {
                    certificadosEntities.cerDocumento_Tem.AddRange(listArchivoCarga);
                    certificadosEntities.SaveChanges();
                }

                bBandera = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                bBandera = false;
            }

            return bBandera;
        }
        public List<cerDocumento_Tem> lstDocumentosAcargar(FiltrosConsultaCarga filtroBusqueda, out int totalR, out int totalRconErrores, out int totalRconObservaciones, out int RegistrosTotales, out int RegistrosSinErrores, int pagina, int bloque)
        {

            totalR = 0;
            totalRconErrores = 0;
            totalRconObservaciones = 0;
            RegistrosTotales = 0;
            RegistrosSinErrores = 0;

            List<cerDocumento_Tem> lstDocumentos = new List<cerDocumento_Tem>();
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                RegistrosTotales = (from documento in certificadosEntities.cerDocumento_Tem
                                    where
                                    documento.carId == filtroBusqueda.idDeCarga && documento.docInsertado != true
                                    select documento
                                    ).Count();

                lstDocumentos = (from documento in certificadosEntities.cerDocumento_Tem
                                 where documento.carId == filtroBusqueda.idDeCarga && documento.docInsertado != true && ((filtroBusqueda.registrosCorrectos == false && filtroBusqueda.registrosConError == false && filtroBusqueda.registrosConObservavion == false && documento.docInsertado != true)

                                 ||
                                 (filtroBusqueda.registrosCorrectos == true && documento.totalObservaciones == 0 && documento.totalErrores == 0) ||

                                 (filtroBusqueda.registrosConError == true && documento.totalErrores > 0) ||
                                 (filtroBusqueda.registrosConObservavion == true && documento.totalObservaciones > 0))



                                 select documento
                                    ).OrderBy(x => x.fila).Skip((pagina - 1) * bloque).Take(bloque).ToList();

                totalR = (from documento in certificadosEntities.cerDocumento_Tem
                          where
                                  documento.carId == filtroBusqueda.idDeCarga && documento.docInsertado != true && ((filtroBusqueda.registrosCorrectos == false && filtroBusqueda.registrosConError == false && filtroBusqueda.registrosConObservavion == false && documento.docInsertado != true)

                                 ||
                                 (filtroBusqueda.registrosCorrectos == true && documento.totalObservaciones == 0 && documento.totalErrores == 0) ||

                                 (filtroBusqueda.registrosConError == true && documento.totalErrores > 0) ||
                                 (filtroBusqueda.registrosConObservavion == true && documento.totalObservaciones > 0))



                          select documento
                                    ).Count();
                totalRconErrores = (from documento in certificadosEntities.cerDocumento_Tem
                                    where
                                     documento.totalErrores > 0 &&
                                     documento.carId == filtroBusqueda.idDeCarga && documento.docInsertado != true
                                    select documento
                                    ).Count();
                totalRconObservaciones = (from documento in certificadosEntities.cerDocumento_Tem
                                          where
                                           documento.totalObservaciones > 0 && documento.totalErrores == 0 &&
                                           documento.carId == filtroBusqueda.idDeCarga && documento.docInsertado != true
                                          select documento
                                  ).Count();
                RegistrosSinErrores = (from documento in certificadosEntities.cerDocumento_Tem
                                       where
                                        documento.totalObservaciones == 0 &&
                                        documento.totalErrores == 0 &&
                                        documento.carId == filtroBusqueda.idDeCarga && documento.docInsertado != true
                                       select documento
                                  ).Count();
            }
            catch (Exception ex)
            {
                return new List<cerDocumento_Tem>();
            }
            return lstDocumentos;
        }
        public int ValidarArchivo(string carId, string idUsuario, string insId, string sp)
        {

            int result = 0;
            try
            {
                using (var conn = new CertificadosMediaSuperiorEntities())
                {
                    SqlDataReader rs = null;
                    var SQLConn = (SqlConnection)conn.Database.Connection;
                    conn.Database.CommandTimeout = 600;
                    SQLConn.Open();

                    using (SqlCommand sc = new SqlCommand(sp + " @carId = '" + carId + "',@idUsuario='" + idUsuario + "'" + " , @institucion='" + insId + "'", SQLConn))
                    {
                        sc.CommandTimeout = 600;

                        rs = sc.ExecuteReader();
                        while (rs.Read())
                        {
                            result = Convert.ToInt32(rs[""]);
                        }

                    }
                }

                return result;
            }
            catch (Exception e)
            {

                return result;
            }

        }
        public int CargarArchivo(string carId, bool cargaConObservaciones, string idUsuario, string sp)
        {

            int result = 0;
            try
            {
                using (var conn = new CertificadosMediaSuperiorEntities())
                {
                    SqlDataReader rs = null;
                    var SQLConn = (SqlConnection)conn.Database.Connection;

                    SQLConn.Open();

                    using (SqlCommand sc = new SqlCommand(sp + " @carId = '" + carId + "',@idUsuario='" + idUsuario + "', @subirConObservaiones=" + cargaConObservaciones, SQLConn))
                    {

                        rs = sc.ExecuteReader();
                        while (rs.Read())
                        {
                            result = Convert.ToInt32(rs[""]);
                        }


                    }
                }

                return result;
            }
            catch (Exception e)
            {

                return result;
            }
        }

        public string GetTipoDocumentoByCarId(string carId)
        {
            try
            {
                CertificadosMediaSuperiorEntities certificadosEntities = new CertificadosMediaSuperiorEntities();
                return (from documento in certificadosEntities.cerDocumento_Tem
                        where
                        documento.carId == carId
                        select documento.docDecTipocertificado
                                    ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
