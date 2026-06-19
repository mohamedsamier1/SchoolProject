using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolProject.Infrustructure.Migrations
{
    /// <inheritdoc />
    public partial class adddatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    SubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Period = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.SubId);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    DId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instructor_ManagerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.DId);
                });

            migrationBuilder.CreateTable(
                name: "departmentsSubjects",
                columns: table => new
                {
                    DeptId = table.Column<int>(type: "int", nullable: false),
                    SubId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departmentsSubjects", x => new { x.SubId, x.DeptId });
                    table.ForeignKey(
                        name: "FK_departmentsSubjects_departments_DeptId",
                        column: x => x.DeptId,
                        principalTable: "departments",
                        principalColumn: "DId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_departmentsSubjects_subjects_SubId",
                        column: x => x.SubId,
                        principalTable: "subjects",
                        principalColumn: "SubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorId = table.Column<int>(type: "int", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_instructors_departments_DId",
                        column: x => x.DId,
                        principalTable: "departments",
                        principalColumn: "DId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_instructors_instructors_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_students_departments_DID",
                        column: x => x.DID,
                        principalTable: "departments",
                        principalColumn: "DId");
                });

            migrationBuilder.CreateTable(
                name: "instructorSubjects",
                columns: table => new
                {
                    IntructorId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    InstructorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instructorSubjects", x => new { x.IntructorId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_instructorSubjects_instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "instructors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_instructorSubjects_subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "subjects",
                        principalColumn: "SubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "studentSubjects",
                columns: table => new
                {
                    StudId = table.Column<int>(type: "int", nullable: false),
                    SubId = table.Column<int>(type: "int", nullable: false),
                    grade = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentSubjects", x => new { x.StudId, x.SubId });
                    table.ForeignKey(
                        name: "FK_studentSubjects_students_StudId",
                        column: x => x.StudId,
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_studentSubjects_subjects_SubId",
                        column: x => x.SubId,
                        principalTable: "subjects",
                        principalColumn: "SubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_departments_Instructor_ManagerId",
                table: "departments",
                column: "Instructor_ManagerId",
                unique: true,
                filter: "[Instructor_ManagerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_departmentsSubjects_DeptId",
                table: "departmentsSubjects",
                column: "DeptId");

            migrationBuilder.CreateIndex(
                name: "IX_instructors_DId",
                table: "instructors",
                column: "DId");

            migrationBuilder.CreateIndex(
                name: "IX_instructors_SupervisorId",
                table: "instructors",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_instructorSubjects_InstructorId",
                table: "instructorSubjects",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_instructorSubjects_SubjectId",
                table: "instructorSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_students_DID",
                table: "students",
                column: "DID");

            migrationBuilder.CreateIndex(
                name: "IX_studentSubjects_SubId",
                table: "studentSubjects",
                column: "SubId");

            migrationBuilder.AddForeignKey(
                name: "FK_departments_instructors_Instructor_ManagerId",
                table: "departments",
                column: "Instructor_ManagerId",
                principalTable: "instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departments_instructors_Instructor_ManagerId",
                table: "departments");

            migrationBuilder.DropTable(
                name: "departmentsSubjects");

            migrationBuilder.DropTable(
                name: "instructorSubjects");

            migrationBuilder.DropTable(
                name: "studentSubjects");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "instructors");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
