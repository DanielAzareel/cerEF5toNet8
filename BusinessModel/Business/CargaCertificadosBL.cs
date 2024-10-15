using BusinessModel.DataAccess;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;
using ClosedXML.Excel;
using ServiciosWeb.ConsultaRenapoWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BusinessModel.Business
{
    public class CargaCertificadosBL
    {
        public List<string> GenerarColumnaLayout(string insId, string tipoDocumento)
        {

            List<string> dt = new List<string>();
            dt.Add("Identificador de plan de estudios");
            dt.Add("Identificador de área de especialidad");

            dt.Add("Nombre");
            dt.Add("Primer apellido");
            dt.Add("Segundo apellido");
            dt.Add("CURP");
            dt.Add("Número de control");
            dt.Add("Correo");


            dt.Add("Periodo de inicio de estudios");
            dt.Add("Periodo de término de estudios");
            dt.Add("Promedio aprovechamiento");

            var clvMateria = LstMateriasCarga(insId, tipoDocumento);
            string[] datos = { "CCT", "IT", "N", "C", "P", "NP" };
            foreach (var materia in clvMateria)
            {
                if (tipoDocumento == "2")
                {
                    foreach (string item in datos)
                    {
                        dt.Add(materia.idMateria + "_" + materia.idPlan + " " + item);
                    }
                }
                else
                {
                    dt.Add(materia.idMateria + "_" + materia.idPlan);
                }
            }

            return dt;
        }
        public bool validarEncabezado(CargaCertificadosML model, string insId)
        {

            bool encabezadoCorrecto = false;
            int ultimaColumna;
            List<string> encabezadoArchivo = new List<string>();
            try
            {
                using (var excelWorkbook = new XLWorkbook(model.certificadoIntegracion.InputStream))
                {

                    var nonEmptyDataRows = excelWorkbook.Worksheet(1).Rows(2, 2);
                    foreach (var dataRow in nonEmptyDataRows)
                    {
                        ultimaColumna = excelWorkbook.Worksheet(1).Row(2).LastCellUsed().WorksheetColumn().ColumnUsed().ColumnNumber();
                        for (int i = 1; i <= ultimaColumna; i++)
                        {
                            encabezadoArchivo.Add(dataRow.Cell(i).Value.ToString());
                        }
                    }
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            List<string> encabezadoSistema = GenerarColumnaLayout(insId, model.tipoDocumento);
            if (encabezadoArchivo.SequenceEqual(encabezadoSistema))
            {
                encabezadoCorrecto = true;
            }

            return encabezadoCorrecto;
        }

        public string[] validarArchivo(CargaCertificadosML model, string insId, string idUsuario)
        {
            string[] result = new string[2];
            if (validarEncabezado(model, insId))
            {
                if (model.tipoDocumento == "1")
                {
                    result = ValidarLayoutBLterminacion(model, insId, idUsuario);
                }
                else if (model.tipoDocumento == "2")
                {
                    result = ValidarLayoutBLparcial(model, insId, idUsuario);
                }
                else
                {
                    result[0] = "False";
                    result[1] = "Error.";
                }
            }
            else
            {
                result[0] = "False";
                result[1] = "El archivo no cuenta con la estructura correcta.";
            }
            return result;
        }
        public string[] ValidarLayoutBLterminacion(CargaCertificadosML model, string insId, string idUsuario)
        {
            CerDocumento_TemDAL obj = new CerDocumento_TemDAL();
            ConsultaRenapoClient SWRenapo = new ConsultaRenapoClient();
            List<MateriasArchivoCarga> lstClavesMaterias = new List<MateriasArchivoCarga>();
            List<string> encabezadoArchivo = new List<string>();
            bool impacto = false;
            string[] result = new string[2];
            string[] AlumnoRENAPO;
            string resultadoRENAPO = "";
            string idCelda = "";
            string calificacion = "";
            int lasMateriasTerminarEnlaColumna;
            bool encuentraMateria;
            var lstNodosConfigurados = new cerParametroValorDAL().GetLstParametrosByIdDocumento(insId, "1");
            var lstNodosPlanes = new CerCatPlanDAL().GetLstPlanes();
            var lstPlantillas = new cerCatPlantillaDAL().GetLstPlantillas(insId, "1");
            var lstAreas = new cerCargaDAL().GetLstAreasConocimiento();
            cerCarga carga = new cerCarga();
            carga.carId = Guid.NewGuid().ToString();
            List<cerDocumento_Tem> listadoDocumentos = new List<cerDocumento_Tem>();

            List<cerCatMateria> cerCatMaterias = new cerCatMateriaDAL().GetLstMaterias();

            List<(string insId, string idPlan, string idAreaConocimiento, string creditos)> materiasEspecialidad = new cerCatMateriaDAL().MateriasAreaEspecializacionByInsId(insId);

            var idMaterias = new cerRelPlanPeriodoAreaConocimientoMateriaDAL().GetLstMateriasByTipoDocumento(insId, model.tipoDocumento);
            var clvMateria = idMaterias.Select(x => new { x.idMateria, x.idPlan }).Distinct().ToList();

            using (var excelWorkbook = new XLWorkbook(model.certificadoIntegracion.InputStream))
            {
                var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();

                foreach (var dataRow in nonEmptyDataRows)
                {
                    List<MateriasArchivoCarga> lstCompetencias = new List<MateriasArchivoCarga>();

                    cerDocumento_Tem documento = new cerDocumento_Tem();
                    if (dataRow.RowNumber() == 2)
                    {

                        lasMateriasTerminarEnlaColumna = excelWorkbook.Worksheet(1).Row(2).LastCellUsed().WorksheetColumn().ColumnUsed().ColumnNumber();
                        for (int i = 12; i <= lasMateriasTerminarEnlaColumna; i++)
                        {
                            lstClavesMaterias.Add(new MateriasArchivoCarga
                            {
                                idCelda = i.ToString(),
                                idMateria = dataRow.Cell(i).Value.ToString()
                            });

                        }

                    }

                    if (dataRow.RowNumber() > 2)
                    {
                        AlumnoRENAPO = null;
                        resultadoRENAPO = "";
                        documento.insId = insId;
                        documento.docId = Guid.NewGuid().ToString();
                        documento.carId = carga.carId;
                        documento.fila = dataRow.RowNumber();
                        documento.docPlanId = dataRow.Cell(1).Value.ToString().Trim() ?? null;//Archivo:idPlan
                        documento.idAreaConocimiento = dataRow.Cell(2).Value.ToString().Trim() ?? null;//Archivo:idArea
                        documento.docCorreo = dataRow.Cell(8).Value.ToString().Trim() ?? null;//Archivo:Correo electrónico


                        //Nodo DEC
                        documento.docDecVersion = lstNodosConfigurados.Where(x => x.parId == "decVersion").Select(x => x.parValor).FirstOrDefault();

                        //Buscar Tipo de Certificado de Acuerdo al campo Plan
                        documento.docDecTipocertificado = "1";


                        documento.docDecFolioControl = documento.docId;
                        //Nodo Servicio
                        documento.docServicioNombreTipoDependencia = lstNodosConfigurados.Where(x => x.parId == "servicioNombreTipoDependencia").Select(x => x.parValor).FirstOrDefault();
                        documento.docServicioIdIEMS = lstNodosConfigurados.Where(x => x.parId == "servicioIdIEMS").Select(x => x.parValor).FirstOrDefault();
                                  //nombreIEMS
                        documento.docServicioNombreIEMS = lstNodosConfigurados.Where(x => x.parId == "servicioNombreIEMS").Select(x => x.parValor).FirstOrDefault();
                        documento.docServicioIdOpcionEducativa = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.servicioIdOpcionEducativa).FirstOrDefault();
                                  //opcionEducativa
                        documento.docServicioOpcionEducativa = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.servicioOpcionEducativa).FirstOrDefault();
                        //Nodo Plantel
                        documento.docPlantelIdTipoPlantel = Convert.ToInt32(lstNodosConfigurados.Where(x => x.parId == "plantelIdTipoPlantel").Select(x => x.parValor).FirstOrDefault());
                        documento.docPlantelTipoPlantel = lstNodosConfigurados.Where(x => x.parId == "plantelTipoPlantel").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelNombreNumeroPlantel = lstNodosConfigurados.Where(x => x.parId == "plantelNombreNumeroPlantel").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelIdLocalidad = lstNodosConfigurados.Where(x => x.parId == "plantelIdLocalidad").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelLocalidad = lstNodosConfigurados.Where(x => x.parId == "plantelLocalidad").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelIdEntidadFederativa = lstNodosConfigurados.Where(x => x.parId == "plantelIdEntidadFederativa").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelEntidadFederativa = lstNodosConfigurados.Where(x => x.parId == "plantelEntidadFederativa").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelCCT = lstNodosConfigurados.Where(x => x.parId == "plantelCCT").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelClaveRvoe = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.planPlantelClaveRvoe).FirstOrDefault();//tomar de la tabla plan
                        documento.docplantelIdGeneroPlantel = lstNodosConfigurados.Where(x => x.parId == "plantelIdGeneroPlantel").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelGeneroPlantel = lstNodosConfigurados.Where(x => x.parId == "plantelGeneroPlantel").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelIdSostenimiento = lstNodosConfigurados.Where(x => x.parId == "plantelIdSostenimiento").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelSostenimiento = lstNodosConfigurados.Where(x => x.parId == "plantelSostenimiento").Select(x => x.parValor).FirstOrDefault();
                        //Nodo Alumno
                        documento.docAlumnoNombre = dataRow.Cell(3).Value.ToString().Trim() ?? null;//Archivo:Nombre 
                        documento.docAlumnoPrimerApellido = dataRow.Cell(4).Value.ToString().Trim() ?? null;//Archivo:Primer apellido
                        documento.docAlumnoSegundoApellido = string.IsNullOrWhiteSpace(dataRow.Cell(5).Value.ToString()) ? null : dataRow.Cell(5).Value.ToString().Trim();//Archivo:Segundo apellido
                        documento.docAlumnoCurp = dataRow.Cell(6).Value.ToString().Trim().ToUpper() ?? null;//Archivo:Curp
                        documento.docAlumnoNumeroControl = dataRow.Cell(7).Value.ToString().Trim() ?? null;//Archivo:Archivo:Número de control
                        try
                        {
                            resultadoRENAPO = SWRenapo.consultarPorCurp(documento.docAlumnoCurp);
                            AlumnoRENAPO = resultadoRENAPO.Split('|');
                            if (AlumnoRENAPO.Count() > 1)
                            {
                                documento.docAlumnoCurpRENAPO = AlumnoRENAPO[0].Trim() ?? null;
                                documento.docAlumnoPrimerApellidoRENAPO = AlumnoRENAPO[1].Trim() ?? null;
                                documento.docAlumnoSegundoApellidoRENAPO = AlumnoRENAPO[2].Trim() ?? null;
                                documento.docAlumnoNombreRENAPO = AlumnoRENAPO[3].Trim() ?? null;
                            }

                        }
                        catch (Exception e)
                        {

                            Console.Write(e.Message);
                        }


                        //Nodo Acreditacion
                        try
                        {
                            documento.docAcreditacionPeriodoInicio = Convert.ToDateTime(dataRow.Cell(9).Value.ToString().Trim()).ToString("dd-MM-yyyy HH:mm:ss") ?? null;//Archivo:Archivo:Periodo inicio

                        }
                        catch (Exception e)
                        {

                            documento.docAcreditacionPeriodoInicio = dataRow.Cell(9).Value.ToString();
                        }
                        try
                        {
                            documento.docAcreditacionPeriodoTermino = Convert.ToDateTime(dataRow.Cell(10).Value.ToString().Trim()).ToString("dd-MM-yyyy HH:mm:ss") ?? null;//Archivo:Periodo término

                        }
                        catch (Exception e)
                        {

                            documento.docAcreditacionPeriodoTermino = dataRow.Cell(10).Value.ToString();
                        }

                        documento.docAcreditacionPromedioAprovechamiento = dataRow.Cell(11).Value.ToString().Trim() ?? null;//Archivo:Promedio aprovechamiento

                        documento.docAcreditacionPromedioAprovechamiento = documento.docAcreditacionPromedioAprovechamiento.Contains(".") ? documento.docAcreditacionPromedioAprovechamiento : documento.docAcreditacionPromedioAprovechamiento + ".0";


                        documento.docAcreditacionIdTipoEstudiosIEMS = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.acreditacionIdTipoEstudiosIEMS).FirstOrDefault();//tomar de la tabla plan;
                        
                        documento.docAcreditacionTipoEstudiosIEMS = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.acreditacionTipoEstudiosIEMS).FirstOrDefault();//tomar de la tabla plan
                        documento.docAcreditacionTipoPerfilProfesionalIEMS = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.acreditacionTipoPerfilProfesionalIEMS).FirstOrDefault();//tomar de la tabla plan
                        documento.docAcreditacionClavePlanEstudios = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.acreditacionClavePlanEstudios).FirstOrDefault();//tomar de la tabla plan
                        documento.docAcreditacionTotalCreditos = lstAreas.Where(x => x.idAreaConocimiento == documento.idAreaConocimiento).Select(x => x.areaTotalCreditos).FirstOrDefault();
                        documento.docAcreditacionCreditosObtenidos = documento.docAcreditacionTotalCreditos;
                        documento.docAcreditacionNombreTipoPerfilProfesionalIEMS = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.acreditacionNombreTipoPerfilProfesionalIEMS).FirstOrDefault();


                        documento.docAcreditacionPromedioAprovechamientoTexto = MetodosGenericosBL.FirstCharToUpper(new ConversionesBL().ConvertirCalificacion(documento.docAcreditacionPromedioAprovechamiento));

                        documento.docAcreditacionPromedioAprovechamientoTexto = string.IsNullOrWhiteSpace(documento.docAcreditacionPromedioAprovechamientoTexto) ? documento.docAcreditacionPromedioAprovechamiento : documento.docAcreditacionPromedioAprovechamientoTexto;
                        //Competencias

                        var materiasArea = (from c in idMaterias where c.idAreaConocimiento == documento.idAreaConocimiento select c).ToList();

                        try
                        {
                            idCelda = "";
                            calificacion = "";
                            foreach (var materia in clvMateria)
                            {

                                idCelda = (from c in lstClavesMaterias where c.idMateria == materia.idMateria + "_" + materia.idPlan select c.idCelda).SingleOrDefault();

                                try
                                {
                                    calificacion = dataRow.Cell(Convert.ToInt32(idCelda)).Value.ToString() ?? "";
                                    Convert.ToDouble(calificacion);
                                    calificacion = calificacion.Length == 1 ? calificacion + ".0" : calificacion;
                                }
                                catch (Exception ex)
                                {

                                    calificacion = dataRow.Cell(idCelda).Value.ToString() != "" ? dataRow.Cell(idCelda).Value.ToString() : idCelda == null ? "" : calificacion;
                                }

                                encuentraMateria = (from c in materiasArea where c.idMateria == materia.idMateria && c.idPlan == materia.idPlan select c).Any();

                                if (!String.IsNullOrWhiteSpace(calificacion) || encuentraMateria == true)
                                {
                                    lstCompetencias.Add(new MateriasArchivoCarga
                                    {
                                        idCelda = idCelda,
                                        idMateria = materia.idMateria,
                                        calificacion = calificacion,
                                        idPlan = materia.idPlan
                                    });
                                }


                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }




                        documento.docCompetencias = CompertenciasxmlPlan(lstCompetencias, documento.docPlanId, documento.idAreaConocimiento, documento.docDecTipocertificado).ToString();
                        //Nodo EgresoCompetencias
                        documento.docEgresoCompetenciasTrayecto = lstAreas.Where(x => x.idAreaConocimiento == documento.idAreaConocimiento).Select(x => x.areaEgresoCompetenciasTrayecto).FirstOrDefault();
                        documento.docEgresoCompetenciasIdCampoDisciplinar = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.planEgresoCompetenciasIdCampoDisciplinar).FirstOrDefault();
                        documento.docEgresoCompetenciasCampoDisciplinar = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.planEgresoCompetenciasCampoDisciplinar).FirstOrDefault();
                        documento.docEgresoCompetenciasTipoPerfilProfesionalIEMS = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.planEgresoCompetenciasTipoPerfilProfesionalIEMS).FirstOrDefault();
                        documento.docEgresoCompetenciasNombrePerfilProfesionalIEMS = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.planEgresoCompetenciasNombrePerfilProfesionalIEMS).FirstOrDefault();
                        //Nodo CompetenciasIEMS
                        documento.docCompetenciasIEMS = CompertenciasIEMSxml(documento.docPlanId).ToString();
                        documento.planId = lstPlantillas.Where(x => x.idPlan == documento.docPlanId && x.planEstatus == true).Select(x => x.planId).FirstOrDefault() ?? null;
                        documento.docObservaciones = "";
                        documento.docInsertado = false;
                        documento.docServicioIemsNombreSEN = lstNodosConfigurados.Where(x => x.parId == "servicioIemsNombreSEN").Select(x => x.parValor).FirstOrDefault();
                        documento.docServicioIemsnombreIEMSparticular = lstNodosConfigurados.Where(x => x.parId == "servicioIemsnombreIEMSparticular").Select(x => x.parValor).FirstOrDefault();
                        documento.docServicioIemsInstitucionRVOE = lstNodosConfigurados.Where(x => x.parId == "servicioIemsInstitucionRVOE").Select(x => x.parValor).FirstOrDefault();
                        documento.docServicioIemsIdTipoIEMS = lstNodosConfigurados.Where(x => x.parId == "servicioIemsIdTipoIEMS").Select(x => x.parValor).FirstOrDefault();
                                  //tipoEstudiosIEMS
                        documento.docServicioIemsTipoIEMS = lstNodosConfigurados.Where(x => x.parId == "servicioIemsTipoIEMS").Select(x => x.parValor).FirstOrDefault();
                        if (lstNodosConfigurados.Where(x => x.parId == "plantelOServicioEducativoFechaInicioRVOE").Select(x => x.parValor).FirstOrDefault() != null)
                        {
                            try
                            {
                                documento.docPlantelOServicioEducativoFechaInicioRVOE = DateTime.Parse(lstNodosConfigurados.Where(x => x.parId == "plantelOServicioEducativoFechaInicioRVOE").Select(x => x.parValor).FirstOrDefault());

                            }
                            catch (Exception)
                            {

                                documento.docPlantelOServicioEducativoFechaInicioRVOE = null;
                            }
                        }
                        documento.docAcreditacionidNivelEstudios = lstNodosConfigurados.Where(x => x.parId == "acreditacionidNivelEstudios").Select(x => x.parValor).FirstOrDefault();
                        documento.docAcreditacionNivelEstudios = lstNodosConfigurados.Where(x => x.parId == "acreditacionNivelEstudios").Select(x => x.parValor).FirstOrDefault();

                        listadoDocumentos.Add(documento);
                    }

                }
                if (listadoDocumentos.Count() != 0)
                {
                    impacto = new CerDocumento_TemDAL().AddRegistrosArchivo(listadoDocumentos);
                    if (impacto)
                    {
                        if (obj.ValidarArchivo(carga.carId, idUsuario, insId, "SpValidarCargaCertificadosTerminacion") == 1)
                        {
                            result[0] = "True";
                            result[1] = carga.carId.ToString();
                        }
                        else
                        {
                            result[0] = "False";
                            result[1] = "La carga, no se puedo realizar intenta nuevamente.";
                        }

                    }
                    else
                    {
                        result[0] = "False";
                        result[1] = "La carga, no se puedo realizar intenta nuevamente.";
                    }
                }
                else
                {
                    result[0] = "False";
                    result[1] = "El archivo no contiene registros válidos.";
                }
            }
            return result;

        }
        public string[] ValidarLayout(CargaCertificadosML model)
        {
            string[] result = new string[2];

            result[0] = "1";
            result[1] = "0";

            return result;
        }
        public XElement CompertenciasxmlPlan(List<MateriasArchivoCarga> lstMateriasCarga, string idPlan, string idAreaConocimiento, string docDecTipocertificado)
        {
            var lstMateriasMalla = new cerCatMateriaDAL().GetLstMaterias(idPlan);
            var lstMateriasOrden = new cerRelPlanPeriodoAreaConocimientoMateriaDAL().GetLstMateriasByPlan(idPlan, docDecTipocertificado);


            List<CompetenciasArchivoML> lstMaterias = (from c in lstMateriasMalla
                                                       join d in lstMateriasCarga
                            on c.idMateria equals d.idMateria
                                                       select new CompetenciasArchivoML
                                                       {
                                                           idMateria = c.idMateria,
                                                           idPeriodo = (from rel in lstMateriasOrden where rel.idPlan == idPlan && rel.idAreaConocimiento == idAreaConocimiento && rel.idMateria == c.idMateria select rel.idPeriodo).FirstOrDefault(),
                                                           idPlan = d.idPlan,
                                                           materiaDescripcion = c.materiaDescripcion,
                                                           materiahoras = c.materiahoras,
                                                           materiaCreditos = c.materiaCreditos,
                                                           calificacion = d.calificacion,
                                                           orden = (from rel in lstMateriasOrden where rel.idPlan == idPlan && rel.idAreaConocimiento == idAreaConocimiento && rel.idMateria == c.idMateria select rel.orden).FirstOrDefault()

                                                       }
                                                   ).ToList();




            List<XElement> lstCompetenciasXML = new List<XElement>();
            XElement xmlCompetencias = new XElement("Competencias", from c in lstMaterias
                                                                    select new
                                       XElement("competencia", new XAttribute("idMateria", c.idMateria),
                                       new XAttribute("idPeriodo", c.idPeriodo),
                                       new XAttribute("idPlan", c.idPlan),
                                       new XAttribute("nombre", c.materiaDescripcion),
                                       new XAttribute("calificacion", c.calificacion),
                                       new XAttribute("totalHorasUAC", c.materiahoras),
                                       new XAttribute("creditos", c.materiaCreditos == null ? null : c.materiaCreditos),
                                       new XAttribute("orden", c.orden))

                );

            return xmlCompetencias;

        }
        public XElement CompertenciasIEMSxml(string idPlan)
        {
            var lstCompetencias = new cerCompetenciasPlanDAL().GetLstCompetenciasIEMS(idPlan);

            XElement xmlCompetenciasIEMS = new XElement("CompetenciaIEMS", from c in lstCompetencias
                                                                           select new
                                            XElement("competencia", new XAttribute("competenciaOrden", c.competenciaOrden), new XAttribute("competenciaDescripcion", c.competenciaDescripcion)));


            return xmlCompetenciasIEMS;

        }
        public int AcreditacionTotalCreditosOptenidos(string insId, string idPlan, string idAreaConocimiento, List<(string insId, string idPlan, string idAreaConocimiento, string creditos)> materias)
        {

            int creditos = 0;
            foreach (var materia in from c in materias where c.idPlan == idPlan && c.idAreaConocimiento == idAreaConocimiento select c)
            {
                try
                {
                    creditos += Convert.ToInt32(Convert.ToDouble(materia.creditos));

                }
                catch (Exception ex)
                {

                }


            }

            return creditos;

        }
        public List<CerDocumento_TemML> GetLstDocumentosValidacion(FiltrosConsultaCarga filtroBusqueda, out int totalR, out int totalRconErrores, out int totalRconObservaciones, out int RegistrosTotales, out int RegistrosSinErrores, int pagina, int bloque)
        {

            List<CerDocumento_TemML> lstDocumentos = new List<CerDocumento_TemML>();
            List<cerDocumento_Tem> lstDocumentosData = new CerDocumento_TemDAL().lstDocumentosAcargar(filtroBusqueda, out totalR, out totalRconErrores, out totalRconObservaciones, out RegistrosTotales, out RegistrosSinErrores, pagina, bloque);
            lstDocumentos = (from c in lstDocumentosData
                             select new CerDocumento_TemML
                             {
                                 fila = c.fila,
                                 docId = c.docId,
                                 carId = c.carId,
                                 docPlanId = c.docPlanId,
                                 idAreaConocimiento = c.idAreaConocimiento,
                                 docDecVersion = c.docDecVersion,
                                 docDecTipocertificado = c.docDecTipocertificado,
                                 docDecFolioControl = c.docDecFolioControl,
                                 docFirmaResponsableNombre = c.docFirmaResponsableNombre,
                                 docFirmaResponsablePrimerApellido = c.docFirmaResponsablePrimerApellido,
                                 docFirmaResponsableSegundoApellido = c.docFirmaResponsableSegundoApellido,
                                 docFirmaResponsableIdCargo = c.docFirmaResponsableIdCargo,
                                 docFirmaResponsableCargo = c.docFirmaResponsableCargo,
                                 docFirmaResponsableCertificadoResponsable = c.docFirmaResponsableCertificadoResponsable,
                                 docfirmaResponsableNoCertificadoResponsable = c.docfirmaResponsableNoCertificadoResponsable,
                                 docServicioNombreTipoDependencia = c.docServicioNombreTipoDependencia,
                                 docServicioIdIEMS = c.docServicioIdIEMS,
                                 docServicioNombreIEMS = c.docServicioNombreIEMS,
                                 docServicioIdOpcionEducativa = c.docServicioIdOpcionEducativa,
                                 docServicioOpcionEducativa = c.docServicioOpcionEducativa,
                                 docPlantelIdTipoPlantel = c.docPlantelIdTipoPlantel,
                                 docPlantelTipoPlantel = c.docPlantelTipoPlantel,
                                 docPlantelNombreNumeroPlantel = c.docPlantelNombreNumeroPlantel,
                                 docPlantelIdLocalidad = c.docPlantelIdLocalidad,
                                 docPlantelLocalidad = c.docPlantelLocalidad,
                                 docPlantelIdEntidadFederativa = c.docPlantelIdEntidadFederativa,
                                 docPlantelEntidadFederativa = c.docPlantelEntidadFederativa,
                                 docPlantelCCT = c.docPlantelCCT,
                                 docPlantelClaveRvoe = c.docPlantelClaveRvoe,
                                 docplantelIdGeneroPlantel = c.docplantelIdGeneroPlantel,
                                 docPlantelGeneroPlantel = c.docPlantelGeneroPlantel,
                                 docPlantelIdSostenimiento = c.docPlantelIdSostenimiento,
                                 docPlantelSostenimiento = c.docPlantelSostenimiento,
                                 docAlumnoNombre = c.docAlumnoNombre,
                                 docAlumnoNombreRENAPO = c.docAlumnoNombreRENAPO,
                                 docAlumnoPrimerApellido = c.docAlumnoPrimerApellido,
                                 docAlumnoPrimerApellidoRENAPO = c.docAlumnoPrimerApellidoRENAPO,
                                 docAlumnoSegundoApellido = c.docAlumnoSegundoApellido,
                                 docAlumnoSegundoApellidoRENAPO = c.docAlumnoSegundoApellidoRENAPO,
                                 docAlumnoCurp = c.docAlumnoCurp,
                                 docAlumnoCurpRENAPO = c.docAlumnoCurpRENAPO,
                                 docAlumnoNumeroControl = c.docAlumnoNumeroControl,
                                 docAcreditacionIdTipoEstudiosIEMS = c.docAcreditacionIdTipoEstudiosIEMS,
                                 docAcreditacionTipoEstudiosIEMS = c.docAcreditacionTipoEstudiosIEMS,
                                 docAcreditacionTipoPerfilProfesionalIEMS = c.docAcreditacionTipoPerfilProfesionalIEMS,
                                 docAcreditacionNombreTipoPerfilProfesionalIEMS = c.docAcreditacionNombreTipoPerfilProfesionalIEMS,
                                 docAcreditacionPeriodoInicio = Convert.ToDateTime(c.docAcreditacionPeriodoInicio).ToString("dd-MM-yyyy"),
                                 docAcreditacionPeriodoTermino = Convert.ToDateTime(c.docAcreditacionPeriodoTermino).ToString("dd-MM-yyyy"),
                                 docAcreditacionCreditosObtenidos = c.docAcreditacionCreditosObtenidos,
                                 docAcreditacionTotalCreditos = c.docAcreditacionTotalCreditos,
                                 docAcreditacionPromedioAprovechamiento = c.docAcreditacionPromedioAprovechamiento,
                                 docAcreditacionPromedioAprovechamientoTexto = c.docAcreditacionPromedioAprovechamientoTexto,
                                 docCompetencias = c.docCompetencias,
                                 docEgresoCompetenciasTrayecto = c.docEgresoCompetenciasTrayecto,
                                 docEgresoCompetenciasIdCampoDisciplinar = c.docEgresoCompetenciasIdCampoDisciplinar,
                                 docEgresoCompetenciasCampoDisciplinar = c.docEgresoCompetenciasCampoDisciplinar,
                                 docEgresoCompetenciasTipoPerfilProfesionalIEMS = c.docEgresoCompetenciasTipoPerfilProfesionalIEMS,
                                 docEgresoCompetenciasNombrePerfilProfesionalIEMS = c.docEgresoCompetenciasNombrePerfilProfesionalIEMS,
                                 docCompetenciasIEMS = c.docCompetenciasIEMS,
                                 docCorreo = c.docCorreo,
                                 docObservaciones = c.docObservaciones,
                                 docAcreditacionClavePlanEstudios = c.docAcreditacionClavePlanEstudios,
                                 insId = c.insId,
                                 totalErrores = c.totalErrores,
                                 totalObservaciones = c.totalObservaciones,

                                 docServicioIemsNombreSEN = c.docServicioIemsNombreSEN,
                                 docServicioIemsnombreIEMSparticular = c.docServicioIemsnombreIEMSparticular,
                                 docServicioIemsInstitucionRVOE = c.docServicioIemsInstitucionRVOE,
                                 docServicioIemsIdTipoIEMS = c.docServicioIemsIdTipoIEMS,
                                 docServicioIemsTipoIEMS = c.docServicioIemsTipoIEMS,
                                 docPlantelOServicioEducativoFechaInicioRVOE = c.docPlantelOServicioEducativoFechaInicioRVOE,
                                 docAcreditacionidNivelEstudios = c.docAcreditacionidNivelEstudios,
                                 docAcreditacionNivelEstudios = c.docAcreditacionNivelEstudios,



                                 listCompetencias = filtroBusqueda.tipoDocumento == "2" ? null : GetLstCompertenciasXML(c.docCompetencias),
                                 listlstCompertenciasIEMS = filtroBusqueda.tipoDocumento == "2" ? null : CompertenciasIEMS(c.docCompetenciasIEMS),
                                 listUACS = filtroBusqueda.tipoDocumento == "2" ? GetLstUACSXML(c.docCompetencias) : null

                             }).ToList();

            return lstDocumentos;
        }
        public List<Competencias> GetLstCompertenciasXML(string competencias)
        {

            string xml = competencias.Replace('"', '\'');
            List<Competencias> lstXml = new List<Competencias>();
            Competencias itemXml = new Competencias();



            try
            {
                var xmlSerializer = new XmlSerializer(typeof(LstCompencias));
                StringReader stringReader = new StringReader(competencias);

                var posts = (LstCompencias)xmlSerializer.Deserialize(stringReader);
                lstXml.AddRange(posts.listCompetencias);

            }
            catch (Exception e)
            {

                Console.Write(e.Message);
            }

            return lstXml;

        }
        public List<CompertenciasIEMS> CompertenciasIEMS(string CompertenciasIEMS)
        {

            List<CompertenciasIEMS> lstXml = new List<CompertenciasIEMS>();
            CompertenciasIEMS itemXml = new CompertenciasIEMS();



            try
            {
                var xmlSerializer = new XmlSerializer(typeof(lstCompertenciasIEMS));
                StringReader stringReader = new StringReader(CompertenciasIEMS);

                var posts = (lstCompertenciasIEMS)xmlSerializer.Deserialize(stringReader);
                lstXml.AddRange(posts.listCompertenciasIEMS);

            }
            catch (Exception e)
            {

                Console.Write(e.Message);
            }

            return lstXml;

        }

        public string[] CargarArchivoBL(string carId, bool cargaConObservaciones, string idUsuario)
        {
            string[] result = new string[2];
            string tipoDocumento = new CerDocumento_TemDAL().GetTipoDocumentoByCarId(carId);
            string sp = "";
            if (tipoDocumento == "1")
            {
                sp = "SpCargarCertificadosTerminacion";
            }
            else if (tipoDocumento == "2")
            {
                sp = "SpCargarCertificadosParciales";
            }

            int resultado = new CerDocumento_TemDAL().CargarArchivo(carId, cargaConObservaciones, idUsuario, sp);
            if (resultado > 0)
            {
                result[0] = "True";
                result[1] = "Carga exitosa, registros insertados.";
            }
            else
            {
                result[0] = "False";
                result[1] = "Error al realizar la carga, intente nuevamente.";
            }
            return result;
        }
        public CargaCertificadosML criteriosBusqueda()
        {
            CargaCertificadosML criterios = new CargaCertificadosML();

            criterios.comboTipoDocumento = (from c in new cerCatTipoDocumentoDAL().CerCatTipoDocumentos()
                                            select new SelectListItem
                                            {
                                                Selected = (c.docTipoId == "1"),
                                                Text = c.docDescripcion,
                                                Value = c.docTipoId
                                            }).ToList();

            return criterios;
        }

        public string[] ValidarLayoutBLparcial(CargaCertificadosML model, string insId, string idUsuario)
        {


            List<cerUACDocumentoML> UACSdocumento = new List<cerUACDocumentoML>();
            CerDocumento_TemDAL obj = new CerDocumento_TemDAL();
            ConsultaRenapoClient SWRenapo = new ConsultaRenapoClient();
            List<MateriasArchivoCarga> lstClavesMaterias = new List<MateriasArchivoCarga>();
            List<string> encabezadoArchivo = new List<string>();
            bool impacto = false;
            string[] literales;
            string[] result = new string[2];
            string[] AlumnoRENAPO;
            string resultadoRENAPO = "";
            string calificacion = "";
            string calificacionMateriaGrado = "";
            List<string> MateriasGradoEstudios = new List<string>();
            int lasMateriasTerminarEnlaColumna;
            decimal creditosObtenidos = 0;
            var lstNodosConfigurados = new cerParametroValorDAL().GetLstParametrosByIdDocumento(insId, "2");
            var lstNodosPlanes = new CerCatPlanDAL().GetLstPlanes();
            var lstPlantillas = new cerCatPlantillaDAL().GetLstPlantillas(insId, "2");
            var lstAreas = new cerCargaDAL().GetLstAreasConocimiento();
            cerCarga carga = new cerCarga();
            carga.carId = Guid.NewGuid().ToString();
            List<cerDocumento_Tem> listadoDocumentos = new List<cerDocumento_Tem>();

            List<cerCatMateria> cerCatMaterias = new cerCatMateriaDAL().GetLstMaterias();

            List<(string insId, string idPlan, string idAreaConocimiento, string creditos)> materiasEspecialidad = new cerCatMateriaDAL().MateriasAreaEspecializacionByInsId(insId);

            var idMaterias = new cerRelPlanPeriodoAreaConocimientoMateriaDAL().GetLstMateriasByTipoDocumento(insId, model.tipoDocumento);
            var clvMateria = idMaterias.Select(x => new { x.idMateria, x.idPlan }).Distinct().ToList();

            string literalesAprobacion = lstNodosConfigurados.Where(x => x.parId == "clavesAprobacion").Select(x => x.parValor).FirstOrDefault();
            string literalesDesercion = lstNodosConfigurados.Where(x => x.parId == "clavesDesercion").Select(x => x.parValor).FirstOrDefault();

            using (var excelWorkbook = new XLWorkbook(model.certificadoIntegracion.InputStream))
            {
                var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();

                foreach (var dataRow in nonEmptyDataRows)
                {
                    List<MateriasArchivoCarga> lstCompetencias = new List<MateriasArchivoCarga>();


                    cerDocumento_Tem documento = new cerDocumento_Tem();
                    if (dataRow.RowNumber() == 2)
                    {

                        lasMateriasTerminarEnlaColumna = excelWorkbook.Worksheet(1).Row(2).LastCellUsed().WorksheetColumn().ColumnUsed().ColumnNumber();

                        for (int i = 12; i <= lasMateriasTerminarEnlaColumna; i++)
                        {
                            lstClavesMaterias.Add(new MateriasArchivoCarga
                            {
                                idCelda = i.ToString(),
                                idDatoMateria = dataRow.Cell(i).Value.ToString()
                            });

                        }

                    }

                    if (dataRow.RowNumber() > 2)
                    {
                        AlumnoRENAPO = null;
                        resultadoRENAPO = "";
                        documento.insId = insId;
                        documento.docId = Guid.NewGuid().ToString();
                        documento.carId = carga.carId;
                        documento.fila = dataRow.RowNumber();
                        documento.docPlanId = dataRow.Cell(1).Value.ToString().Trim() ?? null;//Archivo:idPlan
                        documento.idAreaConocimiento = dataRow.Cell(2).Value.ToString().Trim() ?? null;//Archivo:idArea
                        documento.docCorreo = dataRow.Cell(8).Value.ToString().Trim() ?? null;//Archivo:Correo electrónico

                        documento.docEgresoCompetenciasTrayecto = lstAreas.Where(x => x.idAreaConocimiento == documento.idAreaConocimiento).Select(x => x.areaEgresoCompetenciasTrayectoAcreditado).FirstOrDefault();

                        //Nodo DEC
                        documento.docDecVersion = lstNodosConfigurados.Where(x => x.parId == "decVersion").Select(x => x.parValor).FirstOrDefault();
                        documento.docDecTipocertificado = "2";
                        documento.docDecFolioControl = documento.docId;
                        //Nodo Servicio
                        documento.docServicioNombreTipoDependencia = lstNodosConfigurados.Where(x => x.parId == "servicioNombreTipoDependencia").Select(x => x.parValor).FirstOrDefault();
                        documento.docServicioIdIEMS = lstNodosConfigurados.Where(x => x.parId == "servicioIdIEMS").Select(x => x.parValor).FirstOrDefault();
                                  //nombreIEMS
                        documento.docServicioNombreIEMS = lstNodosConfigurados.Where(x => x.parId == "servicioNombreIEMS").Select(x => x.parValor).FirstOrDefault();
                        documento.docServicioIdOpcionEducativa = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.servicioIdOpcionEducativa).FirstOrDefault();
                                  //opcionEducativa
                        documento.docServicioOpcionEducativa = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.servicioOpcionEducativa).FirstOrDefault();
                        //Nodo Plantel
                        documento.docPlantelIdTipoPlantel = Convert.ToInt32(lstNodosConfigurados.Where(x => x.parId == "plantelIdTipoPlantel").Select(x => x.parValor).FirstOrDefault());
                        documento.docPlantelTipoPlantel = lstNodosConfigurados.Where(x => x.parId == "plantelTipoPlantel").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelNombreNumeroPlantel = lstNodosConfigurados.Where(x => x.parId == "plantelNombreNumeroPlantel").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelIdLocalidad = lstNodosConfigurados.Where(x => x.parId == "plantelIdLocalidad").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelLocalidad = lstNodosConfigurados.Where(x => x.parId == "plantelLocalidad").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelIdEntidadFederativa = lstNodosConfigurados.Where(x => x.parId == "plantelIdEntidadFederativa").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelEntidadFederativa = lstNodosConfigurados.Where(x => x.parId == "plantelEntidadFederativa").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelCCT = lstNodosConfigurados.Where(x => x.parId == "plantelCCT").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelClaveRvoe = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.planPlantelClaveRvoe).FirstOrDefault();//tomar de la tabla plan
                        documento.docplantelIdGeneroPlantel = lstNodosConfigurados.Where(x => x.parId == "plantelIdGeneroPlantel").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelGeneroPlantel = lstNodosConfigurados.Where(x => x.parId == "plantelGeneroPlantel").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelIdSostenimiento = lstNodosConfigurados.Where(x => x.parId == "plantelIdSostenimiento").Select(x => x.parValor).FirstOrDefault();
                        documento.docPlantelSostenimiento = lstNodosConfigurados.Where(x => x.parId == "plantelSostenimiento").Select(x => x.parValor).FirstOrDefault();
                        //Nodo Alumno
                        documento.docAlumnoNombre = dataRow.Cell(3).Value.ToString().Trim() ?? null;//Archivo:Nombre 
                        documento.docAlumnoPrimerApellido = dataRow.Cell(4).Value.ToString().Trim() ?? null;//Archivo:Primer apellido
                        documento.docAlumnoSegundoApellido = string.IsNullOrWhiteSpace(dataRow.Cell(5).Value.ToString()) ? null : dataRow.Cell(5).Value.ToString().Trim();//Archivo:Segundo apellido
                        documento.docAlumnoCurp = dataRow.Cell(6).Value.ToString().Trim().ToUpper() ?? null;//Archivo:Curp
                        documento.docAlumnoNumeroControl = dataRow.Cell(7).Value.ToString().Trim() ?? null;//Archivo:Archivo:Número de control

                        try
                        {
                            resultadoRENAPO = SWRenapo.consultarPorCurp(documento.docAlumnoCurp);

                            AlumnoRENAPO = resultadoRENAPO.Split('|');
                            if (AlumnoRENAPO.Count() > 1)
                            {
                                documento.docAlumnoCurpRENAPO = AlumnoRENAPO[0].Trim() ?? null;
                                documento.docAlumnoPrimerApellidoRENAPO = AlumnoRENAPO[1].Trim() ?? null;
                                documento.docAlumnoSegundoApellidoRENAPO = AlumnoRENAPO[2].Trim() ?? null;
                                documento.docAlumnoNombreRENAPO = AlumnoRENAPO[3].Trim() ?? null;
                            }

                        }
                        catch (Exception e)
                        {

                            Console.Write(e.Message);
                        }

                        //Nodo Acreditacion
                        try
                        {
                            documento.docAcreditacionPeriodoInicio = Convert.ToDateTime(dataRow.Cell(9).Value.ToString().Trim()).ToString("dd-MM-yyyy HH:mm:ss") ?? null;//Archivo:Archivo:Periodo inicio


                        }
                        catch (Exception e)
                        {

                            documento.docAcreditacionPeriodoInicio = dataRow.Cell(9).Value.ToString();
                        }
                        try
                        {
                            documento.docAcreditacionPeriodoTermino = Convert.ToDateTime(dataRow.Cell(10).Value.ToString().Trim()).ToString("dd-MM-yyyy HH:mm:ss") ?? null;//Archivo:Periodo término

                        }
                        catch (Exception e)
                        {

                            documento.docAcreditacionPeriodoTermino = dataRow.Cell(10).Value.ToString();
                        }

                        documento.docAcreditacionPromedioAprovechamiento = dataRow.Cell(11).Value.ToString().Trim() ?? null;//Archivo:Promedio aprovechamiento

                        documento.docAcreditacionPromedioAprovechamiento = documento.docAcreditacionPromedioAprovechamiento.Contains(".") ? documento.docAcreditacionPromedioAprovechamiento : documento.docAcreditacionPromedioAprovechamiento + ".0";

                        documento.docAcreditacionIdTipoEstudiosIEMS = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.acreditacionIdTipoEstudiosIEMS).FirstOrDefault();//tomar de la tabla plan;
                        documento.docAcreditacionTipoEstudiosIEMS = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.acreditacionTipoEstudiosIEMS).FirstOrDefault();//tomar de la tabla plan
                        documento.docAcreditacionTipoPerfilProfesionalIEMS = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.acreditacionTipoPerfilProfesionalIEMS).FirstOrDefault();//tomar de la tabla plan
                        documento.docAcreditacionClavePlanEstudios = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.acreditacionClavePlanEstudios).FirstOrDefault();//tomar de la tabla plan
                                                                                                                                                                                              /*va cambiar*/
                        documento.docAcreditacionTotalCreditos = lstAreas.Where(x => x.idAreaConocimiento == documento.idAreaConocimiento).Select(x => x.areaTotalCreditos).FirstOrDefault();


                        documento.docAcreditacionNombreTipoPerfilProfesionalIEMS = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.acreditacionNombreTipoPerfilProfesionalIEMS).FirstOrDefault();



                        documento.docAcreditacionPromedioAprovechamientoTexto = MetodosGenericosBL.FirstCharToUpper(new ConversionesBL().ConvertirCalificacion(documento.docAcreditacionPromedioAprovechamiento));


                        documento.docAcreditacionPromedioAprovechamientoTexto = string.IsNullOrWhiteSpace(documento.docAcreditacionPromedioAprovechamientoTexto) ? documento.docAcreditacionPromedioAprovechamiento : documento.docAcreditacionPromedioAprovechamientoTexto;

                        //Nodo UACS
                        documento.docUACSidTipoPeriodo = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.uacIdTipoPeriodo).FirstOrDefault();
                        documento.docUACSnombreTipoPeriodo = lstNodosPlanes.Where(x => x.idPlan == documento.docPlanId).Select(x => x.uacNombreTipoPeriodo).FirstOrDefault();

                        //Competencias
                        try
                        {
                            string valor = "";
                            foreach (var materias in lstClavesMaterias)
                            {
                                valor = dataRow.Cell(Convert.ToInt32(materias.idCelda)).Value.ToString() ?? "";

                                lstCompetencias.Add(new MateriasArchivoCarga
                                {
                                    idCelda = materias.idCelda,
                                    idDatoMateria = materias.idDatoMateria,
                                    valor = valor
                                });
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                        var materiasArea = (from c in idMaterias where c.idAreaConocimiento == documento.idAreaConocimiento select c).ToList();

                        try
                        {
                            UACSdocumento = new List<cerUACDocumentoML>();

                            foreach (var materia in clvMateria)
                            {
                                calificacion = (from c in lstCompetencias where c.idDatoMateria == materia.idMateria + "_" + materia.idPlan + " " + "C" select c.valor).FirstOrDefault().Trim();

                                try
                                {
                                    Convert.ToDouble(calificacion);
                                    calificacion = calificacion.Length == 1 ? calificacion + ".0" : calificacion;
                                }
                                catch (Exception ex)
                                {

                                    calificacion = (from c in lstCompetencias where c.idDatoMateria == materia.idMateria + "_" + materia.idPlan + " " + "C" select c.valor).FirstOrDefault().Trim();
                                }

                                bool encuentraMateria = (from c in materiasArea where c.idMateria == materia.idMateria && c.idPlan == materia.idPlan select c).Any();

                                if (!String.IsNullOrWhiteSpace(calificacion) || encuentraMateria == true)
                                {


                                    UACSdocumento.Add(new cerUACDocumentoML
                                    {
                                        idMateria = materia.idMateria,
                                        idPlan = materia.idPlan,
                                        UACcct = (from c in lstCompetencias where c.idDatoMateria == materia.idMateria + "_" + materia.idPlan + " " + "CCT" select c.valor).FirstOrDefault() == "" ? documento.docPlantelCCT : (from c in lstCompetencias where c.idDatoMateria == materia.idMateria + "_" + materia.idPlan + " " + "CCT" select c.valor).FirstOrDefault(),
                                        UACidTipo = (from c in lstCompetencias where c.idDatoMateria == materia.idMateria + "_" + materia.idPlan + " " + "IT" select c.valor).FirstOrDefault(),
                                        UACnombre = (from c in lstCompetencias where c.idDatoMateria == materia.idMateria + "_" + materia.idPlan + " " + "N" select c.valor).FirstOrDefault(),
                                        UACcalificacion = calificacion,
                                        UACperiodoEscolar = (from c in lstCompetencias where c.idDatoMateria == materia.idMateria + "_" + materia.idPlan + " " + "P" select c.valor).FirstOrDefault(),
                                        UACnumeroPeriodo = (from c in lstCompetencias where c.idDatoMateria == materia.idMateria + "_" + materia.idPlan + " " + "NP" select c.valor).FirstOrDefault()

                                    });
                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                        MateriasGradoEstudios = (from c in materiasArea where c.condicionaTrayecto == true && c.idPlan == documento.docPlanId select c.idMateria).ToList();


                        foreach (var MateriaGradoEstudios in MateriasGradoEstudios)
                        {

                       

                        if (!String.IsNullOrWhiteSpace(MateriaGradoEstudios))
                        {
                            calificacionMateriaGrado = (from c in lstCompetencias where c.idDatoMateria == MateriaGradoEstudios + "_" + documento.docPlanId + " " + "C" select c.valor).FirstOrDefault().Trim();

                            if (validarCalificacionUACByCreditos(calificacionMateriaGrado, literalesDesercion) == "***")
                            {
                                documento.docEgresoCompetenciasTrayecto = lstAreas.Where(x => x.idAreaConocimiento == documento.idAreaConocimiento).Select(x => x.areaEgresoCompetenciasTrayectoNoAcreditado).FirstOrDefault();

                                    documento.docAcreditacionNombreTipoPerfilProfesionalIEMS = null;
                                    documento.docAcreditacionTipoPerfilProfesionalIEMS = null;
                                    break;
                            }

                        }
                        }

                        documento.docCompetencias = UACSxml(UACSdocumento, documento.docPlanId, documento.idAreaConocimiento, literalesAprobacion, literalesDesercion, insId, documento.docDecTipocertificado).ToString();
                        documento.planId = lstPlantillas.Where(x => x.idPlan == documento.docPlanId && x.planEstatus == true).Select(x => x.planId).FirstOrDefault() ?? null;

                        var uacs = GetLstUACSXML(documento.docCompetencias).Where(x => x.UACcreditos != "***").ToList();

                        creditosObtenidos = 0;
                        foreach (var creditos in uacs)
                        {
                            creditosObtenidos += Convert.ToDecimal(creditos.UACcreditos);
                        }

                        documento.docAcreditacionCreditosObtenidos = creditosObtenidos.ToString();

                        documento.docObservaciones = "";
                        documento.docInsertado = false;


                        documento.docServicioIemsNombreSEN = lstNodosConfigurados.Where(x => x.parId == "servicioIemsNombreSEN").Select(x => x.parValor).FirstOrDefault();
                        documento.docServicioIemsnombreIEMSparticular = lstNodosConfigurados.Where(x => x.parId == "servicioIemsnombreIEMSparticular").Select(x => x.parValor).FirstOrDefault();
                        documento.docServicioIemsInstitucionRVOE = lstNodosConfigurados.Where(x => x.parId == "servicioIemsInstitucionRVOE").Select(x => x.parValor).FirstOrDefault();
                        documento.docServicioIemsIdTipoIEMS = lstNodosConfigurados.Where(x => x.parId == "servicioIemsIdTipoIEMS").Select(x => x.parValor).FirstOrDefault();
                                  //tipoIEMS
                        documento.docServicioIemsTipoIEMS = lstNodosConfigurados.Where(x => x.parId == "servicioIemsTipoIEMS").Select(x => x.parValor).FirstOrDefault();
                        if (lstNodosConfigurados.Where(x => x.parId == "plantelOServicioEducativoFechaInicioRVOE").Select(x => x.parValor).FirstOrDefault() != null)
                        {
                            try
                            {                            
                                documento.docPlantelOServicioEducativoFechaInicioRVOE = DateTime.Parse(lstNodosConfigurados.Where(x => x.parId == "plantelOServicioEducativoFechaInicioRVOE").Select(x => x.parValor).FirstOrDefault());


                            }
                            catch (Exception e)
                            {
                                documento.docPlantelOServicioEducativoFechaInicioRVOE = null;


                            }
                        }
                        documento.docAcreditacionidNivelEstudios = lstNodosConfigurados.Where(x => x.parId == "acreditacionidNivelEstudios").Select(x => x.parValor).FirstOrDefault();
                        documento.docAcreditacionNivelEstudios = lstNodosConfigurados.Where(x => x.parId == "acreditacionNivelEstudios").Select(x => x.parValor).FirstOrDefault();

                        listadoDocumentos.Add(documento);
                    }

                }
                if (listadoDocumentos.Count() != 0)
                {

                    impacto = new CerDocumento_TemDAL().AddRegistrosArchivo(listadoDocumentos);
                    if (impacto)
                    {
                        if (obj.ValidarArchivo(carga.carId, idUsuario, insId, "SpValidarCargaCertificadosParciales") == 1)
                        {
                            result[0] = "True";
                            result[1] = carga.carId.ToString();
                        }
                        else
                        {
                            result[0] = "False";
                            result[1] = "La carga, no se puedo realizar intenta nuevamente.";
                        }

                    }
                    else
                    {
                        result[0] = "False";
                        result[1] = "La carga, no se puedo realizar intenta nuevamente.";
                    }
                }
                else
                {
                    result[0] = "False";
                    result[1] = "El archivo no contiene registros válidos.";
                }
            }
            return result;

        }
        public XElement UACSxml(List<cerUACDocumentoML> lstUACSCarga, string idPlan, string idAreaConocimiento, string literalesA, string literalesD, string insId, string docDecTipocertificado)
        {
            var lstMateriasMalla = new cerCatMateriaDAL().GetLstMaterias();
            var lstMateriasOrden = new cerRelPlanPeriodoAreaConocimientoMateriaDAL().GetLstMateriasByPlan(idPlan, docDecTipocertificado);
            var lstTipoUac = new cerCatTipoUacDAL().GetLstTipoUacsByInsId(insId);
            string literales = literalesA + "," + literalesD;
            string[] literalesPermitidas = literales.Split(','); //{"A","AC","NP","NI" };
            List<cerUACDocumentoML> lstMaterias = (from d in lstUACSCarga
                                                   join c in lstMateriasMalla
                                                   on new { d.idMateria, d.idPlan } equals new { c.idMateria, c.idPlan }
                                                   into e
                                                   from tablaTemporal in e.DefaultIfEmpty()

                                                   select new cerUACDocumentoML
                                                   {
                                                       idMateria = tablaTemporal.idMateria,
                                                       idPeriodo = (from rel in lstMateriasOrden where rel.idPlan == idPlan && rel.idAreaConocimiento == idAreaConocimiento && rel.idMateria == tablaTemporal.idMateria select rel.idPeriodo).FirstOrDefault(),
                                                       idPlan = d.idPlan,
                                                       UACcct = d.UACcct,
                                                       UACidTipo = d.UACidTipo,
                                                       UACtipo = (from c in lstTipoUac where c.tipoUacId == d.UACidTipo select c.tipoUacDescripcion).FirstOrDefault() ?? "NE",
                                                       UACnombre = (d.UACnombre == "" ? tablaTemporal.materiaDescripcion : d.UACnombre),
                                                       UACcalificacion = d.UACcalificacion,
                                                       UACperiodoEscolar = d.UACperiodoEscolar,
                                                       UACnumeroPeriodo = d.UACnumeroPeriodo,
                                                       UACcreditos = (validarCalificacionUACByCreditos(d.UACcalificacion, literalesD) == "***" ? "***" : tablaTemporal.materiaCreditos),
                                                       UACtotalHorasUAC = tablaTemporal.materiahoras.ToString(),
                                                       orden = (from rel in lstMateriasOrden where rel.idPlan == idPlan && rel.idAreaConocimiento == idAreaConocimiento && rel.idMateria == tablaTemporal.idMateria select rel.orden).FirstOrDefault()


                                                   }
                                                   ).ToList();


            List<XElement> lstCompetenciasXML = new List<XElement>();
            XElement xmlCompetencias = new XElement("UACS", from c in lstMaterias
                                                            select new
                               XElement("UAC", new XAttribute("idMateria", c.idMateria),
                               new XAttribute("idPeriodo", c.idPeriodo),
                               new XAttribute("idPlan", c.idPlan),
                               new XAttribute("UACcct", c.UACcct),
                               new XAttribute("UACidTipo", c.UACidTipo),
                               new XAttribute("UACtipo", c.UACtipo),
                               new XAttribute("UACnombre", c.UACnombre),
                               new XAttribute("UACcalificacion", c.UACcalificacion),
                               new XAttribute("UACperiodoEscolar", c.UACperiodoEscolar),
                               new XAttribute("UACnumeroPeriodo", c.UACnumeroPeriodo),
                               new XAttribute("UACtotalHorasUAC", c.UACtotalHorasUAC),
                               new XAttribute("UACcreditos", c.UACcreditos == null ? null : c.UACcreditos),
                               new XAttribute("orden", c.orden))



                );

            return xmlCompetencias;

        }
        public List<UACS> GetLstUACSXML(string UACS)
        {

            string xml = UACS.Replace('"', '\'');
            List<UACS> lstXml = new List<UACS>();
            UACS itemXml = new UACS();



            try
            {
                var xmlSerializer = new XmlSerializer(typeof(LstUACS));
                StringReader stringReader = new StringReader(UACS);

                var posts = (LstUACS)xmlSerializer.Deserialize(stringReader);
                lstXml.AddRange(posts.lstUACS);

            }
            catch (Exception e)
            {

                Console.Write(e.Message);
            }

            return lstXml;

        }
        public static string validarCalificacionUACByCreditos(string calificaion, string literalesD)
        {
            string result = "";
            literalesD = literalesD + "," + "***";
            string[] literalesNoAcreditada = literalesD.Split(',');
            if (literalesNoAcreditada.Contains(calificaion))
            {
                result = "***";
            }
            else
            {
                try
                {
                    decimal calif = decimal.Parse(calificaion);
                    if (calif < 6)
                    {
                        result = "***";
                    }
                }
                catch (Exception)
                {

                    result = "";
                }

            }

            return result;

        }
        public List<MateriasArchivoCarga> LstMateriasCarga(string insId, string tipoDocumento)
        {
            List<MateriasArchivoCarga> lstMaterias = new List<MateriasArchivoCarga>();
            var idMaterias = new cerRelPlanPeriodoAreaConocimientoMateriaDAL().GetLstMateriasByTipoDocumento(insId, tipoDocumento);
            var clvMateria = idMaterias.Select(x => new { x.idMateria, x.idPlan }).Distinct().ToList();

            lstMaterias = (from c in clvMateria
                           select new MateriasArchivoCarga
                           {
                               idMateria = c.idMateria,
                               idPlan = c.idPlan
                           }).ToList();

            return lstMaterias;

        }


        public Byte[] GenerarPlantillaCarga(string tipoDocumento, string insId)
        {

            var columnas = new CargaCertificadosBL().GenerarColumnaLayout(insId, tipoDocumento);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Datos");
                var ws2 = wb.Worksheets.Add("Ayuda");

                int i = 1;
                foreach (var columna in columnas)
                {

                    ws.Cell(2, i).Value = columna;

                    i++;
                }
                //Hoja de ayuda para tipo documento 2
                if (tipoDocumento == "2" || tipoDocumento == "1")
                {
                    ws2.Range("A1:C1").Style.Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.FromHtml("#808080"));
                    ws2.Cell("A1").SetValue("Sección").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws2.Cell("B1").SetValue("Columna").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws2.Cell("C1").SetValue("Descripción").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                    ws2.Cell("A2").Style.Fill.BackgroundColor = XLColor.FromHtml("#B7DEE8");
                    ws2.Cell("A2").SetValue("Plan");
                    ws2.Cell("B2").SetValue("Identificador de plan de estudios");
                    ws2.Cell("C2").SetValue("33 o 22, obligatorio.");

                    ws2.Cell("A3").Style.Fill.BackgroundColor = XLColor.FromHtml("#CCC0DA");
                    ws2.Cell("A3").SetValue("Especialidad");
                    ws2.Cell("B3").SetValue("Identificador de área de especialidad");
                    ws2.Cell("C3").SetValue("Se valida que no se ingresen los caracteres especiales “|\\~&”, se especificarán las claves de la malla curricular, para el plan 33(H, CAS, FM) y para el plan 22(I).");
                    //XLAlignmentHorizontalValues
                    ws2.Range("A4:A9").Style.Fill.BackgroundColor = XLColor.FromHtml("#D8E4BC");
                    ws2.Range("A4:A9").Merge().SetValue("Alumno").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws2.Cell("B4").SetValue("Nombre");
                    ws2.Cell("C4").SetValue("La validación se realiza con el nombre que arroje el servicio web de RENAPO, obligatorio");
                    ws2.Cell("B5").SetValue("Primer apellido");
                    ws2.Cell("C5").SetValue("La validación se realiza con el nombre que arroje el servicio web de RENAPO, obligatorio");
                    ws2.Cell("B6").SetValue("Segundo apellido");
                    ws2.Cell("C6").SetValue("La validación se realiza con el nombre que arroje el servicio web de RENAPO, no obligatorio.");
                    ws2.Cell("B7").SetValue("CURP");
                    ws2.Cell("C7").SetValue("La validación del CURP se realizará en a través del servicio web de RENAPO, obligatorio.");
                    ws2.Cell("B8").SetValue("Número de control");
                    ws2.Cell("C8").SetValue("Se valida que no se ingresen los caracteres especiales “|\\~&”, es posible contar con más de un documento con la misma matricula, obligatorio.");
                    ws2.Cell("B9").SetValue("Correo");
                    ws2.Cell("C9").SetValue("Contar con una estructura de correo valida, obligatorio.");

                    ws2.Range("A10:A12").Style.Fill.BackgroundColor = XLColor.FromHtml("#E6B8B7");
                    ws2.Range("A10:A12").Merge().SetValue("Acreditación").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws2.Cell("B10").SetValue("Periodo de inicio de estudios");
                    ws2.Cell("C10").SetValue("Fecha DD/MM/AAAA obligatorio.");
                    ws2.Cell("B11").SetValue("Periodo de término de estudios");
                    ws2.Cell("C11").SetValue("Fecha DD/MM/AAAA obligatorio.");
                    ws2.Cell("B12").SetValue("Promedio aprovechamiento");
                    ws2.Cell("C12").SetValue("Número con un decimal, no obligatorio.");

                    if (tipoDocumento == "1")
                    {
                        ws2.Range("A13").Style.Fill.BackgroundColor = XLColor.FromHtml("#B7DEE8");
                        ws2.Cell("A13").SetValue("Competencias");
                        ws2.Cell("B13").SetValue("[identificador de materia]_[identificador de plan de estudios]");
                        ws2.Cell("C13").SetValue("Ingresar el promedio obtenido, puede ser decimal o ingresar una de las claves definidas en la configuración como aprobación y deserción.");
                    }


                    if (tipoDocumento == "2")
                    {
                        ws2.Range("A13:A18").Style.Fill.BackgroundColor = XLColor.FromHtml("#B8CCE4");
                        ws2.Range("A13:A18").Merge().SetValue("UAC").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        ws2.Cell("B13").SetValue("CCT: clave de centro de trabajo");
                        ws2.Cell("C13").SetValue("Clave de centro de trabajo, no es requerido, al dejarla vacia por predeterminadamente toma la configurada, obligatorio.");
                        ws2.Cell("B14").SetValue("IT: identificador de tipo");
                        ws2.Cell("C14").SetValue("Contiene el identificador de tipo UAC: se define por UAC en la malla curricular. Los valores pueden ser (2,3 o 5), obligatorio.");
                        ws2.Cell("B15").SetValue("N: nombre").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        ws2.Cell("C15").SetValue(" Si se deja vacío nombre de la materia tomará el establecido en el catálogo.\n" +
                            "Si se llena el campo se toma este valor para el nombre de la materia y las horas quedan con (***).\n" +
                            "Se debe colocar el tipo de amparo entre los cuales pueden ser:\n" +
                            "1. Rev. Revalidación de estudios.\n" +
                            "2.Equiv.Equivalencia de estudios.\n" +
                            "3.Port.Portabilidad de estudios");
                        ws2.Cell("B16").SetValue("C: calificación");
                        ws2.Cell("C16").SetValue("Ingresar el promedio obtenido, puede ser decimal o ingresar una de las claves definidas en la configuración como aprobación y deserción.");
                        ws2.Cell("B17").SetValue("P:  Periodo escolar");
                        ws2.Cell("C17").SetValue("año/mes/qna del mes ejemplo 2015/03/A o B en equivalencias y reprobados se define como (***)");
                        ws2.Cell("B18").SetValue("NP : número de periodo");
                        ws2.Cell("C18").SetValue("Atributo que indica el número de periodo de la UAC: Atributo que indica el número por orden jerárquico del periodo escolar o de acreditación en el que aprobó el alumno la UAC. De igual forma, en los casos de NI (No inscrito),(***). ");

                    }
                    ws2.Columns().AdjustToContents();
                }

                var lastColumn = ws.LastColumnUsed().RangeAddress.FirstAddress.ColumnLetter;


                ws.Cell(1, 1).Value = "Plan";
                ws.Cell(1, 2).Value = "Especialidad";
                ws.Range("C1:H1").Row(1).Merge().Value = "Alumno";
                ws.Range("I1:K1").Row(1).Merge().Value = "Acreditación";
                ws.Range("L1:" + lastColumn + "1").Row(1).Merge().Value = "Competencias";
                var rango_1 = ws.Range("A1:" + lastColumn + "1");
                rango_1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; //Alineamos horizontalmente
                rango_1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;  //Alineamos verticalmente
                rango_1.Style.Font.FontSize = 11; //Indicamos el tamaño de la fuente
                rango_1.Style.Font.FontColor = XLColor.Black;
                rango_1.Style.Font.SetBold(true);

                ws.Range("A1:A2").Style.Fill.BackgroundColor = XLColor.FromHtml("#B7DEE8");
                ws.Range("A1:A2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick);//Generamos las lineas exteriores
                ws.Range("A1:A2").Style.Border.SetInsideBorder(XLBorderStyleValues.Medium);//Generamos las lineas interiores
                ws.Range("B1:B2").Style.Fill.BackgroundColor = XLColor.FromHtml("#CCC0DA");
                ws.Range("B1:B2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick);
                ws.Range("B1:B2").Style.Border.SetInsideBorder(XLBorderStyleValues.Medium);
                ws.Range("C1:H2").Style.Fill.BackgroundColor = XLColor.FromHtml("#D8E4BC");
                ws.Range("C1:H2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick);
                ws.Range("C1:H2").Style.Border.SetInsideBorder(XLBorderStyleValues.Medium);
                ws.Range("I1:k2").Style.Fill.BackgroundColor = XLColor.FromHtml("#E6B8B7");
                ws.Range("I1:k2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick);
                ws.Range("I1:k2").Style.Border.SetInsideBorder(XLBorderStyleValues.Medium);
                ws.Range("L1:" + lastColumn + "2").Style.Fill.BackgroundColor = XLColor.FromHtml("#B8CCE4");
                ws.Range("L1:" + lastColumn + "2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick);
                ws.Range("L1:" + lastColumn + "2").Style.Border.SetInsideBorder(XLBorderStyleValues.Medium);


                ws.SheetView.SplitRow = 2;

                var rango_2 = ws.Range("A2:" + lastColumn + "2"); //Seleccionamos un rango

                rango_2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left; //Alineamos horizontalmente
                rango_2.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;  //Alineamos verticalmente
                rango_2.Style.Font.FontSize = 11; //Indicamos el tamaño de la fuente
                rango_2.Style.Font.FontColor = XLColor.Black;

                rango_2.RangeUsed().SetAutoFilter();

                ws.ColumnsUsed().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }

        }


    }

}


