namespace BRD.WebStore.Catalog.Infrastructure.Interfaces;

public interface IUserContext
{
    int? CurrentUserId { get; set; }
}