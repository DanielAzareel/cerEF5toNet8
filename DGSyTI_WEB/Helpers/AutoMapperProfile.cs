using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DGSyTI_WEB.Models;
using BusinessModel.Models;
using BusinessModel.Persistence.BD_FrameWork;
using CertificadosEletronicosMS.Models;
using BusinessModel.Persistence.CertificadosElectronicosMS;

namespace Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Uso de la instrucción create map para mapear las clases
          
            CreateMap<CatTipoIncorporacionViewModel, CatTipoIncorporacionML>();
            CreateMap<CatTipoIncorporacionViewModel, CatTipoIncorporacionML>().ReverseMap();

       
            CreateMap<VerificacionViewModel, TituloElectronico>();
            CreateMap<VerificacionViewModel, TituloElectronico>().ReverseMap();
         
            
            CreateMap<SelladoViewModel, SelladoML>();
            CreateMap<SelladoViewModel, SelladoML>().ReverseMap();

            CreateMap<CriteriosBusquedaMonitoreoViewModel, CriteriosBusquedaMonitoreoModel>();
            CreateMap<CriteriosBusquedaMonitoreoViewModel, CriteriosBusquedaMonitoreoModel>().ReverseMap();
           
            CreateMap<CertificadoML, CertificadoViewModel>();
            CreateMap<CertificadoML, CertificadoViewModel>().ReverseMap();

            CreateMap<cerDocumento, DetalleCertificadoViewModel>();
            CreateMap<cerDocumento, DetalleCertificadoViewModel>().ReverseMap();
        }
    }
}