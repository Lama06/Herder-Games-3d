using System;
using System.Collections;
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

        private IEnumerator Start()
        {
            LoadData();
            
            while (true)
            {
#if UNITY_EDITOR
                yield return new WaitForSeconds(1);
#else
                yield return new WaitForSeconds(5);
#endif
                SaveData();
            }
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
                    Debug.Log(
                        $"Ein {nameof(PersistentDataContainer)} hat wahrscheinlich versucht in seiner SaveData() Methode auf sein GameObject zuzugreifen, " +
                        $"obwohl das schon deaktiviert ist: {e}");
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

            // Wenn die Daten nach dem Löschen gespeichert werden, werden die Daten vor der Löschung wiederhergestellt
            ShouldSaveData = false;
        }
    }
}