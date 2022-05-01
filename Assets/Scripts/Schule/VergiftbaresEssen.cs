using System.Collections;
using System.Collections.Generic;
using HerderGames.Player;
using HerderGames.Util;
using UnityEngine;

namespace HerderGames.Schule
{
    public class VergiftbaresEssen : MonoBehaviour, PersistentDataContainer
    {
        [SerializeField] private string Id;
        [SerializeField] private Transform Standpunkt;
        [SerializeField] private string InteraktionsMenuNameNormal;
        [SerializeField] private string InteraktionsMenuNameOrthamol;
        [SerializeField] private int ZeitZumVergiften;
        [SerializeField] private float SchwereDesVerbrechens;
        [SerializeField] private int SchadenFuerDieSchule;

        public bool Vergiftet { get; private set; }
        public VergiftungsType VergiftungsTyp { get; private set; }
        public bool VergiftungBemerkt { get; private set; }
        private Player.Player PlayerInTrigger;

        public Vector3 GetStandpunkt()
        {
            return Standpunkt.position;
        }

        public void Entgiften()
        {
            Vergiftet = false;
        }

        public void Bemerken()
        {
            VergiftungBemerkt = true;
        }
        
        private void Vergiften(VergiftungsType type)
        {
            Vergiftet = true;
            VergiftungsTyp = type;
            VergiftungBemerkt = false;
        }
        
        private void Start()
        {
            StartCoroutine(ManageInterationsMenu());
        }

        private IEnumerator ManageInterationsMenu()
        {
            bool ShouldShow() => PlayerInTrigger != null && !Vergiftet;

            void AddEintrag(string name, VergiftungsType type, List<int> ids)
            {
                ids.Add(PlayerInTrigger.InteraktionsMenu.AddEintrag(new InteraktionsMenuEintrag
                {
                    Name = name,
                    Callback = _ =>
                    {
                        PlayerInTrigger.VerbrechenManager.VerbrechenStarten(ZeitZumVergiften, SchwereDesVerbrechens, () =>
                        {
                            PlayerInTrigger.Score.SchadenFuerDieSchule += SchadenFuerDieSchule;
                            Vergiften(type);
                        });
                    }
                }));
            }

            while (true)
            {
                yield return new WaitUntil(ShouldShow);
                var player = PlayerInTrigger;
                var ids = new List<int>();
                AddEintrag(InteraktionsMenuNameNormal, VergiftungsType.Normal, ids);
                AddEintrag(InteraktionsMenuNameOrthamol, VergiftungsType.Orthamol, ids);

                yield return new WaitUntil(() => !ShouldShow());
                ids.ForEach(id => player.InteraktionsMenu.RemoveEintrag(id)); // Hier kann nicht PlayerInTrigger benutzt werden, weil er null ist
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

        private string GetSaveKeyRoot()
        {
            return $"essen.{Id}";
        }

        public void LoadData()
        {
            Vergiftet = PlayerPrefsUtil.GetBool($"{GetSaveKeyRoot()}.vergiftet", false);
            VergiftungsTyp = (VergiftungsType) PlayerPrefs.GetInt($"{GetSaveKeyRoot()}.vergiftungs_type", 0);
            VergiftungBemerkt = PlayerPrefsUtil.GetBool($"{GetSaveKeyRoot()}.vergiftung_bemerkt", false);
        }

        public void SaveData()
        {
            PlayerPrefsUtil.SetBool($"{GetSaveKeyRoot()}.vergiftet", Vergiftet);
            PlayerPrefs.SetInt($"{GetSaveKeyRoot()}.vergiftungs_type", (int) VergiftungsTyp);
            PlayerPrefsUtil.SetBool($"{GetSaveKeyRoot()}.vergiftung_bemerkt", VergiftungBemerkt);
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteKey($"{GetSaveKeyRoot()}.ergiftet");
            PlayerPrefs.DeleteKey($"{GetSaveKeyRoot()}.vergiftungs_type");
            PlayerPrefs.DeleteKey($"{GetSaveKeyRoot()}.vergiftung_bemerkt");
        }
    }
    
    public enum VergiftungsType
    {
        Normal,
        Orthamol
    }
}
