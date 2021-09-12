using NCD.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NCD.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
        IUnitOfWork UnitOfWork { get; }
    }
}
