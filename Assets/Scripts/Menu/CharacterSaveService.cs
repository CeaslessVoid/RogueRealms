using UnityEngine;

namespace RogueRealms
{
    public static class CharacterSaveService
    {
        const string KeyBody = "RR_Body";
        const string KeyHead = "RR_Head";
        const string KeyHair = "RR_Hair";
        const string KeyClass = "RR_Class";

        static bool loaded;

        public static void EnsureProfileLoaded()
        {
            if (loaded) return;
            loaded = true;

            if (PlayerPrefs.HasKey(KeyBody))
            {
                CharacterProfile.body = DefDatabase<BodyTypeDef>.Get(PlayerPrefs.GetString(KeyBody));
                CharacterProfile.head = DefDatabase<HeadTypeDef>.Get(PlayerPrefs.GetString(KeyHead));

                var hairName = PlayerPrefs.GetString(KeyHair, "");
                CharacterProfile.hair = string.IsNullOrEmpty(hairName) ? null : DefDatabase<HairDef>.Get(hairName);

                var classNameSaved = PlayerPrefs.GetString(KeyClass, "");
                CharacterProfile.selectedClass = string.IsNullOrEmpty(classNameSaved) ? null : DefDatabase<ClassDef>.Get(classNameSaved);
            }
            else
            {
                CharacterProfile.body = DefDatabase<BodyTypeDef>.Random();
                CharacterProfile.head = DefDatabase<HeadTypeDef>.Random();
                CharacterProfile.hair = DefDatabase<HairDef>.Random();
            }

            if (CharacterProfile.body == null) CharacterProfile.body = DefDatabase<BodyTypeDef>.Random();
            if (CharacterProfile.head == null) CharacterProfile.head = DefDatabase<HeadTypeDef>.Random();

            if (CharacterProfile.body == null) Debug.LogWarning("[CharacterSaveService] No BodyTypeDef found under Resources/Defs.");
            if (CharacterProfile.head == null) Debug.LogWarning("[CharacterSaveService] No HeadTypeDef found under Resources/Defs.");
        }

        public static void Save()
        {
            if (CharacterProfile.body != null) PlayerPrefs.SetString(KeyBody, CharacterProfile.body.defName);
            if (CharacterProfile.head != null) PlayerPrefs.SetString(KeyHead, CharacterProfile.head.defName);
            PlayerPrefs.SetString(KeyHair, CharacterProfile.hair != null ? CharacterProfile.hair.defName : "");
            PlayerPrefs.SetString(KeyClass, CharacterProfile.selectedClass != null ? CharacterProfile.selectedClass.defName : "");
            PlayerPrefs.Save();
        }
    }
}
