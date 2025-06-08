namespace HSS.System.V2.Services.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveAllChanges();
}
