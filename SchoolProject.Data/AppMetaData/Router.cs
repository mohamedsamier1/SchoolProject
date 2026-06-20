namespace SchoolProject.Data.AppMetaData
{
    public static class Router
    {
        public const string root = "Api";
        public const string version = "V1";
        public const string Rule = root + "/" + version + "/";
        public static class StudentRouting
        {
            public const string Prefix = Rule + "Student" + "/";
            public const string GetStudentlist = Prefix + "List";
            public const string GetStudentbyid = Prefix + "{id}";
            public const string CreateStudent = Prefix + "Create";
            public const string EditStudent = Prefix + "Edit";
            public const string DeleteStudent = Prefix + "Delete" + "{id}";
            public const string PaginatedStudent = Prefix + "Paginated";

        }
        public static class DepartmentRouting
        {
            public const string Prefix = Rule + "Department" + "/";
            public const string GetDepartmentlist = Prefix + "List";
            public const string GetDepartmentById = Prefix + "Id";
            public const string CreateDepartment = Prefix + "Create";
            public const string EditDepartment = Prefix + "Edit";
            public const string DeleteDepartment = Prefix + "Delete" + "{id}";
            public const string PaginatedDepartment = Prefix + "Paginated";

        }
        public static class UserRouting
        {
            public const string Prefix = Rule + "Department" + "/";
            public const string GetUsertlist = Prefix + "List";
            public const string GetUserById = Prefix + "{id}";
            public const string CreateUser = Prefix + "Create";
            public const string EditUser = Prefix + "Edit";
            public const string DeleteUser = Prefix + "Delete" + "{id}";
            public const string PaginatedUser = Prefix + "Paginated";

        }



    }
}
