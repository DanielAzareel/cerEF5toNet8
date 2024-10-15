using BusinessModel.DataAccess;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using DocumentFormat.OpenXml.Office.CustomUI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessModel.Business
{
    public class PlantillaCertificadosBL
    {

        public static string GetIdPlan(string idPlantilla)
        {
            return new cerCatPlantillaDAL().GetIdPlantByIdPlantilla(idPlantilla);
        }
        public static List<KeyValuePair<string, string>> lstDatosCertificadoTerminacion(string docId, string idPlantilla, out string numeroControl, cerDocumento documento)
        {
            int i = 1;
            DateTime fechaDeHoy = DateTime.Today;
            //string fechaDeImpresion = new ConversionesBL().ConvertirFecha(fechaDeHoy.Day, fechaDeHoy.Month, fechaDeHoy.Year);
            string fechaDeImpresion = new ConversionesBL().ConvertirFecha(documento.docSepFechaSep.Value.Day, documento.docSepFechaSep.Value.Month, documento.docSepFechaSep.Value.Year);

            Dec Certificado = new CertificadosBL().ValidarDocumentoXML(documento.docXMLRetorno);
           // Dec Certificado = new CertificadosBL().ValidarDocumentoXML(documento.docXMLEnvio);
            numeroControl = Certificado.Alumno.numeroControl;
            var fecha = Certificado.Acreditacion.periodoInicio;
            string dia = fecha.Day.ToString();
            int mes = fecha.Month;
            string anio = fecha.Year.ToString();

            List<KeyValuePair<string, string>> xmlcertificado = new List<KeyValuePair<string, string>>();

            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsableNombre", Certificado.FirmaResponsable.nombre));
            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsablePrimerApellido", Certificado.FirmaResponsable.primerApellido));
            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsableSegundoApellido", Certificado.FirmaResponsable.segundoApellido));
            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsableCargo", Certificado.FirmaResponsable.cargo));
            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsableSello", Certificado.FirmaResponsable.sello));
            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsableNoCertificadoResponsable", Certificado.FirmaResponsable.noCertificadoResponsable));

            xmlcertificado.Add(new KeyValuePair<string, string>("IEMSNombreIEMS", Certificado.Iems.nombreIEMS));

            xmlcertificado.Add(new KeyValuePair<string, string>("PlantelCCT", Certificado.PlantelOServicioEducativo.cct));
            xmlcertificado.Add(new KeyValuePair<string, string>("PlantelEntidadFederativa", Certificado.PlantelOServicioEducativo.entidadFederativa));
            xmlcertificado.Add(new KeyValuePair<string, string>("PlantelMunicipio", Certificado.PlantelOServicioEducativo.municipio));

            xmlcertificado.Add(new KeyValuePair<string, string>("AlumnoCurp", Certificado.Alumno.curp));
            xmlcertificado.Add(new KeyValuePair<string, string>("AlumnoNombre", Certificado.Alumno.nombre));
            xmlcertificado.Add(new KeyValuePair<string, string>("AlumnoNumeroControl", Certificado.Alumno.numeroControl));
            xmlcertificado.Add(new KeyValuePair<string, string>("AlumnoPrimerApellido", Certificado.Alumno.primerApellido));
            xmlcertificado.Add(new KeyValuePair<string, string>("AlumnoSegundoApellido", Certificado.Alumno.segundoApellido));

            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionCreditosObtenidos", Certificado.Acreditacion.creditosObtenidos));
            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionPeriodoInicio", Certificado.Acreditacion.periodoInicio.Day.ToString().PadLeft(2, '0') + " de " + ConversionesBL.ConvertirMesFonetico(Certificado.Acreditacion.periodoInicio.Month) + " de " + Certificado.Acreditacion.periodoInicio.Year.ToString()));
            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionPeriodoTermino", Certificado.Acreditacion.periodoTermino.Day.ToString().PadLeft(2, '0') + " de " + ConversionesBL.ConvertirMesFonetico(Certificado.Acreditacion.periodoTermino.Month) + " de " + Certificado.Acreditacion.periodoTermino.Year.ToString()));
            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionPromedioAprovechamiento", Certificado.Acreditacion.promedioAprovechamiento));
            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionPromedioAprovechamientoTexto", char.ToUpper(Certificado.Acreditacion.promedioAprovechamientoTexto[0]) + Certificado.Acreditacion.promedioAprovechamientoTexto.Substring(1)));
            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionTotalCreditos", Certificado.Acreditacion.totalCreditos));
            xmlcertificado.Add(new KeyValuePair<string, string>("SepFolioDigital", documento.docSepFolioDigital));
            xmlcertificado.Add(new KeyValuePair<string, string>("SepFechaSep", documento.docSepFechaSep == null ? "" : documento.docSepFechaSep.Value.ToString("dd/MM/yyyy HH:mm:ss")));
            xmlcertificado.Add(new KeyValuePair<string, string>("SepSelloDec", documento.docSepSelloDec));
            xmlcertificado.Add(new KeyValuePair<string, string>("SepNoCertificadoSep", documento.docSepNoCertificadoSep));
            xmlcertificado.Add(new KeyValuePair<string, string>("SepSelloSep", documento.docSepSelloSep));
            
            //xmlcertificado.Add(new KeyValuePair<string, string>("formacionUAC", documento.));

            //Revisar 03-10-23
            //xmlcertificado.Add(new KeyValuePair<string, string>("PerfilEgresoEspecificoTrayecto", char.ToUpper(documento.docEgresoCompetenciasTrayecto[0]) + documento.docEgresoCompetenciasTrayecto.Substring(1)));

            string perfilEgreso = "";

            try
            {
                perfilEgreso = perfilEgreso + char.ToUpper(documento.docEgresoCompetenciasTrayecto[0]) + documento.docEgresoCompetenciasTrayecto.Substring(1);
            }
            catch
            {
                perfilEgreso = perfilEgreso + documento.docEgresoCompetenciasTrayecto;
            }
            
            xmlcertificado.Add(new KeyValuePair<string, string>("PerfilEgresoEspecificoTrayecto", perfilEgreso));

            xmlcertificado.Add(new KeyValuePair<string, string>("FechaImpresion", fechaDeImpresion));





            foreach (var competencia in Certificado.PerfilEgresoEspecifico.CompetenciasEspecificas)
            {
                xmlcertificado.Add(new KeyValuePair<string, string>("PerfilEgresoEspecificoNombreLaboral" + i, competencia.nombreCompetenciasLaborales));

                i++;
            }

            var especialidad = Certificado.UacsdeFt.ElementAt(0);
            xmlcertificado.Add(new KeyValuePair<string, string>("Calificacion", especialidad.calificacion));
            xmlcertificado.Add(new KeyValuePair<string, string>("Creditos", especialidad.creditos));
            xmlcertificado.Add(new KeyValuePair<string, string>("TotalHorasUAC", especialidad.totalHorasUAC));
            xmlcertificado.Add(new KeyValuePair<string, string>("SepFechaSepLetra", documento.docSepFechaSep == null ? "" :ConversionesBL.UpperFirstChar(new ConversionesBL().ConvertirFecha(documento.docSepFechaSep.Value.Day, documento.docSepFechaSep.Value.Month, documento.docSepFechaSep.Value.Year))));



            return (xmlcertificado);
        }
        public static byte[] ConcatenatePdfs(List<byte[]> documents)
        {
            using (var ms = new MemoryStream())
            {
                var outputDocument = new Document();
                var writer = new PdfCopy(outputDocument, ms);
                outputDocument.Open();

                foreach (var doc in documents)
                {
                    var reader = new PdfReader(doc);
                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        writer.AddPage(writer.GetImportedPage(reader, i));
                    }
                    writer.FreeReader(reader);
                    reader.Close();
                }

                writer.Close();
                outputDocument.Close();
                var allPagesContent = ms.GetBuffer();
                ms.Flush();

                return allPagesContent;
            }
        }
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public Stream GenerarCertificado(string docId, string idPlantilla, out string numeroControl)
        {
            //Se recupera el documento de cerDocumento 
            cerDocumento documento = new CertificadosBL().ObtenerDocumentoViewXML(docId);
            numeroControl = "NoGenerado";
            Stream archivo = null;
            if (string.IsNullOrWhiteSpace(idPlantilla))
            {
                idPlantilla = documento.planId;

            }

            //Verifica que exista la plantilla
            if (VerificarExistenciaPlantilla(idPlantilla))
            {
                string rutaPlantilla = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaPlantillas"], idPlantilla);
                var viewPlantilla = new cerCatPlantillaDAL().GetViewPlantillaById(idPlantilla);


                List<byte[]> hojas = new List<byte[]>();
                string usarURLSIGED = ConfigurationManager.AppSettings["usarURLSIGED"];
                string rutaQR = "";


                numeroControl = "";
                List<KeyValuePair<string, string>> formsValues = new List<KeyValuePair<string, string>>();
                Task<Stream>[] taskArray = new Task<Stream>[Convert.ToInt32(viewPlantilla.planNumHojas)];

                switch (documento.docTipoId)
                {
                    case "1":

                        formsValues = lstDatosCertificadoTerminacion(docId, idPlantilla, out numeroControl, documento);
                        break;
                    case "2":
                        formsValues = lstDatosCertificadoParcial(docId, idPlantilla, out numeroControl);

                        break;
                    default:
                        break;
                }

                formsValues.Add(new KeyValuePair<string, string>("VerificacionUrl", ConfigurationManager.AppSettings["urlPortalVerificacion"] +documento.docSepFolioDigital));
                formsValues.Add(new KeyValuePair<string, string>("VerificacionUrlSinFolio", ConfigurationManager.AppSettings["urlPortalVerificacion"]));
                formsValues.Add(new KeyValuePair<string, string>("", ""));
                if (usarURLSIGED == "1")
                {
                    rutaQR = ConfigurationManager.AppSettings["urlPortalVerificacion"] + documento.docSepFolioDigital;

                }
                else
                {

                    //rutaQR = ConfigurationManager.AppSettings["urlPortalVerificacionInterno"] + docId;
                    rutaQR = ConfigurationManager.AppSettings["urlPortalVerificacionInterno"] + documento.docSepFolioDigital;


                }


                if (viewPlantilla.planNumHojas > 1)
                {
                    for (int j = 1, i = 0; i < viewPlantilla.planNumHojas; i++, j++)
                    {
                        taskArray[i] = Task<Stream>.Factory.StartNew((Object obj) =>
                         {
                             ObjetoAux data = obj as ObjetoAux;

                             return PDF.GeneratePDFfromList(Path.Combine(rutaPlantilla, data.numeroHoja + ".docx"), formsValues, rutaQR);

                         },
                                                new ObjetoAux() { numeroHoja = j/*Se asigna el bloque al atriburo del objeto*/});
                    }


                    Task.WaitAll(taskArray);

                    foreach (var hoja in taskArray)
                    {

                        hojas.Add(ReadFully(hoja.Result));

                    }

                    var archivoConcatenado = ConcatenatePdfs(hojas);

                    archivo = new MemoryStream(archivoConcatenado);
                }
                else
                {
                    archivo = PDF.GeneratePDFfromList(Path.Combine(rutaPlantilla, 1 + ".docx"), formsValues, rutaQR);
                }


            }


            return archivo;
        }

        public class ObjetoAux
        {
            public int numeroHoja { set; get; }

        }
        public static List<KeyValuePair<string, string>> lstDatosCertificadoParcial(string docId, string idPlantilla, out string numeroControl)
        {
            int i = 1;
            DateTime fechaDeHoy = DateTime.Today;
            
            cerDocumento documento = new CertificadosBL().ObtenerDocumentoViewXML(docId);

            //string fechaDeImpresion = new ConversionesBL().ConvertirFecha(fechaDeHoy.Day, fechaDeHoy.Month, fechaDeHoy.Year);
            string fechaDeImpresion = new ConversionesBL().ConvertirFecha(documento.docSepFechaSep.Value.Day, documento.docSepFechaSep.Value.Month, documento.docSepFechaSep.Value.Year);


            BusinessModel.Models.Parcial.Dec Certificado = new CertificadosBL().DescerializarDecParcial(documento.docXMLRetorno);
            numeroControl = Certificado.Alumno.numeroControl;
            var fecha = Certificado.Acreditacion.periodoInicio;
            string dia = fecha.Day.ToString();
            int mes = fecha.Month;
            string anio = fecha.Year.ToString();
            List<KeyValuePair<string, string>> xmlcertificado = new List<KeyValuePair<string, string>>();

            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsableNombre", Certificado.FirmaResponsable.nombre));
            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsablePrimerApellido", Certificado.FirmaResponsable.primerApellido));
            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsableSegundoApellido", Certificado.FirmaResponsable.segundoApellido));
            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsableCargo", Certificado.FirmaResponsable.cargo));
            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsableSello", Certificado.FirmaResponsable.sello));
            xmlcertificado.Add(new KeyValuePair<string, string>("FirmaResponsableNoCertificadoResponsable", Certificado.FirmaResponsable.noCertificadoResponsable));

            xmlcertificado.Add(new KeyValuePair<string, string>("IEMSNombreIEMS", Certificado.Iems.nombreIEMS));
            xmlcertificado.Add(new KeyValuePair<string, string>("IEMSNombreSEN", Certificado.Iems.nombreSEN));
            xmlcertificado.Add(new KeyValuePair<string, string>("IEMSInstitucionRVOE", Certificado.Iems.institucionRVOE));
            xmlcertificado.Add(new KeyValuePair<string, string>("IEMSNombreParticular", Certificado.Iems.nombreIEMSparticular));
            xmlcertificado.Add(new KeyValuePair<string, string>("IEMSTipoIEMS", Certificado.Iems.tipoIEMS));

            xmlcertificado.Add(new KeyValuePair<string, string>("PlantelCCT", Certificado.PlantelOServicioEducativo.cct));
            xmlcertificado.Add(new KeyValuePair<string, string>("PlantelEntidadFederativa", Certificado.PlantelOServicioEducativo.entidadFederativa));
            xmlcertificado.Add(new KeyValuePair<string, string>("PlantelMunicipio", Certificado.PlantelOServicioEducativo.municipio));
            xmlcertificado.Add(new KeyValuePair<string, string>("PlantelFechaInicioRvoe", Certificado.PlantelOServicioEducativo.fechaInicioRvoe.ToString()));


            xmlcertificado.Add(new KeyValuePair<string, string>("AlumnoCurp", Certificado.Alumno.curp));
            xmlcertificado.Add(new KeyValuePair<string, string>("AlumnoNombre", Certificado.Alumno.nombre));
            xmlcertificado.Add(new KeyValuePair<string, string>("AlumnoNumeroControl", Certificado.Alumno.numeroControl));
            xmlcertificado.Add(new KeyValuePair<string, string>("AlumnoPrimerApellido", Certificado.Alumno.primerApellido));
            xmlcertificado.Add(new KeyValuePair<string, string>("AlumnoSegundoApellido", Certificado.Alumno.segundoApellido));

            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionCreditosObtenidos", Certificado.Acreditacion.creditosObtenidos));
            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionPeriodoInicio", Certificado.Acreditacion.periodoInicio.Day.ToString().PadLeft(2, '0') + " de " + ConversionesBL.ConvertirMesFonetico(Certificado.Acreditacion.periodoInicio.Month) + " de " + Certificado.Acreditacion.periodoInicio.Year.ToString()));
            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionPeriodoTermino", Certificado.Acreditacion.periodoTermino.Day.ToString().PadLeft(2, '0') + " de " + ConversionesBL.ConvertirMesFonetico(Certificado.Acreditacion.periodoTermino.Month) + " de " + Certificado.Acreditacion.periodoTermino.Year.ToString()));
            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionPromedioAprovechamiento", Certificado.Acreditacion.promedioAprovechamiento));
            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionPromedioAprovechamientoTexto", char.ToUpper(Certificado.Acreditacion.promedioAprovechamientoTexto[0]) + Certificado.Acreditacion.promedioAprovechamientoTexto.Substring(1)));
            xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionTotalCreditos", Certificado.Acreditacion.totalCreditos));


            xmlcertificado.Add(new KeyValuePair<string, string>("SepFolioDigital", documento.docSepFolioDigital));
            xmlcertificado.Add(new KeyValuePair<string, string>("SepFechaSep", documento.docSepFechaSep == null ? "" : documento.docSepFechaSep.Value.ToString("dd/MM/yyyy HH:mm:ss")));
            xmlcertificado.Add(new KeyValuePair<string, string>("SepSelloDec", documento.docSepSelloDec));
            xmlcertificado.Add(new KeyValuePair<string, string>("SepNoCertificadoSep", documento.docSepNoCertificadoSep));
            xmlcertificado.Add(new KeyValuePair<string, string>("SepSelloSep", documento.docSepSelloSep));

            xmlcertificado.Add(new KeyValuePair<string, string>("PerfilEgresoEspecificoTrayecto", char.ToUpper(documento.docEgresoCompetenciasTrayecto[0]) + documento.docEgresoCompetenciasTrayecto.Substring(1)));

            xmlcertificado.Add(new KeyValuePair<string, string>("FechaImpresion", fechaDeImpresion));


            //xmlcertificado.Add(new KeyValuePair<string, string>("PerfilProfIEMS", "en "+documento.docAcreditacionNombreTipoPerfilProfesionalIEMS));


            var j = 1;
            bool reproboMateria = false;
            foreach (var uac in Certificado.Uacs.Uac)
            {
                xmlcertificado.Add(new KeyValuePair<string, string>("PlantelCCTUAC" + j, uac.cct));

                xmlcertificado.Add(new KeyValuePair<string, string>("MatUAC" + j, uac.nombreUAC));

                xmlcertificado.Add(new KeyValuePair<string, string>("CalUAC" + j, uac.calificacionUAC));

                xmlcertificado.Add(new KeyValuePair<string, string>("HrsUAC" + j, uac.totalHorasUAC));

                xmlcertificado.Add(new KeyValuePair<string, string>("CreditosUAC" + j, uac.creditosUAC));
                xmlcertificado.Add(new KeyValuePair<string, string>("TipoUAC" + j, uac.tipoUAC));
                xmlcertificado.Add(new KeyValuePair<string, string>("AcreditacionUAC" + j, uac.periodoEscolarUAC));
                j++;


                if (uac.tipoUAC == "Profesional básica")
                {
                    if (double.TryParse(uac.calificacionUAC, out double calificacionNumerica))
                    {

                        // Verificar si el alumno reprobó
                        if (calificacionNumerica < 6)
                        {
                            reproboMateria = true;
                        }
                    }
                    else
                    {
                        //si el string uac.calificacionUAC que no se pudo convertir a número es igual a: NI,NP ó NAc reporbo materia tiene que ser igual a true
                        if (uac.calificacionUAC == "NI" || uac.calificacionUAC == "NP" || uac.calificacionUAC == "NAc")
                        {
                            reproboMateria = true;
                        }
                    }
                }
                

            }
            if (reproboMateria == false)
            {
                xmlcertificado.Add(new KeyValuePair<string, string>("PerfilProfIEMS", "en " + documento.docAcreditacionNombreTipoPerfilProfesionalIEMS));
            }
            xmlcertificado.Add(new KeyValuePair<string, string>("SepFechaSepLetra", documento.docSepFechaSep == null ? "" :ConversionesBL.UpperFirstChar(new ConversionesBL().ConvertirFecha(documento.docSepFechaSep.Value.Day, documento.docSepFechaSep.Value.Month, documento.docSepFechaSep.Value.Year))));
            return (xmlcertificado);
        }


        //Asegurar que la plantilla se encuentre en disco.
        public bool VerificarExistenciaPlantilla(string idPlantilla)
        {
            if (!Directory.Exists(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaPlantillas"], idPlantilla)))
            {
                var plantilla = new cerCatPlantillaDAL().GetPlantillasById(idPlantilla);

                try
                {
                    string rutaCarpetaContent = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings["rutaPlantillas"]);

                    if (!Directory.Exists(Path.Combine(rutaCarpetaContent, idPlantilla)))
                    {

                        try
                        {
                            File.WriteAllBytes(Path.Combine(rutaCarpetaContent, idPlantilla + ".zip"), plantilla.planArchivo);
                            ZipFile.ExtractToDirectory(Path.Combine(rutaCarpetaContent, idPlantilla + ".zip"), Path.Combine(Path.Combine(rutaCarpetaContent, idPlantilla)));

                            try
                            {
                                File.Delete(Path.Combine(rutaCarpetaContent, idPlantilla + ".zip"));
                                Directory.Delete(Path.Combine(rutaCarpetaContent, plantilla.planIdAnterior), true);
                            }
                            catch (Exception ex)
                            {

                            }
                            return true;
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
            return true;
        }


    }
}