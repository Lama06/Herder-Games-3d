using UnityEngine;

namespace HerderGames.Schule
{
    public class InternetManager : MonoBehaviour
    {
        public LanDose[] LanDosen { get; private set; }
        
        private void Awake()
        {
            LanDosen = FindObjectsOfType<LanDose>();
        }

        public bool IsInternetVerfuegbar()
        {
            foreach (var lanDose in LanDosen)
            {
                if (lanDose.Mic)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
