using AutoMapper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Victory.DataAccess;
using Victory.VCash.Domain.Models;
using Victory.VCash.Infrastructure.Repositories.Abstraction;

namespace Victory.VCash.Infrastructure.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        private ICacheContext _cacheContext;

        public UserRepository(IDataContext dataContext, ICacheContext cacheContext) : base(dataContext, cacheContext)
        {
            _cacheContext = cacheContext;
        }

        public void SaveUser(User user)
        {
            var parentUserId = user.ParentUserId.HasValue ? user.ParentUserId.Value.ToString() : "null";
            var userStatusId = user.UserStatusId.HasValue ?  ((int)user.UserStatusId).ToString() : "null";
            var userTypeId = user.UserTypeId.HasValue ? ((int)user.UserTypeId).ToString() : "null";
            var sql = $@"WITH tmp (user_id, parent_user_id, user_name, email, mobile_number, user_type_id, user_status_id, 
                                   name, last_name, citizenId, affiliate_code, insert_date) AS (
                            VALUES ({user.UserId}, {parentUserId}, '{user.UserName}', '{user.Email}', '{user.MobileNumber}', {userTypeId}, {userStatusId},
                                   '{user.Name}','{user.LastName}', '{user.AffiliateCode}', NOW() AT time zone 'utc')
                         )

                         INSERT INTO public.user(user_id, parent_user_id, user_name, email, mobile_number, user_type_id, user_status_id, 
                                                 name, last_name, affiliate_code, insert_date)
                         SELECT t.user_id, t.parent_user_id, t.user_name, t.email, t.mobile_number, t.user_type_id, t.user_status_id, 
                                t.name, t.last_name, t.affiliate_code, t.insert_date
                         FROM tmp t
                             ON CONFLICT(user_id) DO
                                UPDATE SET parent_user_id = EXCLUDED.parent_user_id,
                                           user_name = EXCLUDED.user_name,
                                           email = EXCLUDED.email,
                                           mobile_number = EXCLUDED.mobile_number,
                                           user_type_id = EXCLUDED.user_type_id,
                                           user_status_id = EXCLUDED.user_status_id,
                                           name = EXCLUDED.mobile_number,
                                           last_name = EXCLUDED.last_name,
                                           affiliate_code = EXCLUDED.mobile_number;";
            DataContext.ExecuteSql(sql);
        }

        public User GetUser(int userId)
        {
            var sql = $@"SELECT user_id, user_name, user_type_id, user_status_id, insert_date
                         FROM public.user
                         WHERE user_id = {userId}";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, User>()
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.UserTypeId, opt => opt.MapFrom(src => src["user_type_id"]))
                   .ForMember(d => d.UserStatusId, opt => opt.MapFrom(src => src["user_status_id"]));
            }).CreateMapper();
            
            var user = DataContext.ExecuteSql<User>(sql, mapper).SingleOrDefault();
            return user;
        }

        public User GetUserByAffiliateCode(string affiliateCode)
        {
            var sql = $@"SELECT user_id, user_name, user_type_id, user_status_id, insert_date
                         FROM public.user
                         WHERE affiliate_code = '{affiliateCode}'";

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IDictionary<string, object>, User>()
                   .ForMember(d => d.UserId, opt => opt.MapFrom(src => src["user_id"]))
                   .ForMember(d => d.UserName, opt => opt.MapFrom(src => src["user_name"]))
                   .ForMember(d => d.UserTypeId, opt => opt.MapFrom(src => src["user_type_id"]))
                   .ForMember(d => d.UserStatusId, opt => opt.MapFrom(src => src["user_status_id"]));
            }).CreateMapper();

            var user = DataContext.ExecuteSql<User>(sql, mapper).SingleOrDefault();
            return user;
        }
    }
}


