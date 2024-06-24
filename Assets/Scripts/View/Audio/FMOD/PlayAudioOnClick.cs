using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

namespace RogueIslands.View.Audio.FMOD
{
    public class PlayAudioOnClick : MonoBehaviour
    {
        [SerializeField] private EventReference _clip;
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(PlayAudio);
        }
        
        private void PlayAudio()
        {
            RuntimeManager.PlayOneShot(_clip);
        }
    }
}