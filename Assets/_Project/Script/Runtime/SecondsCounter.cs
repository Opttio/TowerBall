using System.Threading;
using _Project.Script.Core.EventBus;
using _Project.Script.UI.Models;
using Cysharp.Threading.Tasks;

namespace _Project.Script.Runtime
{
    public static class SecondsCounter
    {
        private static CancellationTokenSource _cts;
        public static void StartSecondsCounter()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            SecondsCountAsync(_cts.Token).Forget();
        }
        
        public static void StopSecondsCounter()
        {
            _cts?.Cancel();
            _cts = null;
        }

        private static async UniTask SecondsCountAsync(CancellationToken token)
        {
            while (GameModels.Seconds > 0 && !token.IsCancellationRequested)
            {
                GameModels.Seconds--;
                GameEventBus.ChangeSeconds(GameModels.Seconds);
                await UniTask.Delay(1000);
            }
        }
    }
}