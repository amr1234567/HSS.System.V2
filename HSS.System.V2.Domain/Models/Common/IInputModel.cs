namespace HSS.System.V2.Domain.Models.Common
{
    public interface IInputModel<T>
    {
        /// <summary>
        /// Method that convert the current DTO object to its model.
        /// </summary>
        /// <returns></returns>
        T ToModel();
    }
}
