using AutoMapper;
using DataAccess.Entities;
using Models.DTOs;

namespace BankPaymentAPI.Profiles
{
        public class MappingProfiles : Profile
        {
            public MappingProfiles()
            {
                
                CreateMap<Bank, BankDto>().ReverseMap();
                CreateMap<CreateBankDto, Bank>();
                CreateMap<UpdateBankDto, Bank>();

               
                CreateMap<Account, AccountDto>().ReverseMap();
                CreateMap<CreateAccountDto, Account>();
                CreateMap<UpdateAccountDto, Account>();

                
                CreateMap<Transaction, TransactionDto>().ReverseMap();

             
                CreateMap<Transfer, TransferResultDto>()
                    .ForMember(dest => dest.TransferId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                    .ForMember(dest => dest.FromBalance, opt => opt.Ignore()) 
                    .ForMember(dest => dest.ToBalance, opt => opt.Ignore());
                CreateMap<Payment, PaymentIntentResponseDto>()
                    .ForMember(dest => dest.TransferId, opt => opt.MapFrom(src => src.TransferId))
                    .ForMember(dest => dest.PaymentIntentId, opt => opt.MapFrom(src => src.PaymentIntentId))
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                    .ForMember(dest => dest.ClientSecret, opt => opt.Ignore()); 
            }
        }
    }


