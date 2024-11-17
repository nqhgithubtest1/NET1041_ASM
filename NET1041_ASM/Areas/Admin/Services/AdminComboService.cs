using NET1041_ASM.Context;
using NET1041_ASM.Models;

namespace NET1041_ASM.Areas.Admin.Services
{
    public class AdminComboService : IAdminComboService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminComboService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Combo> GetAll()
        {
            return _dbContext.Combos.ToList();
        }

        public Combo GetById(int id)
        {
            return _dbContext.Combos.FirstOrDefault(c => c.ComboID == id);
        }

        public void Add(Combo combo)
        {
            ValidateCombo(combo);
            if (string.IsNullOrWhiteSpace(combo.Name))
            {
                throw new ArgumentException("Combo name cannot be empty.");
            }

            combo.IsAvailable = true;
            _dbContext.Combos.Add(combo);
            _dbContext.SaveChanges();
        }

        public void Update(Combo combo)
        {
            ValidateCombo(combo);
            var existingCombo = _dbContext.Combos.FirstOrDefault(c => c.ComboID == combo.ComboID);
            if (existingCombo == null)
            {
                throw new KeyNotFoundException("Combo not found.");
            }

            existingCombo.Name = combo.Name;
            existingCombo.Description = combo.Description;
            existingCombo.Price = combo.Price;
            existingCombo.ImagePath = combo.ImagePath;
            existingCombo.IsAvailable = combo.IsAvailable;

            _dbContext.SaveChanges();
        }

        private void ValidateCombo(Combo combo)
        {
            var isDuplicateName = _dbContext.Combos
                .Any(c => c.Name == combo.Name && c.ComboID != combo.ComboID);

            if (isDuplicateName)
            {
                throw new ArgumentException("Combo name already exists. Please choose a different name.");
            }

            // Validate image only if creating or updating with a new image
            if (string.IsNullOrWhiteSpace(combo.ImagePath))
            {
                throw new ArgumentException("An image is required for the combo item.");
            }

            if (combo.ComboFoodItems == null || combo.ComboFoodItems.Count <= 1)
            {
                throw new ArgumentException("Combo must include at least 2 food.");
            }
        }

        public void Deactivate(int id)
        {
            var combo = _dbContext.Combos.FirstOrDefault(c => c.ComboID == id);
            if (combo == null)
            {
                throw new KeyNotFoundException("Combo not found.");
            }

            combo.IsAvailable = false;
            _dbContext.SaveChanges();
        }

        public void Activate(int id)
        {
            var combo = _dbContext.Combos.FirstOrDefault(c => c.ComboID == id);
            if (combo == null)
            {
                throw new KeyNotFoundException("Combo not found.");
            }

            combo.IsAvailable = true;
            _dbContext.SaveChanges();
        }
    }
}
