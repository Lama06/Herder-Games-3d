using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Player
{
    [RequireComponent(typeof(Player))]
    public class InteraktionsMenu : MonoBehaviour
    {
        private int CurrentEintragId;
        public Dictionary<int, InteraktionsMenuEintrag> Eintraege { get; } = new();

        public int AddEintrag(InteraktionsMenuEintrag eintrag)
        {
            var id = CurrentEintragId++;
            Eintraege.Add(id, eintrag);
            return id;
        }

        public void RemoveEintrag(int eintrag)
        {
            Eintraege.Remove(eintrag);
        }
    }
}