using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShopDemo.Core.Models;

namespace WebShopDemo.Core.Contracts
{
    /// <summary>
    /// Manipulates product data
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of products</returns>
        Task<IEnumerable<ProductDto>> GetAll();
    }
}
