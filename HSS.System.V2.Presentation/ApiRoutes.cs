namespace HSS.System.V2.Presentation
{
    public static class ApiRoutes
    {
        public static class Patient
        {
            public const string Base = "api/patient";

            // Notifications
            public const string NotificationCount = "/Notifications/count";
            public const string GetNotifications = "/Notifications";
            public const string GetNotification = "/Notifications/{notificationId}";
            public const string SeenNotification = "/Notifications/{notificationId}/seen";

            // Tests
            public const string SugerTest = "/Survey/tests suger";

            // Appointments
            public const string GetCurrentAppointments = "/Home/appointments current";
            public const string GetAppointmentDetails = "/Home/appointments_details/{appointmentId}";
            public const string CancelAppointment = "/Home/appointments_cancel/{appointmentId}";
            public const string GetAppointmentContent = "/appointments/{appointmentId}/content";

            #region Appointment
            // Specifications
            public const string GetAllSpecifications = "/Appointment/Get_All_Specilzation";

            // Radiology & Medical Lab Tests
            public const string GetAllRadiologyTests = "/Appointment/Get_All_Radiology_Tests";
            public const string GetAllMedicalLabTests = "/Appointment/Get_All_Medical-Lab_Tests";

            // Hospitals
            public const string GetHospitalsBySpecificationId = "/Appointment/Get_Hospitals_By_Specification-Id/{specializationId}";
            public const string GetHospitalsByRadiologyTestId = "/Appointment/Get_Hospitals_By_RadiologyTest-Id/{radiologyTestId}";
            public const string GetHospitalsByMedicalLabTestId = "/Appointment/Get_Hospitals_By_Medical-LabTest-Id/{medicalLabTestId}";

            // Tickets
            public const string GetActiveTicketsInHospital = "/Appointment/Get_Active_Ticket_In_Hospital/{hospitalId}";
            public const string CreateTicket = "/Appointment/Create_Ticket/{hospitalId}";

            // Departments
            public const string GetClinics = "/Appointment/Get_Clinics/{hospitalId}/{specificationId}";
            public const string GetRadiologyCenters = "/Appointment/Get_Radiology-Center/{hospitalId}/{testId}";
            public const string GetMedicalLabs = "/Appointment/Get_Medical-Labs/{hospitalId}/{testId}";

            // Appointments Creation
            public const string CreateClinicAppointment = "/Appointment/Create_Clinic_Appointment";
            public const string CreateRadiologyAppointment = "/Appointment/Create_Radiology_Appointment";
            public const string CreateMedicalLabAppointment = "/Appointment/Create_Medical-Lab_Appointment";
            #endregion

            public const string GetAllSpecificationsPaged = "/specifications/paged";

            

            

            
            public const string GetAllActiveTicketsInHospital = "/hospitals/{hospitalId}/tickets/active/all";
            
            public const string GetTicketContent = "/Profile/tickets_content/{ticketId}";
            public const string GetAllActiveTickets = "/Profile/active_tickets";
            public const string GetAllInactiveTickets = "/Profile/inactive_tickets";

            

            

            // Required Tests
            public const string GetMedicalLabTestsRequired = "/Required/medical-lab/tests";
            public const string GetRadiologyTestsRequired = "/Required/radiology/tests";

            // Prescriptions
            public const string GetAllPrescriptionsRequired = "/Required/prescriptions";
            public const string GetMedicineByPrescriptionId = "/Required/Medicines/{prescriptionId}";

            // Current Appointments by Type
            public const string GetCurrentClinicAppointments = "/Current Appointments/clinic";
            public const string GetCurrentMedicalLabAppointments = "/Current Appointments/medical-lab";
            public const string GetCurrentRadiologyCenterAppointments = "/Current Appointments/radiology-center";

            // Profile
            public const string UploadProfilePicture = "/Profile/picture";
            public const string GetPatientProfile = "/Profile/Informations";
        }
    }
}