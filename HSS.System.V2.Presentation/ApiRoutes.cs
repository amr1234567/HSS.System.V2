namespace HSS.System.V2.Presentation
{
    public static class ApiRoutes
    {
        public static class Patient
        {
            public const string Base = "api/patient";

            //Auth
            public const string Register = "/register";
            public const string ConfirmAccount = "/confirm-account/{nationalId}";
            public const string login = "/login";

            // Notifications
            public const string NotificationCount = "/notifications/count";
            public const string GetNotifications = "/notifications";
            public const string GetNotification = "/notifications/{notificationId}";
            public const string SeenNotification = "/notifications/{notificationId}/seen";

            // Tests
            public const string SugerTest = "/survey/tests-suger";

            // Appointments
            public const string GetCurrentAppointments = "/current-appointments";
            public const string GetAppointmentDetails = "/appointment-details/{appointmentId}";
            public const string CancelAppointment = "/cancel-appointment/{appointmentId}";
            public const string GetAppointmentContent = "/appointment-content/{appointmentId}";

            #region Appointment
            // Specifications
            public const string GetAllSpecifications = "/specilzation";

            // Radiology & Medical Lab Tests
            public const string GetAllRadiologyTests = "/radiology-tests";
            public const string GetAllMedicalLabTests = "/medical-lab-tests";

            // Hospitals
            public const string GetHospitalsBySpecificationId = "/specilization/{specializationId}/hospitals";
            public const string GetHospitalsByRadiologyTestId = "/radiology-test/{radiologyTestId}/hospitals";
            public const string GetHospitalsByMedicalLabTestId = "/medical-lab-test/{medicalLabTestId}/hospitals";

            // Tickets
            public const string GetActiveTicketsInHospital = "/hospital/{hospitalId}/tickets/active";
            public const string CreateTicket = "/create-ticket/{hospitalId}";

            // Departments
            public const string GetClinics = "/hospital/{hospitalId}/clinics/{specificationId}";
            public const string GetRadiologyCenters = "/hospital/{hospitalId}/radiology-centers/{testId}";
            public const string GetMedicalLabs = "/hospital/{hospitalId}/medical-labs/{testId}";

            // Appointments Creation
            public const string CreateClinicAppointment = "/create/clinic-appointment";
            public const string CreateRadiologyAppointment = "/create/radiology-appointment";
            public const string CreateMedicalLabAppointment = "/create/medical-Lab-appointment";
            #endregion

            public const string GetAllSpecilizations = "/specilizations";

            
            public const string GetAllActiveTicketsInHospital = "/hospitals/{hospitalId}/tickets/active";
            
            public const string GetTicketContent = "/Profile/ticket/{ticketId}";
            public const string GetAllActiveTickets = "/Profile/tickets/active";
            public const string GetAllInactiveTickets = "/Profile/tickets/inactive";

            // Required Tests
            public const string GetMedicalLabTestsRequired = "/tests-required/medical-lab";
            public const string GetRadiologyTestsRequired = "/tests-required/radiology";

            // Prescriptions
            public const string GetAllPrescriptionsRequired = "/prescriptions";
            public const string GetMedicineByPrescriptionId = "/prescription/{prescriptionId}/medicines";

            // Current Appointments by Type
            public const string GetCurrentClinicAppointments = "/appointments/clinic/current";
            public const string GetCurrentMedicalLabAppointments = "/appointments/medical-lab/current";
            public const string GetCurrentRadiologyCenterAppointments = "/appointments/radiology-center/current";

            // Profile
            public const string UploadProfilePicture = "/profile/picture";
            public const string GetPatientProfile = "/profile";
        }
    }
}