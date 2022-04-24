using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HerderGames.Lehrer.Sprache;
using UnityEngine;

namespace HerderGames.Lehrer.AI.Goals
{
    public class ErschoepfungGoal : GoalBase
    {
        [SerializeField] private float MaximaleDistanzProMinute;
        [SerializeField] private float MaxiamleHoeheProMinute;
        [SerializeField] private float LaengeDerPause;
        [SerializeField] private SaetzeMoeglichkeitenMehrmals Saetze;

        private Vector3 LastPosition;
        private readonly List<float> DistanzLetzeMinute = new();
        private readonly List<float> HoeheLetzteMinute = new();
        private bool MachtGeradePause;

        public override bool ShouldRun(bool currentlyRunning)
        {
            return DistanzLetzeMinute.Sum() >= MaximaleDistanzProMinute ||
                   HoeheLetzteMinute.Sum() >= MaxiamleHoeheProMinute || MachtGeradePause;
        }

        public override void OnGoalEnd(GoalEndReason reason)
        {
            MachtGeradePause = false;
        }

        public override IEnumerator Execute()
        {
            DistanzLetzeMinute.Clear();
            HoeheLetzteMinute.Clear();
            MachtGeradePause = true;
            Lehrer.Sprache.SaetzeMoeglichkeiten = Saetze;
            yield return new WaitForSeconds(LaengeDerPause);
            MachtGeradePause = false;
        }

        private void Start()
        {
            StartCoroutine(RecordDistanz());
        }
        
        public IEnumerator RecordDistanz()
        {
            float GetYDistance(Vector3 pos1, Vector3 pos2)
            {
                return Mathf.Abs(pos1.y - pos2.y);
            }
            
            float GetXAndZDistance(Vector3 pos1, Vector3 pos2)
            {
                return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.z - pos2.z);
            }
            
            void AddEintrag(List<float> liste, float eintrag)
            {
                if (liste.Count >= 60)
                {
                    liste.RemoveAt(0);
                }

                liste.Add(eintrag);
            }

            LastPosition = transform.position;
            yield return null;

            while (true)
            {
                var currentPosition = transform.position;

                var distance = GetXAndZDistance(LastPosition, currentPosition);
                AddEintrag(DistanzLetzeMinute, distance);

                var hoehe = GetYDistance(LastPosition, currentPosition);
                AddEintrag(HoeheLetzteMinute, hoehe);

                LastPosition = currentPosition;
                
                yield return new WaitForSeconds(1f);
            }
        }
    }
}