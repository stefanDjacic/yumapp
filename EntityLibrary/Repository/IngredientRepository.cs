using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public class IngredientRepository : ICRDRepository<Ingredient>
    {
        private readonly YumAppDbContext _context;

        public IngredientRepository(YumAppDbContext context)
        {
            _context = context;
        }

        public async Task<Ingredient> Add(Ingredient instance)
        {
            if (instance != null)
            {
                await _context.Ingredients.AddAsync(instance);
                await _context.SaveChangesAsync();

                return instance;
            }

            return null;
        }

        public IQueryable<Ingredient> GetAll()
        {
            return _context.Ingredients;
        }

        public async Task Remove(Ingredient instance)
        {
            if (instance != null)
            {
                _context.Ingredients.Remove(instance);
                await _context.SaveChangesAsync();
            }
        }
    }
}
