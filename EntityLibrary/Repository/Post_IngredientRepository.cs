using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public class Post_IngredientRepository : ICRDRepository<Post_Ingredient>
    {
        private readonly YumAppDbContext _context;

        public Post_IngredientRepository(YumAppDbContext context)
        {
            _context = context;
        }

        public async Task<Post_Ingredient> Add(Post_Ingredient instance)
        {
            if (instance != null)
            {
                await _context.Post_Ingredients.AddAsync(instance);
                await _context.SaveChangesAsync();

                return instance;
            }

            return null;
        }

        public IQueryable<Post_Ingredient> GetAll()
        {
            return _context.Post_Ingredients;
        }

        public async Task Remove(Post_Ingredient instance)
        {
            if (instance != null)
            {
                _context.Post_Ingredients.Remove(instance);
                await _context.SaveChangesAsync();
            }
        }
    }
}
