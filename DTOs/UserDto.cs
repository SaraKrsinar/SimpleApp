namespace SimpleApp.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

    }
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

}
