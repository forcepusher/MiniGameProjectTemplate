using UnityEngine;

namespace BananaParty.Minigame
{
    public interface IMinigame<TPlayResult>
    {
        /// <summary>
        /// Use this layer as your default minigame layer to avoid conflicts with the main game scene.
        /// Light settings Culling Mask must be enabled only for <see cref="MainMinigameLayer"/> and <see cref="AdditionalMinigameLayer"/> exclusively.
        /// </summary>
        public int MainMinigameLayer { get => 30; }
        /// <summary>
        /// Use this layer for your custom minigame logic that might need an extra layer.
        /// </summary>
        public int AdditionalMinigameLayer { get => 31; }

        public AsyncOperation StartMinigame();

        public AsyncOperation EndMinigame();

        public bool IsMinigameFinished { get; }

        public TPlayResult MinigamePlayResult { get; }
    }
}
