using System.Collections.Generic;
using UnityEngine;

namespace RogueRealms
{
    public static class DefDatabase<T> where T : Def
    {
        static Dictionary<string, T> _byName;
        static List<T> _list;

        static void EnsureLoaded()
        {
            if (_byName != null) return;

            _byName = new Dictionary<string, T>();
            var all = Resources.LoadAll<T>("Defs");
            foreach (var def in all)
            {
                if (string.IsNullOrEmpty(def.defName))
                {
                    Debug.LogWarning($"[DefDatabase] {def.name} ({typeof(T).Name}) missing defName.");
                    continue;
                }
                if (_byName.ContainsKey(def.defName))
                {
                    Debug.LogWarning($"[DefDatabase] Duplicate defName '{def.defName}' for {typeof(T).Name}.");
                    continue;
                }
                _byName.Add(def.defName, def);
            }
        }

        public static T Get(string defName)
        {
            EnsureLoaded();
            _byName.TryGetValue(defName, out var def);
            if (def == null)
                Debug.LogError($"[DefDatabase] Missing {typeof(T).Name} '{defName}'.");
            return def;
        }

        public static bool TryGet(string defName, out T def)
        {
            EnsureLoaded();
            return _byName.TryGetValue(defName, out def);
        }

        public static IEnumerable<T> All()
        {
            EnsureLoaded();
            return _byName.Values;
        }

        public static T Random()
        {
            EnsureLoaded();
            if (_list == null) _list = new List<T>(_byName.Values);
            if (_list.Count == 0) return null;
            return _list[UnityEngine.Random.Range(0, _list.Count)];
        }

        public static void Clear()
        {
            _byName = null;
            _list = null;
        }
    }
}
