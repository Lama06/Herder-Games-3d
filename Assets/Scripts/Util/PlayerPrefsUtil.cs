using HerderGames.Zeit;
using UnityEngine;

namespace HerderGames.Util
{
    public static class PlayerPrefsUtil
    {
        public static bool GetBool(string path, bool defaultValue)
        {
            return PlayerPrefs.GetInt(path, defaultValue ? 1 : 0) == 1;
        }

        public static void SetBool(string path, bool value)
        {
            PlayerPrefs.SetInt(path, value ? 1 : 0);
        }

        public static void SetVector(string path, Vector3 vector)
        {
            PlayerPrefs.SetFloat($"{path}.x", vector.x);
            PlayerPrefs.SetFloat($"{path}.y", vector.y);
            PlayerPrefs.SetFloat($"{path}.z", vector.z);
        }

        public static Vector3 GetVector(string path, Vector3 defaultValue)
        {
            var x = PlayerPrefs.GetFloat($"{path}.x", defaultValue.x);
            var y = PlayerPrefs.GetFloat($"{path}.y", defaultValue.y);
            var z = PlayerPrefs.GetFloat($"{path}.z", defaultValue.z);
            return new Vector3(x, y, z);
        }

        public static void DeleteVector(string path)
        {
            PlayerPrefs.DeleteKey($"{path}.x");
            PlayerPrefs.DeleteKey($"{path}.y");
            PlayerPrefs.DeleteKey($"{path}.z");
        }

        public static Zeitpunkt GetZeitpunkt(string path, Zeitpunkt defaultValue)
        {
            var kalenderwoche = PlayerPrefs.GetInt($"{path}.kalenderwoche", defaultValue.Kalenderwoche);
            var wochentag = (Wochentag) PlayerPrefs.GetInt($"{path}.wochentag", (int) defaultValue.Wochentag);
            var zeit = PlayerPrefs.GetFloat($"{path}.zeit", defaultValue.Time);
            return new Zeitpunkt
            {
                Kalenderwoche = kalenderwoche,
                Wochentag = wochentag,
                Time = zeit
            };
        }

        public static void SetZeitpunkt(string path, Zeitpunkt zeitpunkt)
        {
            PlayerPrefs.SetInt($"{path}.kalenderwoche", zeitpunkt.Kalenderwoche);
            PlayerPrefs.SetInt($"{path}.wochentag", (int) zeitpunkt.Wochentag);
            PlayerPrefs.SetFloat($"{path}.time", zeitpunkt.Time);
        }

        public static void DeleteZeitpunkt(string path)
        {
            PlayerPrefs.DeleteKey($"{path}.kalenderwoche");
            PlayerPrefs.DeleteKey($"{path}.wochentag");
            PlayerPrefs.DeleteKey($"{path}.time");
        }
    }
}
