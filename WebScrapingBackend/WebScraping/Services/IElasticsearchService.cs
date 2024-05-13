namespace WebScraping.Services
{
    public interface IElasticsearchService<T>
    {
        Task<string> CreateDocumentAsync(T document);
        Task<string> DeleteDocumentAsync(int id);
        Task<IEnumerable<T>> GetAllDocuments();
        Task<T> GetDocumentAsync(int id);
        Task<string> UpdateDocumentAsync(T document);
    }
}
