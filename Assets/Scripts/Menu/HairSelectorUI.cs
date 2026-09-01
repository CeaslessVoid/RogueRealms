using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RogueRealms
{
    public class HairSelectorUI : MonoBehaviour
    {
        public Transform content;
        public DefListItemButton itemPrefab;
        public TMP_InputField searchField;
        public HumanoidBodyDrawer drawer;

        List<HairDef> all;

        void Start()
        {
            all = new List<HairDef>(DefDatabase<HairDef>.All());
            Populate(all);
            if (searchField != null) searchField.onValueChanged.AddListener(OnSearchChanged);
        }

        void OnSearchChanged(string query)
        {
            var filtered = string.IsNullOrEmpty(query)
                ? all
                : all.Where(d => d.displayName.ToLower().Contains(query.ToLower())).ToList();
            Populate(filtered);
        }

        void Populate(List<HairDef> defs)
        {
            foreach (Transform child in content) Destroy(child.gameObject);

            foreach (var def in defs)
            {
                var item = Instantiate(itemPrefab, content);
                item.Setup(def.displayName, def.sprites.north, def.sprites.east, def.sprites.south,
                    () => { drawer.SetHair(def); CharacterProfile.hair = def; });
            }
        }
    }
}
