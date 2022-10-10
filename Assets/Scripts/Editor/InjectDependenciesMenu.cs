using HerderGames.Schule;
using HerderGames.Zeit;
using UnityEditor;
using UnityEngine;

namespace HerderGames.Editor
{
    public static class InjectDependenciesMenu
    {
        [MenuItem("Herder Games/Inject Dependencies")]
        public static void InjectDependencies()
        {
            if (Selection.count != 1)
            {
                return;
            }

            var player = Object.FindObjectOfType<Player.Player>();
            var timeManager = Object.FindObjectOfType<TimeManager>();
            var internetManager = Object.FindObjectOfType<InternetManager>();
            var alarmManager = Object.FindObjectOfType<AlarmManager>();

            var lehrer = Selection.gameObjects[0];
            foreach (var component in lehrer.GetComponents<Component>())
            {
                var serializedObject = new SerializedObject(component);
                if (serializedObject.FindProperty("Player") != null) serializedObject.FindProperty("Player").objectReferenceValue = player;
                if (serializedObject.FindProperty("TimeManager") != null) serializedObject.FindProperty("TimeManager").objectReferenceValue = timeManager;
                if (serializedObject.FindProperty("Internet") != null) serializedObject.FindProperty("Internet").objectReferenceValue = internetManager;
                if (serializedObject.FindProperty("AlarmManager") != null) serializedObject.FindProperty("AlarmManager").objectReferenceValue = alarmManager;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
