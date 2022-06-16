using PlatformService.Models;

namespace PlatformService.Data {
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _dbContext;

        public PlatformRepo(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void CreatePlatform(Platform newPlatform)
        {
            if (newPlatform == null) {
                throw new ArgumentNullException(nameof(newPlatform));
            }

            _dbContext?.Platforms?.Add(newPlatform);
        }

        public IEnumerable<Platform> GetAllPlatforms() => _dbContext?.Platforms?.ToList();

        public Platform GetPlatformById(int id) => _dbContext?.Platforms?.FirstOrDefault(platform => platform.Id == id);

        public bool SaveChanges() => _dbContext.SaveChanges() >= 0;
    }
}