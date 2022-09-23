namespace TaskManager.Core.Interfaces
{
    public interface IHttpCommandHandlers
    {
        Task<TRes> DeleteRequest<TRes>(string id) where TRes : class;
        void Dispose();
        Task<TRes> GetRequest<TRes>(string requestUrl) where TRes : class;
        Task<TRes> PostRequest<TRes, TReq>(TReq requestModel, string requestUrl)
            where TRes : class
            where TReq : class;
    }
}