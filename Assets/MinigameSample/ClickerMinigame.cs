using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BananaParty.Minigame.Sample
{
    public class ClickerMinigame : IMinigame<int>
    {
        private const string SceneName = "ClickerMinigameScene";

        private string _languageCode = "en";
        private float _volume = 1f;

        private ClickerMinigameCanvas _clickerMinigameCanvas;

        public bool IsMinigameFinished => _clickerMinigameCanvas ? _clickerMinigameCanvas.IsGameFinished : false;

        public int MinigamePlayResult => _clickerMinigameCanvas ? _clickerMinigameCanvas.ClickCount : 0;

        public MinigameAsyncOperation StartMinigame()
        {
            MinigameAsyncOperation minigameAsyncOperation = new();
            StartMinigameAsync(minigameAsyncOperation);
            return minigameAsyncOperation;
        }

        private async void StartMinigameAsync(MinigameAsyncOperation startAsyncOperation)
        {
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            
            while (!loadingOperation.isDone)
                await Task.Yield();

            // Custom startup code should be here if needed
            _clickerMinigameCanvas = Object.FindAnyObjectByType<ClickerMinigameCanvas>();
            SetSoundVolume(_volume);
            _clickerMinigameCanvas.SetLanguage(_languageCode);
            //

            startAsyncOperation.Complete();
        }

        public MinigameAsyncOperation StopMinigame()
        {
            MinigameAsyncOperation minigameAsyncOperation = new();
            StopMinigameAsync(minigameAsyncOperation);
            return minigameAsyncOperation;
        }

        private async void StopMinigameAsync(MinigameAsyncOperation stopAsyncOperation)
        {
            AsyncOperation unloadingOperation = SceneManager.UnloadSceneAsync(SceneName);

            // Custom cleanup code should be here if needed

            //

            while (!unloadingOperation.isDone)
                await Task.Yield();

            stopAsyncOperation.Complete();
        }

        public void SetSoundVolume(float volume)
        {
            _volume = volume;

            foreach (AudioSource audioSource in Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
                audioSource.volume = volume;
        }

        public void SetLanguage(string languageCode)
        {
            _languageCode = languageCode;
        }
    }
}
