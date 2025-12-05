using PharmaPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaPro.Repositories
{
    internal interface IProductRepository
    {
        IEnumerable<Product> SearchProducts(string searchTerm);
    }
}
