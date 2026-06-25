namespace SchoolProject.Core.Features.Authorization.Quaries.Result
{
    public class ManageUserRolesResponse
    {
        public int UserId { get; set; }
        public List<Roles> Roles { get; set; }
    }
    public class Roles
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool HasRole { get; set; }
    }

}
