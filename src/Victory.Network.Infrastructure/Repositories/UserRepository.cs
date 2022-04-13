using AutoMapper;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.Network.Infrastructure.Repositories.Abstraction;
using Victory.Network.Infrastructure.Repositories.Enums;
using Victory.Network.Infrastructure.Repositories.Models;

namespace Victory.Network.Infrastructure.Repositories
{
    public class UserRepository : Repository, IUserRepository
	{
		private Brand _brand = Brand.Victory247SRB;

		public UserRepository(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
		{
			//_redisClientManager = redisClientManager;
		}

		public async Task<User> GetUser(int userId)
        {
			var mapper = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<IDictionary<string, object>, User>()
				.ForMember(d => d.Id, opt => opt.MapFrom(src => src["Id"]))
				.ForMember(d => d.IsDeleted, opt => opt.MapFrom(src => src["IsDeleted"]))
				.ForMember(d => d.ExternalId, opt => opt.MapFrom(src => src["ExternalId"]))
				.ForMember(d => d.UserName, opt => opt.MapFrom(src => src["Username"]))
				.ForMember(d => d.IsTest, opt => opt.MapFrom(src => src["IsTest"]))
				.ForMember(d => d.UserType, opt => opt.MapFrom(src => src["UserTypeId"]))
				.ForMember(d => d.UserStatus, opt => opt.MapFrom(src => src["UserStatusId"]))
				.ForMember(d => d.ParentId, opt => opt.MapFrom(src => src["ParentId"]))
				.ForMember(d => d.Brand, opt => opt.MapFrom(src => src["BrandId"]))
				.ForMember(d => d.AuditCreatedBy, opt => opt.MapFrom(src => src["AuditCreatedBy"]))
				.ForMember(d => d.AuditLastUpdatedBy, opt => opt.MapFrom(src => src["AuditLastUpdatedBy"]))
				.ForMember(d => d.AuditCreatedOn, opt => opt.MapFrom(src => src["AuditCreatedOn"]))
				.ForMember(d => d.AuditLastUpdateOn, opt => opt.MapFrom(src => src["AuditLastUpdateOn"]))
				.ForMember(d => d.AuditChangeFlag, opt => opt.MapFrom(src => src["AuditChangeFlag"]))
				.ForMember(d => d.Department, opt => opt.MapFrom(src => src["Department"]))
				.ForMember(d => d.ActualUserId, opt => opt.MapFrom(src => src["ActualUserId"]))
				.ForMember(d => d.RegistrationDate, opt => opt.MapFrom(src => src["RegistrationDate"]))
				.ForMember(d => d.CurrencyISOCode, opt => opt.MapFrom(src => src["CurrencyISOCode"]))
				.ForMember(d => d.Category, opt => opt.MapFrom(src => src["CategoryId"]))
				.ForMember(d => d.UserStatus, opt => opt.MapFrom(src => src["UserStatusId"]));
			}).CreateMapper();

			var parameters = new DynamicParameters(new
			{
				@Id = userId,
				@BrandListID = _brand
			});

			var user = (await DataContext.ExecuteReaderProcedureAsync<User>("[Users].[GetUserByID]", mapper, parameters: parameters)).SingleOrDefault();

			return user;
        }

		public async Task<UserDetail> GetUserDetails(int userId)
		{
			var parameters = new DynamicParameters(new
			{
				@Id = userId,
				@BrandListID = _brand
			});

			var result = (await DataContext.ExecuteReaderProcedureAsync<UserDetail>("[Users].[GetUserDetailByID]", parameters: parameters)).SingleOrDefault();
			return result;
		}


		public async Task<List<UserExtraDetail>> GetUserExtraDetails(int userId)
		{
			var parameters = new DynamicParameters(new
			{
				@Id = userId,
				@BrandListID = _brand
			});

			var result = await DataContext.ExecuteReaderProcedureAsync<UserExtraDetail>("[Users].[GetExtraDetailByUserID]", parameters: parameters);
			return result;
		}

		public async Task SaveUserDetailExtraProperty(int userId, string propertyName, string propertyValue)
        {
			var sql = $"MERGE Users.UserDetailExtraProperty AS [target] " +
					  $"USING(SELECT Id AS [UserDetailId], '{propertyName}' AS [PropertyName], '{propertyValue}' AS [PropertyValue] " +
					  $"FROM Users.UserDetail " +
					  $"WHERE UserId = {userId}) AS[source] " +
					  $"	ON[target].UserDetailId = [source].UserDetailId AND[target].PropertyName = [source].PropertyName " +
					  $"WHEN MATCHED THEN " +
					  $"	UPDATE SET[target].PropertyValue = [source].PropertyValue " +
					  $"WHEN NOT MATCHED THEN " +
					  $"	INSERT(UserDetailId, PropertyName, PropertyValue) " +
					  $"	VALUES([source].UserDetailId, [source].PropertyName, [source].PropertyValue); ";
			await DataContext.ExecuteSqlAsync(sql);
        }
    }
}


