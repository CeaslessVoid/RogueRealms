using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueRealms
{
    public class MainMenuController : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
