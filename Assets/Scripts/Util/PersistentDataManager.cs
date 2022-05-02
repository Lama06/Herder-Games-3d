using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Util
{
    public class PersistentDataManager : MonoBehaviour
    {
        private bool ShouldSaveData = true;
        private readonly List<PersistentDataContainer> PersistentData = new();

        private void Awake()
        {
            foreach (var component in FindObjectsOfType<MonoBehaviour>())
            {
                if (component is PersistentDataContainer persistent)
                {
                    PersistentData.Add(persistent);
                }
            }
        }

        private void Start()
        {
            LoadData();
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                return;
            }

            SaveData();
        }

        private void OnDestroy()
        {
            SaveData();
        }

        private void LoadData()
        {
            foreach (var container in PersistentData)
            {
                container.LoadData();
            }
        }

        private void SaveData()
        {
            if (!ShouldSaveData)
            {
                return;
            }

            foreach (var container in PersistentData)
            {
                try
                {
                    container.SaveData();
                }
                catch (MissingReferenceException e)
                {
                    Debug.Log($"Ein {nameof(PersistentDataContainer)} hat wahrscheinlich versucht in seiner SaveData() Methode auf sein GameObject zuzugreifen, " +
                              $"obwohl das schon deaktiviert: {e}");
                }
            }

            PlayerPrefs.Save();
        }

        public void DeleteData()
        {
            foreach (var container in PersistentData)
            {
                container.DeleteData();
            }

            // Wenn die Daten nach dem Löschen gespeichert werden, werden die daten vor der Löschung wiederhergestellt
            ShouldSaveData = false;
        }
    }
}
