using _Project.Script.Core.EventBus;
using _Project.Script.Runtime;
using _Project.Script.UI.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Script.Core.Managers
{
    public class GameManager : MonoBehaviour
    {
        private void OnEnable()
        {
            GameEventBus.OnPause += PlayPause;
            GameEventBus.OnRestart += RestartGame;
        }

        private void OnDisable()
        {
            GameEventBus.OnPause -= PlayPause;
            GameEventBus.OnRestart -= RestartGame;
        }

        private void Start()
        {
            SetInitialParameters();
        }

        private void SetInitialParameters()
        {
            GameModels.ViewId = 0;
            GameEventBus.ChangeView(GameModels.ViewId);
            GameModels.IsPaused = true;
            GameEventBus.PauseGameTrigger(GameModels.IsPaused);
        }

        private void PlayPause(bool isPause)
        {
            Time.timeScale = isPause ? 0f : 1f;
        }
        
        private void RestartGame()
        {
            SecondsCounter.StopSecondsCounter();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}