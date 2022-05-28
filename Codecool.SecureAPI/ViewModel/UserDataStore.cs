using Codecool.SecureAPI.Model;

namespace Codecool.SecureAPI.ViewModel
{
    public class UserDataStore
    {
        public static UserDataStore Current { get; } = new UserDataStore();
        public List<UserViewModel> Users { get; set; }
        public UserDataStore()
        {
            Users = new List<UserViewModel>()
            {
                new UserViewModel(){Name = "Tamas", Id= 1, Role = "Admin"}
            };
        }

    } 
}
