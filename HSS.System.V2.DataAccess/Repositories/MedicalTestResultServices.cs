using FluentResults;
using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Appointments;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class MedicalTestResultServices(AppDbContext context) : IMedicalLabTestResultServices
    {
        public async Task<Result> CreateTestResultAsync(string medicalLapAppointmentId, TestResultDto result)
        {
            try
            {
                if (string.IsNullOrEmpty(medicalLapAppointmentId) || result is null)
                    return Result.Fail("Medical Lab Appointment Id and Result is Requried");

                var medicalLapAppointment = await context.MedicalLabAppointments.FindAsync(medicalLapAppointmentId);
                if (medicalLapAppointment is null)
                    return EntityNotExistsError.Happen<MedicalLabAppointment>(medicalLapAppointmentId);

                var resultAsJson = JsonSerializer.Serialize(result);
                medicalLapAppointment.Result = resultAsJson;
                await context.AddRangeAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result> DeleteTestResultAsync(string medicalLapAppointmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(medicalLapAppointmentId))
                    return Result.Fail("Id is required");

                var medicalLapAppointment = await context.MedicalLabAppointments.FindAsync(medicalLapAppointmentId);
                if (medicalLapAppointment is null)
                    return EntityNotExistsError.Happen<MedicalLabAppointment>(medicalLapAppointmentId);

                medicalLapAppointment.Result = null;
                await context.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<TestResultDto>> GetTestResultForMedicalLabAppointment(string medicalLapAppointmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(medicalLapAppointmentId))
                    return Result.Fail("Id is required");

                var medicalLapAppointment = await context.MedicalLabAppointments.FindAsync(medicalLapAppointmentId);
                if (medicalLapAppointment is null)
                    return EntityNotExistsError.Happen<MedicalLabAppointment>(medicalLapAppointmentId);

                var result = JsonSerializer.Deserialize<TestResultDto>(medicalLapAppointment.Result);
                return result is not null ?
                    Result.Ok(result) :
                    Result.Fail<TestResultDto>(new UnSupportedBehaviorError("Can't convert the result string to model"));
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result> UpdateTestResultAsync(string medicalLapAppointmentId, TestResultDto result)
        {
            try
            {
                if (string.IsNullOrEmpty(medicalLapAppointmentId) || result is null)
                    return Result.Fail("Medical Lab Appointment Id and Result is Required");

                var medicalLapAppointment = await context.MedicalLabAppointments.FindAsync(medicalLapAppointmentId);
                if (medicalLapAppointment == null)
                    return EntityNotExistsError.Happen<MedicalLabAppointment>(medicalLapAppointmentId);

                medicalLapAppointment.Result = JsonSerializer.Serialize(result);
                await context.SaveChangesAsync();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }
    }
}
