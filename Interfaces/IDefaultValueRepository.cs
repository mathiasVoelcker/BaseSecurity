using System.Collections.Generic;
using System.Threading.Tasks;
using BaseSecurity.Models;

namespace BaseSecurity.Interfaces
{
    public interface IDefaultValueRepository
    {
        Task<List<DefaultValue>> GetAll();

        Task<bool> Create(DefaultValue defaultValue);
    }
}