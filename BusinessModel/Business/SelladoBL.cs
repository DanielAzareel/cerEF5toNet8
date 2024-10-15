using BusinessModel.Business.JavaScience;
using BusinessModel.DataAccess;
using BusinessModel.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BusinessModel.Models.Parcial;
namespace BusinessModel.Business
{
    public class SelladoBL
    {


        public CriteriosBusquedaML criteriosBusqueda(PerfilML perfil)
        {
            CriteriosBusquedaML criterios = new CriteriosBusquedaML();
            criterios.comboPlanEstudios = (from c in new CerCatPlanDAL().GetLstPlanesByIns(perfil.insId)
                                           select new SelectListItem
                                           {
                                               Text = c.planDescripcion,
                                               Value = c.idPlan
                                           }).ToList();

            criterios.comboTipoDocumento = (from c in new cerCatTipoDocumentoDAL().CerCatTipoDocumentos()
                                            select new SelectListItem
                                            {
                                                Text = c.docDescripcion,
                                                Value = c.docTipoId
                                            }).ToList();

            return criterios;
        }

        public SelladoML GetVWGridSelladosPagina(CriteriosBusquedaML criteriosBusquedaML, PerfilML perfilML, int pagina, int bloque)
        {
            SelladoML selladoML = new SelladoML();
            string where = PrepararCriteriosBusqueda(criteriosBusquedaML, perfilML);
            int regInicio = ((pagina - 1) * bloque);
            selladoML.documentos = new VWGridSelladoDAL().GetVWGridSelladoPaginacion(where, regInicio, bloque);
            selladoML.totalRegistros = new VWGridSelladoDAL().GetVWGridSelladoPaginacionCount(where, regInicio, bloque);
            return selladoML;

        }

        private string PrepararCriteriosBusqueda(CriteriosBusquedaML criteriosBusquedaML, PerfilML perfilML)
        {

            criteriosBusquedaML.insId = perfilML.insId;

            criteriosBusquedaML.listaCurp = !string.IsNullOrEmpty(criteriosBusquedaML.curp) ? criteriosBusquedaML.curp.Split(',').ToList<string>() : criteriosBusquedaML.listaCurp;

            criteriosBusquedaML.listaEstatus = criteriosBusquedaML.estatus != null ? criteriosBusquedaML.estatus.Split(',').Select(Int32.Parse).ToList() : null;
            criteriosBusquedaML.listaFolios = !string.IsNullOrEmpty(criteriosBusquedaML.folioControl) ? criteriosBusquedaML.folioControl.Split(',').ToList<string>() : criteriosBusquedaML.listaFolios;
            criteriosBusquedaML.listaAccesoPlanEstudios = (from c in perfilML.nivelAcceso select c.planes).ToList();

            if (!criteriosBusquedaML.listaAccesoPlanEstudios.Where(x => x != "").Any())
            {
                criteriosBusquedaML.listaAccesoPlanEstudios = new CerCatPlanDAL().GetLstPlanesByIns(perfilML.insId).Select(c => c.idPlan).ToList();
            }


            string sqlwhere = " insId='" + perfilML.insId + "' ";
            sqlwhere += " and estDocumentoId in ('" + String.Join("','", criteriosBusquedaML.listaEstatus) + "')";

            if (!String.IsNullOrEmpty(criteriosBusquedaML.curp))
            {
                sqlwhere += " and docAlumnoCurp in ('" + String.Join("','", criteriosBusquedaML.listaCurp) + "')";
            }

            if (!String.IsNullOrEmpty(criteriosBusquedaML.folioControl))
            {
                sqlwhere += " and docAlumnoNumeroControl in ('" + String.Join("','", criteriosBusquedaML.listaFolios) + "')";
            }

            if (criteriosBusquedaML.listaAccesoPlanEstudios.Any())
            {

                sqlwhere += " and docPlanId in ('" + String.Join("','", criteriosBusquedaML.listaAccesoPlanEstudios) + "')";
            }

            if (criteriosBusquedaML.listaTipoDocumento.Any())
            {

                sqlwhere += " and docTipoId in ('" + String.Join("','", criteriosBusquedaML.listaTipoDocumento) + "')";
            }

            if (criteriosBusquedaML.listaPlanEstudios.Any())
            {

                sqlwhere += " and docPlanId in ('" + String.Join("','", criteriosBusquedaML.listaPlanEstudios) + "')";
            }
            if (!string.IsNullOrEmpty(criteriosBusquedaML.nombreFiltro))
            {
                sqlwhere += " and docAlumnoNombre +' '+ docAlumnoPrimerApellido+' '+docAlumnoSegundoApellido like '%" + criteriosBusquedaML.nombreFiltro + "%'";
            }
            return sqlwhere;
        }



        public string[] CacelarRegistro(string documentoId, string insId, string Login)
        {
            string[] resultado = new string[2];
            resultado[0] = "False";
            resultado[1] = "No fue posible cancelar el registro.";
            try
            {
                if (new CerDocumentoDAL().EliminarDocumentoById(new cerDocumento { docId = documentoId, insId = insId }))
                {
                    resultado[0] = "True";
                    resultado[1] = "Se canceló el registro.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return resultado;
        }



        public List<cerDocumento> GetCerDocumentos(CriteriosBusquedaML criterios, PerfilML perfilML)
        {
            CerDocumentoDAL documentosDAL = new CerDocumentoDAL();
            cerUACDocumentoDAL uacAL = new cerUACDocumentoDAL();
            string where = PrepararCriteriosBusqueda(criterios, perfilML);
            var ids = new VWGridSelladoDAL().GetVWGridSelladoPaginacion(where, 0, 100000000).Select(x => x.docId).ToList();


            List<cerDocumento> documentos = documentosDAL.GetCerDocumentosByIds(ids);
            List<cerCompetenciaDocumento> competenciaDocumento = documentosDAL.GetCompetenciasDocumentoByIds(ids);
            List<cerCompetenciasIEMS> competenciasIEMS = documentosDAL.GetCompetenciasIEMSByDocIds(ids);
            List<cerUACDocumento> uacs = uacAL.GetListUACDocumento(ids);


            documentos.ForEach(x => x.cerCompetenciaDocumento = (from c in competenciaDocumento where c.docId == x.docId select c).ToList());
            documentos.ForEach(x => x.cerCompetenciasIEMS = (from c in competenciasIEMS where c.docId == x.docId select c).ToList());
            documentos.ForEach(x => x.cerUACDocumento = (from c in uacs where c.docId == x.docId select c).ToList());
             
            return documentos;
        }

        public string[] SellarRegistros(CriteriosBusquedaML criterios, FirmanteML firmanteML, PerfilML perfilML, string userLogin)
        {
            string[] resultado = new string[2];

            CerDocumentoDAL documentosDAL = new CerDocumentoDAL();
            criterios.estatus = "1";
            try
            {
                var firmanteConfiguracion = new FirmanteBL().GetFirmanteActivo(firmanteML.insId);
                firmanteML.firCargo = firmanteConfiguracion.firCargo;
                firmanteML.firIdCargo = firmanteConfiguracion.firIdCargo;
                firmanteML.firNombre = firmanteConfiguracion.firNombre;
                firmanteML.firPrimerApellido = firmanteConfiguracion.firPrimerApellido;
                firmanteML.firSegundoApellido = firmanteConfiguracion.firSegundoApellido;
                firmanteML.firNumeroCertificado = firmanteConfiguracion.firNumeroCertificado;
                firmanteML.firCurp = firmanteConfiguracion.firCurp;
                firmanteML.firCorreo = firmanteConfiguracion.firCorreo;
                firmanteML.firPredeterminado = firmanteConfiguracion.firPredeterminado;
                firmanteML.firCertificadoPublico = firmanteConfiguracion.firCertificadoPublico;
                var keyFirmante = MetodosGenericosBL.httpBaseToByte(firmanteML.archivoKey);
                Boolean passCorrecta = false;
                string sello = "";
                try
                {
                    sello = GenerarSello("Cadena de prueba", keyFirmante, firmanteML.contrasenia);
                    passCorrecta = true;
                }
                catch (Exception ex)
                {

                }

                if (passCorrecta)
                {

                    if (ValidarCertificado(firmanteML, "Cadena de prueba", sello, keyFirmante))
                    {

                        List<cerDocumento> documentos = GetCerDocumentos(criterios, perfilML);

                        foreach (var documento in documentos)
                        {
                            try
                            {
                                documento.docFirmaResponsableNombre = firmanteML.firNombre;
                                documento.docFirmaResponsablePrimerApellido = firmanteML.firPrimerApellido;
                                documento.docFirmaResponsableSegundoApellido = firmanteML.firSegundoApellido;
                                documento.docFirmaResponsableCurp = firmanteML.firCurp;
                                documento.docFirmaResponsableIdCargo = firmanteML.firIdCargo;
                                documento.docFirmaResponsableCargo = firmanteML.firCargo;

                                if (documento.docTipoId=="1")
                                {
                                    documento.docCadenaOriginal = GenerarCadenaOriginalTerminacion(LlenarObjetoCertificadoTerminacionMediaSuperior(documento),documento.docTipoId, documento.docPlanId);

                                }else
                                {
                                    documento.docCadenaOriginal = GenerarCadenaOriginalParcial(LlenarObjetoCertificadoParcialMediaSuperior(documento), documento.docTipoId, documento.docPlanId);
                                }



                                documento.docFirmaResponsableSello = GenerarSello(documento.docCadenaOriginal, keyFirmante, firmanteML.contrasenia);
                                documento.docFirmaResponsableCertificadoResponsable = firmanteML.firCertificadoPublico;
                                documento.docfirmaResponsableNoCertificadoResponsable = firmanteML.firNumeroCertificado;
                            }
                            catch (Exception ex1)
                            {
                                documentos.Remove(documento);
                            }
                        }

                        //Realizar guardado de la información 

                        var datatableDocumentos = MetodosGenericosBL.ConvertToDataTable((from doc in documentos
                                                                                         select new
                                                                                         {
                                                                                             docId = doc.docId,
                                                                                             docFirmaResponsableNombre = doc.docFirmaResponsableNombre,
                                                                                             docFirmaResponsablePrimerApellido = doc.docFirmaResponsablePrimerApellido,
                                                                                             docFirmaResponsableSegundoApellido = doc.docFirmaResponsableSegundoApellido,
                                                                                             docFirmaResponsableCurp = doc.docFirmaResponsableCurp,
                                                                                             docFirmaResponsableIdCargo = doc.docFirmaResponsableIdCargo,
                                                                                             docFirmaResponsableCargo = doc.docFirmaResponsableCargo,
                                                                                             docCadenaOriginal = doc.docCadenaOriginal,
                                                                                             docFirmaResponsableSello = doc.docFirmaResponsableSello,
                                                                                             docFirmaResponsableCertificadoResponsable = doc.docFirmaResponsableCertificadoResponsable,
                                                                                             docfirmaResponsableNoCertificadoResponsable = doc.docfirmaResponsableNoCertificadoResponsable,
                                                                                             fechaSellado=DateTime.Now 
                                                                                         }).ToList());
                        var resultadoImpactoDB = StoredProcedure.Merged(datatableDocumentos, "typeDocumentoSellado", "spMergedSelladoDocumento", "typeDocumentoSellado").Split('|');


                        if (resultadoImpactoDB[0] == "1")
                        {
                            resultado[0] = "True";
                            resultado[1] = "Se realizó el sellado de " + documentos.Count + " registros. Puedes consultar su estatus en el módulo de monitoreo SEP.";

                            Task[] tasks = new Task[]{
                    Task.Factory.StartNew(() => new EnvioSEPBL().ProcesarSolicitud(resultadoImpactoDB[1], userLogin))

            };

                             
                           

                        }
                    }
                    else
                    {
                        resultado[0] = "False";
                        resultado[1] = "La llave privada no corresponde al certificado público (.cer) autorizado para realizar el proceso del sellado.";
                    }
                }
                else
                {
                    resultado[0] = "False";
                    resultado[1] = "La contraseña es incorrecta.";

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                resultado[0] = "False";
                resultado[1] = "No fue posible realizar el sellado.";

            }

            return resultado;
        }

        public bool ValidarSellado(String cadenaOriginal, String selloCadenaOriginal, byte[] bytesCertificado)
        {
            bool banderaValidacion = false;


            X509Certificate2 certificado = new X509Certificate2(bytesCertificado);

            byte[] bytesCadenaOriginal = Encoding.UTF8.GetBytes(cadenaOriginal);
            byte[] bytesSellados = Convert.FromBase64String(selloCadenaOriginal);

            SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();
            RSACryptoServiceProvider RSA = (RSACryptoServiceProvider)certificado.PublicKey.Key;
            banderaValidacion = RSA.VerifyData(bytesCadenaOriginal, hasher, bytesSellados);

            return banderaValidacion;
        }
         
        /***********************************GenerarCadenaOriginal de Certificado de Terminación***************************************************************
         El nodo Sep del Documento Electrónico de Certificación, será integrado posterior a la validación realizada por la SEP. Dicho nodo no se integrará a la formación de la cadena original del Documento Electrónico de Certificación, las reglas de conformación de la cadena original se describen a continuación.
               ||a. version|b. tipoCertificado|a. curp|b. idCargo|a. nombreTipoDependencia|b. idIEMS||c. idOpcionEducativa|a. idTipoPlantel|b. idLocalidad|c. idEntidadFederativa|d. cct|e. claveRvoe|a. curp|b. nombre|c. primerApellido|d. segundoApellido|e. numeroControl|a. idTipoEstudiosIEMS|b. periodoInicio|c. periodoTermino|d. creditosObtenidos|e. totalCreditos|f. promedioAprovechamiento|a. nombre|b. calificacion|c. totalHorasUAC|a. trayecto|b. campoDisciplinar|c. tipoPerfilProfesionalIEMS|d. nombrePerfilProfesionalIEMS||
      *****************************************************************************************************/
        public static string GenerarCadenaOriginalTerminacion(Dec dec, String tipoCer, String planID) //tipoCER puede ser 1 o 2 (terminado o parcial)
        {
            string cadenaOriginal = "||";
            try
            {
                #region Dec
                if (!String.IsNullOrEmpty(dec.version))
                {
                    cadenaOriginal += dec.version;
                }

                cadenaOriginal += "|";

                if (!String.IsNullOrEmpty(tipoCer))
                {
                    //cadenaOriginal += planID;
                    //Cambio a 3.1 que agrega tipo de Certificado Nov2023
                    if (planID == "22")
                    {
                        cadenaOriginal += "12";
                    }
                    else if (planID == "33")
                    {
                        cadenaOriginal += "14";
                    }
                    else
                    {
                        cadenaOriginal += "1";
                    }
                    //Termina cambio
                }

                #endregion

                #region FirmaResponsable
                if (dec.FirmaResponsable != null)
                {
                    cadenaOriginal += "|";
                    if (!string.IsNullOrEmpty(dec.FirmaResponsable.curp))
                    {
                        cadenaOriginal += dec.FirmaResponsable.curp;
                    }

                    cadenaOriginal += "|";
                    if (!string.IsNullOrEmpty(dec.FirmaResponsable.idCargo))
                    {
                        cadenaOriginal +=   dec.FirmaResponsable.idCargo;
                    }
                }
                #endregion

                #region Servicio/IEMS

                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Iems.nombreDependencia))
                {
                    cadenaOriginal += dec.Iems.nombreDependencia;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Iems.idIEMS))
                {
                    cadenaOriginal +=  dec.Iems.idIEMS;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Iems.idOpcionEducativa))
                {
                    cadenaOriginal +=  dec.Iems.idOpcionEducativa;
                }

                #endregion

                #region PlantelOServicioEducativo 
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.PlantelOServicioEducativo.idTipoPlantel))
                {
                    cadenaOriginal +=   dec.PlantelOServicioEducativo.idTipoPlantel;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.PlantelOServicioEducativo.idMunicipio))
                {
                    cadenaOriginal +=   dec.PlantelOServicioEducativo.idMunicipio;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.PlantelOServicioEducativo.idEntidadFederativa))
                {
                    cadenaOriginal +=   dec.PlantelOServicioEducativo.idEntidadFederativa;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.PlantelOServicioEducativo.cct))
                {
                    cadenaOriginal +=   dec.PlantelOServicioEducativo.cct;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.PlantelOServicioEducativo.claveRvoe))
                {
                    cadenaOriginal +=   dec.PlantelOServicioEducativo.claveRvoe;
                }
                cadenaOriginal += "|";
                if ( dec.PlantelOServicioEducativo.fechaInicioRvoeSpecified)
                {
                    cadenaOriginal += dec.PlantelOServicioEducativo.fechaInicioRvoe;
                }

                #endregion

                #region Alumno

                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Alumno.curp))
                {
                    cadenaOriginal +=  dec.Alumno.curp;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Alumno.nombre))
                {
                    cadenaOriginal +=   dec.Alumno.nombre;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Alumno.primerApellido))
                {
                    cadenaOriginal +=   dec.Alumno.primerApellido;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Alumno.segundoApellido))
                {
                    cadenaOriginal +=  dec.Alumno.segundoApellido;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Alumno.numeroControl))
                {
                    cadenaOriginal +=  dec.Alumno.numeroControl;
                }
                #endregion

                #region Acreditacion 

                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.idTipoEstudiosIEMS))
                {
                    cadenaOriginal +=   dec.Acreditacion.idTipoEstudiosIEMS;
                }

                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.periodoInicio.ToString()))
                {
                    cadenaOriginal +=   dec.Acreditacion.periodoInicio.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                }

                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.periodoTermino.ToString()))
                {

                    cadenaOriginal +=  dec.Acreditacion.periodoTermino.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                }

                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.creditosObtenidos))
                {
                    cadenaOriginal +=  dec.Acreditacion.creditosObtenidos;
                }

                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.totalCreditos))
                {

                    cadenaOriginal +=   dec.Acreditacion.totalCreditos;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.promedioAprovechamiento))
                {

                    cadenaOriginal +=  dec.Acreditacion.promedioAprovechamiento;
                }

                #endregion

                #region Competencias;
                if (dec.UacsdeFt != null && dec.UacsdeFt.Length > 0)
                {
                    foreach (var obj in dec.UacsdeFt)
                    {

                        cadenaOriginal += "|";
                        if (!string.IsNullOrEmpty(obj.nombre))
                        {
                            cadenaOriginal += obj.nombre;
                        }
                        cadenaOriginal += "|";
                        if (!string.IsNullOrEmpty(obj.calificacion))
                        {
                            cadenaOriginal +=   obj.calificacion;
                        }

                        cadenaOriginal += "|";
                        if (!string.IsNullOrEmpty(obj.totalHorasUAC))
                        {
                            cadenaOriginal +=   obj.totalHorasUAC;
                        }
                    }
                }
                #endregion

                #region Egreso competencias
                if (dec.PerfilEgresoEspecifico != null)
                {

                    cadenaOriginal += "|";
                    if (!string.IsNullOrEmpty(dec.PerfilEgresoEspecifico.trayecto))
                    {
                        cadenaOriginal +=  dec.PerfilEgresoEspecifico.trayecto;
                    }
                    cadenaOriginal += "|";
                    if (!string.IsNullOrEmpty(dec.PerfilEgresoEspecifico.campoDisciplinar))
                    {
                        cadenaOriginal +=   dec.PerfilEgresoEspecifico.campoDisciplinar;
                    }
                    cadenaOriginal += "|";
                    if (!string.IsNullOrEmpty(dec.PerfilEgresoEspecifico.tipoPerfilLaboralEMS))
                    {
                        cadenaOriginal +=   dec.PerfilEgresoEspecifico.tipoPerfilLaboralEMS;
                    }
                    cadenaOriginal += "|";
                    if (!string.IsNullOrEmpty(dec.PerfilEgresoEspecifico.nombrePerfilLaboralEMS))
                    {
                        cadenaOriginal += dec.PerfilEgresoEspecifico.nombrePerfilLaboralEMS;
                    }

                }
                #endregion


                return cadenaOriginal += "||";
            }
            catch (Exception ex) { Console.Write(ex); return null; }
        }

        /***********************************GenerarCadenaOriginal de Certificado Parcial***************************************************************
         El nodo Sep del Documento Electrónico de Certificación, será integrado posterior a la validación realizada por la SEP. Dicho nodo no se integrará a la formación de la cadena original del Documento Electrónico de Certificación, las reglas de conformación de la cadena original se describen a continuación.
               ||a. version|b. tipoCertificado|a. curp|b. idCargo|a. nombreTipoDependencia|b. idIEMS|c. idOpcionEducativa|a. idTipoPlantel|b. idLocalidad|c. idEntidadFederativa|d. cct|e. claveRvoe|a. curp|b. nombre|c. primerApellido|d. segundoApellido|e. numeroControl|a. idTipoEstudiosIEMS|b. periodoInicio|c. periodoTermino|d. creditosObtenidos|e. totalCreditos|f. promedioAprovechamiento|a.idTipoPeriodo|a.cct|b.nombre|c.calificación|d.periodoEscolar|e.numeroPeriodo||       
        
      *****************************************************************************************************/
        public static string GenerarCadenaOriginalParcial(BusinessModel.Models.Parcial.Dec dec, String tipoCer, String planID)
        {
            string cadenaOriginal = "||";
            try
            {
                #region Dec
                if (!String.IsNullOrEmpty(dec.version))
                {
                    cadenaOriginal += dec.version;
                }

                cadenaOriginal += "|";
                if (!String.IsNullOrEmpty(tipoCer))
                {
                    //cadenaOriginal += planID;
                    //Cambio a 3.1 que agrega tipo de Certificado Nov2023
                    if (planID == "22")
                    {
                        cadenaOriginal += "13";
                    }
                    else if (planID == "33")
                    {
                        cadenaOriginal += "15";
                    }
                    else
                    {
                        cadenaOriginal += "2";
                    }
                    //Termina cambio
                }

                #endregion

                #region FirmaResponsable
                if (dec.FirmaResponsable != null)
                {
                    cadenaOriginal += "|";
                    if (!string.IsNullOrEmpty(dec.FirmaResponsable.curp))
                    {
                        cadenaOriginal += dec.FirmaResponsable.curp;
                    }

                    cadenaOriginal += "|";
                    if (!string.IsNullOrEmpty(dec.FirmaResponsable.idCargo))
                    {
                        cadenaOriginal += dec.FirmaResponsable.idCargo;
                    }
                }
                #endregion

                #region IEMS 

                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Iems.nombreDependencia))
                {
                    cadenaOriginal += dec.Iems.nombreDependencia;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Iems.idIEMS))
                {
                    cadenaOriginal += dec.Iems.idIEMS;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Iems.idOpcionEducativa))
                {
                    cadenaOriginal += dec.Iems.idOpcionEducativa;
                }

                #endregion

                #region PlantelOServicioEducativo 
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.PlantelOServicioEducativo.idTipoPlantel))
                {
                    cadenaOriginal += dec.PlantelOServicioEducativo.idTipoPlantel;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.PlantelOServicioEducativo.idMunicipio))
                {
                    cadenaOriginal += dec.PlantelOServicioEducativo.idMunicipio;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.PlantelOServicioEducativo.idEntidadFederativa))
                {
                    cadenaOriginal += dec.PlantelOServicioEducativo.idEntidadFederativa;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.PlantelOServicioEducativo.cct))
                {
                    cadenaOriginal += dec.PlantelOServicioEducativo.cct;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.PlantelOServicioEducativo.claveRvoe))
                {
                    cadenaOriginal += dec.PlantelOServicioEducativo.claveRvoe;
                }
                cadenaOriginal += "|";
                if (dec.PlantelOServicioEducativo.fechaInicioRvoeSpecified)
                {
                    cadenaOriginal += dec.PlantelOServicioEducativo.fechaInicioRvoe;
                }

                #endregion

                #region Alumno

                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Alumno.curp))
                {
                    cadenaOriginal += dec.Alumno.curp;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Alumno.nombre))
                {
                    cadenaOriginal += dec.Alumno.nombre;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Alumno.primerApellido))
                {
                    cadenaOriginal += dec.Alumno.primerApellido;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Alumno.segundoApellido))
                {
                    cadenaOriginal += dec.Alumno.segundoApellido;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Alumno.numeroControl))
                {
                    cadenaOriginal += dec.Alumno.numeroControl;
                }
                #endregion

                #region Acreditacion 

                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.idTipoEstudiosIEMS))
                {
                    cadenaOriginal += dec.Acreditacion.idTipoEstudiosIEMS;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.periodoInicio.ToString()))
                {
                    cadenaOriginal += dec.Acreditacion.periodoInicio.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.periodoTermino.ToString()))
                {
                    cadenaOriginal += dec.Acreditacion.periodoTermino.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.creditosObtenidos))
                {
                    cadenaOriginal += dec.Acreditacion.creditosObtenidos;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.totalCreditos))
                {
                    cadenaOriginal += dec.Acreditacion.totalCreditos;
                }
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Acreditacion.promedioAprovechamiento))
                {
                    cadenaOriginal += dec.Acreditacion.promedioAprovechamiento;
                }

                #endregion

                #region Uacs
                cadenaOriginal += "|";
                if (!string.IsNullOrEmpty(dec.Uacs.idTipoPeriodo))
                {
                    cadenaOriginal += dec.Uacs.idTipoPeriodo;
                }
                #endregion

                #region UAC;
                if (dec.Uacs != null && dec.Uacs.Uac.Any()  && dec.Uacs.Uac.Length > 0)
                {
                    foreach (var uac in dec.Uacs.Uac)
                    {

                        cadenaOriginal += "|";
                        if (!string.IsNullOrEmpty(uac.cct))
                        {
                            cadenaOriginal += uac.cct;
                        }


                        cadenaOriginal += "|";
                        if (!string.IsNullOrEmpty(uac.nombreUAC))
                        {
                            cadenaOriginal += uac.nombreUAC;
                        }
                        cadenaOriginal += "|";
                        if (!string.IsNullOrEmpty(uac.calificacionUAC))
                        {
                            cadenaOriginal += uac.calificacionUAC;
                        }

                        cadenaOriginal += "|";
                        if (!string.IsNullOrEmpty(uac.periodoEscolarUAC))
                        {
                            cadenaOriginal += uac.periodoEscolarUAC;
                        }

                        cadenaOriginal += "|";
                        if (!string.IsNullOrEmpty(uac.numeroPeriodoUAC))
                        {
                            cadenaOriginal += uac.numeroPeriodoUAC;
                        }
                    }
                }
                #endregion
                 

                return cadenaOriginal += "||";
            }
            catch (Exception ex) { Console.Write(ex); return null; }
        }

        public bool ValidarCertificado(FirmanteML firmante, string cadenaOriginal, string sello, byte[] keyFirmante)
        {
            bool resultado = false;
            try
            {
                resultado = ValidarSellado(cadenaOriginal, sello, Encoding.UTF8.GetBytes(firmante.firCertificadoPublico));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);


            }
            return resultado;
        }

        public bool ValidarSellado(String cadenaOriginal, String selloCadenaOriginal, X509Certificate2 certificado)
        {
            bool banderaValidacion = false;
            byte[] bytesCadenaOriginal = Encoding.UTF8.GetBytes(cadenaOriginal);
            byte[] bytesSellados = Convert.FromBase64String(selloCadenaOriginal);

            SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();
            RSACryptoServiceProvider RSA = (RSACryptoServiceProvider)certificado.PublicKey.Key;
            banderaValidacion = RSA.VerifyData(bytesCadenaOriginal, hasher, bytesSellados);

            return banderaValidacion;
        }

        public static string GenerarSello(string cadenaOriginal, byte[] fileKey, string password)
        {
            string strLlavePwd = password;
            System.Security.SecureString passwordSeguro = new System.Security.SecureString();
            passwordSeguro.Clear();

            foreach (char c in strLlavePwd.ToCharArray())
                passwordSeguro.AppendChar(c);

            SHA256CryptoServiceProvider hasher = new SHA256CryptoServiceProvider();
            RSACryptoServiceProvider rsa = OpenSSLKey.DecodeEncryptedPrivateKeyInfo(fileKey, passwordSeguro);
            byte[] bytesFirmados = rsa.SignData(System.Text.Encoding.UTF8.GetBytes(cadenaOriginal), hasher);

            return Convert.ToBase64String(bytesFirmados);
        }

        public static Dec LlenarObjetoCertificadoTerminacionMediaSuperior(cerDocumento documento)
        {
            Dec oCertificadoMediaSuperior = new Dec();


            oCertificadoMediaSuperior.Acreditacion = new DecAcreditacion();
            oCertificadoMediaSuperior.Acreditacion.clavePlanEstudios = documento.docAcreditacionClavePlanEstudios;
            oCertificadoMediaSuperior.Acreditacion.creditosObtenidos = documento.docAcreditacionCreditosObtenidos;
            oCertificadoMediaSuperior.Acreditacion.idTipoEstudiosIEMS = documento.docAcreditacionIdTipoEstudiosIEMS;
            oCertificadoMediaSuperior.Acreditacion.tipoEstudiosIEMS = documento.docAcreditacionTipoEstudiosIEMS;
            oCertificadoMediaSuperior.Acreditacion.tipoPerfilLaboralEMS = documento.docAcreditacionTipoPerfilProfesionalIEMS;
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



            oCertificadoMediaSuperior.UacsdeFt = (from l in documento.cerCompetenciaDocumento
                                                      orderby l.idPeriodo,l.orden
                                                      select new DecUacdeFt
                                                      {
                                                          calificacion= l.competenciaCalificacion,
                                                          creditos = l.competenciaCreditos,
                                                          dictamen = l.competenciaDictamen,
                                                          nombre = l.competenciaNombre,
                                                          totalHorasUAC = l.competenciaTotalHorasUAC
                                                      }).ToArray();



            oCertificadoMediaSuperior.PerfilEgresoEspecifico = new DecPerfilEgresoEspecifico();
            oCertificadoMediaSuperior.PerfilEgresoEspecifico.campoDisciplinar = documento.docEgresoCompetenciasCampoDisciplinar;
            oCertificadoMediaSuperior.PerfilEgresoEspecifico.CompetenciasEspecificas = (from l in documento.cerCompetenciasIEMS
                                                                            orderby l.CompetenciasIEMSorden
                                                                            select new DecPerfilEgresoEspecificoCompetenciasEspecificas
                                                                            {
                                                                                nombreCompetenciasLaborales = l.CompetenciasIEMSnombreProfesional
                                                                            }).ToArray();



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
            }
            else if (documento.docPlanId == "33")
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
            oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoeSpecified = documento.docPlantelOServicioEducativoFechaInicioRVOE == null ? false:true;
            if (oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoeSpecified)
            {
                oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoe = documento.docPlantelOServicioEducativoFechaInicioRVOE.Value;
            }
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



            oCertificadoMediaSuperior.Iems = new DecIems();
            oCertificadoMediaSuperior.Iems.idIEMS = documento.docServicioIdIEMSidIEMS;
            oCertificadoMediaSuperior.Iems.idOpcionEducativa = documento.docServicioIdOpcionEducativa;
            oCertificadoMediaSuperior.Iems.nombreIEMS = documento.docServicioNombreIEMS;
            oCertificadoMediaSuperior.Iems.nombreDependencia = documento.docServicioNombreTipoDependencia;
            oCertificadoMediaSuperior.Iems.opcionEducativa = documento.docServicioOpcionEducativa;



            return oCertificadoMediaSuperior;
        }
        public static BusinessModel.Models.Parcial.Dec LlenarObjetoCertificadoParcialMediaSuperior(cerDocumento documento)
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

            oCertificadoMediaSuperior.Alumno = new BusinessModel.Models.Parcial.DecAlumno();
            oCertificadoMediaSuperior.Alumno.curp = documento.docAlumnoCurp;
            oCertificadoMediaSuperior.Alumno.nombre = documento.docAlumnoNombre;
            oCertificadoMediaSuperior.Alumno.numeroControl = documento.docAlumnoNumeroControl;
            oCertificadoMediaSuperior.Alumno.primerApellido = documento.docAlumnoPrimerApellido;
            oCertificadoMediaSuperior.Alumno.segundoApellido = documento.docAlumnoSegundoApellido;

            oCertificadoMediaSuperior.Uacs = new Models.Parcial.DecUacs();
            oCertificadoMediaSuperior.Uacs.nombreTipoPeriodo = documento.docUACSnombreTipoPeriodo;
            oCertificadoMediaSuperior.Uacs.idTipoPeriodo = documento.docUACSidTipoPeriodo;

            oCertificadoMediaSuperior.Uacs.Uac = (from l in documento.cerUACDocumento  orderby l.idPeriodo,l.orden
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
            oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoeSpecified = documento.docPlantelOServicioEducativoFechaInicioRVOE == null ? false : true;
            if (oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoeSpecified)
            {
                oCertificadoMediaSuperior.PlantelOServicioEducativo.fechaInicioRvoe = documento.docPlantelOServicioEducativoFechaInicioRVOE.Value;
            }
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

            oCertificadoMediaSuperior.Iems = new BusinessModel.Models.Parcial.DecIems();
            oCertificadoMediaSuperior.Iems.idIEMS = documento.docServicioIdIEMSidIEMS;
            oCertificadoMediaSuperior.Iems.idOpcionEducativa = documento.docServicioIdOpcionEducativa;
            oCertificadoMediaSuperior.Iems.nombreIEMS = documento.docServicioNombreIEMS;
            oCertificadoMediaSuperior.Iems.nombreDependencia = documento.docServicioNombreTipoDependencia;
            oCertificadoMediaSuperior.Iems.opcionEducativa = documento.docServicioOpcionEducativa;

            return oCertificadoMediaSuperior;
        }

    }
}
