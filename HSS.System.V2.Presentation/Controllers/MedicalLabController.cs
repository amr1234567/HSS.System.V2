using HSS.System.V2.Domain.Attributes;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Presentation.Controllers.Base;
using HSS.System.V2.Presentation.Models;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.MedicalLabDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;

using Microsoft.AspNetCore.Mvc;

namespace HSS.System.V2.Presentation.Controllers;

[AuthorizeByEnum(UserRole.MedicalLabTester)]
[ApiExplorerSettings(GroupName = "MedicalLabAPI")]
public class MedicalLabController : CustomBaseController
{
    private readonly IMedicalLabServices _medcialLabServices;
    public MedicalLabController(IMedicalLabServices medicalLabServices)
    {
        _medcialLabServices = medicalLabServices;
    }

    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [HttpPost(ApiRoutes.MedicalLab.AddMedicalLabTestResult)]
    public async Task<IActionResult> AddMedicalLabTestResult([FromBody] AddMedicalLabAppointmentResultModel model, [FromRoute] string appointmentId)
    {
        var result = await _medcialLabServices.AddMedicalLabTestResult(model.Results, appointmentId);
        return GetResponseWithoutType(result);
    }

    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [HttpPost(ApiRoutes.MedicalLab.EndAppointment)]
    public async Task<IActionResult> EndAppointment([FromRoute] string appointmentId)
    {
        var result = await _medcialLabServices.EndAppointment(appointmentId);
        return GetResponseWithoutType(result);
    }

    [ProducesResponseType(typeof(ApiResponse<MedicaLabAppointmentModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MedicaLabAppointmentModel>), StatusCodes.Status400BadRequest)]
    [HttpGet(ApiRoutes.MedicalLab.GetCurrentMedicalLabAppointment)]
    public async Task<IActionResult> GetCurrentRadiologyAppointment([FromRoute] string appointmentId)
    {
        var result = await _medcialLabServices.GetCurrentMedicalLabAppointment(appointmentId);
        return GetResponse(result);
    }

    [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status400BadRequest)]
    [HttpGet(ApiRoutes.MedicalLab.GetQueueForMedicalLab)]
    public async Task<IActionResult> GetQueueForRadiologyCenter([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var departmentId = GetDepartmentId();
        var result = await _medcialLabServices.GetQueueForMedicalLab(departmentId, page, pageSize);
        return GetResponse(result);
    }

    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TestResultField>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TestResultField>>), StatusCodes.Status400BadRequest)]
    [HttpGet(ApiRoutes.MedicalLab.GetTestResultFields)]
    public async Task<IActionResult> GetTestResultFields([FromRoute] string testId)
    {
        var result = await _medcialLabServices.GetTestResultField(testId);
        return GetResponse(result);
    }
}
