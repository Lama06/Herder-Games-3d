using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Lehrer.AI.Goals;
using UnityEngine;

namespace HerderGames.Lehrer.Sprache
{
    public class Gespraech : MonoBehaviour
    {
        [SerializeField] private Lehrer[] Teilnehmer;
        [SerializeField] private DialogEintrag[] Dialog;
        [SerializeField] private float Delay;

        private readonly HashSet<Lehrer> Anwesend = new();

        private void Start()
        {
            StartCoroutine(ManageAnwesendListe());
            StartCoroutine(ManageDialog());
        }

        private IEnumerator ManageAnwesendListe()
        {
            while (true)
            {
                foreach (var lehrer in Teilnehmer)
                {
                    var angekommen = IsLehrerAngekommen(lehrer);

                    if (angekommen)
                    {
                        Anwesend.Add(lehrer);
                    }
                    else
                    {
                        Anwesend.Remove(lehrer);
                    }
                }

                yield return null;
            }
        }

        private bool IsLehrerAngekommen(Lehrer lehrer)
        {
            if (lehrer.AI.CurrentGoal is not AnGespraechTeilnehmenGoal goal)
            {
                return false;
            }

            if (goal.GetGespraech() != this)
            {
                return false;
            }

            return goal.IsAngekommen;
        }

        private bool AlleAnwesend => Teilnehmer.Length == Anwesend.Count;

        private IEnumerator ManageDialog()
        {
            while (true)
            {
                yield return new WaitUntil(() => AlleAnwesend);

                foreach (var eintrag in Dialog)
                {
                    if (!AlleAnwesend)
                    {
                        goto OuterContinue;
                    }

                    eintrag.Sprecher.Sprache.Say(eintrag.Satz);
                    yield return new WaitForSeconds(Delay);
                }

                OuterContinue: ;
            }
        }

        [Serializable]
        public class DialogEintrag
        {
            public Lehrer Sprecher;
            public string Satz;
        }
    }
}