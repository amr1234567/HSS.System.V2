using HSS.System.V2.Domain;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.Notifications;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Prescriptions;
using HSS.System.V2.Domain.Models.Queues;

using Microsoft.EntityFrameworkCore;

namespace HSS.System.V2.DataAccess.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        #region Users 
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<MedicalLabTester> MedicalLabTesters { get; set; }
        public DbSet<RadiologyTester> RadiologyTesters { get; set; }
        public DbSet<Pharmacist> Pharmacists { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<LoginActivity> LoginActivities { get; set; }
        public DbSet<AppNotification> Notifications { get; set; }

        #endregion

        #region Appointments 
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalLabAppointment> MedicalLabAppointments { get; set; }
        public DbSet<RadiologyCeneterAppointment> RadiologyCeneterAppointments { get; set; }
        public DbSet<ClinicAppointment> ClinicAppointments { get; set; }
        #endregion

        #region Queues 
        public DbSet<SystemQueue> SystemQueues { get; set; }
        public DbSet<ClinicQueue> ClinicQueues { get; set; }
        public DbSet<MedicalLabQueue> MedicalLabQueues { get; set; }
        public DbSet<RadiologyCenterQueue> RadiologyCenterQueues { get; set; } 
        #endregion

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicineItem> PrescriptionMedicineItems { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<RadiologyCenter> RadiologyCenters { get; set; }
        public DbSet<MedicalLab> MedicalLabs { get; set; }
        public DbSet<Reception> Receptions { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<RadiologyTest> RadiologyTests { get; set; }
        public DbSet<MedicalLabTest> MedicalLabTests { get; set; }
        public DbSet<TestRequired> TestsRequired { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<MedicinePharmacy> MedicinePharmacies { get; set; }
        public DbSet<RadiologyReseltImage> RadiologyReseltImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClinicAppointment>()
                .HasOne(c => c.PreExamiationClinicAppointemnt)
                .WithOne()
                .HasForeignKey<ClinicAppointment>(c => c.PreExamiationClinicAppointemntId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<ClinicAppointment>()
                .HasOne(c => c.ReExamiationClinicAppointemnt)
                .WithOne()
                .HasForeignKey<ClinicAppointment>(c => c.ReExamiationClinicAppointemntId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Clinic>()
                .HasOne(c => c.CurrentWorkingDoctor)
                .WithOne()
                .HasForeignKey<Clinic>(c => c.CurrentWorkingDoctorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<RadiologyCenter>()
                .HasOne(c => c.CurrentWorkingTester)
                .WithOne()
                .HasForeignKey<RadiologyCenter>(c => c.CurrentWorkingTesterId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<MedicalLab>()
                .HasOne(c => c.CurrentWorkingTester)
                .WithOne()
                .HasForeignKey<MedicalLab>(c => c.CurrentWorkingTesterId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Ticket>()
                .HasOne(c => c.FirstClinicAppointment)
                .WithOne()
                .HasForeignKey<Ticket>(c => c.FirstClinicAppointmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<MedicalHistory>()
                .HasOne(c => c.FirstClinicAppointment)
                .WithOne()
                .HasForeignKey<MedicalHistory>(c => c.FirstClinicAppointmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            //#region Global Includes 
            //modelBuilder.Entity<ClinicAppointment>()
            //    .Navigation(c => c.Clinic)
            //    .AutoInclude();
            //modelBuilder.Entity<ClinicAppointment>()
            //    .Navigation(c => c.Doctor)
            //    .AutoInclude();


            //modelBuilder.Entity<MedicalLabAppointment>()
            //    .Navigation(c => c.MedicalLab)
            //    .AutoInclude();
            //modelBuilder.Entity<MedicalLabAppointment>()
            //    .Navigation(c => c.Tester)
            //    .AutoInclude();

            //modelBuilder.Entity<RadiologyCeneterAppointment>()
            //    .Navigation(c => c.RadiologyCeneter)
            //    .AutoInclude();
            //modelBuilder.Entity<RadiologyCeneterAppointment>()
            //    .Navigation(c => c.Tester)
            //    .AutoInclude();

            modelBuilder.Entity<ClinicQueue>()
                .Navigation(c => c.ClinicAppointments)
                .AutoInclude();
            modelBuilder.Entity<MedicalLabQueue>()
                .Navigation(c => c.MedicalLabAppointments)
                .AutoInclude();
            modelBuilder.Entity<RadiologyCenterQueue>()
                .Navigation(c => c.RadiologyCeneterAppointments)
                .AutoInclude();
            //#endregion

        }
    }
}
