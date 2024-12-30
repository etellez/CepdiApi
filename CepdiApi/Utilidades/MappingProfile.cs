using AutoMapper;
using CepdiModel;

namespace CepdiApi.Utilidades
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, CrearUsuarioDTO>().ReverseMap();
            CreateMap<Medicamento, CrearMedicamentoDTO>().ReverseMap();
        }
    }

}
