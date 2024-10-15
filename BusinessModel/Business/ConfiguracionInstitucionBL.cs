using BusinessModel.DataAccess;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using ServiciosWeb.ConsultaRenapoWS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessModel.Business
{
    public class ConfiguracionInstitucionBL
    {

        public List<cerConfiguracionInstitucion> GetConfiguracionInstituciones()
        {
            return new CerConfiguracionInstitucionDAL().GetInstituciones();
        }

        public cerConfiguracionInstitucion GetConfiguracionInstitucionByIdIns(string sIdInstitucion)
        {
            return new CerConfiguracionInstitucionDAL().ObtenerRegistroConfiguracionPorId(sIdInstitucion);
        }

        public bool AddOrUpdateInstitucion(cerConfiguracionInstitucion oCerConfiguracionInstitucion)
        {
            return new CerConfiguracionInstitucionDAL().AddOrUpdateInstitucion(oCerConfiguracionInstitucion);
        }

        public List<cerCatTipoDocumento> GetListCerCatTipoDocumento()
        {
            return new cerCatTipoDocumentoDAL().GetListCerCatTipoDocumento();
        }
        public List<cerCatPlan> GetListCerCatPlan(string sIdInstitucion)
        {
            return new CerCatPlanDAL().GetLstPlanesByIns(sIdInstitucion);
        }

        public List<cerParametroValor> GetListCerParametroValor(string sIdInstitucion, string sIdTipoDocumento, int iPagina = 1, int iBloque = 10)
        {
            int iRegInicio = ((iPagina - 1) * iBloque);
            List<cerParametroValor> cerParametroValors = new cerParametroValorDAL().GetLstParametros(sIdInstitucion, sIdTipoDocumento, iRegInicio, iBloque);
            List<cerCatTipoDocumento> listCerCatTipoDocumento = new EnvioSEPBL().GetListCerCatTipoDocumentos();

            foreach (var parametro in cerParametroValors)
            {
                parametro.cerCatTipoDocumento = listCerCatTipoDocumento.Where(x => x.docTipoId == parametro.docTipoId).FirstOrDefault();
            }
            return cerParametroValors;
        }
        public int GetCountCerParametroValor(string sIdInstitucion, string sIdTipoDocumento)
        {
            return new cerParametroValorDAL().GetCountParametros(sIdInstitucion, sIdTipoDocumento);
        }

        public List<ViewPlantillas> GetPlantillas(string sIdInstitucion, string sIdTipoDocumento, string sIdPlan, int iPagina, int iBloque)
        {
            int iRegInicio = ((iPagina - 1) * iBloque);
            return new CerConfiguracionInstitucionDAL().GetListViewPlantillas(sIdInstitucion, sIdTipoDocumento, sIdPlan, iRegInicio, iBloque);
        }

        public int GetCountPlantillas(string sIdInstitucion, string sIdTipoDocumento, string sIdPlan)
        {
            return new CerConfiguracionInstitucionDAL().GetCountViewPlantillas(sIdInstitucion, sIdTipoDocumento, sIdPlan);
        }

        public bool EditarEtiqueta(string parId, string docTipoId, string parValor)
        {
            return new cerParametroValorDAL().EditarParametro(parId, docTipoId, parValor);
        }

        public bool AgregarPlantilla(PlantillaCertificadoML oPlantillaCertificadoML)
        {
            cerCatPlantilla cerCatPlantilla = new cerCatPlantilla();
            cerCatPlantilla.planId = Guid.NewGuid().ToString();
            cerCatPlantilla.planNombre = oPlantillaCertificadoML.plaNombre;
            cerCatPlantilla.planNumHojas = oPlantillaCertificadoML.plaNumHojas;
            cerCatPlantilla.insId = oPlantillaCertificadoML.insId;
            cerCatPlantilla oCerCatPlantilla = GetPlantillaById(oPlantillaCertificadoML.planId);

            if (oCerCatPlantilla != null)
            {
                cerCatPlantilla.planEstatus = oCerCatPlantilla.planEstatus;
                if (oPlantillaCertificadoML.archivoPlantilla != null)
                {
                    cerCatPlantilla.planArchivo = createFilePlantilla(oPlantillaCertificadoML.archivoPlantilla, cerCatPlantilla.planId);
                }
                else
                {
                    cerCatPlantilla.planArchivo = createFilePlantillaByte(oCerCatPlantilla.planArchivo, cerCatPlantilla.planId);
                }
            }
            else
            {
                cerCatPlantilla.planArchivo = createFilePlantilla(oPlantillaCertificadoML.archivoPlantilla, cerCatPlantilla.planId);
                cerCatPlantilla.planEstatus = false;
            }



            cerCatPlantilla.docTipoId = oPlantillaCertificadoML.docTipoId;
            cerCatPlantilla.idPlan = oPlantillaCertificadoML.idPlan;


            if (!String.IsNullOrEmpty(oPlantillaCertificadoML.planId))
            {
                oPlantillaCertificadoML.sIdPlantillaAnterior = oPlantillaCertificadoML.planId;

                bool bResultado = new cerCatPlantillaDAL().EditarPlantilla(cerCatPlantilla, oPlantillaCertificadoML.sIdPlantillaAnterior);

                if (bResultado)
                {
                    DeleteFilePlantilla(oPlantillaCertificadoML.sIdPlantillaAnterior);
                }

                return bResultado;
            }
            else
            {
                return new cerCatPlantillaDAL().AgregarPlantilla(cerCatPlantilla);
            }
        }


        public byte[] createFilePlantilla(HttpPostedFileBase file, string nameFile)
        {
            string rutaCarpetaContent = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaPlantillas"]);
            //Crear carpeta
            Directory.CreateDirectory(Path.Combine(rutaCarpetaContent, nameFile));

            if (Path.GetExtension(file.FileName) == ".zip")
            {

                file.SaveAs(Path.Combine(rutaCarpetaContent, nameFile + Path.GetExtension(nameFile + ".zip")));
                ZipFile.ExtractToDirectory(Path.Combine(rutaCarpetaContent, nameFile + Path.GetExtension(nameFile + ".zip")), Path.Combine(rutaCarpetaContent, nameFile));
            }
            else
            {
                file.SaveAs(Path.Combine(rutaCarpetaContent, nameFile, "1.docx"));
                //Crea ZIP
                ZipFile.CreateFromDirectory(Path.Combine(rutaCarpetaContent, nameFile), Path.Combine(rutaCarpetaContent, nameFile + ".zip"), CompressionLevel.Fastest, false);

            }
            return File.ReadAllBytes(Path.Combine(rutaCarpetaContent, nameFile + ".zip"));

        }


        public byte[] createFilePlantillaByte(byte[] file, string nameFile)
        {
            string rutaCarpetaContent = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaPlantillas"]);
            

            if (Path.GetExtension(nameFile) == ".zip")
            {
                File.WriteAllBytes(Path.Combine(rutaCarpetaContent, nameFile + Path.GetExtension(nameFile + ".zip")), file);
                ZipFile.ExtractToDirectory(Path.Combine(rutaCarpetaContent, nameFile + Path.GetExtension(nameFile + ".zip")), Path.Combine(rutaCarpetaContent, nameFile));
            }
            else
            {
                if(!File.Exists(Path.Combine(rutaCarpetaContent, nameFile + ".zip")))
                {
                    File.WriteAllBytes(Path.Combine(rutaCarpetaContent, nameFile + ".zip"), file);
                }

                ZipFile.ExtractToDirectory(Path.Combine(rutaCarpetaContent, nameFile + ".zip"), Path.Combine(rutaCarpetaContent, nameFile));
                

            }

            //retornar Zip
            return File.ReadAllBytes(Path.Combine(rutaCarpetaContent, nameFile + ".zip"));


        }

        public bool DeleteFilePlantilla(string sNombrePlantilla)
        {
            bool bResultado = false;
            try
            {
                string rutaCarpetaContent = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaPlantillas"], sNombrePlantilla);
                Directory.Delete(rutaCarpetaContent, true);
                bResultado = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return bResultado;
        }

        public cerCatPlantilla GetPlantillaById(string sIdPlantilla)
        {
            return new cerCatPlantillaDAL().GetPlantillasById(sIdPlantilla);
        }

        public string[] EliminarPlantilla(string sIdPlantilla)
        {
            string[] arrResultado = new string[2];

            ViewPlantillas viewPlantilla = new cerCatPlantillaDAL().GetViewPlantillaById(sIdPlantilla);

            if (viewPlantilla.registros == 0)
            {
                if (viewPlantilla.planEstatus.GetValueOrDefault())
                {
                    arrResultado[0] = "False";
                    arrResultado[1] = "No se puede eliminar la plantilla cuando es predeterminada.";
                }
                else
                {

                    bool bResultado = new cerCatPlantillaDAL().EliminarPlantillaById(sIdPlantilla);

                    if (bResultado)
                    {
                        arrResultado[0] = "True";
                        arrResultado[1] = "Se ha eliminado la plantilla correctamente.";
                    }
                    else
                    {
                        arrResultado[0] = "False";
                        arrResultado[1] = "No se ha eliminado la plantilla correctamente.";
                    }
                }
            }
            else
            {
                arrResultado[0] = "False";
                arrResultado[1] = "No se puede eliminar la plantilla por que tiene registros asociados.";
            }

            return arrResultado;
        }

        public bool AsignarPlantilla(string sIdPlantilla)
        {
            return new cerCatPlantillaDAL().AsignarPlantilla(sIdPlantilla);
        }

        public bool ExisteNombrePlantilla(string sIdPlantilla, string sNombrePlantilla, string sIdInstitucion)
        {
            int iResultado = new cerCatPlantillaDAL().ExisteNombrePlantilla(sIdPlantilla, sNombrePlantilla, sIdInstitucion);

            if (iResultado == 0) { return true; }
            else
            {
                return false;
            }
        }

        public string[] AgregarEditarFirmante(FirmanteCertificadoML firmanteCertificadoML)
        {
            string[] arrResultado = new string[2];
            bool bEdicion = false;

            arrResultado[0] = "False";
            arrResultado[1] = "No se ha guardado la información correctamente.";
            try
            {
                cerCatFirmante cerCatFirmante = new cerCatFirmante();
                cerCatFirmante.firNombre = firmanteCertificadoML.firNombre;
                cerCatFirmante.firPrimerApellido = firmanteCertificadoML.firPrimerApellido;
                cerCatFirmante.firSegundoApellido = firmanteCertificadoML.firSegundoApellido;
                cerCatFirmante.firIdCargo = firmanteCertificadoML.firIdCargo;
                cerCatFirmante.firCargo = firmanteCertificadoML.firCargo;
                cerCatFirmante.insId = firmanteCertificadoML.insId;
                cerCatFirmante.firCorreo = firmanteCertificadoML.firCorreo;
                cerCatFirmante.firPredeterminado = firmanteCertificadoML.firPredeterminado;
                cerCatFirmante.firId = firmanteCertificadoML.firId;
                cerCatFirmante.firVigenciaCertificado = firmanteCertificadoML.firVigenciaCertificado;

                if (!String.IsNullOrEmpty(firmanteCertificadoML.firId))
                {
                    bEdicion = true;
                }
                else
                {
                    cerCatFirmante.firId = Guid.NewGuid().ToString();
                }

                if (firmanteCertificadoML.firArchivoCertificadoEditar != null)
                {
                    firmanteCertificadoML.firArchivoCertificado = firmanteCertificadoML.firArchivoCertificadoEditar;

                }

                if (firmanteCertificadoML.firArchivoCertificado != null)
                {
                    byte[] data;
                    using (Stream inputStream = firmanteCertificadoML.firArchivoCertificado.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }

                    X509Certificate2 certificado = new X509Certificate2(data);
                    

                    var datosFirmante = ObtenerCurpCertificado(certificado);

                    if(DateTime.Now > certificado.NotAfter)
                    {
                        arrResultado[0] = "False";
                        arrResultado[1] = "La fecha vigencia del certificado público ya expiró.";

                        return arrResultado;
                    }

                    cerCatFirmante.firVigenciaCertificado = certificado.NotAfter;
                    cerCatFirmante.firCertificadoPublico = Convert.ToBase64String(data);
                    cerCatFirmante.firNumeroCertificado = datosFirmante[1].Replace("3", "");

                    ConsultaRenapoClient SWRenapo = new ConsultaRenapoClient();

                    string sResultadoRENAPO = SWRenapo.consultarPorCurp(datosFirmante[0]);
                    string[] arrRENAPO = sResultadoRENAPO.Split('|');
                    if (arrRENAPO.Count() > 1)
                    {
                        cerCatFirmante.firCurp = arrRENAPO[0].Trim() ?? null;
                    }
                    else
                    {
                        arrResultado[0] = "False";
                        arrResultado[1] = "El certificado público no es valido.";

                        return arrResultado;
                    }
                }
                else
                {
                    cerCatFirmante oCerCatFirmante = getFirmanteById(firmanteCertificadoML.firId, firmanteCertificadoML.insId);
                    cerCatFirmante.firCertificadoPublico = oCerCatFirmante.firCertificadoPublico;
                    cerCatFirmante.firNumeroCertificado = oCerCatFirmante.firNumeroCertificado;
                    cerCatFirmante.firCurp = oCerCatFirmante.firCurp;
                }

                if (!bEdicion)
                {
                    cerCatFirmante oCerCatFirmante1 = getFirmanteByNumeroCertificado(cerCatFirmante.firNumeroCertificado, cerCatFirmante.insId);
                    if (oCerCatFirmante1 != null)
                    {
                        arrResultado[0] = "false";
                        arrResultado[1] = "Ya existe un firmante con ese número de certificado.";

                        return arrResultado;
                    }
                }
                else
                {
                    cerCatFirmante oCerCatFirmante1 = getFirmanteByNumeroCertificadoAndId(cerCatFirmante.firNumeroCertificado, cerCatFirmante.insId, cerCatFirmante.firId);
                    if (oCerCatFirmante1 != null)
                    {
                        arrResultado[0] = "false";
                        arrResultado[1] = "Ya existe un firmante con ese número de certificado.";

                        return arrResultado;
                    }
                }


                if (new CerCatFirmanteDAL().AgregarEditarFirmante(cerCatFirmante))
                {
                    arrResultado[0] = "True";
                    arrResultado[1] = "Se ha guardado la información correctamente.";
                }
                else
                {
                    arrResultado[0] = "False";
                    arrResultado[1] = "No se ha guardado la información correctamente.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return arrResultado;
        }

        public string[] ObtenerCurpCertificado(X509Certificate2 certificado)
        {
            string[] arrDatosFirmante = new string[2];
            String[] datosFirmante = certificado.SubjectName.Name.Split(',');
            String[] claveValor = datosFirmante[0].Split('=');
            arrDatosFirmante[0] = claveValor[1];
            arrDatosFirmante[1] = certificado.SerialNumber;
            return arrDatosFirmante;
        }

        public List<cerCatFirmante> GetListFirmantes(string sIdInstitucion, int iPagina, int iBloque)
        {
            int iRegInicio = ((iPagina - 1) * iBloque);
            return new CerCatFirmanteDAL().GetListFirmantes(sIdInstitucion, iRegInicio, iBloque);
        }

        public int GetCountFirmantes(string sIdInstitucion)
        {
            return new CerCatFirmanteDAL().GetCountFirmantes(sIdInstitucion);
        }

        public bool AsignarFirmante(string firId, string insId)
        {
            return new CerCatFirmanteDAL().AsignarFirmante(firId, insId);
        }

        public cerCatFirmante getFirmanteById(string firId, string insId)
        {
            return new CerCatFirmanteDAL().getFirmanteById(firId, insId);
        }

        public cerCatFirmante getFirmanteByNumeroCertificado(string firNumeroCertificado, string insId)
        {
            return new CerCatFirmanteDAL().getFirmanteByNumeroCertificado(firNumeroCertificado, insId);
        }

        public cerCatFirmante getFirmanteByNumeroCertificadoAndId(string firNumeroCertificado, string insId, string firId)
        {
            return new CerCatFirmanteDAL().getFirmanteByNumeroCertificadoAndId(firNumeroCertificado, insId, firId);
        }

        public string[] EliminarFirmanteById(string firId, string insId)
        {
            string[] arrResultado = new string[2];
            cerCatFirmante cerCatFirmante = new CerCatFirmanteDAL().getFirmanteById(firId, insId);
            if (cerCatFirmante.firPredeterminado)
            {
                arrResultado[0] = "False";
                arrResultado[1] = "No se puede eliminar el firmante, se encuentra como predeterminado.";
            }
            else
            {
                bool bResultado = new CerCatFirmanteDAL().EliminarFirmanteById(firId, insId);

                if (bResultado)
                {
                    arrResultado[0] = "True";
                    arrResultado[1] = "Se ha eliminado el firmante correctamente.";
                }
                else
                {
                    arrResultado[0] = "False";
                    arrResultado[1] = "No se ha eliminado el firmante correctamente.";
                }
            }
            return arrResultado;
        }

        public cerCatPlantilla GetPlantillaFileById(string sIdPlantilla)
        {
            cerCatPlantilla cerCatPlantilla = new cerCatPlantillaDAL().GetPlantillaFileById(sIdPlantilla);
            byte[] bArchivo = extraerFilePlantilla(cerCatPlantilla.planArchivo, cerCatPlantilla.planId, cerCatPlantilla.planNumHojas);
            cerCatPlantilla.planArchivo = bArchivo;
            return cerCatPlantilla;
        }

        public static byte[] extraerFilePlantilla(Byte[] fileZip, string fileName, int? iNumHojas)
        {
            try
            {
                string rutaCarpetaContent = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaPlantillas"]);

                if (!File.Exists(Path.Combine(rutaCarpetaContent, fileName + ".zip")))
                {
                    File.WriteAllBytes(Path.Combine(rutaCarpetaContent, fileName + ".zip"), fileZip);
                }

                if (iNumHojas > 1)
                {

                    if (!Directory.Exists(Path.Combine(Path.Combine(rutaCarpetaContent, fileName))))
                    {
                        ZipFile.ExtractToDirectory(Path.Combine(rutaCarpetaContent, fileName + ".zip"), Path.Combine(Path.Combine(rutaCarpetaContent, fileName)));
                    }

                    return File.ReadAllBytes(Path.Combine(rutaCarpetaContent, fileName + ".zip"));
                }
                else
                {
                    if (!File.Exists(Path.Combine(rutaCarpetaContent, fileName, "1.docx")))
                    {
                        
                            ZipFile.ExtractToDirectory(Path.Combine(rutaCarpetaContent, fileName + ".zip"), Path.Combine(rutaCarpetaContent, fileName));
                        
                    }

                    return File.ReadAllBytes(Path.Combine(rutaCarpetaContent, fileName, "1.docx"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public cerCatFirmante GetCertificadoFileById(string firId, string insId)
        {
            return new CerCatFirmanteDAL().getFirmanteById(firId, insId);
        }

        public cerCatFirmante GetFirmanteActivoSinArchivo(string insId)
        {
            return new CerCatFirmanteDAL().GetFirmanteActivoSinArchivo(insId);
        }

        public cerCatFirmante GetRFCFirmantePredeterminado(string insId)
        {
            cerCatFirmante catFirmante = new CerCatFirmanteDAL().GetFirmanteActivoSinArchivo(insId);

            return catFirmante;
        }
    }
}
