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
            public const string GetAllHospitalDepartments = Base + "/hospital-departments";
            public const string GetAllSpecializations = Base + "/specializations";
            public const string GetAllClinics = Base + "/clinics/{specializationId}";
            public const string GetAllRadiologyTestsAvailableInHospital = Base + "/radiology/tests";
            public const string GetAllMedicalLabTestsAvailableInHospital = Base + "/medical-lab/tests";
            public const string GetAllRadiologyCenters = Base + "/radiology-centers";
            public const string GetAllRadiologyCentersDoTest = Base + "/radiology-centers/test/{radiologyTestId}";
            public const string GetAllMedicalLabs = Base + "/medical-labs";
            public const string GetAllMedicalLabsDoTest = Base + "/medical-labs/test/{medicalTestId}";

            // Appointments
            public const string GetAllAppointmentsForClinic = Base + "/appointments/clinic/{clinicId}";
            public const string GetQueueForClinic = Base + "/queue/clinic/{clinicId}";
            public const string GetAllAppointmentsForRadiologyCenter = Base + "/appointments/radiology/{radiologyCenterId}";
            public const string GetQueueForRadiologyCenter = Base + "/queue/radiology/{radiologyCenterId}";
            public const string GetAllAppointmentsForMedicalLab = Base + "/appointments/medical-lab/{medicalLabId}";
            public const string GetQueueForMedicalLab = Base + "/queue/medical-lab/{medicalLabId}";

            // Tickets
            public const string GetOpenTicketsByNationalId = Base + "/tickets/national-id/{nationalId}";
            public const string GetOpenTicketsByPatientId =         Base + "/tickets/patient/{patientId}";
            public const string CreateNewTicket = Base + "/tickets";

            // Appointment Management
            public const string CreateClinicAppointment = Base + "/appointments/clinic";
            public const string CreateRadiologyAppointment = Base + "/appointments/radiology";
            public const string CreateMedicalLabAppointment = Base + "/appointments/medical-lab";
            public const string TerminateAppointment = Base + "/appointments/{appointmentId}/terminate";

            // Appointment Swapping
            public const string SwapClinicAppointments =  Base + "/appointments/clinic/swap";
            public const string SwapMedicalLabAppointments = Base + "/appointments/medical-lab/swap";
            public const string SwapRadiologyCenterAppointments = Base + "/appointments/radiology/swap";

            // Appointment Rescheduling
            public const string RescheduleClinicAppointment = Base + "/appointments/clinic/{appointmentId}/reschedule";
            public const string RescheduleMedicalLabAppointment = Base + "/appointments/medical-lab/{appointmentId}/reschedule";
            public const string RescheduleRadiologyAppointment = Base + "/appointments/radiology/{appointmentId}/reschedule/{departmentId}";

            // Queue Management
            public const string RemoveClinicAppointmentFromQueue = Base + "/appointments/clinic/{appointmentId}/remove-from-queue";
            public const string RemoveMedicalLabAppointmentFromQueue = Base + "/appointments/medical-lab/{appointmentId}/remove-from-queue";
            public const string RemoveRadiologyCenterAppointmentFromQueue = Base + "/appointments/radiology/{appointmentId}/remove-from-queue";
            public const string AddClinicAppointmentForQueue = Base + "/appointments/clinic/{appointmentId}/add-to-queue";
            public const string AddMedicalLabAppointmentForQueue = Base + "/appointments/medical-lab/{appointmentId}/add-to-queue";
            public const string AddRadiologyCenterAppointmentForQueue = Base + "/appointments/radiology/{appointmentId}/add-to-queue";

            // Time Slots
            public const string GetAvailableTimeSlotsForClinic = Base + "/time-slots/clinic/{clinicId}";
            public const string GetAvailableTimeSlotsForMedicalLab = Base + "/time-slots/medical-lab/{medicalLabId}";
            public const string GetAvailableTimeSlotsForRadiologyCenter = Base + "/time-slots/radiology/{radiologyCenterId}";

            // Appointment Start
            public const string StartClinicAppointment = Base + "/appointments/clinic/{appointmentId}/start";
            public const string StartRadiologyAppointment = Base + "/appointments/radiology/{appointmentId}/start";
            public const string StartMedicalLabAppointment = Base + "/appointments/medical-lab/{appointmentId}/start";
        }


        public static class General
        {
            public const string Base = "api/general";

            public const string RadiologyTests = Base + "/radiology-tests";
            public const string MedicalLabTests = Base + "/medical-lab-tests";
        }

        public static class RadiologyCenter
        {
            public const string Base = "api/radiology";

            public const string CurrentRadiologyAppointment = Base + "/appointments/{appointmentId}";
            public const string GetQueueForRadiologyCenter = Base + "/queue";
            public const string RadiologyAppointmentResult = Base + "/appointments/{appointmentId}/result";
            public const string EndAppointment = Base + "/appointments/{appointmentId}/end";
        }

        public static class MedicalLab
        {
            public const string Base = "api/medicaLab";

            public const string EndAppointment = Base + "/appointment/{appointmentId}/end";
            public const string AddMedicalLabTestResult = Base + "/appointments/result";
            public const string GetTestResultField = Base + "/Test-Field/{testId}";
            public const string GetCurrentMedicalLabAppointment = Base + "/appointment/{appointmentId}";
            public const string GetQueueForMedicalLab = Base + "/queue/{medicalLabId}";

        }
    }
}