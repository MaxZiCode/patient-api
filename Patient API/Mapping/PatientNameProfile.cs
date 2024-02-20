using AutoMapper;

using PatientApi.Actions.Create;
using PatientApi.Actions.Read;
using PatientApi.Actions.Update;
using PatientApi.Models;

namespace PatientApi.Mapping
{
    public class PatientNameProfile : Profile
    {
        public PatientNameProfile()
        {
            CreateMap<CreatePatientNameDto, PatientName>();
            CreateMap<UpdatePatientNameDto, PatientName>();
            CreateMap<PatientName, ReadPatientNameDto>();
        }
    }
}
