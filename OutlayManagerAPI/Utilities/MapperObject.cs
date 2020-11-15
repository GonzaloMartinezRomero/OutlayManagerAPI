using AutoMapper;
using OutlayManagerAPI.Model;
using OutlayManagerCore.Model.Transaction;
using System;

namespace OutlayManagerAPI.Utilities
{
    public class MapperObject: Profile
    {
        public MapperObject()
        {
            //Model -> View
            CreateMap<Transaction, WSTransaction>()
                .ForMember(dest => dest.ID,
                                      opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Amount,
                                      opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Date,
                                      opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.DetailTransaction,
                                      opt => opt.MapFrom(src => src.DetailTransaction));

            CreateMap<TransactionDetail, WSDetailTransaction>()
                .ForMember(dest => dest.Code,
                                      opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Description,
                                      opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Type,
                                      opt => opt.MapFrom(src => src.Type));
            //View -> Model
            CreateMap<WSTransaction, Transaction>()
                .ForMember(dest => dest.ID,
                                      opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Amount,
                                      opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Date,
                                      opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.DetailTransaction,
                                      opt => opt.MapFrom(src => src.DetailTransaction));

            CreateMap<WSDetailTransaction, TransactionDetail>()
                .ForMember(dest => dest.Code,
                                      opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Description,
                                      opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Type,
                                      opt => opt.MapFrom(src => src.Type));
        }

        public static WSTransaction ToWSTransaction(Transaction outlayCoreTransaction)
        {
            return (outlayCoreTransaction == null)
                ? new WSTransaction() { DetailTransaction = new WSDetailTransaction() }
                : new WSTransaction()
                {
                    ID = (int)outlayCoreTransaction.ID,
                    Amount = outlayCoreTransaction.Amount,
                    Date = outlayCoreTransaction.Date,
                    DetailTransaction = new WSDetailTransaction()
                    {
                        Code = outlayCoreTransaction.DetailTransaction.Code,
                        Description = outlayCoreTransaction.DetailTransaction.Description,
                        Type = outlayCoreTransaction.DetailTransaction.Type.ToString()
                    }
                };
        }

        public static Transaction ToTransaction(WSTransaction wsTransaction)
        {
            TransactionDetail.TypeTransaction typeTransaction;

            if (!Enum.TryParse(wsTransaction.DetailTransaction.Type, out typeTransaction))
                throw new Exception($"Type {wsTransaction.DetailTransaction.Type } is not allowed"); 

            return new Transaction()
            {
                ID = (uint)wsTransaction.ID,
                Amount = wsTransaction.Amount,
                Date = wsTransaction.Date,
                DetailTransaction = new TransactionDetail()
                {
                    Code = wsTransaction.DetailTransaction.Code,
                    Description = wsTransaction.DetailTransaction.Description,
                    Type = typeTransaction
                }
            };
        }
    }
}
