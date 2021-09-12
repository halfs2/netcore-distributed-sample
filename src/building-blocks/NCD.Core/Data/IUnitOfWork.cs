using System.Threading.Tasks;

namespace NCD.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
