using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Util
{
    public class PersistentDataManager : MonoBehaviour
    {
        private bool ShouldSaveData = true;
    
        private List<PersistentDataContainer> GetContainers()
        {
            var result = new List<PersistentDataContainer>();
            
            foreach (var component in FindObjectsOfType<MonoBehaviour>())
            {
                if (component is PersistentDataContainer persistent)
                {
                    result.Add(persistent);
                }
            }

            return result;
        }

        private void LoadData()
        {
            foreach (var container in GetContainers())
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
            
            foreach (var container in GetContainers())
            {
                container.SaveData();
            }
            
            PlayerPrefs.Save();
        }

        public void DeleteData()
        {
            foreach (var container in GetContainers())
            {
                container.DeleteData();
            }
            
            // Wenn die Daten nach dem Löschen gespeichert werden, werden die daten vor der Löschung geladen
            ShouldSaveData = false;
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
    }
}
