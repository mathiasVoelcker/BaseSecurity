using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseSecurity.Db.Scripts;
using BaseSecurity.Interfaces;
using BaseSecurity.Models;
using Dapper;

namespace BaseSecurity.Repositories
{
    public class DefaultValueRepository : BaseRepository, IDefaultValueRepository
    {
        public DefaultValueRepository(IDbSession dbSession) : base(dbSession) { }

        public async Task<bool> Create(DefaultValue defaultValue)
        {
            try {
                using (var conn = CreateConnection()) {
                    await conn.ExecuteAsync(DefaultValueScripts.Create, defaultValue);
                    return true;
                }
            } catch (Exception) {
                return false;
            }
        }

        public async Task<List<DefaultValue>> GetAll()
        {
            using (var conn = CreateConnection()) {
                var listDefaultValue = await conn.QueryAsync<DefaultValue>(DefaultValueScripts.Get);
                return listDefaultValue.AsList();
            }
        }
    }
}