using GymManagmentSystem.BLL.Services.AttachmentService;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.MemberViewModels;
using GymManagmentSystem.DAL.Models;
using GymManagmentSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagmentSystem.BLL.Services.Calassess
{
    public class MemberService : IMemberService

    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IAttachmentService _attachmentService;
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IGenericRepository<Member> memberRepository, IGenericRepository<Membership> membershipRepository,IGenericRepository<Plan> planRepository,
            IGenericRepository<HealthRecord> healthRecordRepository,IGenericRepository<Booking> bookingRepository, IAttachmentService attachmentService,  IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
            _planRepository = planRepository;
            _healthRecordRepository = healthRecordRepository;
            _bookingRepository = bookingRepository;

            _attachmentService = attachmentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateMemberAsync(
           CreateMemberViewModel model,
           CancellationToken ct = default)
        {
            try
            {
                var emailExist =
                    await _memberRepository.AnyAsync(
                        x => x.Email == model.Email,
                        ct);

                if (emailExist)
                    return false;

                var phoneExist =
                    await _memberRepository.AnyAsync(
                        x => x.Phone == model.Phone,
                        ct);

                if (phoneExist)
                    return false;

                if (model.Photo == null)
                    return false;

                var photoName =
                    await _attachmentService.UploadAsync(
                        model.Photo.OpenReadStream(),
                        model.Photo.FileName,
                        "images",
                        ct);

                if (string.IsNullOrEmpty(photoName))
                    return false;

                var member = new Member
                {
                    Name = model.Name,

                    Email = model.Email,

                    Phone = model.Phone,

                    DateOfBirth = model.DateOfBirth,

                    Gender = model.Gender,

                    Photo = photoName,

                    IsActive = true,

                    Address = new Address
                    {
                        BuildingNumber = model.BuildingNumber,

                        City = model.City,

                        Street = model.Street
                    },

                    HealthRecord = new HealthRecord
                    {
                        Height =
                            model.HealthRecordViewModel.Height,

                        Weight =
                            model.HealthRecordViewModel.Weight,

                        BloodType =
                            model.HealthRecordViewModel.BloodType,

                        Note =
                            model.HealthRecordViewModel.Note
                    }
                };

                await _memberRepository.AddAsync(member);

                var result =
                    await _unitOfWork.SaveChangesAsync(ct);

                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<MemberViewModel>> GetALLMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepository.GetAllAsync(ct: ct);
            if (!members.Any())
            {
                return Enumerable.Empty<MemberViewModel>();
            }
            var MemberViewModels = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString(),
            });
            return MemberViewModels;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var Member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (Member is null)
            {
                return null;
            }
            var ViewModel = new MemberViewModel
            {
             
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Gender=Member.Gender.ToString(),
                Address= $"{Member.Address.BuildingNumber}-{Member.Address.Street}-{Member.Address.City}",


            };
            var ActiveMembership =await  _membershipRepository.FirstOrDefaultAsync(m=> m.MemberId == memberId && m.EndDate > DateTime.Now, ct:ct);
           if(ActiveMembership is not null)
            {
                var ActivePlan = await _planRepository.GetByIdAsync(ActiveMembership.PlanId,ct) ;
                ViewModel.PlanName = ActivePlan?.Name;
                ViewModel.MembershipStartDate= ActiveMembership.CreatedAt.ToShortDateString();
                ViewModel.MembershipEndDate= ActiveMembership.EndDate.ToShortDateString();
            }
            return ViewModel;
        }

        public async Task<HealthRecordViewModel?> GetmemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var Record=await _healthRecordRepository.FirstOrDefaultAsync(x => x.MemberId == memberId, ct:ct);
            if (Record is null) return null;
            else
            {
                return new HealthRecordViewModel()
                {
                    
                    Weight = Record.Weight,
                    BloodType = Record.BloodType,
                    Height = Record.Height,
                    Note = Record.Note
                };

            }
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberid, CancellationToken ct = default)
        {
            var Member = await _memberRepository.GetByIdAsync(memberid, ct);
            if (Member is null)
            {
                return null;
            }
            else
            {
                return new MemberToUpdateViewModel()
                {
                    Name = Member.Name,
                    Email = Member.Email,
                    Phone = Member.Phone,
                    Street = Member.Address.Street,
                    City = Member.Address.City,
                    BuildingNumber = Member.Address.BuildingNumber,
                    Photo = Member.Photo,


                };

            }
        }

        public async Task<bool> RemoveMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(id, ct);

            if (member is null)
                return false;

            var hasFutureSessions =
                await _bookingRepository.AnyAsync(
                    b => b.MemberId == id &&
                         b.Session.StartDate > DateTime.Now);

            if (hasFutureSessions)
                return false;

            var result = await _memberRepository.DeleteAsync(member);

            if (result > 0)
            {
                if (!string.IsNullOrEmpty(member.Photo))
                {
                    _attachmentService.Delete( member.Photo, "images");
                }

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var Member =await _memberRepository.GetByIdAsync(id, ct);
            if (Member is null) return false;

            if(await _memberRepository.AnyAsync(M => M.Email == model.Email && M.Id != id, ct) )
            {
                return false;
            }
            if (await _memberRepository.AnyAsync(M => M.Phone == model.Phone && M.Id != id, ct))
            {
                return false;
            }
            Member.Email = model.Email;
            Member.Phone = model.Phone;
            Member.Address.City = model.City;
            Member.Address.BuildingNumber = model.BuildingNumber;
            Member.Address.Street = model.Street;
            Member.UpdatedAt= DateTime.Now;

            var result = await _memberRepository.UpdateAsync(Member);
            return result > 0?true:false;


        }
    }
}
