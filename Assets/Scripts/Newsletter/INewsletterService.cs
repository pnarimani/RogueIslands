using System.Threading;
using Cysharp.Threading.Tasks;

namespace Newsletter
{
    public interface INewsletterService
    {
        UniTask SubscribeAsync(string email, CancellationToken cancellationToken = default);
    }
}
