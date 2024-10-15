using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModel.Models
{
    public class DatosXML
    {
        public String TituloElectronico_version { get; set; }
        public String TituloElectronico_folioControl { get; set; }
        public String FirmaResponsable_nombre { get; set; }
        public String FirmaResponsable_primerApellido { get; set; }
        public String FirmaResponsable_segundoApellido { get; set; }
        public String FirmaResponsable_curp { get; set; }
        public String FirmaResponsable_idCargo { get; set; }
        public String FirmaResponsable_cargo { get; set; }
        public String FirmaResponsable_abrTitulo { get; set; }
        public String FirmaResponsable_sello { get; set; }
        public String FirmaResponsable_certificadoResponsable { get; set; }
        public String FirmaResponsable_noCertificadoResponsable { get; set; }
        public String Institucion_cveInstitucion { get; set; }
        public String Institucion_nombreInstitucion { get; set; }
        public String Carrera_cveCarrera { get; set; }
        public String Carrera_nombreCarrera { get; set; }
        public String Carrera_fechaInicio { get; set; }
        public String Carrera_fechaTerminacion { get; set; }
        public String Carrera_idAutorizacionReconocimiento { get; set; }
        public String Carrera_autorizacionReconocimiento { get; set; }
        public String Carrera_numeroRvoe { get; set; }
        public String Profesionista_curp { get; set; }
        public String Profesionista_nombre { get; set; }
        public String Profesionista_primerApellido { get; set; }
        public String Profesionista_segundoApellido { get; set; }
        public String Profesionista_correoElectronico { get; set; }
        public String Expedicion_fechaExpedicion { get; set; }
        public String Expedicion_idModalidadTitulacion { get; set; }
        public String Expedicion_modalidadTitulacion { get; set; }
        public String Expedicion_fechaExamenProfesional { get; set; }
        public String Expedicion_fechaExencionExamenProfesional { get; set; }
        public String Expedicion_idFundamentoLegalServicioSocial { get; set; }
        public String Expedicion_fundamentoLegalServicioSocial { get; set; }
        public String Expedicion_idEntidadFederativa { get; set; }
        public String Expedicion_entidadFederativa { get; set; }
        public String Antecedente_institucionProcedencia { get; set; }
        public String Antecedente_idTipoEstudioAntecedente { get; set; }
        public String Antecedente_tipoEstudioAntecedente { get; set; }
        public String Antecedente_idEntidadFederativa { get; set; }
        public String Antecedente_entidadFederativa { get; set; }
        public String Antecedente_fechaInicio { get; set; }
        public String Antecedente_fechaTerminacion { get; set; }
        public String Antecedente_noCedula { get; set; }

    }
}
