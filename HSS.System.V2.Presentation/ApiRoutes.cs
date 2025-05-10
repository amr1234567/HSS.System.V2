namespace HSS.System.V2.Presentation
{
    public static class ApiRoutes
    {
        public static class Patient
        {
            public const string Base = "api/patient";

            // Notifications
            public const string NotificationCount = "/notifications/count";
            public const string GetNotifications = "/notifications";
            public const string GetNotification = "/notifications/{notificationId}";
            public const string SeenNotification = "/notifications/{notificationId}/seen";

            // Tests
            public const string SugerTest = "/tests/suger";

            // Appointments
            public const string GetCurrentAppointments = "/appointments/current";
            public const string GetAppointmentDetails = "/appointments/{appointmentId}/details";
            public const string CancelAppointment = "/appointments/{appointmentId}/cancel";
            public const string GetAppointmentContent = "/appointments/{appointmentId}/content";

            // Specifications
            public const string GetAllSpecifications = "/specifications";
            public const string GetAllSpecificationsPaged = "/specifications/paged";

            // Radiology & Medical Lab Tests
            public const string GetAllRadiologyTests = "/tests/radiology";
            public const string GetAllMedicalLabTests = "/tests/medical-lab";

            // Hospitals
            public const string GetHospitalsBySpecificationId = "/hospitals/specifications/{specializationId}";
            public const string GetHospitalsByRadiologyTestId = "/hospitals/radiology-tests/{radiologyTestId}";
            public const string GetHospitalsByMedicalLabTestId = "/hospitals/medical-lab-tests/{medicalLabTestId}";

            // Tickets
            public const string GetActiveTicketsInHospital = "/hospitals/{hospitalId}/tickets/active";
            public const string GetAllActiveTicketsInHospital = "/hospitals/{hospitalId}/tickets/active/all";
            public const string CreateTicket = "/hospitals/{hospitalId}/tickets";
            public const string GetTicketContent = "/tickets/{ticketId}/content";
            public const string GetAllActiveTickets = "/tickets/active";
            public const string GetAllInactiveTickets = "/tickets/inactive";

            // Departments
            public const string GetClinics = "/hospitals/{hospitalId}/clinics/{specificationId}";
            public const string GetRadiologyCenters = "/hospitals/{hospitalId}/radiology-centers/{testId}";
            public const string GetMedicalLabs = "/hospitals/{hospitalId}/medical-labs/{testId}";

            // Appointments Creation
            public const string CreateClinicAppointment = "/appointments/clinic";
            public const string CreateRadiologyAppointment = "/appointments/radiology";
            public const string CreateMedicalLabAppointment = "/appointments/medical-lab";

            // Required Tests
            public const string GetMedicalLabTestsRequired = "/tests/medical-lab/required";
            public const string GetRadiologyTestsRequired = "/tests/radiology/required";

            // Prescriptions
            public const string GetAllPrescriptionsRequired = "/prescriptions";
            public const string GetMedicineByPrescriptionId = "/prescriptions/{prescriptionId}/medicines";

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