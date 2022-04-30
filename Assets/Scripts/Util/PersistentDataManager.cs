using System;
using System.Collections.Generic;
using UnityEngine;

namespace HerderGames.Util
{
    public class PersistentDataManager : MonoBehaviour
    {
        private readonly List<PersistentDataContainer> Persistent = new();

        private void Awake()
        {
            foreach (var component in FindObjectsOfType<MonoBehaviour>())
            {
                if (component is PersistentDataContainer persistent)
                {
                    Persistent.Add(persistent);
                }
            }
        }

        private void Start()
        {
            foreach (var persistent in Persistent)
            {
                persistent.LoadData();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                return;
            }

            foreach (var persistent in Persistent)
            {
                persistent.SaveData();
            }
        }

        private void OnDestroy()
        {
            foreach (var persistent in Persistent)
            {
                persistent.SaveData();
            }
        }

        public void Reset()
        {
            foreach (var persistent in Persistent)
            {
                persistent.ResetData();
            }
        }
    }
}
