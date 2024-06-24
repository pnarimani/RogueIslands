using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RogueIslands.View.Lose
{
    public class LoseScreen : MonoBehaviour
    {
        [SerializeField] private Button _back;
        
        private void Start()
        {
            _back.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        }
    }
}
