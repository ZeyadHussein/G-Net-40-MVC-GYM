using AutoMapper;
using GymManagmentSystem.BLL.Common;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.SessionViewModels;
using GymManagmentSystem.DAL.Models;
using GymManagmentSystem.DAL.Models.Enums;
using GymManagmentSystem.DAL.Repositories.Classes;
using GymManagmentSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.BLL.Services.Calassess
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork._SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct: ct);
            if(sessions?.Any() != true) return null;
            sessions = sessions.OrderByDescending(x => x.StartDate);
            var MappingSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach(var session in MappingSessions)
            {
                session.AvailableSlots=session.Capacity-(await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(session.Id,ct));
            }
            return MappingSessions;
        }


        public async Task<SessionViewModel?> GetSessionByIdAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork._SessionRepository.GetSessionWithTrainerAndCategoryAsync(SessionId, ct);
             if(session == null) return null;
             var MappingSession=_mapper.Map<Session,SessionViewModel>(session);
            MappingSession.AvailableSlots = MappingSession.Capacity - (await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct));
            return MappingSession;




        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int SessionId, CancellationToken ct = default)
        {
           var session=await _unitOfWork.GetRepository<Session>().GetByIdAsync(SessionId);
            if(session is null) return null;
            if(!await IsSessionAvabiableForUpdatingAsync(session,ct)) return null;
            return _mapper.Map<UpdateSessionViewModel>(session);
        }
        #region Helper Methods
        private async Task<bool>IsSessionAvabiableForUpdatingAsync(Session session, CancellationToken ct = default)
        {
            if (session.StartDate <= DateTime.Now) return false;
            var Booked = await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            return Booked == 0;

        }
        #endregion
        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if(model.EndDate<=model.StartDate)
            {
                return Result.Validation("End Date Must Be after start date");
            }
            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must Be In The Future");

            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetByIdAsync(model.TrainerId, ct);


            if (Trainer is null)
                return Result.NotFound("Invalid Trainer Id");
            var CategoryRepo = _unitOfWork.GetRepository<Category>();
            var Category = await CategoryRepo.GetByIdAsync(model.CategoryId, ct);
            if (Category is null)
                return Result.NotFound("Category Not Found");
            var IsValidSpecialty = Enum.TryParse<Specialites>(Category.CategoryName, true, out var CategorySpecialty);
            if (!IsValidSpecialty || Trainer.Speciality != CategorySpecialty)
                return Result.Validation("Trainer Speciality Does Not Match Session Category");

            var session=_mapper.Map<Session>(model);
            await _unitOfWork.GetRepository<Session>().AddAsync(session);
            var AffectedRows= await _unitOfWork.SaveChangesAsync();

            return AffectedRows > 0 ? Result.OK() : Result.Fail("Failed To Create Session");
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
           var sessionRepo=_unitOfWork.GetRepository<Session>();
            var Session = await sessionRepo.GetByIdAsync(id, ct);
            if (Session is null) return Result.NotFound("Session Not Found");
            if (Session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot Update Session That Has Already Started");

            var BookedCount = await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if (BookedCount > 0) return Result.Fail("Can Not Update A Session That Has Bookings");

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must Be After start Date");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must Be In The Future");

            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var Trainer = await TrainerRepo.GetByIdAsync(model.TrainerId, ct);


            if (Trainer is null)
                return Result.NotFound("Invalid Trainer Id");
            var CategoryRepo = _unitOfWork.GetRepository<Category>();
            var Category = await CategoryRepo.GetByIdAsync(Session.CategoryId, ct);
            if (Category is null)
                return Result.NotFound("Category Not Found");
            var IsValidSpecialty = Enum.TryParse<Specialites>(Category.CategoryName, true, out var CategorySpecialty);
            if (!IsValidSpecialty || Trainer.Speciality != CategorySpecialty)
                return Result.Validation("Trainer Speciality Does Not Match Session Category");

            _mapper.Map(model, Session);
            Session.UpdatedAt = DateTime.Now;
            await sessionRepo.UpdateAsync(Session);

            var AffectedRows = await _unitOfWork.SaveChangesAsync();

            return AffectedRows > 0 ? Result.OK() : Result.Fail("Failed To Create Session");


        }

        public async Task<Result> RemoveSessionAsync(int SessionId, CancellationToken ct = default)
        {
            var sessionRepo = _unitOfWork.GetRepository<Session>();
            var Session = await sessionRepo.GetByIdAsync(SessionId, ct);
            if (Session is null) return Result.NotFound("Session Not Found");

            if (Session.EndDate >= DateTime.Now)
                return Result.Fail("Can Not Delete A Session That Has Not Yest Ended");
            var BookedCount = await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);

            if (BookedCount > 0) return Result.Fail("Can Not Delete A session That Has Booking");

            await sessionRepo.DeleteAsync(Session);

            var AffectedRows = await _unitOfWork.SaveChangesAsync();

            return AffectedRows > 0 ? Result.OK() : Result.Fail("Failed To Create Session");

        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            return Enum.GetValues(typeof(Specialites))
                .Cast<Specialites>()
                .Select(e => new CategorySelectViewModel
                {
                    Id = (int)e,
                    CategoryName = e.ToString()
                })
                .ToList();
        }




        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);

            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }
        public async Task<AnalyticsViewModel> GetAnalyticsAsync(CancellationToken ct = default)
{
    var membersRepo = _unitOfWork.GetRepository<Member>();
    var trainerRepo = _unitOfWork.GetRepository<Trainer>();
    var sessionRepo = _unitOfWork.GetRepository<Session>();

    var allMembers = await membersRepo.GetAllAsync(ct: ct);
    var allSessions = await sessionRepo.GetAllAsync(ct: ct);
    var allTrainers = await trainerRepo.GetAllAsync(ct: ct);

    var now = DateTime.Now;

    return new AnalyticsViewModel
    {
        TotalMembers = allMembers.Count(),
        ActiveMembers = allMembers.Count(x => x.IsActive),
        TotalTrainers = allTrainers.Count(),
        UpcomingSessions = allSessions.Count(x => x.StartDate > now),
        OngoingSessions = allSessions.Count(x => x.StartDate <= now && x.EndDate >= now),
        CompletedSessions = allSessions.Count(x => x.EndDate < now)
    };
}



    }
}
