using AutoMapper;

using PatientApi.Actions.Create;
using PatientApi.Actions.Read;
using PatientApi.Actions.Update;
using PatientApi.Models;

namespace PatientApi.Mapping
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<CreatePatientDto, Patient>()
                .ForMember(p => p.PatientName, cfg => cfg.MapFrom(dto => dto.Name));
            CreateMap<UpdatePatientDto, Patient>()
                .ForMember(p => p.PatientName, cfg => cfg.MapFrom(dto => dto.Name));
            CreateMap<Patient, ReadPatientDto>()
                .ForMember(p => p.Name, cfg => cfg.MapFrom(dto => dto.PatientName));
        }
    }
}
