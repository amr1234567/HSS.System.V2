using FluentResults;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.Domain.ResultHelpers.Errors
{
    public class EntityAlreadyExistsError : Error
    {
        public EntityAlreadyExistsError(string message) : base(message)
        { }

        public EntityAlreadyExistsError() : base("Entity Already Exist") { }

        public static EntityAlreadyExistsError Happen<T>()
        {
            return new EntityAlreadyExistsError($"Entity of type '{typeof(T).Name}' already exists");
        }

        public static EntityAlreadyExistsError Happen<T>(object identifier)
        {
            return new EntityAlreadyExistsError($"Entity of type '{typeof(T).Name}' and identifier : '{identifier}' already exists");
        }
    }
}
