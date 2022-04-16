using HerderGames.Time;
using UnityEngine;

namespace HerderGames.AI.Goals
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SchuleBetretenVerlassenSharedData : MonoBehaviour
    {
        public TimeManager TimeManager;
        public bool InDerSchule;
        public WoechentlicheZeitspannen ZeitInDerSchule;

        private MeshRenderer Renderer;

        private void Awake()
        {
            Renderer = GetComponent<MeshRenderer>();
        }

        public bool ShouldBeInSchool()
        {
            return ZeitInDerSchule.IsInside(TimeManager.GetCurrentWochentag(), TimeManager.GetCurrentTime());
        }

        public void SetVisible(bool visible)
        {
            Renderer.enabled = visible;
        }
    }
}