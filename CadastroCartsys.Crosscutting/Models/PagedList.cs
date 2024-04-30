namespace CadastroCartsys.Crosscutting.Models;

public class PagedList<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string Order { get; set; }
    public int TotalPages { get; set; }
    public int TotalItens { get; set; }
    public ICollection<T> Itens { get; set; } = new List<T>();
}