using System.Net;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public interface ITextRepository
    {
        Task<dynamic> Create(Text newText);
        Task<dynamic> GetAll();
        Task<dynamic> GetByKey(string key);
        Text Remove(string key);
        Task<dynamic> Update(Text text);
    }
}
