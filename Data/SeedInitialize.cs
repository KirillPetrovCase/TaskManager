using TaskManager.Data.MongoDb;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Data
{
    public class SeedInitialize
    {
        public static void InitializeAsync(MongoDbUserRepository userRepository, MongoDbPlacementRepository placementRepository)
        {
            if (userRepository.GetByLoginAsync("admin").Result == null)
            {
                userRepository.Add(new Owner()
                {
                    Login = "admin",
                    Name = "Администратор",
                    Role = Contracts.Role.Administrator,
                    HashPassword = SecurePasswordHasherService.Hash("admin")
                });
            }

            if (placementRepository.GetCountDocuments() == 0)
            {
                var placements = new Placement[] { new Placement() { Name = "Администрация" }, new Placement() { Name = "Расположение 1" }, new Placement() { Name = "Расположение 2" } };

                placementRepository.AddMany(placements);
            }
        }
    }
}