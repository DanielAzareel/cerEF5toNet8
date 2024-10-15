using BusinessModel.DataAccess;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using BusinessModel.ServiceAccess;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ClienteIEMS.Models;
using ClienteIEMS.Controllers;
//using Newtonsoft.Json;
using System.Web.Caching;

namespace BusinessModel.Business
{
    public class EnvioSEPBL
    {
        public List<cerCatEstatusSolicitud> GetListEstatusSolicitud()
        {
            return new CerCatEstatusSolicitudDAL().GetListEstatusSolicitud();
        }

        public List<cerCatEstatusDocumento> GetListEstatusDocumento()
        {
            return new CerCatEstatusDocumentoDAL().GetListEstatusDocumento();
        }

        public List<cerCatPlan> GetListPlanByInstitucion(string sIdInstitucion)
        {
            return new CerCatPlanDAL().GetLstPlanesByIns(sIdInstitucion);
        }

        public List<cerCatTipoDocumento> GetListCerCatTipoDocumentos()
        {
            return new cerCatTipoDocumentoDAL().CerCatTipoDocumentos();
        }

        public MonitoreoSEPML GetListadoSolicitudes(CriteriosBusquedaMonitoreoModel oCriteriosBusquedaMonitoreoModel, int iPagina, int iBloque)
        {
            MonitoreoSEPML oMonitoreoSEPML = new MonitoreoSEPML();
            cerSolicitudDAL solicitudDAL = new cerSolicitudDAL();
            int iRegInicio = ((iPagina - 1) * iBloque);

            if (!String.IsNullOrEmpty(oCriteriosBusquedaMonitoreoModel.sFolioControl))
            {
                if (oCriteriosBusquedaMonitoreoModel.sFolioControl.Contains(","))
                {
                    oCriteriosBusquedaMonitoreoModel.lstFolio = oCriteriosBusquedaMonitoreoModel.sFolioControl.Split(',').ToList();
                }
                else
                {

                    oCriteriosBusquedaMonitoreoModel.lstFolio.Add(oCriteriosBusquedaMonitoreoModel.sFolioControl);
                }
            }

            if (!String.IsNullOrEmpty(oCriteriosBusquedaMonitoreoModel.sCURP))
            {
                if (oCriteriosBusquedaMonitoreoModel.sCURP.Contains(","))
                {
                    oCriteriosBusquedaMonitoreoModel.lstCURP = oCriteriosBusquedaMonitoreoModel.sCURP.Split(',').ToList();
                }
                else
                {

                    oCriteriosBusquedaMonitoreoModel.lstCURP.Add(oCriteriosBusquedaMonitoreoModel.sCURP);
                }
            }


            oMonitoreoSEPML.listViewSolicitud = solicitudDAL.ObtenerRegistrosSolicitud(oCriteriosBusquedaMonitoreoModel, iRegInicio, iBloque);
            oMonitoreoSEPML.iTotalRegistrosSolicitudes = solicitudDAL.ObtenerCountSolicitud(oCriteriosBusquedaMonitoreoModel);

            return oMonitoreoSEPML;
        }

        public ViewSolicitud GetSolicitudById(string sIdSolicitud)
        {
            return new cerSolicitudDAL().GetSolicitudById(sIdSolicitud);
        }

        public MonitoreoSEPML GetListadoCertificados(CriteriosBusquedaMonitoreoModel oCriteriosBusquedaMonitoreoModel, int iPagina, int iBloque)
        {
            MonitoreoSEPML oMonitoreoSEPML = new MonitoreoSEPML();
            CerDocumentoDAL oCerDocumentoDAL = new CerDocumentoDAL();
            int iRegInicio = ((iPagina - 1) * iBloque);
            if (!String.IsNullOrEmpty(oCriteriosBusquedaMonitoreoModel.sFolioControl))
            {
                if (oCriteriosBusquedaMonitoreoModel.sFolioControl.Contains(","))
                {
                    oCriteriosBusquedaMonitoreoModel.lstFolio = oCriteriosBusquedaMonitoreoModel.sFolioControl.Split(',').ToList();
                }
                else
                {
                    oCriteriosBusquedaMonitoreoModel.lstFolio.Add(oCriteriosBusquedaMonitoreoModel.sFolioControl);
                }
            }

            if (!String.IsNullOrEmpty(oCriteriosBusquedaMonitoreoModel.sCURP))
            {
                if (oCriteriosBusquedaMonitoreoModel.sCURP.Contains(","))
                {
                    oCriteriosBusquedaMonitoreoModel.lstCURP = oCriteriosBusquedaMonitoreoModel.sCURP.Split(',').ToList();
                }
                else
                {
                    oCriteriosBusquedaMonitoreoModel.lstCURP.Add(oCriteriosBusquedaMonitoreoModel.sCURP);
                }
            }


            oMonitoreoSEPML.listCerDocumento = oCerDocumentoDAL.ObtenerListaDocumentos(oCriteriosBusquedaMonitoreoModel, iRegInicio, iBloque);
            oMonitoreoSEPML.iTotalRegistrosCertificados = oCerDocumentoDAL.ObtenerCountDocumentos(oCriteriosBusquedaMonitoreoModel);

            return oMonitoreoSEPML;
        }

        public string[] ProcesarSolicitud(string solId, string userId)
        {
            cerSolicitud solicitud = new cerSolicitudDAL().ObtenerRegistroSolicitud(solId);

            int? iEstatusSolicitud = solicitud.estSolicitudId;

            string[] resultado = new string[2];

            resultado[0] = "false";
            resultado[1] = "No fue posible realizar la acción.";

            cerConfiguracionInstitucion configuracionInstitucion = new cerConfiguracionInstitucion();

            configuracionInstitucion = new CerConfiguracionInstitucionDAL().ObtenerRegistroConfiguracionPorId(solicitud.insId);

            List<cerDocumento> documentos = new CerDocumentoDAL().ObtenerListaDocumentosBySolId(solicitud.solId);
            if (documentos.Count > 0)
            {
                solicitud.estSolicitudId = 2;//Estatus a procesando solicitud para que no se realicen peticiones sobre la misma solicitud.
                solicitud.solEnProceso = true;
                new cerSolicitudDAL().AgregarActualizarSolicitud(solicitud);

                solicitud.estSolicitudId = iEstatusSolicitud;

                if (solicitud.estSolicitudId == 1)
                {
                    EnviarLote(configuracionInstitucion, ref documentos, ref solicitud, userId);
                }

                if (solicitud.estSolicitudId == 3)
                {
                    ObtenerEstatusLote(configuracionInstitucion, ref documentos, ref solicitud);
                }

                if (solicitud.estSolicitudId == 4)
                {
                    DescargarResultadoLote(configuracionInstitucion, ref solicitud);

                }

                if (solicitud.estSolicitudId == 5)
                {
                    ActualizarRegistrosSolicitud(ref solicitud, userId);
                }

                solicitud.solEnProceso = false;
                new cerSolicitudDAL().AgregarActualizarSolicitud(solicitud);
            }
            else
            {
                resultado[1] = "No se encontraron documentos asociados a la solicitud.";
            }
            return resultado;
        }

        public Boolean EnviarLote(cerConfiguracionInstitucion configuracionInstitucion, ref List<cerDocumento> documentos, ref cerSolicitud solicitud, string userId)
        {
            Boolean resultado = false;
            int numeroIntentos = 10;
            try
            {
                solicitud.estSolicitudId = 1; //Se regresa al estatus inicial para que en caso de que no se envie con exito se quede disponible para enviar nuevamente.

                if (solicitud.solArchivoEnvio == null)
                {
                    List<string> listDocId = (from documento in documentos select documento.docId).ToList();
                    List<cerCompetenciaDocumento> listcerCompetenciaDocumento = new cerCompetenciaDocumentoDAL().GetCompetenciaByListIdDocumento(listDocId);
                    List<cerUACDocumento> listcerUACDocumento = new cerUACDocumentoDAL().GetListUACDocumento(listDocId);
                    List<cerCompetenciasIEMS> listcerCompetenciaIEMS = new cerCompetenciasIEMSDAL().GetCompetenciaByIdDocumento(listDocId);

                    List<(Dec, string)> listcertificadoTerminacionMS = (from documento in documentos
                                                                        where documento.docTipoId == "1"
                                                                        select
                                           (LlenarObjetoCertificadoTerminacionMediaSuperior(documento,
                                           (listcerCompetenciaDocumento.Where(x => x.docId == documento.docId).ToArray()),
                                           (listcerCompetenciaIEMS.Where(x => x.docId == documento.docId).ToArray())
                                           ), documento.docId)).ToList();

                    List<(BusinessModel.Models.Parcial.Dec, string)> listcertificadoParcialMS = (from documento in documentos
                                                                                                 where documento.docTipoId == "2"
                                                                                                 select
                                                                            (LlenarObjetoCertificadoParcialMediaSuperior(documento,
                                                                            (listcerUACDocumento.Where(x => x.docId == documento.docId).ToArray()))
                                                                            , documento.docId)).ToList();

                    solicitud.solArchivoEnvio = GenerarZip(listcertificadoTerminacionMS, listcertificadoParcialMS, solicitud.solId, userId);
                }

                int intento = 0;
                while (!resultado && intento < numeroIntentos)
                {

                    var resultadoCarga = new SEPSA().EnviarCarga(solicitud.solArchivoEnvio, solicitud.solId + ".zip", configuracionInstitucion.insUsuarioWS, configuracionInstitucion.insContrasenaWS);

                    //var resultadoCarga = new SEPSAAsync().EnviarCarga(solicitud.solArchivoEnvio, solicitud.solId + ".zip", configuracionInstitucion.insUsuarioWS, configuracionInstitucion.insContrasenaWS).Result;

                    if (resultadoCarga != null && !string.IsNullOrWhiteSpace(resultadoCarga.numeroLote))
                    {
                        solicitud.solFolioLoteSEP = resultadoCarga.numeroLote;
                        solicitud.solMensajeEnvio = resultadoCarga.mensaje;
                        solicitud.solFechaEnvio = DateTime.Now;
                        solicitud.estSolicitudId = 3;
                        resultado = true;
                    }
                    solicitud.solMensajeEnvio = resultadoCarga!=null?resultadoCarga.mensaje:"";
                    intento++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            new cerSolicitudDAL().AgregarActualizarSolicitud(solicitud);

            return resultado;
        }

        public Boolean ObtenerEstatusLote(cerConfiguracionInstitucion configuracionInstitucion, ref List<cerDocumento> documentos, ref cerSolicitud solicitud)
        {
            Boolean resultado = false;
            int numeroIntentos = 0;
            try
            {
                while (!resultado && numeroIntentos < 31)
                {
                    var estatusLote = new SEPSA().ConsultarEstatusCarga(solicitud.solFolioLoteSEP, configuracionInstitucion.insUsuarioWS, configuracionInstitucion.insContrasenaWS);

                    resultado = !String.IsNullOrEmpty(estatusLote.numeroLote) ? true : false;
                    solicitud.solMensajeResultado = estatusLote.mensaje;
                    solicitud.solFechaResultado = DateTime.Now;

                    //Decodificando el string del resultado en byte
                    byte[] archivoBase64 = Convert.FromBase64String(estatusLote.excelBase64);


                    solicitud.solArchivoResultado = archivoBase64; 


                    if (resultado)
                    {
                        if (ActualizarRegistrosSolicitudEstatus(ref solicitud))
                        {
                            solicitud.estSolicitudId = 4;
                        }
                        else
                        {
                            resultado = false;
                            Thread.Sleep(60000);
                        }
                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }

                    numeroIntentos++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            new cerSolicitudDAL().AgregarActualizarSolicitud(solicitud);
            return resultado;
        }

        public Boolean DescargarResultadoLote(cerConfiguracionInstitucion configuracionInstitucion, ref cerSolicitud solicitud)
        {
            Boolean resultado = false;
            int numeroIntentos = 0;
            try
            {
                while (!resultado && numeroIntentos < 31)
                {
                    var descarga = new SEPSA().DescargaResultado(solicitud.solFolioLoteSEP, configuracionInstitucion.insUsuarioWS, configuracionInstitucion.insContrasenaWS);
                    //Decodificando el string del resultado en byte
                    byte[] archivoBase64 = Convert.FromBase64String(descarga.certificadosBase64);

                    solicitud.solArchivoRetorno = archivoBase64;
                    solicitud.solMensajeRetorno = descarga.mensaje;
                    solicitud.solFechaRetorno = DateTime.Now;

                    if (descarga.certificadosBase64 != null)
                    {
                        solicitud.estSolicitudId = 5;
                        resultado = true;
                    }
                    else if (solicitud.solMensajeRetorno == "No hay certificados validos para el número de lote " + solicitud.solFolioLoteSEP + ".")
                    {
                        solicitud.estSolicitudId = 7;
                        resultado = true;
                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                    numeroIntentos++;
                }
            }
            catch (Exception ex)
            {
                solicitud.solMensajeRetorno = ex.Message;
                Console.WriteLine(ex);
            }
            new cerSolicitudDAL().AgregarActualizarSolicitud(solicitud);
            return resultado;
        }


        private Boolean ActualizarRegistrosSolicitud(ref cerSolicitud solicitud, string userId)
        {
            Boolean resultado = false;
            try
            {
                string rutaExcel = ""; rutaExcel = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), "Temporal", "Recursos", "Resultado");
                if (!Directory.Exists(rutaExcel))
                {
                    Directory.CreateDirectory(rutaExcel);
                }
                List<ResultExcelML> datosExcel = new List<ResultExcelML>();
                File.WriteAllBytes(Path.Combine(rutaExcel, "_" + solicitud.solId + ".zip"), solicitud.solArchivoRetorno);

                List<ResultXMLSelladoML> listXMLSellado = new List<ResultXMLSelladoML>();
                FileStream zipToOpen = new FileStream(Path.Combine(rutaExcel, "_" + solicitud.solId + ".zip"), FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(Dec));
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    var archivos = archive.Entries;

                    foreach (var entrie in archive.Entries)
                    {
                        Byte[] documento;
                        using (MemoryStream file = new MemoryStream())
                        {
                            XmlReader reader = XmlReader.Create(entrie.Open());
                            var archivo = entrie.Open();
                            archivo.CopyTo(file);
                            documento = file.ToArray();

                            var datos = (Dec)serializer.Deserialize(reader);

                            if (datos != null)
                            {
                                listXMLSellado.Add(new ResultXMLSelladoML()
                                {
                                    docId = Path.GetFileNameWithoutExtension(entrie.Name),
                                    bXML = documento,
                                    dFechaSsep = datos.Sep.fechaSep,
                                    sFolioDigital = datos.Sep.folioDigital,
                                    SNoCertificadoSep = datos.Sep.noCertificadoSep,
                                    sSelloDec = datos.Sep.selloDec,
                                    sSelloSep = datos.Sep.selloSep,
                                    sVersion = datos.Sep.version
                                });
                            }
                        }
                    }


                }

                DataTable dtXMLSellado = StoredProcedure.ConvertToDataTable(listXMLSellado);
                string sResult = StoredProcedure.Merged(dtXMLSellado, "typeCerDocumentoXMLSellado", "spMergedCerDocumentoXMLSellado", "typeCerDocumentoXMLSellado");

                if (sResult == "1")
                {
                    //Colocar condición de notificación a profesionista
                    solicitud.estSolicitudId = 6;
                    Task[] tasks = new Task[]{
                    Task.Factory.StartNew(() => SendMailDocumento(listXMLSellado, userId))};

                }
            }
            catch (Exception ex)
            {

            }

            new cerSolicitudDAL().AgregarActualizarSolicitud(solicitud);

            return resultado;

            }
            //private Boolean ActualizarRegistrosSolicitud(ref cerSolicitud solicitud, string userId)
            //{
            //    Boolean resultado = false;
            //    try
            //    {
            //        string rutaExcel = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), "Temporal", "Recursos", "Resultado");
            //        if (!Directory.Exists(rutaExcel))
            //        {
            //            Directory.CreateDirectory(rutaExcel);
            //        }
            //        List<ResultXMLSelladoML> listXMLSellado = new List<ResultXMLSelladoML>();
            //        File.WriteAllBytes(Path.Combine(rutaExcel, "_" + solicitud.solId + ".zip"), solicitud.solArchivoRetorno);

            //        FileStream zipToOpen = new FileStream(Path.Combine(rutaExcel, "_" + solicitud.solId + ".zip"), FileMode.Open);
            //        XmlSerializer serializer = new XmlSerializer(typeof(Dec));

            //        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
            //        {
            //            var archivos = archive.Entries;

            //            foreach (var entrie in archivos)
            //            {
            //                try
            //                {
            //                    Byte[] documento;
            //                    using (MemoryStream file = new MemoryStream())
            //                    {
            //                        var archivo = entrie.Open();
            //                        archivo.CopyTo(file);
            //                        documento = file.ToArray();

            //                        using (XmlReader reader = XmlReader.Create(new MemoryStream(documento)))
            //                        {
            //                            var datos = (Dec)serializer.Deserialize(reader);

            //                            if (datos != null)
            //                            {
            //                                listXMLSellado.Add(new ResultXMLSelladoML()
            //                                {
            //                                    docId = Path.GetFileNameWithoutExtension(entrie.Name),
            //                                    bXML = documento,
            //                                    dFechaSsep = datos.Sep.fechaSep,
            //                                    sFolioDigital = datos.Sep.folioDigital,
            //                                    SNoCertificadoSep = datos.Sep.noCertificadoSep,
            //                                    sSelloDec = datos.Sep.selloDec,
            //                                    sSelloSep = datos.Sep.selloSep,
            //                                    sVersion = datos.Sep.version
            //                                });
            //                            }
            //                        }
            //                    }
            //                }
            //                catch (Exception innerEx)
            //                {
            //                    // Log or handle the individual file error
            //                    Console.WriteLine($"Error procesando el archivo {entrie.Name}: {innerEx.Message}");
            //                    // Optionally log innerEx.StackTrace or other details
            //                }
            //            }
            //        }

            //        DataTable dtXMLSellado = StoredProcedure.ConvertToDataTable(listXMLSellado);
            //        string sResult = StoredProcedure.Merged(dtXMLSellado, "typeCerDocumentoXMLSellado", "spMergedCerDocumentoXMLSellado", "typeCerDocumentoXMLSellado");

            //        if (sResult == "1")
            //        {
            //            // Colocar condición de notificación a profesionista
            //            solicitud.estSolicitudId = 6;
            //            Task[] tasks = new Task[]
            //            {
            //        Task.Factory.StartNew(() => SendMailDocumento(listXMLSellado, userId))
            //            };
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // Log or handle the general error
            //        Console.WriteLine($"Error general: {ex.Message}");
            //        // Optionally log ex.StackTrace or other details
            //    }

            //    new cerSolicitudDAL().AgregarActualizarSolicitud(solicitud);

            //    return resultado;
            //}

            private Boolean ActualizarRegistrosSolicitudEstatus(ref cerSolicitud solicitud)
        {
            Boolean resultado = false;
            try
            {
                string rutaExcel = "";

                rutaExcel = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), "Temporal", "Recursos", "Resultado");
                if (!Directory.Exists(rutaExcel))
                {
                    Directory.CreateDirectory(rutaExcel);
                }
                List<ResultExcelEstatusML> datosExcel = new List<ResultExcelEstatusML>();
                File.WriteAllBytes(Path.Combine(rutaExcel, "_" + solicitud.solId + ".zip"), solicitud.solArchivoResultado);

                if (!Directory.Exists(Path.Combine(rutaExcel, "_" + solicitud.solId)))
                {
                    Directory.CreateDirectory(Path.Combine(rutaExcel, "_" + solicitud.solId));
                }
                else
                {
                    Directory.Delete(Path.Combine(rutaExcel, "_" + solicitud.solId), true);
                    Directory.CreateDirectory(Path.Combine(rutaExcel, "_" + solicitud.solId));
                }

                try
                {
                    ZipFile.ExtractToDirectory(Path.Combine(rutaExcel, "_" + solicitud.solId + ".zip"), Path.Combine(rutaExcel, "_" + solicitud.solId));
                }
                catch (Exception ex) { }

                var directorioResultado = new DirectoryInfo(Path.Combine(rutaExcel, "_" + solicitud.solId));

                foreach (var file in directorioResultado.GetFiles())
                {
                    rutaExcel = Path.Combine(rutaExcel, "_" + solicitud.solId, file.Name);
                }
                try
                {
                    using (FileStream stream = File.Open(rutaExcel, FileMode.Open, FileAccess.Read))
                    {
                        IExcelDataReader excelReader = rutaExcel.Contains(".xlsx") ? ExcelReaderFactory.CreateOpenXmlReader(stream) : ExcelReaderFactory.CreateBinaryReader(stream);

                        var result = excelReader.AsDataSet();
                        Boolean rowEncabezado = true;
                        ResultExcelEstatusML resultExcelML = new ResultExcelEstatusML();

                        while (excelReader.Read())
                        {
                            if (!rowEncabezado)
                            {
                                resultExcelML = new ResultExcelEstatusML();
                                resultExcelML.docId = Path.GetFileNameWithoutExtension(excelReader[0].ToString());
                                resultExcelML.estatus = excelReader[1].ToString().Equals("1") ? 4 : 5;
                                resultExcelML.mensaje = excelReader[2].ToString();

                                datosExcel.Add(resultExcelML);
                            }

                            rowEncabezado = false;
                        }
                    }

                    DataTable dtResultExcel = StoredProcedure.ConvertToDataTable(datosExcel);
                    string sResult = StoredProcedure.Merged(dtResultExcel, "typeResultExcel", "spMergedDocumentoEstatusResult", "typeResultExcel");
                    if (sResult == "1")
                    {
                        resultado = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            catch (Exception ex)
            {

            }
            return resultado;

        }

        private static Byte[] GenerarZip(List<(Dec certificadoMS, string docId)> listcertificadoTerminacionMS, List<(BusinessModel.Models.Parcial.Dec certificadoMS, string docId)> listcertificadoParcialMS, string idBloque, string userId)
        {
            string pathZIP = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), "Temporal", "Recursos", "Envio");
            if (!Directory.Exists(pathZIP))
            {
                Directory.CreateDirectory(pathZIP);
            }

            List<(string, byte[])> foliosXML = new List<(string, byte[])>();
            List<CerDocumentoML> listCerDocumentoML = new List<CerDocumentoML>();
            List<BitacoraML> listTitBitacora = new List<BitacoraML>();

            Directory.CreateDirectory(pathZIP + "//" + idBloque);

            if (File.Exists(Path.Combine(pathZIP, idBloque + ".zip")))
            {
                File.Delete(Path.Combine(pathZIP, idBloque + ".zip"));
            }

            ZipFile.CreateFromDirectory(pathZIP + "//" + idBloque, pathZIP + "//" + idBloque + ".zip", System.IO.Compression.CompressionLevel.Optimal, false);
            FileStream zipToOpen = new FileStream(pathZIP + "//" + idBloque + ".zip", FileMode.Open);
            Directory.Delete(pathZIP + "//" + idBloque);

            try
            {
                var serializer = new XmlSerializer(typeof(Dec));

                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (var certificadoMS in listcertificadoTerminacionMS)
                    {
                        var nameFile = certificadoMS.docId + ".xml";
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(nameFile);

                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            using (ExtendedStringWriter stringXML = new ExtendedStringWriter(Encoding.UTF8))
                            {
                                serializer.Serialize(stringXML, certificadoMS.certificadoMS);
                                writer.WriteLine(stringXML.ToString());
                                writer.Flush();
                                writer.Close();


                            }
                        }

                        //Pasar el archivo a byte[] para que se guarde en la base de datos en el campo xmlEnvioSEP
                        Byte[] documento;
                        using (MemoryStream file = new MemoryStream())
                        {
                            var archivo = readmeEntry.Open();
                            archivo.CopyTo(file);
                            documento = file.ToArray();

                            listCerDocumentoML.Add(new CerDocumentoML() { docId = certificadoMS.docId, sXML = documento });
                            listTitBitacora.Add(new BitacoraML() { bitId = Guid.NewGuid().ToString(), accId = "envioSEP", bitExitoso = true, bitDescripcion = "Envio Sep correcto docId: " + certificadoMS.docId + "", bitFecha = DateTime.Now, bitUsuario = userId });

                            foliosXML.Add((idBloque, documento));
                        }
                    }
                    var serializerParcial = new XmlSerializer(typeof(BusinessModel.Models.Parcial.Dec));
                    foreach (var certificadoMS in listcertificadoParcialMS)
                    {
                        var nameFile = certificadoMS.docId + ".xml";
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(nameFile);

                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            using (ExtendedStringWriter stringXML = new ExtendedStringWriter(Encoding.UTF8))
                            {
                                serializerParcial.Serialize(stringXML, certificadoMS.certificadoMS);
                                writer.WriteLine(stringXML.ToString());                                
                                writer.Flush();
                                writer.Close();
                            }
                            

                            //Pasar el archivo a byte[] para que se guarde en la base de datos en el campo xmlEnvioSEP
                            Byte[] documento;
                            using (MemoryStream file = new MemoryStream())
                            {
                                var archivo = readmeEntry.Open();
                                archivo.CopyTo(file);
                                documento = file.ToArray();

                                listCerDocumentoML.Add(new CerDocumentoML() { docId = certificadoMS.docId, sXML =  documento});
                                listTitBitacora.Add(new BitacoraML() { bitId = Guid.NewGuid().ToString(), accId = "envioSEP", bitExitoso = true, bitDescripcion = "Envio Sep correcto docId: " + certificadoMS.docId + "", bitFecha = DateTime.Now, bitUsuario = userId });

                                foliosXML.Add((idBloque, documento));
                            }
                        }
                    }
                }

                DataTable dtTitDocumento = StoredProcedure.ConvertToDataTable(listCerDocumentoML);
                StoredProcedure.Merged(dtTitDocumento, "typeCerDocumentoXML", "spMergedCerDocumentoXML", "typeDocumento");

                DataTable dttitBitacora = StoredProcedure.ConvertToDataTable(listTitBitacora);
                StoredProcedure.Merged(dttitBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

                Byte[] zip = File.ReadAllBytes(pathZIP + "//" + idBloque + ".zip");
                return zip;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return null;
            }

        }
        //Pendiente leer configuración y envión de correo....

        private static Dec LlenarObjetoCertificadoTerminacionMediaSuperior(cerDocumento documento, cerCompetenciaDocumento[] cerCompetenciaDocumentos, cerCompetenciasIEMS[] cerCompetenciasIEMs)
        {
            Dec oCertificadoMediaSuperior = new Dec();
            oCertificadoMediaSuperior.Acreditacion = new DecAcreditacion();
            oCertificadoMediaSuperior.Acreditacion.idNivelEstudios = documento.docAcreditacionidNivelEstudios;
            oCertificadoMediaSuperior.Acreditacion.nivelEstudios = documento.docAcreditacionNivelEstudios;
            oCertificadoMediaSuperior.Acreditacion.clavePlanEstudios = documento.docAcreditacionClavePlanEstudios;
            oCertificadoMediaSuperior.Acreditacion.creditosObtenidos = documento.docAcreditacionCreditosObtenidos;
            oCertificadoMediaSuperior.Acreditacion.idTipoEstudiosIEMS = documento.docAcreditacionIdTipoEstudiosIEMS;
            oCertificadoMediaSuperior.Acreditacion.tipoEstudiosIEMS = documento.docAcreditacionTipoEstudiosIEMS;
            oCertificadoMediaSuperior.Acreditacion.tipoPerfilLaboralEMS = documento.docAcreditacionTipoPerfilProfesionalIEMS;
            oCertificadoMediaSuperior.Acreditacion.nombreTipoPerfilLaboralEMS = documento.docAcreditacionNombreTipoPerfilProfesionalIEMS;
            oCertificadoMediaSuperior.Acreditacion.totalCreditos = documento.docAcreditacionTotalCreditos;
            oCertificadoMediaSuperior.Acreditacion.periodoInicio = documento.docAcreditacionPeriodoInicio;
            oCertificadoMediaSuperior.Acreditacion.periodoTermino = documento.docAcreditacionPeriodoTermino;
            oCertificadoMediaSuperior.Acreditacion.promedioAprovechamiento = documento.docAcreditacionPromedioAprovechamiento;
            oCertificadoMediaSuperior.Acreditacion.promedioAprovechamientoTexto = documento.docAcreditacionPromedioAprovechamientoTexto;

            oCertificadoMediaSuperior.Alumno = new DecAlumno();
            oCertificadoMediaSuperior.Alumno.curp = documento.docAlumnoCurp;
            oCertificadoMediaSuperior.Alumno.nombre = documento.docAlumnoNombre;
            oCertificadoMediaSuperior.Alumno.numeroControl = documento.docAlumnoNumeroControl;
            oCertificadoMediaSuperior.Alumno.primerApellido = documento.docAlumnoPrimerApellido;
            oCertificadoMediaSuperior.Alumno.segundoApellido = documento.docAlumnoSegundoApellido;

            oCertificadoMediaSuperior.UacsdeFt = (from l in cerCompetenciaDocumentos
                                                  orderby l.idPeriodo, l.orden
                                                  select new DecUacdeFt
                                                  {
                                                      calificacion = l.competenciaCalificacion,
                                                      creditos = l.competenciaCreditos,
                                                      dictamen = l.competenciaDictamen,
                                                      nombre = l.competenciaNombre,
                                                      totalHorasUAC = l.competenciaTotalHorasUAC
                                                  }).ToArray();

            oCertificadoMediaSuperior.PerfilEgresoEspecifico = new DecPerfilEgresoEspecifico();
            oCertificadoMediaSuperior.PerfilEgresoEspecifico.campoDisciplinar = documento.docEgresoCompetenciasCampoDisciplinar;

            if (cerCompetenciasIEMs.Count() == 0)
            {
                oCertificadoMediaSuperior.PerfilEgresoEspecifico.CompetenciasEspecificas = new DecPerfilEgresoEspecificoCompetenciasEspecificas[1];
                DecPerfilEgresoEspecificoCompetenciasEspecificas s = new DecPerfilEgresoEspecificoCompetenciasEspecificas();

                oCertificadoMediaSuperior.PerfilEgresoEspecifico.CompetenciasEspecificas[0] = s;
            }
            else
            {

                oCertificadoMediaSuperior.PerfilEgresoEspecifico.CompetenciasEspecificas = (from l in cerCompetenciasIEMs
                                                                                            orderby l.CompetenciasIEMSorden
                                                                                            select new DecPerfilEgresoEspecificoCompetenciasEspecificas
                                                                                            {
                                                                                                nombreCompetenciasLaborales = l.CompetenciasIEMSnombreProfesional
                                                                                            }).ToArray();

            }

            oCertificadoMediaSuperior.PerfilEgresoEspecifico.idCampoDisciplinar = documento.docEgresoCompetenciasIdCampoDisciplinar;
            oCertificadoMediaSuperior.PerfilEgresoEspecifico.nombrePerfilLaboralEMS = documento.docEgresoCompetenciasNombrePerfilProfesionalIEMS;
            oCertificadoMediaSuperior.PerfilEgresoEspecifico.tipoPerfilLaboralEMS = documento.docEgresoCompetenciasTipoPerfilProfesionalIEMS;
            oCertificadoMediaSuperior.PerfilEgresoEspecifico.trayecto = documento.docEgresoCompetenciasTrayecto;

            //Cambio a 3.0 que agrega a servicio firmante Oct2023
            oCertificadoMediaSuperior.ServicioFirmante = new DecServicioFirmante();
            oCertificadoMediaSuperior.ServicioFirmante.idEntidad = documento.docServicioIdIEMSidIEMS;
            //Termina cambio

            //Cambio a 3.1 que agrega tipo de Certificado Nov2023
            if (documento.docPlanId == "22")
            {
                oCertificadoMediaSuperior.tipoCertificado = DecTipoCertificado.Item12;
            }else if (documento.docPlanId == "33")
            {
                oCertificadoMediaSuperior.tipoCertificado = DecTipoCertificado.Item14;
            }
            else
            {
                oCertificadoMediaSuperior.tipoCertificado = DecTipoCertificado.Item1;
            }
            //Termina cambio


            oCertificadoMediaSuperior.FirmaResponsable = new DecFirmaResponsable();
            oCertificadoMediaSuperior.FirmaResponsable.cargo = documento.docFirmaResponsableCargo;
            oCertificadoMediaSuperior.FirmaResponsable.certificadoResponsable = documento.docFirmaResponsableCertificadoResponsable;
            oCertificadoMediaSuperior.FirmaResponsable.curp = documento.docFirmaResponsableCurp;
            oCertificadoMediaSuperior.FirmaResponsable.idCargo = documento.docFirmaResponsableIdCargo;
            oCertificadoMediaSuperior.FirmaResponsable.noCertificadoResponsable = documento.docfirmaResponsableNoCertificadoResponsable;
            oCertificadoMediaSuperior.FirmaResponsable.nombre = documento.docFirmaResponsableNombre;
            oCertificadoMediaSuperior.FirmaResponsable.primerApellido = documento.docFirmaResponsablePrimerApellido;
            oCertificadoMediaSuperior.FirmaResponsable.segundoApellido = documento.docFirmaResponsableSegundoApellido;
            oCertificadoMediaSuperior.FirmaResponsable.sello = documento.docFirmaResponsableSello;


            oCertificadoMediaSuperior.folioControl = documento.docDecFolioControl;
            oCertificadoMediaSuperior.PlantelOServicioEducativo = new DecPlantelOServicioEducativo();
            oCertificadoMediaSuperior.PlantelOServicioEducativo.cct = documento.docPlantelCCT;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.claveRvoe = documento.docPlantelClaveRvoe;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.entidadFederativa = documento.docPlantelEntidadFederativa;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.generoPlantel = documento.docPlantelGeneroPlantel;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.idEntidadFederativa = documento.docPlantelIdEntidadFederativa;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.idGeneroPlantel = documento.docplantelIdGeneroPlantel;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.idMunicipio = documento.docPlantelIdLocalidad;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.idSostenimiento = documento.docPlantelIdSostenimiento;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.idTipoPlantel = documento.docPlantelIdTipoPlantel;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.municipio = documento.docPlantelLocalidad;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.nombreNumeroPlantel = documento.docPlantelNombreNumeroPlantel;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.sostenimiento = documento.docPlantelSostenimiento;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.tipoPlantel = documento.docPlantelTipoPlantel;

            if (documento.docPlantelOServicioEducativoFechaInicioRVOE != null)
            {
                oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoeSpecified = true;
                oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoe = Convert.ToDateTime(documento.docPlantelOServicioEducativoFechaInicioRVOE);
            }
            else
            {
                oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoeSpecified = false;
            }

            oCertificadoMediaSuperior.Iems = new DecIems();
            oCertificadoMediaSuperior.Iems.idIEMS = documento.docServicioIdIEMSidIEMS;
            oCertificadoMediaSuperior.Iems.idOpcionEducativa = documento.docServicioIdOpcionEducativa;
            //oCertificadoMediaSuperior.Iems.nombreIEMS = documento.docServicioNombreIEMS;
            oCertificadoMediaSuperior.Iems.nombreDependencia = documento.docServicioNombreTipoDependencia;
            //oCertificadoMediaSuperior.Iems.opcionEducativa = documento.docServicioOpcionEducativa;
            oCertificadoMediaSuperior.Iems.nombreSEN = documento.docServicioIemsNombreSEN;
            oCertificadoMediaSuperior.Iems.nombreIEMSparticular = documento.docServicioIemsnombreIEMSparticular;
            oCertificadoMediaSuperior.Iems.institucionRVOE = documento.docServicioIemsInstitucionRVOE;
            oCertificadoMediaSuperior.Iems.idTipoIEMS = documento.docServicioIemsIdTipoIEMS;
            //oCertificadoMediaSuperior.Iems.tipoIEMS = documento.docServicioIemsTipoIEMS;


            return oCertificadoMediaSuperior;
        }

        private static BusinessModel.Models.Parcial.Dec LlenarObjetoCertificadoParcialMediaSuperior(cerDocumento documento, cerUACDocumento[] cerUACDocumentos)
        {
            BusinessModel.Models.Parcial.Dec oCertificadoMediaSuperior = new BusinessModel.Models.Parcial.Dec();
            oCertificadoMediaSuperior.Acreditacion = new BusinessModel.Models.Parcial.DecAcreditacion();
            oCertificadoMediaSuperior.Acreditacion.creditosObtenidos = documento.docAcreditacionCreditosObtenidos;
            oCertificadoMediaSuperior.Acreditacion.idTipoEstudiosIEMS = documento.docAcreditacionIdTipoEstudiosIEMS;
            oCertificadoMediaSuperior.Acreditacion.tipoEstudiosIEMS = documento.docAcreditacionTipoEstudiosIEMS;
            oCertificadoMediaSuperior.Acreditacion.totalCreditos = documento.docAcreditacionTotalCreditos;
            oCertificadoMediaSuperior.Acreditacion.periodoInicio = documento.docAcreditacionPeriodoInicio;
            oCertificadoMediaSuperior.Acreditacion.periodoTermino = documento.docAcreditacionPeriodoTermino;
            oCertificadoMediaSuperior.Acreditacion.promedioAprovechamiento = documento.docAcreditacionPromedioAprovechamiento;
            oCertificadoMediaSuperior.Acreditacion.promedioAprovechamientoTexto = documento.docAcreditacionPromedioAprovechamientoTexto;
            oCertificadoMediaSuperior.Acreditacion.tipoPerfilLaboralEMS = documento.docAcreditacionTipoPerfilProfesionalIEMS;
            oCertificadoMediaSuperior.Acreditacion.nombreTipoPerfilLaboralEMS = documento.docAcreditacionNombreTipoPerfilProfesionalIEMS;
            oCertificadoMediaSuperior.Acreditacion.idNivelEstudios = documento.docAcreditacionidNivelEstudios;
            oCertificadoMediaSuperior.Acreditacion.nivelEstudios = documento.docAcreditacionNivelEstudios;

            oCertificadoMediaSuperior.Alumno = new BusinessModel.Models.Parcial.DecAlumno();
            oCertificadoMediaSuperior.Alumno.curp = documento.docAlumnoCurp;
            oCertificadoMediaSuperior.Alumno.nombre = documento.docAlumnoNombre;
            oCertificadoMediaSuperior.Alumno.numeroControl = documento.docAlumnoNumeroControl;
            oCertificadoMediaSuperior.Alumno.primerApellido = documento.docAlumnoPrimerApellido;
            oCertificadoMediaSuperior.Alumno.segundoApellido = documento.docAlumnoSegundoApellido;

            oCertificadoMediaSuperior.Uacs = new Models.Parcial.DecUacs();
            oCertificadoMediaSuperior.Uacs.nombreTipoPeriodo = documento.docUACSnombreTipoPeriodo;
            oCertificadoMediaSuperior.Uacs.idTipoPeriodo = documento.docUACSidTipoPeriodo;

            oCertificadoMediaSuperior.Uacs.Uac = (from l in cerUACDocumentos
                                                  orderby l.idPeriodo,l.orden
                                                  select new Models.Parcial.DecUacsUac
                                                  {
                                                      calificacionUAC = l.UACcalificacion,
                                                      cct = l.UACcct,
                                                      creditosUAC = l.UACcreditos,
                                                      dictamenUAC = l.UACdictamen,
                                                      nombreUAC = l.UACnombre,
                                                      numeroPeriodoUAC = l.UACnumeroPeriodo,
                                                      periodoEscolarUAC = l.UACperiodoEscolar,
                                                      totalHorasUAC = l.UACtotalHorasUAC,
                                                      tipoUAC = l.UACtipo,
                                                      idTipoUAC = l.UACidTipo
                                                  }).ToArray();

            //Cambio a 2.0 que agrega a servicio firmante Oct2023
            oCertificadoMediaSuperior.ServicioFirmante = new BusinessModel.Models.Parcial.DecServicioFirmante();
            oCertificadoMediaSuperior.ServicioFirmante.idEntidad = documento.docServicioIdIEMSidIEMS;
            //Termina cambio

            //Cambio a 2.1 que agrega tipo de Certificado Nov2023
            if (documento.docPlanId == "22")
            {
                oCertificadoMediaSuperior.tipoCertificado = BusinessModel.Models.Parcial.DecTipoCertificado.Item13;
            }
            else if (documento.docPlanId == "33")
            {
                oCertificadoMediaSuperior.tipoCertificado = BusinessModel.Models.Parcial.DecTipoCertificado.Item15;
            }
            else
            {
                oCertificadoMediaSuperior.tipoCertificado = BusinessModel.Models.Parcial.DecTipoCertificado.Item2;
            }
            //Termina cambio

            oCertificadoMediaSuperior.FirmaResponsable = new BusinessModel.Models.Parcial.DecFirmaResponsable();
            oCertificadoMediaSuperior.FirmaResponsable.cargo = documento.docFirmaResponsableCargo;
            oCertificadoMediaSuperior.FirmaResponsable.certificadoResponsable = documento.docFirmaResponsableCertificadoResponsable;
            oCertificadoMediaSuperior.FirmaResponsable.curp = documento.docFirmaResponsableCurp;
            oCertificadoMediaSuperior.FirmaResponsable.idCargo = documento.docFirmaResponsableIdCargo;
            oCertificadoMediaSuperior.FirmaResponsable.noCertificadoResponsable = documento.docfirmaResponsableNoCertificadoResponsable;
            oCertificadoMediaSuperior.FirmaResponsable.nombre = documento.docFirmaResponsableNombre;
            oCertificadoMediaSuperior.FirmaResponsable.primerApellido = documento.docFirmaResponsablePrimerApellido;
            oCertificadoMediaSuperior.FirmaResponsable.segundoApellido = documento.docFirmaResponsableSegundoApellido;
            oCertificadoMediaSuperior.FirmaResponsable.sello = documento.docFirmaResponsableSello;


            oCertificadoMediaSuperior.folioControl = documento.docDecFolioControl;
            oCertificadoMediaSuperior.PlantelOServicioEducativo = new BusinessModel.Models.Parcial.DecPlantelOServicioEducativo();
            oCertificadoMediaSuperior.PlantelOServicioEducativo.cct = documento.docPlantelCCT;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.claveRvoe = documento.docPlantelClaveRvoe;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.entidadFederativa = documento.docPlantelEntidadFederativa;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.generoPlantel = documento.docPlantelGeneroPlantel;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.idEntidadFederativa = documento.docPlantelIdEntidadFederativa;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.idGeneroPlantel = documento.docplantelIdGeneroPlantel;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.idMunicipio = documento.docPlantelIdLocalidad;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.idSostenimiento = documento.docPlantelIdSostenimiento;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.idTipoPlantel = documento.docPlantelIdTipoPlantel;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.municipio = documento.docPlantelLocalidad;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.nombreNumeroPlantel = documento.docPlantelNombreNumeroPlantel;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.sostenimiento = documento.docPlantelSostenimiento;
            oCertificadoMediaSuperior.PlantelOServicioEducativo.tipoPlantel = documento.docPlantelTipoPlantel;
            if (documento.docPlantelOServicioEducativoFechaInicioRVOE != null)
            {
                oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoeSpecified = true;
                oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoe = Convert.ToDateTime(documento.docPlantelOServicioEducativoFechaInicioRVOE);
            }
            else
            {
                oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoeSpecified = false;
            }

            oCertificadoMediaSuperior.Iems = new BusinessModel.Models.Parcial.DecIems();
            oCertificadoMediaSuperior.Iems.idIEMS = documento.docServicioIdIEMSidIEMS;
            oCertificadoMediaSuperior.Iems.idOpcionEducativa = documento.docServicioIdOpcionEducativa;
            //oCertificadoMediaSuperior.Iems.nombreIEMS = documento.docServicioNombreIEMS;
            oCertificadoMediaSuperior.Iems.nombreDependencia = documento.docServicioNombreTipoDependencia;
            //oCertificadoMediaSuperior.Iems.opcionEducativa = documento.docServicioOpcionEducativa;
            oCertificadoMediaSuperior.Iems.institucionRVOE = documento.docServicioIemsInstitucionRVOE;
            oCertificadoMediaSuperior.Iems.nombreIEMSparticular = documento.docServicioIemsnombreIEMSparticular;
            //oCertificadoMediaSuperior.Iems.tipoIEMS = documento.docServicioIemsTipoIEMS;
            oCertificadoMediaSuperior.Iems.idTipoIEMS = documento.docServicioIemsIdTipoIEMS;
            oCertificadoMediaSuperior.Iems.nombreIEMS = documento.docServicioNombreIEMS;
            oCertificadoMediaSuperior.Iems.nombreSEN = documento.docServicioIemsNombreSEN;


            return oCertificadoMediaSuperior;
        }


        public cerDocumento GetDocumentoById(string idDocumento)
        {
            cerDocumento oCerDocumento = new CerDocumentoDAL().GetDatosDocumentoById(idDocumento);
            oCerDocumento.cerCompetenciaDocumento = new cerCompetenciaDocumentoDAL().GetCompetenciaByIdDocumento(idDocumento);
            oCerDocumento.cerCompetenciasIEMS = new cerCompetenciasIEMSDAL().GetCompetenciaIEMSByDocumento(idDocumento);
            return oCerDocumento;
        }

        public MonitoreoSEPML GetPlantillasByInstitucion(string sIdInstitucion, string sIdPlan, string sIdTipoDocumento, int pagina, int bloque)
        {
            MonitoreoSEPML oMonitoreoSEPML = new MonitoreoSEPML();
            int iRegInicio = ((pagina - 1) * bloque);
            oMonitoreoSEPML.listCerCatPlantillas = new cerCatPlantillaDAL().GetLstPlantillasByInstitucion(sIdInstitucion, sIdPlan, sIdTipoDocumento, iRegInicio, bloque);
            oMonitoreoSEPML.iTotalRegistrosPlantillas = new cerCatPlantillaDAL().GetCountPlantillasByInstitucion(sIdInstitucion, sIdPlan, sIdTipoDocumento);
            return oMonitoreoSEPML;
        }

        public string GetPlantillaByidDoc(string sIdDocumento)
        {
            return new CerDocumentoDAL().GetPlantillaByIdDocumento(sIdDocumento);
        }

        public bool AsignarPlantilla(string sDocId, string sIdPlantilla)
        {
            return new cerCatPlantillaDAL().AsignarPlantilla(sDocId, sIdPlantilla);
        }

        public CertificadoML getDatosDocumentoPortalDescarga(string sIdDocumento)
        {
            return new CerDocumentoDAL().getDatosDocumentoPortalDescarga(sIdDocumento);
        }

        public bool ActualizarCorreoByIdDocumento(string sIdDocumento, string sCorreo)
        {
            return new CerDocumentoDAL().ActualizarCorreoByIdDocumento(sIdDocumento, sCorreo);
        }

        public byte[] GetFileEnvioSEP(string sSolId)
        {
            return new cerSolicitudDAL().GetFileEnvioSEP(sSolId);
        }

        public byte[] GetFileResultadoSEP(string sSolId)
        {
            return new cerSolicitudDAL().GetFileResultadoSEP(sSolId);
        }

        public byte[] GetFileRetornoSEP(string sSolId)
        {
            return new cerSolicitudDAL().GetFileRetornoSEP(sSolId);
        }
        public MemoryStream GetXMLRetornoByDocId(string sDocId)
        {
            XmlDocument doc = new XmlDocument();
            string sXMLData = new CerDocumentoDAL().GetXMLRetornoByDocId(sDocId);
            doc.Load(new StringReader(sXMLData));

            XmlDeclaration xmldecl;
            xmldecl = doc.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";

            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmldecl, root);

            MemoryStream xmlStream = new MemoryStream();
            doc.Save(xmlStream);

            xmlStream.Flush();
            xmlStream.Position = 0;

            return xmlStream;
        }

        public MemoryStream GetXMLEnvioByDocId(string sDocId)
        {
            XmlDocument doc = new XmlDocument();
            string sXMLData = new CerDocumentoDAL().GetXMLEnvioByDocId(sDocId);
            doc.Load(new StringReader(sXMLData));

            // Create an XML declaration.
            XmlDeclaration xmldecl;
            xmldecl = doc.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";

            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmldecl, root);

            MemoryStream xmlStream = new MemoryStream();
            doc.Save(xmlStream);

            xmlStream.Flush();
            xmlStream.Position = 0;

            return xmlStream;
        }

        public String[] CancelarCertificadoSEP(string sIdDocumento, string usrLogin, string sObservaciones)
        {
            String[] result = new String[2];
            cerDocumento oCerDocumento = new CerDocumentoDAL().GetDatosDocumentoById(sIdDocumento);
            List<BitacoraML> listBitacora = new List<BitacoraML>();
            string sMensaje = "";
            if (oCerDocumento != null)
            {

                cerConfiguracionInstitucion configuracionInstitucion = new cerConfiguracionInstitucion();

                configuracionInstitucion = new CerConfiguracionInstitucionDAL().ObtenerRegistroConfiguracionPorId(oCerDocumento.insId);

                var vResultCancelar = new SEPSA().CancelaCertificadoElectronico(oCerDocumento.docSepFolioDigital, configuracionInstitucion.insUsuarioWS, configuracionInstitucion.insContrasenaWS);
                if (vResultCancelar != null)
                {
                    if (vResultCancelar.codigo == "0")
                    {
                        if (new CerDocumentoDAL().CancelarDocumentoSEPByDocId(oCerDocumento.docId, sObservaciones))
                        {
                            sMensaje = "Se canceló el documento en SEP correctamente.";
                        }
                        result[0] = "True";
                        result[1] = vResultCancelar.mensaje;

                    }
                    else
                    {
                        result[0] = "False";
                        result[1] = vResultCancelar.mensaje;
                    }
                }
                else
                {
                    result[0] = "False";
                    result[1] = "No se ha cancelado el certificado correctamente.";
                }
                listBitacora.Add(new BitacoraML()
                {
                    bitId = Guid.NewGuid().ToString(),
                    bitFecha = DateTime.Now,
                    bitUsuario = usrLogin,
                    bitDescripcion = "docId: " + oCerDocumento.docId + " correo: " + oCerDocumento.docCorreo + " mensaje: " + result[1] + " " + sMensaje,
                    bitExitoso = Convert.ToBoolean(result[0]),
                    accId = "cancelarcertificado"
                });
            }
            else
            {
                result[0] = "False";
                result[1] = "No se ha cancelado el certificado correctamente.";

                listBitacora.Add(new BitacoraML()
                {
                    bitId = Guid.NewGuid().ToString(),
                    bitFecha = DateTime.Now,
                    bitUsuario = usrLogin,
                    bitDescripcion = "No se ha cancelado el certificado correctamente.",
                    bitExitoso = Convert.ToBoolean(result[0]),
                    accId = "cancelarcertificado"
                });
            }




            DataTable dTBitacora = MetodosGenericosBL.ConvertToDataTable(listBitacora);
            StoredProcedure.Merged(dTBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

            return result;
        }

        public void SendMailDocumento(List<ResultXMLSelladoML> listXMLSellado, string userId)
        {
            bool bBandera = true;
            try
            {
                List<string> lstIdDocumento = listXMLSellado.Select(x => x.docId).ToList();
                string[] result = new string[2];
                List<BitacoraML> listBitacora = new List<BitacoraML>();
                foreach (var sIdDoc in lstIdDocumento)
                {
                    CertificadoML documentoML = new EnvioSEPBL().getDatosDocumentoPortalDescarga(sIdDoc);

                    if (!String.IsNullOrEmpty(documentoML.docCorreo))
                    {
                        result = new CertificadosBL().enviaCorreoProfesionista(documentoML, new CertificadoML.criteriosBusquedaCertificadosML(), documentoML.docAlumnoCurp);

                        if (result[0] == "True")
                        {
                            listBitacora.Add(new BitacoraML()
                            {
                                bitId = Guid.NewGuid().ToString(),
                                bitFecha = DateTime.Now,
                                bitUsuario = userId,
                                bitDescripcion = "Se envió correo electrónico de certificado a: " + documentoML.docCorreo + " con idDocumento" + documentoML.docId,
                                bitExitoso = true,
                                accId = "enviocorreo"
                            });
                        }
                        else
                        {
                            listBitacora.Add(new BitacoraML()
                            {
                                bitId = Guid.NewGuid().ToString(),
                                bitFecha = DateTime.Now,
                                bitUsuario = userId,
                                bitDescripcion = "No se encontró envió el correo electrónico correctamente a: " + documentoML.docCorreo + " con idDocumento" + documentoML.docId,
                                bitExitoso = false,
                                accId = "enviocorreo"
                            });
                        }
                    }
                    else
                    {
                        listBitacora.Add(new BitacoraML()
                        {
                            bitId = Guid.NewGuid().ToString(),
                            bitFecha = DateTime.Now,
                            bitUsuario = userId,
                            bitDescripcion = "No se encontró el correo electrónico correctamente con idDocumento" + documentoML.docId,
                            bitExitoso = false,
                            accId = "enviocorreo"
                        });
                    }
                }

                DataTable dttitBitacora = StoredProcedure.ConvertToDataTable(listBitacora);
                StoredProcedure.Merged(dttitBitacora, "typeBitacora", "spMergedtitBitacora", "typeBitacora");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public cerCatTipoDocumento GetTipoDocumentoById(string sIdTipoDocumento)
        {
            return new cerCatTipoDocumentoDAL().GetListCerCatTipoDocumentoById(sIdTipoDocumento);
        }
    }
}
