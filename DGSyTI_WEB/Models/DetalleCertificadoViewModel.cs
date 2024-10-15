using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DGSyTI_WEB.Models
{
    public class DetalleCertificadoViewModel
    {
        public string docId { get; set; }
        public string docTipoId { get; set; }
        public string docPlanId { get; set; }
        public string docAreaEspecializacion { get; set; }
        public string planId { get; set; }
        public string carId { get; set; }
        public string solId { get; set; }
        public string estDocumentoId { get; set; }
        public string docCadenaOriginal { get; set; }
        public string docDecFolioControl { get; set; }
        public string docDecTipoCertificado { get; set; }
        public string docFirmaResponsableNombre { get; set; }
        public string docFirmaResponsablePrimerApellido { get; set; }
        public string docFirmaResponsableSegundoApellido { get; set; }
        public string docFirmaResponsableCurp { get; set; }
        public string docFirmaResponsableIdCargo { get; set; }
        public string docFirmaResponsableCargo { get; set; }
        public string docFirmaResponsableSello { get; set; }
        public string docFirmaResponsableCertificadoResponsable { get; set; }
        public string docfirmaResponsableNoCertificadoResponsable { get; set; }
        public string docServicioNombreTipoDependencia { get; set; }
        public string docServicioIdIEMSidIEMS { get; set; }
        public string docServicioNombreIEMS { get; set; }
        public string docServicioIdOpcionEducativa { get; set; }
        public string docServicioOpcionEducativa { get; set; }
        public string docPlantelIdTipoPlantel { get; set; }
        public string docPlantelTipoPlantel { get; set; }
        public string docPlantelNombreNumeroPlantel { get; set; }
        public string docPlantelIdLocalidad { get; set; }
        public string docPlantelLocalidad { get; set; }
        public string docPlantelIdEntidadFederativa { get; set; }
        public string docPlantelEntidadFederativa { get; set; }
        public string docPlantelCCT { get; set; }
        public string docPlantelClaveRvoe { get; set; }
        public string docplantelIdGeneroPlantel { get; set; }
        public string docPlantelGeneroPlantel { get; set; }
        public string docPlantelIdSostenimiento { get; set; }
        public string docPlantelSostenimiento { get; set; }
        public string docAlumnoNombre { get; set; }
        public string docAlumnoPrimerApellido { get; set; }
        public string docAlumnoSegundoApellido { get; set; }
        public string docAlumnoCurp { get; set; }
        public string docAlumnoNumeroControl { get; set; }
        public string docAcreditacionIdTipoEstudiosIEMS { get; set; }
        public string docAcreditacionTipoEstudiosIEMS { get; set; }
        public string docAcreditacionTipoPerfilProfesionalIEMS { get; set; }
        public string docAcreditacionNombreTipoPerfilProfesionalIEMS { get; set; }
        public string docAcreditacionClavePrograma { get; set; }
        public string docAcreditacionPeriodoInicio { get; set; }
        public string docAcreditacionPeriodoTermino { get; set; }
        public string docAcreditacionCreditosObtenidos { get; set; }
        public string docAcreditacionTotalCreditos { get; set; }
        public string docAcreditacionPromedioAprovechamiento { get; set; }
        public string docAcreditacionPromedioAprovechamientoTexto { get; set; }
        public string docEgresoCompetenciasTrayecto { get; set; }
        public string docEgresoCompetenciasIdCampoDisciplinar { get; set; }
        public string docEgresoCompetenciasCampoDisciplinar { get; set; }
        public string docEgresoCompetenciasTipoPerfilProfesionalIEMS { get; set; }
        public string docEgresoCompetenciasNombrePerfilProfesionalIEMS { get; set; }
        public string docSepFolioDigital { get; set; }
        public string docSepFechaSep { get; set; }
        public string docSepSelloDec { get; set; }
        public string docSepNoCertificadoSep { get; set; }
        public string docSepSelloSep { get; set; }
        public string docXMLEnvio { get; set; }
        public string docXMLRetorno { get; set; }
        public string docCorreo { get; set; }
        public string insId { get; set; }
        public DateTime docFechaRegistro { get; set; }
        public DateTime docFechaSellado { get; set; }
        public string docDecVersion { get; set; }
        public string docObservaciones { get; set; }
        public string docMensajeResultado { get; set; }
        public string docSEPVersion { get; set; }
        public string sDocTipoCertificado { get; set; }
        public DateTime? docPlantelOServicioEducativoFechaInicioRVOE { get; set; }
        public string docAcreditacionidNivelEstudios { get; set; }
        public string docAcreditacionNivelEstudios { get; set; }

        public string docServicioIemsNombreSEN { get; set; }
        public string docServicioIemsnombreIEMSparticular { get; set; }
        public string docServicioIemsInstitucionRVOE { get; set; }
        public string docServicioIemsIdTipoIEMS { get; set; }
        public string docServicioIemsTipoIEMS { get; set; }
        public string docUACSidTipoPeriodo { get; set; }
        public string docUACSnombreTipoPeriodo { get; set; }

        public List<cerCompetenciaDocumento> cerCompetenciaDocumento { get; set; } = new List<cerCompetenciaDocumento>();
        public List<cerCompetenciasIEMS> cerCompetenciasIEMS { get; set; } = new List<cerCompetenciasIEMS>();

        public List<cerUACDocumento> listUACDocumento { set; get; } = new List<cerUACDocumento>();
    }

    public class cerCompetenciaDocumento
    {
        public string idMateria { get; set; }
        public string docId { get; set; }
        public string competenciaNombre { get; set; }
        public string competenciaCalificacion { get; set; }
        public string competenciaDictamen { get; set; }
        public string competenciaTotalHorasUAC { get; set; }
        public string competenciaCreditos { get; set; }
        public string orden { get; set; }
    }

    public class cerCompetenciasIEMS
    {
        public string CompetenciasIEMSnombreProfesional { get; set; }
    }


}