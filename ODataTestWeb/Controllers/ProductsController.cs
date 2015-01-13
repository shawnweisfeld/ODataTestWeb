using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using ODataTestWeb.Models;

namespace ODataTestWeb.Controllers
{
    public class ProductsController : ODataController
    {
        IQueryable<Models.Product> _products = null;

        public ProductsController()
        {
            _products = (new List<Product>()
            {
                new Product () 
                {
                    ID = 1,
                    Name = "Product 1",
                    Price = 1,
                    Category = new Category()
                                {
                                    ID = 1,
                                    Name = "Category 1"
                                }
                },
                new Product () 
                {
                    ID = 2,
                    Name = "Product 2",
                    Price = 2,
                    Category = new Category()
                                {
                                    ID = 2,
                                    Name = "Category 2"
                                }
                },
                new Product () 
                {
                    ID = 3,
                    Name = "Product 3",
                    Price = 3,
                    Category = new Category()
                                {
                                    ID = 3,
                                    Name = "Category 3"
                                }
                },
            }).AsQueryable();
        }

        // GET: odata/Products
        [EnableQuery]
        public IQueryable<Product> GetProducts()
        {
            return _products;
        }

        // GET: odata/Products(5)
        [EnableQuery]
        public SingleResult<Product> GetProduct([FromODataUri] int key)
        {
            return SingleResult.Create(_products.Where(product => product.ID == key));
        }

        // PUT: odata/Products(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Product> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = _products.FirstOrDefault(x => x.ID == key); //await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            patch.Put(product);

            try
            {
                await Task.Delay(100); //db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(product);
        }

        // POST: odata/Products
        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //db.Products.Add(product);
            //await db.SaveChangesAsync();

            await Task.Delay(100);

            return Created(product);
        }

        // PATCH: odata/Products(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = _products.FirstOrDefault(x => x.ID == key); //await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            patch.Patch(product);

            try
            {
                //await db.SaveChangesAsync();
                await Task.Delay(100);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(product);
        }

        // DELETE: odata/Products(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Product product = _products.FirstOrDefault(x => x.ID == key); //await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            //db.Products.Remove(product);
            //await db.SaveChangesAsync();
            await Task.Delay(100);

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            base.Dispose(disposing);
        }

        private bool ProductExists(int key)
        {
            return _products.Count(e => e.ID == key) > 0;
        }
    }
}
