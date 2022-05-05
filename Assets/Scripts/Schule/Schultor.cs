using System;
using System.Collections;
using System.Collections.Generic;
using HerderGames.Player;
using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Schule
{
    public class Schultor : MonoBehaviour
    {
        [SerializeField] private TimeManager TimeManager;
        [SerializeField] private float Zeit;

        private Player.Player PlayerInTrigger;

        private void Start()
        {
            StartCoroutine(ManageInteraktionsMenu());
        }

        private IEnumerator ManageInteraktionsMenu()
        {
            bool ShouldShow() => PlayerInTrigger != null;

            while (true)
            {
                yield return new WaitUntil(ShouldShow);
                var player = PlayerInTrigger;
                var ids = new List<int>
                {
                    player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                    {
                        Name = "Nacht/Wochenende überspringen",
                        Callback = _ =>
                        {
                            if (StundenPlanRaster.GetCurrentStundenPlanEintrag(TimeManager) != null)
                            {
                                player.Chat.SendChatMessage("Du kannst keine Zeit überspringen, wenn gerade Unterricht / Lernezeit / Pause ist");
                                return;
                            }

                            Wochentag naechsterWochentag;
                            if (TimeManager.GetCurrentTime() < StundenPlanRaster.SchuleBeginn)
                            {
                                naechsterWochentag = TimeManager.GetCurrentWochentag();
                            }
                            else if (TimeManager.GetCurrentWochentag() is Wochentag.Freitag or Wochentag.Samstag)
                            {
                                naechsterWochentag = Wochentag.Montag;
                                TimeManager.SetCurrentKalenderwoche(TimeManager.GetCurrentKalenderwoche() + 1);
                            }
                            else
                            {
                                naechsterWochentag = TimeManager.GetCurrentWochentag().GetNextWochentag();
                            }
                            
                            TimeManager.SetCurrentWochentag(naechsterWochentag);
                            TimeManager.SetCurrentTime(Zeit);
                        }
                    })
                };
                yield return new WaitWhile(ShouldShow);
                ids.ForEach(player.InteraktionsMenu.RemoveEintrag);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player.Player>(out var player))
            {
                PlayerInTrigger = player;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Player.Player>() != null)
            {
                PlayerInTrigger = null;
            }
        }
    }
}
