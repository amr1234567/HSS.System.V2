namespace HSS.System.V2.Domain.Models.Common
{
    public interface IInputModel<T>
    {
        T ToModel();
    }
}
