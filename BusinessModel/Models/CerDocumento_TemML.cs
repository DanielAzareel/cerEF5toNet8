using BusinessModel.Persistence.CertificadosElectronicosMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessModel.Models
{
    public class CerDocumento_TemML:cerDocumento_Tem
    {

        public List<Competencias> listCompetencias = new List<Competencias>();
        public List<CompertenciasIEMS> listlstCompertenciasIEMS = new List<CompertenciasIEMS>();
        public List<UACS> listUACS = new List<UACS>();


    }
    public class MateriasArchivoCarga
    {
        public string idCelda { get; set; }
        public string idMateria { get; set; }
        public string calificacion { get; set; }
        public string idDatoMateria { get; set; }
        public string idPlan { get; set; }
        public string valor { get; set; }

    }
    public class CompetenciasArchivoML: cerCatMateria
    {
        public string calificacion { get; set; }
        public int idPeriodo { get; set; }
        public int orden { get; set; }

    }
    public class Competencias
    {
        [XmlAttribute("idMateria")]
        public string idMateria { get; set; }
        [XmlAttribute("idPlan")]
        public string idPlan { get; set; }
        [XmlAttribute("nombre")]
        public string nombre { get; set; }
        [XmlAttribute("calificacion")]
        public string calificacion { get; set; }
        [XmlAttribute("totalHorasUAC")]
        public string totalHorasUAC {get;set;}
        [XmlAttribute("creditos")]
        public string creditos { get; set; }

       
        
    }
    [XmlRoot("Competencias")]
    public class LstCompencias
    {
        [XmlElement("competencia")]
        public List<Competencias> listCompetencias { get; set; }
        
    }
    public class CompertenciasIEMS
    {
        [XmlAttribute("competenciaDescripcion")]
        public string competenciaDescripcion { get; set; }
    }
    [XmlRoot("CompetenciaIEMS")]
    public class lstCompertenciasIEMS
    {
        [XmlElement("competencia")]
        public List<CompertenciasIEMS> listCompertenciasIEMS { get; set; }
    }
    public class UACS
    {
        [XmlAttribute("idMateria")]
        public string idMateria { get; set; }
        [XmlAttribute("idPlan")]
        public string idPlan { get; set; }
        [XmlAttribute("UACcct")]
        public string UACcct { get; set; }
        [XmlAttribute("UACidTipo")]
        public string UACidTipo { get; set; }
        [XmlAttribute("UACnombre")]
        public string UACnombre { get; set; }
        [XmlAttribute("UACcalificacion")]
        public string UACcalificacion { get; set; }
        [XmlAttribute("UACperiodoEscolar")]
        public string UACperiodoEscolar { get; set; }
        [XmlAttribute("UACnumeroPeriodo")]
        public string UACnumeroPeriodo {get; set;}
        [XmlAttribute("UACcreditos")]
        public string UACcreditos { get; set; }
    }
    [XmlRoot("UACS")]
    public class LstUACS
    {
        [XmlElement("UAC")]
        public List<UACS> lstUACS { get; set; }

    }
}
