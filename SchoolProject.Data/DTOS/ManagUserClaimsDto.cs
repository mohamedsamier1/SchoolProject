namespace SchoolProject.Data.DTOS
{
    public class ManagUserClaimsDto
    {
        public int UsertId { get; set; }
        public List<UserClaims> userClaims { get; set; }

    }
    public class UserClaims
    {
        public string Type { get; set; }
        public bool Value { get; set; }
    }

}
