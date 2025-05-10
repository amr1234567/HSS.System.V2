namespace HSS.System.V2.Domain.Models.Common
{
    public interface IOutputDto<TDto, TModel>
    {
        TDto MapFromModel(TModel model);
    }
}
