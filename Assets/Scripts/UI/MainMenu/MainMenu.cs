using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip clickSound;
        private UIDocument _uiDocument;

        private AudioSource _audioSource;

        public void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _audioSource = GetComponent<AudioSource>();
            
            Button playButton = _uiDocument.rootVisualElement.Q<Button>(UiConstants.PlayButton);
            playButton.clicked += PlayButtonOnClicked;
            playButton.RegisterCallback<MouseEnterEvent>(OnHover);
            
            Button settingsButton = _uiDocument.rootVisualElement.Q<Button>(UiConstants.SettingsButton);
            settingsButton.clicked += SettingsButtonOnClicked;
            settingsButton.RegisterCallback<MouseEnterEvent>(OnHover);
            
            Button quitButton = _uiDocument.rootVisualElement.Q<Button>(UiConstants.QuitButton);
            quitButton.clicked += QuitButtonOnClicked;
            quitButton.RegisterCallback<MouseEnterEvent>(OnHover);
        }

        private void SettingsButtonOnClicked()
        {
            PlayClickSound();
        }

        private void QuitButtonOnClicked()
        {
            Debug.Log("Quitting");
            PlayClickSound();
            Application.Quit();
        }

        private void PlayButtonOnClicked()
        {
            PlayClickSound();
            SceneManager.LoadScene(SceneConstants.DebugScene);
        }

        private void OnHover(MouseEnterEvent enterEvent)
        {
            _audioSource.PlayOneShot(hoverSound); 
        }

        private void PlayClickSound()
        {
            _audioSource.PlayOneShot(clickSound);
        }
    }
}