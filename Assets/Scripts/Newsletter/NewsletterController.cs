using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Newsletter
{
    public class NewsletterController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _emailField;
        [SerializeField] private GameObject _form;
        [SerializeField] private GameObject _submissionSuccessful, _submissionFailed;
        [SerializeField] private Button _submit;

        private INewsletterService _service;

        private static bool HasSignedUp
        {
            get => PlayerPrefs.GetInt("HasSignedUpForNewsletter", 0) == 1;
            set => PlayerPrefs.SetInt("HasSignedUpForNewsletter", value ? 1 : 0);
        }

        private void Awake()
        {
            if (HasSignedUp)
            {
                Destroy(gameObject);
                return;
            }

            if (_service == null)
                _form.SetActive(false);

            _submissionSuccessful.SetActive(false);
            _submissionFailed.SetActive(false);

            _submit.onClick.AddListener(OnSubmit);
        }

        public void SetService(INewsletterService service)
        {
            _service = service;
            _form.SetActive(true);
        }

        private async void OnSubmit()
        {
            try
            {
                await _service.SubscribeAsync(_emailField.text);
                HasSignedUp = true;
                _submissionSuccessful.SetActive(true);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                _submissionFailed.SetActive(true);
            }

            _form.SetActive(false);
        }
    }
}