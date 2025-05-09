using FluentResults;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class EntityNotExistsError : Error
    {
        public EntityNotExistsError(string message) : base(message)
        { }

        public EntityNotExistsError() : base("Entity not found") { }

        public static EntityNotExistsError Happen<T>()
        {
            return new EntityNotExistsError($"Entity of type '{typeof(T).Name}'not found");
        }

        public static EntityNotExistsError Happen<T>(object identifier)
        {
            return new EntityNotExistsError($"Entity of type '{typeof(T).Name}' and identifier : '{identifier}' not found");
        }
    }
}
