﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;
namespace BusinessModel.Models.Parcial
{

    // 
    // Este código fuente fue generado automáticamente por xsd, Versión=4.7.3081.0.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/certificados/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.siged.sep.gob.mx/certificados/", IsNullable = false)]
    
    public partial class Dec
    {

        private DecFirmaResponsable firmaResponsableField;

        private DecIems iemsField;

        private DecPlantelOServicioEducativo plantelOServicioEducativoField;

        private DecAlumno alumnoField;

        private DecAcreditacion acreditacionField;

        private DecUacs uacsField;

        private DecSep sepField;

        private string versionField;

        private string tipoCertificadoField;

        private string folioControlField;

        public Dec()
        {
            this.versionField = "2.0";
            this.tipoCertificadoField = "2";
        }

        /// <remarks/>
        public DecFirmaResponsable FirmaResponsable
        {
            get
            {
                return this.firmaResponsableField;
            }
            set
            {
                this.firmaResponsableField = value;
            }
        }

        /// <remarks/>
        public DecIems Iems
        {
            get
            {
                return this.iemsField;
            }
            set
            {
                this.iemsField = value;
            }
        }

        /// <remarks/>
        public DecPlantelOServicioEducativo PlantelOServicioEducativo
        {
            get
            {
                return this.plantelOServicioEducativoField;
            }
            set
            {
                this.plantelOServicioEducativoField = value;
            }
        }

        /// <remarks/>
        public DecAlumno Alumno
        {
            get
            {
                return this.alumnoField;
            }
            set
            {
                this.alumnoField = value;
            }
        }

        /// <remarks/>
        public DecAcreditacion Acreditacion
        {
            get
            {
                return this.acreditacionField;
            }
            set
            {
                this.acreditacionField = value;
            }
        }

        /// <remarks/>
        public DecUacs Uacs
        {
            get
            {
                return this.uacsField;
            }
            set
            {
                this.uacsField = value;
            }
        }

        /// <remarks/>
        public DecSep Sep
        {
            get
            {
                return this.sepField;
            }
            set
            {
                this.sepField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string tipoCertificado
        {
            get
            {
                return this.tipoCertificadoField;
            }
            set
            {
                this.tipoCertificadoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string folioControl
        {
            get
            {
                return this.folioControlField;
            }
            set
            {
                this.folioControlField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/certificados/")]
    public partial class DecFirmaResponsable
    {

        private string nombreField;

        private string primerApellidoField;

        private string segundoApellidoField;

        private string curpField;

        private string idCargoField;

        private string cargoField;

        private string selloField;

        private string certificadoResponsableField;

        private string noCertificadoResponsableField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombre
        {
            get
            {
                return this.nombreField;
            }
            set
            {
                this.nombreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string primerApellido
        {
            get
            {
                return this.primerApellidoField;
            }
            set
            {
                this.primerApellidoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string segundoApellido
        {
            get
            {
                return this.segundoApellidoField;
            }
            set
            {
                this.segundoApellidoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string curp
        {
            get
            {
                return this.curpField;
            }
            set
            {
                this.curpField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idCargo
        {
            get
            {
                return this.idCargoField;
            }
            set
            {
                this.idCargoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cargo
        {
            get
            {
                return this.cargoField;
            }
            set
            {
                this.cargoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sello
        {
            get
            {
                return this.selloField;
            }
            set
            {
                this.selloField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string certificadoResponsable
        {
            get
            {
                return this.certificadoResponsableField;
            }
            set
            {
                this.certificadoResponsableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string noCertificadoResponsable
        {
            get
            {
                return this.noCertificadoResponsableField;
            }
            set
            {
                this.noCertificadoResponsableField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/certificados/")]
    public partial class DecIems
    {

        private string nombreSENField;

        private string nombreDependenciaField;

        private string idIEMSField;

        private string nombreIEMSField;

        private string idTipoIEMSField;

        private string tipoIEMSField;

        private string nombreIEMSparticularField;

        private string institucionRVOEField;

        private string idOpcionEducativaField;

        private string opcionEducativaField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombreSEN
        {
            get
            {
                return this.nombreSENField;
            }
            set
            {
                this.nombreSENField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombreDependencia
        {
            get
            {
                return this.nombreDependenciaField;
            }
            set
            {
                this.nombreDependenciaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idIEMS
        {
            get
            {
                return this.idIEMSField;
            }
            set
            {
                this.idIEMSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombreIEMS
        {
            get
            {
                return this.nombreIEMSField;
            }
            set
            {
                this.nombreIEMSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idTipoIEMS
        {
            get
            {
                return this.idTipoIEMSField;
            }
            set
            {
                this.idTipoIEMSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipoIEMS
        {
            get
            {
                return this.tipoIEMSField;
            }
            set
            {
                this.tipoIEMSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombreIEMSparticular
        {
            get
            {
                return this.nombreIEMSparticularField;
            }
            set
            {
                this.nombreIEMSparticularField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string institucionRVOE
        {
            get
            {
                return this.institucionRVOEField;
            }
            set
            {
                this.institucionRVOEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idOpcionEducativa
        {
            get
            {
                return this.idOpcionEducativaField;
            }
            set
            {
                this.idOpcionEducativaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string opcionEducativa
        {
            get
            {
                return this.opcionEducativaField;
            }
            set
            {
                this.opcionEducativaField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/certificados/")]
    public partial class DecPlantelOServicioEducativo
    {

        private string idTipoPlantelField;

        private string tipoPlantelField;

        private string nombreNumeroPlantelField;

        private string idMunicipioField;

        private string municipioField;

        private string idEntidadFederativaField;

        private string entidadFederativaField;

        private string cctField;

        private string claveRvoeField;

        private System.DateTime fechaInicioRvoeField;

        private bool fechaInicioRvoeFieldSpecified;

        private string idGeneroPlantelField;

        private string generoPlantelField;

        private string idSostenimientoField;

        private string sostenimientoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idTipoPlantel
        {
            get
            {
                return this.idTipoPlantelField;
            }
            set
            {
                this.idTipoPlantelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipoPlantel
        {
            get
            {
                return this.tipoPlantelField;
            }
            set
            {
                this.tipoPlantelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombreNumeroPlantel
        {
            get
            {
                return this.nombreNumeroPlantelField;
            }
            set
            {
                this.nombreNumeroPlantelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string idMunicipio
        {
            get
            {
                return this.idMunicipioField;
            }
            set
            {
                this.idMunicipioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string municipio
        {
            get
            {
                return this.municipioField;
            }
            set
            {
                this.municipioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string idEntidadFederativa
        {
            get
            {
                return this.idEntidadFederativaField;
            }
            set
            {
                this.idEntidadFederativaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string entidadFederativa
        {
            get
            {
                return this.entidadFederativaField;
            }
            set
            {
                this.entidadFederativaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cct
        {
            get
            {
                return this.cctField;
            }
            set
            {
                this.cctField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string claveRvoe
        {
            get
            {
                return this.claveRvoeField;
            }
            set
            {
                this.claveRvoeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime fechaInicioRvoe
        {
            get
            {
                return this.fechaInicioRvoeField;
            }
            set
            {
                this.fechaInicioRvoeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool fechaInicioRvoeSpecified
        {
            get
            {
                return this.fechaInicioRvoeFieldSpecified;
            }
            set
            {
                this.fechaInicioRvoeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idGeneroPlantel
        {
            get
            {
                return this.idGeneroPlantelField;
            }
            set
            {
                this.idGeneroPlantelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string generoPlantel
        {
            get
            {
                return this.generoPlantelField;
            }
            set
            {
                this.generoPlantelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idSostenimiento
        {
            get
            {
                return this.idSostenimientoField;
            }
            set
            {
                this.idSostenimientoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sostenimiento
        {
            get
            {
                return this.sostenimientoField;
            }
            set
            {
                this.sostenimientoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/certificados/")]
    public partial class DecAlumno
    {

        private string nombreField;

        private string primerApellidoField;

        private string segundoApellidoField;

        private string curpField;

        private string numeroControlField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombre
        {
            get
            {
                return this.nombreField;
            }
            set
            {
                this.nombreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string primerApellido
        {
            get
            {
                return this.primerApellidoField;
            }
            set
            {
                this.primerApellidoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string segundoApellido
        {
            get
            {
                return this.segundoApellidoField;
            }
            set
            {
                this.segundoApellidoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string curp
        {
            get
            {
                return this.curpField;
            }
            set
            {
                this.curpField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string numeroControl
        {
            get
            {
                return this.numeroControlField;
            }
            set
            {
                this.numeroControlField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/certificados/")]
    public partial class DecAcreditacion
    {

        private string idNivelEstudiosField;

        private string nivelEstudiosField;

        private string idTipoEstudiosIEMSField;

        private string tipoEstudiosIEMSField;

        private string tipoPerfilLaboralEMSField;

        private string nombreTipoPerfilLaboralEMSField;

        private System.DateTime periodoInicioField;

        private System.DateTime periodoTerminoField;

        private string creditosObtenidosField;

        private string totalCreditosField;

        private string promedioAprovechamientoField;

        private string promedioAprovechamientoTextoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idNivelEstudios
        {
            get
            {
                return this.idNivelEstudiosField;
            }
            set
            {
                this.idNivelEstudiosField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nivelEstudios
        {
            get
            {
                return this.nivelEstudiosField;
            }
            set
            {
                this.nivelEstudiosField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idTipoEstudiosIEMS
        {
            get
            {
                return this.idTipoEstudiosIEMSField;
            }
            set
            {
                this.idTipoEstudiosIEMSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipoEstudiosIEMS
        {
            get
            {
                return this.tipoEstudiosIEMSField;
            }
            set
            {
                this.tipoEstudiosIEMSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipoPerfilLaboralEMS
        {
            get
            {
                return this.tipoPerfilLaboralEMSField;
            }
            set
            {
                this.tipoPerfilLaboralEMSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombreTipoPerfilLaboralEMS
        {
            get
            {
                return this.nombreTipoPerfilLaboralEMSField;
            }
            set
            {
                this.nombreTipoPerfilLaboralEMSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime periodoInicio
        {
            get
            {
                return this.periodoInicioField;
            }
            set
            {
                this.periodoInicioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime periodoTermino
        {
            get
            {
                return this.periodoTerminoField;
            }
            set
            {
                this.periodoTerminoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string creditosObtenidos
        {
            get
            {
                return this.creditosObtenidosField;
            }
            set
            {
                this.creditosObtenidosField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string totalCreditos
        {
            get
            {
                return this.totalCreditosField;
            }
            set
            {
                this.totalCreditosField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string promedioAprovechamiento
        {
            get
            {
                return this.promedioAprovechamientoField;
            }
            set
            {
                this.promedioAprovechamientoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string promedioAprovechamientoTexto
        {
            get
            {
                return this.promedioAprovechamientoTextoField;
            }
            set
            {
                this.promedioAprovechamientoTextoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/certificados/")]
    public partial class DecUacs
    {

        private DecUacsUac[] uacField;

        private string idTipoPeriodoField;

        private string nombreTipoPeriodoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Uac")]
        public DecUacsUac[] Uac
        {
            get
            {
                return this.uacField;
            }
            set
            {
                this.uacField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idTipoPeriodo
        {
            get
            {
                return this.idTipoPeriodoField;
            }
            set
            {
                this.idTipoPeriodoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombreTipoPeriodo
        {
            get
            {
                return this.nombreTipoPeriodoField;
            }
            set
            {
                this.nombreTipoPeriodoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/certificados/")]
    public partial class DecUacsUac
    {

        private string cctField;

        private string idTipoUACField;

        private string tipoUACField;

        private string nombreUACField;

        private string calificacionUACField;

        private string dictamenUACField;

        private string totalHorasUACField;

        private string creditosUACField;

        private string periodoEscolarUACField;

        private string numeroPeriodoUACField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cct
        {
            get
            {
                return this.cctField;
            }
            set
            {
                this.cctField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string idTipoUAC
        {
            get
            {
                return this.idTipoUACField;
            }
            set
            {
                this.idTipoUACField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipoUAC
        {
            get
            {
                return this.tipoUACField;
            }
            set
            {
                this.tipoUACField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombreUAC
        {
            get
            {
                return this.nombreUACField;
            }
            set
            {
                this.nombreUACField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string calificacionUAC
        {
            get
            {
                return this.calificacionUACField;
            }
            set
            {
                this.calificacionUACField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dictamenUAC
        {
            get
            {
                return this.dictamenUACField;
            }
            set
            {
                this.dictamenUACField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string totalHorasUAC
        {
            get
            {
                return this.totalHorasUACField;
            }
            set
            {
                this.totalHorasUACField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string creditosUAC
        {
            get
            {
                return this.creditosUACField;
            }
            set
            {
                this.creditosUACField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string periodoEscolarUAC
        {
            get
            {
                return this.periodoEscolarUACField;
            }
            set
            {
                this.periodoEscolarUACField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string numeroPeriodoUAC
        {
            get
            {
                return this.numeroPeriodoUACField;
            }
            set
            {
                this.numeroPeriodoUACField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.siged.sep.gob.mx/certificados/")]
    public partial class DecSep
    {

        private string versionField;

        private string folioDigitalField;

        private System.DateTime fechaSepField;

        private string selloDecField;

        private string noCertificadoSepField;

        private string selloSepField;

        public DecSep()
        {
            this.versionField = "1.0";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string folioDigital
        {
            get
            {
                return this.folioDigitalField;
            }
            set
            {
                this.folioDigitalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime fechaSep
        {
            get
            {
                return this.fechaSepField;
            }
            set
            {
                this.fechaSepField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string selloDec
        {
            get
            {
                return this.selloDecField;
            }
            set
            {
                this.selloDecField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string noCertificadoSep
        {
            get
            {
                return this.noCertificadoSepField;
            }
            set
            {
                this.noCertificadoSepField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string selloSep
        {
            get
            {
                return this.selloSepField;
            }
            set
            {
                this.selloSepField = value;
            }
        }
    }
}