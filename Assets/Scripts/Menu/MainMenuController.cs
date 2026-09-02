using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueRealms
{
    public class MainMenuController : MonoBehaviour
    {
        public void PlayGame()
        {
            CharacterSaveService.Save();
            SceneManager.LoadScene("Game");
        }
    }
}
