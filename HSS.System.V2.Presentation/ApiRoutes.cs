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

        public static class Reception
        {
            public const string Base = "api/reception";

            // Hospital Departments
            public const string GetAllHospitalDepartments = "/hospital-departments";
            public const string GetAllSpecializations = "/specializations";
            public const string GetAllClinics = "/clinics/{specializationId}";
            public const string GetAllRadiologyCenters = "/radiology-centers";
            public const string GetAllRadiologyCentersDoTest = "/radiology-centers/test/{radiologyTestId}";
            public const string GetAllMedicalLabs = "/medical-labs";
            public const string GetAllMedicalLabsDoTest = "/medical-labs/test/{medicalTestId}";

            // Appointments
            public const string GetAllAppointmentsForClinic = "/appointments/clinic/{clinicId}";
            public const string GetQueueForClinic = "/queue/clinic/{clinicId}";
            public const string GetAllAppointmentsForRadiologyCenter = "/appointments/radiology/{radiologyCenterId}";
            public const string GetQueueForRadiologyCenter = "/queue/radiology/{radiologyCenterId}";
            public const string GetAllAppointmentsForMedicalLab = "/appointments/medical-lab/{medicalLabId}";
            public const string GetQueueForMedicalLab = "/queue/medical-lab/{medicalLabId}";

            // Tickets
            public const string GetOpenTicketsByNationalId = "/tickets/national-id/{nationalId}";
            public const string GetOpenTicketsByPatientId = "/tickets/patient/{patientId}";
            public const string CreateNewTicket = "/tickets";

            // Appointment Management
            public const string CreateClinicAppointment = "/appointments/clinic";
            public const string CreateRadiologyAppointment = "/appointments/radiology";
            public const string CreateMedicalLabAppointment = "/appointments/medical-lab";
            public const string TerminateAppointment = "/appointments/{appointmentId}/terminate";

            // Appointment Swapping
            public const string SwapClinicAppointments = "/appointments/clinic/swap";
            public const string SwapMedicalLabAppointments = "/appointments/medical-lab/swap";
            public const string SwapRadiologyCenterAppointments = "/appointments/radiology/swap";

            // Appointment Rescheduling
            public const string RescheduleClinicAppointment = "/appointments/clinic/{appointmentId}/reschedule";
            public const string RescheduleMedicalLabAppointment = "/appointments/medical-lab/{appointmentId}/reschedule";
            public const string RescheduleRadiologyAppointment = "/appointments/radiology/{appointmentId}/reschedule";

            // Queue Management
            public const string RemoveClinicAppointmentFromQueue = "/appointments/clinic/{appointmentId}/remove-from-queue";
            public const string RemoveMedicalLabAppointmentFromQueue = "/appointments/medical-lab/{appointmentId}/remove-from-queue";
            public const string RemoveRadiologyCenterAppointmentFromQueue = "/appointments/radiology/{appointmentId}/remove-from-queue";
            public const string AddClinicAppointmentForQueue = "/appointments/clinic/{appointmentId}/add-to-queue";
            public const string AddMedicalLabAppointmentForQueue = "/appointments/medical-lab/{appointmentId}/add-to-queue";
            public const string AddRadiologyCenterAppointmentForQueue = "/appointments/radiology/{appointmentId}/add-to-queue";

            // Time Slots
            public const string GetAvailableTimeSlotsForClinic = "/time-slots/clinic/{clinicId}";
            public const string GetAvailableTimeSlotsForMedicalLab = "/time-slots/medical-lab/{medicalLabId}";
            public const string GetAvailableTimeSlotsForRadiologyCenter = "/time-slots/radiology/{radiologyCenterId}";

            // Appointment Start
            public const string StartClinicAppointment = "/appointments/clinic/{appointmentId}/start";
            public const string StartRadiologyAppointment = "/appointments/radiology/{appointmentId}/start";
            public const string StartMedicalLabAppointment = "/appointments/medical-lab/{appointmentId}/start";
        }
    }
}