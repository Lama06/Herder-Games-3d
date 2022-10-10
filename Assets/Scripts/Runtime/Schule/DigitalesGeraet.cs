using System;
using System.Collections.Generic;
using HerderGames.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HerderGames.Schule
{
    public class DigitalesGeraet : MonoBehaviour
    {
        private static readonly List<string> DefaultUrls = new()
        {
            "https://www.youtube.com/watch?v=MSRKOhcxnow", // Kuckuksmilch
            "https://www.youtube.com/watch?v=EqekSuj5HCo", // Walter Frosch,
            "https://www.youtube.com/watch?v=lfcGEQRyaqI", // Neulich bei einer Umfrage
            "https://www.youtube.com/watch?v=Jx_ng-kGZE4", // Es wird Suppe gegessen
            "https://www.youtube.com/watch?v=PeETF41yXbo", // Was ist denn mit Karsten los
            "https://www.youtube.com/watch?v=Dqh_wGAYjls", // Ich bin reich und ich scheiß auf alles
            "https://www.youtube.com/watch?v=dhETW09NFDk", // Big Tasty Kritiker,
            "https://www.youtube.com/watch?v=rUsxFl-1JtE", // Es ist Obst im Haus
        };

        [SerializeField] private string InteraktionsMenuName;
        [SerializeField] private bool UseDefaultUrls = true;
        [SerializeField] private string[] Urls;

        private int InteraktionsMenuId;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Player.Player>(out var player))
            {
                return;
            }

            InteraktionsMenuId = player.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
            {
                Name = InteraktionsMenuName,
                Callback = _ =>
                {
                    string url;
                    
                    if (UseDefaultUrls)
                    {
                        url = DefaultUrls[Random.Range(0, DefaultUrls.Count)];
                    }
                    else
                    {
                        if (Urls.Length == 0)
                        {
                            return;
                        }

                        url = Urls[Random.Range(0, Urls.Length)];
                    }
                    
                    Application.OpenURL(url);
                }
            });
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Player.Player>(out var player))
            {
                player.InteraktionsMenu.RemoveEintrag(InteraktionsMenuId);
            }
        }
    }
}
