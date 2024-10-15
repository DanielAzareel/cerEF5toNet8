using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessModel.Persistence.BD_FrameWork;

using BusinessModel.Models;
using AutoMapper;
using BusinessModel.Persistence.CertificadosElectronicosMS;

namespace BusinessModel.MapperProfile
{
    public class BussinessMapperProfile : Profile
    {

        public BussinessMapperProfile()
        {
           

            CreateMap<CatTipoIncorporacionML, catTipoIncorp>();
            CreateMap<CatTipoIncorporacionML, catTipoIncorp>().ReverseMap();


            CreateMap<CertificadoML, cerDocumento>();
            CreateMap<CertificadoML, cerDocumento>().ReverseMap();

            CreateMap<FirmanteML, cerCatFirmante>();
            CreateMap<FirmanteML, cerCatFirmante>().ReverseMap();

        }

    }
}
