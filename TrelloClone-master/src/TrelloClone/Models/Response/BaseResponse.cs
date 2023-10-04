using TrelloClone.Models.Enum;

namespace TrelloClone.Data.Repositories
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string? Description { get; set; }
        public StatusCodes StatusCode { get; set; }
        public T? Data { get; set; }

    }

    public interface IBaseResponse<T>
    {
        string? Description { get; }
        StatusCodes StatusCode { get; }
        T? Data { get; }
    }
}
