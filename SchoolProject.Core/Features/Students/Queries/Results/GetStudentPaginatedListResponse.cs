namespace SchoolProject.Core.Features.Students.Queries.Results
{
    public class GetStudentPaginatedListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string DepatmentName { get; set; }
        public GetStudentPaginatedListResponse(int id, string name, string address, string deparmentname)
        {
            Id = id;
            Name = name;
            Address = address;
            DepatmentName = deparmentname;
        }
    }
}
