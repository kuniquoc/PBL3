namespace DaNangTourism.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool status { get; set; } // 0: inactive, 1: active

        public Account()
        {
            
        }
    }
}
