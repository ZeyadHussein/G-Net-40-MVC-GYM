using AutoMapper;
using GymManagmentSystem.BLL.ViewModels.SessionViewModels;
using GymManagmentSystem.DAL.Models;

namespace GymManagmentSystem.BLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(d => d.CategoryName,
                    o => o.MapFrom(s => s.Category != null ? s.Category.CategoryName : ""))

                .ForMember(d => d.TrainerName,
                    o => o.MapFrom(s => s.Trainer != null ? s.Trainer.Name : ""))

                .ForMember(d => d.AvailableSlots, o => o.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();

            // Dropdowns
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
        }
    }
}