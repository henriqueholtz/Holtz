using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holtz.CQRS.Application.Interfaces
{
    public interface IProductsCommandRepository
    {
        Task<long> AddProductAsync(); 
    }
}
