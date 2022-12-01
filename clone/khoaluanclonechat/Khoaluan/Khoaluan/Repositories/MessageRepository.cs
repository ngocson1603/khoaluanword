using Dapper;
using Khoaluan.Interfaces;
using Khoaluan.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Khoaluan.Repositories
{
    public class MessageRepository : GameStoreRepository<Message>, IMessageRepository
    {
        public MessageRepository(GameStoreDbContext context) : base(context)
        {
            
        }
        public List<Message> getMess()
        {
            var query = @"select * from Message";
            var parameter = new DynamicParameters();
            var data = Context.Database.GetDbConnection().Query<Message>(query, parameter);
            return data.ToList();
        }
    }
}
