using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DVLD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationTypes",
                columns: table => new
                {
                    ApplicationTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationTypeTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ApplicationFees = table.Column<decimal>(type: "smallmoney", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationTypes", x => x.ApplicationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "LicenseClasses",
                columns: table => new
                {
                    LicenseClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClassDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MinimumAllowedAge = table.Column<byte>(type: "tinyint", nullable: false),
                    DefaultValidityLength = table.Column<byte>(type: "tinyint", nullable: false),
                    ClassFees = table.Column<decimal>(type: "smallmoney", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseClasses", x => x.LicenseClassID);
                });

            migrationBuilder.CreateTable(
                name: "TestTypes",
                columns: table => new
                {
                    TestTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestTypeTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TestTypeDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TestTypeFees = table.Column<decimal>(type: "smallmoney", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTypes", x => x.TestTypeID);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ThirdName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: false),
                    Gendor = table.Column<byte>(type: "tinyint", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NationalityCountryID = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonID);
                    table.ForeignKey(
                        name: "FK_People_Countries_NationalityCountryID",
                        column: x => x.NationalityCountryID,
                        principalTable: "Countries",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantPersonID = table.Column<int>(type: "int", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationTypeID = table.Column<int>(type: "int", nullable: false),
                    ApplicationStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    LastStatusDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidFees = table.Column<decimal>(type: "smallmoney", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationID);
                    table.ForeignKey(
                        name: "FK_Applications_ApplicationTypes_ApplicationTypeID",
                        column: x => x.ApplicationTypeID,
                        principalTable: "ApplicationTypes",
                        principalColumn: "ApplicationTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_People_ApplicantPersonID",
                        column: x => x.ApplicantPersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "smalldatetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverID);
                    table.ForeignKey(
                        name: "FK_Drivers_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "PersonID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drivers_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocalDrivingLicenseApplications",
                columns: table => new
                {
                    LocalDrivingLicenseApplicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationID = table.Column<int>(type: "int", nullable: false),
                    LicenseClassID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalDrivingLicenseApplications", x => x.LocalDrivingLicenseApplicationID);
                    table.ForeignKey(
                        name: "FK_LocalDrivingLicenseApplications_Applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocalDrivingLicenseApplications_LicenseClasses_LicenseClassID",
                        column: x => x.LicenseClassID,
                        principalTable: "LicenseClasses",
                        principalColumn: "LicenseClassID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    LicenseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationID = table.Column<int>(type: "int", nullable: false),
                    DriverID = table.Column<int>(type: "int", nullable: false),
                    LicenseClass = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PaidFees = table.Column<decimal>(type: "smallmoney", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IssueReason = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.LicenseID);
                    table.ForeignKey(
                        name: "FK_Licenses_Applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Licenses_Drivers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "DriverID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Licenses_LicenseClasses_LicenseClass",
                        column: x => x.LicenseClass,
                        principalTable: "LicenseClasses",
                        principalColumn: "LicenseClassID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Licenses_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestAppointments",
                columns: table => new
                {
                    TestAppointmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestTypeID = table.Column<int>(type: "int", nullable: false),
                    LocalDrivingLicenseApplicationID = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    PaidFees = table.Column<decimal>(type: "smallmoney", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    RetakeTestApplicationID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAppointments", x => x.TestAppointmentID);
                    table.ForeignKey(
                        name: "FK_TestAppointments_Applications_RetakeTestApplicationID",
                        column: x => x.RetakeTestApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestAppointments_LocalDrivingLicenseApplications_LocalDrivingLicenseApplicationID",
                        column: x => x.LocalDrivingLicenseApplicationID,
                        principalTable: "LocalDrivingLicenseApplications",
                        principalColumn: "LocalDrivingLicenseApplicationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestAppointments_TestTypes_TestTypeID",
                        column: x => x.TestTypeID,
                        principalTable: "TestTypes",
                        principalColumn: "TestTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestAppointments_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetainedLicenses",
                columns: table => new
                {
                    DetainID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseID = table.Column<int>(type: "int", nullable: false),
                    DetainDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FineFees = table.Column<decimal>(type: "smallmoney", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    IsReleased = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleasedByUserID = table.Column<int>(type: "int", nullable: true),
                    ReleaseApplicationID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetainedLicenses", x => x.DetainID);
                    table.ForeignKey(
                        name: "FK_DetainedLicenses_Applications_ReleaseApplicationID",
                        column: x => x.ReleaseApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetainedLicenses_Licenses_LicenseID",
                        column: x => x.LicenseID,
                        principalTable: "Licenses",
                        principalColumn: "LicenseID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetainedLicenses_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetainedLicenses_Users_ReleasedByUserID",
                        column: x => x.ReleasedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InternationalLicenses",
                columns: table => new
                {
                    InternationalLicenseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationID = table.Column<int>(type: "int", nullable: false),
                    DriverID = table.Column<int>(type: "int", nullable: false),
                    IssuedUsingLocalLicenseID = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternationalLicenses", x => x.InternationalLicenseID);
                    table.ForeignKey(
                        name: "FK_InternationalLicenses_Applications_ApplicationID",
                        column: x => x.ApplicationID,
                        principalTable: "Applications",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternationalLicenses_Drivers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "DriverID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternationalLicenses_Licenses_IssuedUsingLocalLicenseID",
                        column: x => x.IssuedUsingLocalLicenseID,
                        principalTable: "Licenses",
                        principalColumn: "LicenseID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternationalLicenses_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    TestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestAppointmentID = table.Column<int>(type: "int", nullable: false),
                    TestResult = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.TestID);
                    table.ForeignKey(
                        name: "FK_Tests_TestAppointments_TestAppointmentID",
                        column: x => x.TestAppointmentID,
                        principalTable: "TestAppointments",
                        principalColumn: "TestAppointmentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tests_Users_CreatedByUserID",
                        column: x => x.CreatedByUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicantPersonID",
                table: "Applications",
                column: "ApplicantPersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationTypeID",
                table: "Applications",
                column: "ApplicationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CreatedByUserID",
                table: "Applications",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_DetainedLicenses_CreatedByUserID",
                table: "DetainedLicenses",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_DetainedLicenses_LicenseID",
                table: "DetainedLicenses",
                column: "LicenseID");

            migrationBuilder.CreateIndex(
                name: "IX_DetainedLicenses_ReleaseApplicationID",
                table: "DetainedLicenses",
                column: "ReleaseApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_DetainedLicenses_ReleasedByUserID",
                table: "DetainedLicenses",
                column: "ReleasedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CreatedByUserID",
                table: "Drivers",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_PersonID",
                table: "Drivers",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalLicenses_ApplicationID",
                table: "InternationalLicenses",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalLicenses_CreatedByUserID",
                table: "InternationalLicenses",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalLicenses_DriverID",
                table: "InternationalLicenses",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalLicenses_IssuedUsingLocalLicenseID",
                table: "InternationalLicenses",
                column: "IssuedUsingLocalLicenseID");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_ApplicationID",
                table: "Licenses",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_CreatedByUserID",
                table: "Licenses",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_DriverID",
                table: "Licenses",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_LicenseClass",
                table: "Licenses",
                column: "LicenseClass");

            migrationBuilder.CreateIndex(
                name: "IX_LocalDrivingLicenseApplications_ApplicationID",
                table: "LocalDrivingLicenseApplications",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_LocalDrivingLicenseApplications_LicenseClassID",
                table: "LocalDrivingLicenseApplications",
                column: "LicenseClassID");

            migrationBuilder.CreateIndex(
                name: "IX_People_NationalityCountryID",
                table: "People",
                column: "NationalityCountryID");

            migrationBuilder.CreateIndex(
                name: "IX_TestAppointments_CreatedByUserID",
                table: "TestAppointments",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_TestAppointments_LocalDrivingLicenseApplicationID",
                table: "TestAppointments",
                column: "LocalDrivingLicenseApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_TestAppointments_RetakeTestApplicationID",
                table: "TestAppointments",
                column: "RetakeTestApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_TestAppointments_TestTypeID",
                table: "TestAppointments",
                column: "TestTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_CreatedByUserID",
                table: "Tests",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_TestAppointmentID",
                table: "Tests",
                column: "TestAppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonID",
                table: "Users",
                column: "PersonID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetainedLicenses");

            migrationBuilder.DropTable(
                name: "InternationalLicenses");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "TestAppointments");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "LocalDrivingLicenseApplications");

            migrationBuilder.DropTable(
                name: "TestTypes");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "LicenseClasses");

            migrationBuilder.DropTable(
                name: "ApplicationTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
