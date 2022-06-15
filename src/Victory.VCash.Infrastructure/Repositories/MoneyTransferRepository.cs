using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Domain.Enums;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Repositories.Abstraction;

namespace Victory.VCash.Infrastructure.Repositories
{
    public class MoneyTransferRepository : Repository, IMoneyTransferRepository
    {
        private ICacheContext _cacheContext;

        public MoneyTransferRepository(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
        {
            _cacheContext = cacheContext;
        }

        public List<Transaction> GetTransactions(long moneyTransferId)
        {
            var sql = $@"SELECT transaction_id, external_transaction_id, external_transaction_type_id, external_group_identifier, user_id, amount, money_transfer_id
                         FROM ""transaction""
                         WHERE money_transfer_id = {moneyTransferId}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Transaction>()
                   .ForMember(d => d.TransactionId, opt => opt.MapFrom(src => src["transaction_id"]))
                   .ForMember(d => d.ExternalTransactionId, opt => opt.MapFrom(src => src["external_transaction_id"]))
                   .ForMember(d => d.ExternalTransactionTypeId, opt => opt.MapFrom(src => src["external_transaction_type_id"]))
                   .ForMember(d => d.ExternalGroupIdentifier, opt => opt.MapFrom(src => src["external_group_identifier"]))
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.Amount, opt => opt.MapFrom(src => src["amount"]))
                   .ForMember(d => d.MoneyTransferId, opt => opt.MapFrom(src => src["money_transfer_id"]));
            }).CreateMapper();

            return DataContext.ExecuteSql<Transaction>(sql, mapper);
        }

        public MoneyTransfer GetMoneyTransfer(long moneyTransferId)
        {
            var sql = $@"SELECT money_transfer_id, from_user_id, to_user_id, amount, money_transfer_status_id, error, created_by, modified_by, insert_date
                         FROM money_transfer
                         WHERE money_transfer_id = {moneyTransferId}";


            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, MoneyTransfer>()
                   .ForMember(d => d.MoneyTransferId, opt => opt.MapFrom(src => src["money_transfer_id"]))
                   .ForMember(d => d.FromUserId, opt => opt.MapFrom(src => src["from_user_id"]))
                   .ForMember(d => d.ToUserId, opt => opt.MapFrom(src => src["to_user_id"]))
                   .ForMember(d => d.Amount, opt => opt.MapFrom(src => src["amount"]))
                   .ForMember(d => d.MoneyTransferStatusId, opt => opt.MapFrom(src => src["money_transfer_status_id"]))
                   .ForMember(d => d.Error, opt => opt.MapFrom(src => src["error"]))
                   .ForMember(d => d.CreatedBy, opt => opt.MapFrom(src => src["created_by"]))
                   .ForMember(d => d.ModifiedBy, opt => opt.MapFrom(src => src["modify_by"]))
                   .ForMember(d => d.InsertDate, opt => opt.MapFrom(src => src["insert_date"]));
            }).CreateMapper();

            var moneyTransfer = DataContext.ExecuteSql<MoneyTransfer>(sql, mapper).FirstOrDefault();
            if(moneyTransfer != null)
                moneyTransfer.Transactions = GetTransactions(moneyTransferId);

            return moneyTransfer;
        }

        public MoneyTransfer SaveMoneyTransfer(MoneyTransfer moneyTransfer)
        {
            var s_money_transfer_id = moneyTransfer?.MoneyTransferId?.ToString() ?? "null";
            var s_moneyTransferStatusId = ((int?)moneyTransfer.MoneyTransferStatusId)?.ToString() ?? "null";
            var s_createdBy = !string.IsNullOrEmpty(moneyTransfer.CreatedBy)? $"'{ moneyTransfer.CreatedBy }'" : "null";
            var s_modifiedBy = !string.IsNullOrEmpty(moneyTransfer.ModifiedBy) ? $"'{ moneyTransfer.ModifiedBy }'" : "null";
            var unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var sql = $@"DO $$
                         DECLARE
                             _money_transfer_id BIGINT := {s_money_transfer_id};
                             _from_user_id INT := {moneyTransfer.FromUserId};
                             _to_user_id INT := {moneyTransfer.ToUserId};
                             _amount DECIMAL(19,4) := {moneyTransfer.Amount}; 
                             _money_transfer_status_id INT := {s_moneyTransferStatusId};
                             _error TEXT := '{moneyTransfer.Error}';
                             _note TEXT := '{moneyTransfer.Note}';
                             _created_by UUID := {s_createdBy};
                             _modified_by UUID := {s_modifiedBy};
                             _now TIMESTAMP := NOW() AT time zone 'utc';
                         BEGIN 
	                         IF EXISTS(SELECT 'x' FROM money_transfer WHERE money_transfer_id = _money_transfer_id) THEN
		                        UPDATE money_transfer SET from_user_id = _from_user_id,
									                      to_user_id = _to_user_id, 
									                      amount = _amount, 
									                      money_transfer_status_id = _money_transfer_status_id, 
                                                          error = _error,
                                                          note = _note,
									                      created_by = _created_by, 
									                      modified_by = _modified_by, 
									                      update_date = _now
		                        WHERE money_transfer_id = _money_transfer_id;
	                         ELSE
		                        INSERT INTO money_transfer(from_user_id, to_user_id, amount, money_transfer_status_id, error, note, created_by, modified_by, insert_date, update_date) 
		                        VALUES (_from_user_id, _to_user_id, _amount, _money_transfer_status_id, _error, _note, _created_by, _modified_by, _now, NULL);
		                        
                                _money_transfer_id := currval(pg_get_serial_sequence('money_transfer','money_transfer_id'));
	                         END IF;
	 
	                         -- create temp table with affected rows
	                         CREATE TEMPORARY TABLE _tmp{unixMs} ON COMMIT DROP 
	                         AS
	                         SELECT * 
	                         FROM money_transfer 
	                         WHERE money_transfer_id = _money_transfer_id;
                         END $$;
                         
                         -- result
                         SELECT * 
                         FROM _tmp{unixMs}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, MoneyTransfer>()
                   .ForMember(d => d.MoneyTransferId, opt => opt.MapFrom(src => src["money_transfer_id"]))
                   .ForMember(d => d.FromUserId, opt => opt.MapFrom(src => src["from_user_id"]))
                   .ForMember(d => d.ToUserId, opt => opt.MapFrom(src => src["to_user_id"]))
                   .ForMember(d => d.Amount, opt => opt.MapFrom(src => src["amount"]))
                   .ForMember(d => d.MoneyTransferStatusId, opt => opt.MapFrom(src => src["money_transfer_status_id"]))
                   .ForMember(d => d.Error, opt => opt.MapFrom(src => src["error"]))
                   .ForMember(d => d.Note, opt => opt.MapFrom(src => src["note"]))
                   .ForMember(d => d.CreatedBy, opt => opt.MapFrom(src => src["created_by"]))
                   .ForMember(d => d.ModifiedBy, opt => opt.MapFrom(src => src["modified_by"]))
                   .ForMember(d => d.InsertDate, opt => opt.MapFrom(src => src["insert_date"]));
            }).CreateMapper();

            var result = DataContext.ExecuteSql<MoneyTransfer>(sql, mapper).FirstOrDefault();

            //Save transactions if exist
            if (moneyTransfer.Transactions?.Any() ?? false)
            {
                moneyTransfer.Transactions.ForEach(x => x.MoneyTransferId = result.MoneyTransferId);
                result.Transactions = SaveTransactions(moneyTransfer.Transactions)?.ToList();
            }

            return result;
        }

        public Transaction SaveTransaction(Transaction transaction)
        {
            string s_transactionId = transaction?.TransactionId?.ToString() ?? "null";
            string s_externalTransactionId = transaction?.ExternalTransactionId?.ToString() ?? "null";
            string s_externalTransactionTypeId = transaction?.ExternalTransactionTypeId?.ToString() ?? "null";
            string s_externalGroupIdentifier = transaction?.ExternalGroupIdentifier?.ToString() ?? "null";
            var unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var sql = $@"DO $$
						 DECLARE
                             _transaction_id BIGINT := {s_transactionId};
							 _external_transaction_id BIGINT := {s_externalTransactionId}; 
							 _external_transaction_type_id INT := {s_externalTransactionTypeId}; 
							 _external_group_identifier VARCHAR(20) := {s_externalGroupIdentifier}; 
							 _user_id INT := {transaction.UserId}; 
							 _amount DECIMAL(19,4) := {transaction.Amount}; 
							 _money_transfer_id INT := {transaction.MoneyTransferId};
                             _now TIMESTAMP := NOW() AT time zone 'utc';
						 BEGIN 
							 IF EXISTS(SELECT 'x' FROM ""transaction"" WHERE transaction_id = _transaction_id) THEN
								UPDATE ""transaction"" SET external_transaction_id = _external_transaction_id,
                                                           external_transaction_type_id = _external_transaction_type_id,
														   external_group_identifier = _external_group_identifier, 
														   user_id = _user_id, 
														   amount = _amount, 
														   money_transfer_id = _money_transfer_id
								WHERE transaction_id = _transaction_id;
							 ELSE
								INSERT INTO ""transaction""(external_transaction_id, external_transaction_type_id, external_group_identifier, user_id, amount, money_transfer_id, insert_date) 
								VALUES (_external_transaction_id, _external_transaction_type_id, _external_group_identifier, _user_id, _amount, _money_transfer_id, _now);

                                _transaction_id := currval(pg_get_serial_sequence('transaction','transaction_id'));
							 END IF;

							 -- create temp table with affected rows
							 CREATE TEMPORARY TABLE _tmp{unixMs} ON COMMIT DROP 
							 AS
							 SELECT * 
							 FROM ""transaction"" 
							 WHERE transaction_id = _transaction_id;
						 END $$;

						 -- result
						 SELECT * 
						 FROM _tmp{unixMs}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Transaction>()
                   .ForMember(d => d.TransactionId, opt => opt.MapFrom(src => src["transaction_id"]))
                   .ForMember(d => d.ExternalTransactionId, opt => opt.MapFrom(src => src["external_transaction_id"]))
                   .ForMember(d => d.ExternalTransactionTypeId, opt => opt.MapFrom(src => src["external_transaction_type_id"]))
                   .ForMember(d => d.ExternalGroupIdentifier, opt => opt.MapFrom(src => src["external_group_identifier"]))
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.Amount, opt => opt.MapFrom(src => src["amount"]))
                   .ForMember(d => d.MoneyTransferId, opt => opt.MapFrom(src => src["money_transfer_id"]))
                   .ForMember(d => d.InsertDate, opt => opt.MapFrom(src => src["insert_date"]));
            }).CreateMapper();

            return DataContext.ExecuteSql<Transaction>(sql, mapper).FirstOrDefault();
        }

        public IEnumerable<Transaction> SaveTransactions(List<Transaction> transactions)
        {
            foreach(var transaction in transactions)
            {
                yield return SaveTransaction(transaction);
            }
        }

        public List<MoneyTransfer> GetMoneyTransfers(long? moneyTransferId = null, 
                                                     int? fromUserId = null, int? toUserId = null, 
                                                     decimal? amountFrom = null, decimal? amountTo = null, 
                                                     DateTime? dateFrom = null, DateTime? dateTo = null,
                                                     MoneyTransferStatus? status = null)
        {
            string s_moneyTransferId = moneyTransferId?.ToString() ?? "null";
            string s_fromUserId = fromUserId?.ToString() ?? "null";
            string s_toUserId = toUserId?.ToString() ?? "null";
            string s_amountFrom = amountFrom?.ToString() ?? "null";
            string s_amountTo = amountTo?.ToString() ?? "null";
            string s_dateFrom = (dateFrom != null)? $"'{dateFrom.Value.ToString("yyyy-MM-dd HH:mm")}'" : "null";
            string s_dateTo = (dateTo != null) ? $"'{dateTo.Value.ToString("yyyy-MM-dd HH:mm")}'" : "null";
            string s_status = (status != null) ? $"{(int)status}" : "null";

            var sql = $@"SELECT m.money_transfer_id, m.from_user_id, m.to_user_id, m.amount, m.money_transfer_status_id, m.error, m.created_by, m.modified_by, m.insert_date, m.update_date
                         FROM money_transfer m
                         WHERE m.money_transfer_id = COALESCE({s_moneyTransferId}, m.money_transfer_id)
                           AND m.from_user_Id = COALESCE({s_fromUserId}, m.from_user_Id)
                           AND m.to_user_Id = COALESCE({s_toUserId}, m.to_user_Id)
                           AND m.money_transfer_status_id = COALESCE({s_status}, m.money_transfer_status_id)
                           AND m.insert_date BETWEEN COALESCE({s_dateFrom}, m.insert_date) AND COALESCE({s_dateTo}, m.insert_date)
                           AND m.amount BETWEEN COALESCE({s_amountFrom}, m.amount) AND COALESCE({s_amountTo}, m.amount)
                        ORDER BY m.insert_date DESC";

            //money transfers
            var moneyTransMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, MoneyTransfer>()
                   .ForMember(d => d.MoneyTransferId, opt => opt.MapFrom(src => src["money_transfer_id"]))
                   .ForMember(d => d.FromUserId, opt => opt.MapFrom(src => src["from_user_id"]))
                   .ForMember(d => d.ToUserId, opt => opt.MapFrom(src => src["to_user_id"]))
                   .ForMember(d => d.Amount, opt => opt.MapFrom(src => src["amount"]))
                   .ForMember(d => d.MoneyTransferStatusId, opt => opt.MapFrom(src => src["money_transfer_status_id"]))
                   .ForMember(d => d.Error, opt => opt.MapFrom(src => src["error"]))
                   .ForMember(d => d.CreatedBy, opt => opt.MapFrom(src => src["created_by"]))
                   .ForMember(d => d.ModifiedBy, opt => opt.MapFrom(src => src["modified_by"]))
                   .ForMember(d => d.InsertDate, opt => opt.MapFrom(src => src["insert_date"]))
                   .ForMember(d => d.UpdateDate, opt => opt.MapFrom(src => src["update_date"]));
            }).CreateMapper();

            var moneyTransfers = DataContext.ExecuteSql<MoneyTransfer>(sql, moneyTransMapper);

            //transactions (settlement)
            sql = @$"SELECT t.transaction_id, t.external_transaction_id, t.amount, t.external_group_identifier, t.external_transaction_type_id, t.user_id, t.money_transfer_id
                     FROM money_transfer m
                     JOIN ""transaction"" t on m.money_transfer_id = t.money_transfer_id
                     WHERE m.money_transfer_id = COALESCE({s_moneyTransferId}, m.money_transfer_id)
                       AND m.from_user_Id = COALESCE({s_fromUserId}, m.from_user_Id)
                       AND m.to_user_Id = COALESCE({s_toUserId}, m.to_user_Id)
                       AND m.money_transfer_status_id = COALESCE({s_status}, m.money_transfer_status_id)
                       AND m.insert_date BETWEEN COALESCE({s_dateFrom}, m.insert_date) AND COALESCE({s_dateTo}, m.insert_date)
                       AND m.amount BETWEEN COALESCE({s_amountFrom}, m.amount) AND COALESCE({s_amountTo}, m.amount)
                     ORDER BY m.insert_date DESC";

            var transactionsMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, Transaction>()
                   .ForMember(d => d.TransactionId, opt => opt.MapFrom(src => src["transaction_id"]))
                   .ForMember(d => d.ExternalTransactionId, opt => opt.MapFrom(src => src["external_transaction_id"]))
                   .ForMember(d => d.MoneyTransferId, opt => opt.MapFrom(src => src["money_transfer_id"]))
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.Amount, opt => opt.MapFrom(src => src["amount"]))
                   .ForMember(d => d.InsertDate, opt => opt.MapFrom(src => src["insert_date"]));
            }).CreateMapper();

            var transactions = DataContext.ExecuteSql<Transaction>(sql, transactionsMapper);

            //link transactions to money transfers
            moneyTransfers?.ForEach(x => x.Transactions = transactions?.Where(t => t.MoneyTransferId == x.MoneyTransferId).ToList());

            return moneyTransfers;
        }
    }
}
