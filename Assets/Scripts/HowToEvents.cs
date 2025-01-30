using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HowToEvents : MonoBehaviour
{
    private UIDocument _document;
    private Button _back;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();

        _back = _document.rootVisualElement.Q<Button>("back");

        if (_back != null)
        {
            _back.RegisterCallback<ClickEvent>(OnPlayGameClick);
        }
    }

    private void OnDisable()
    {
        if (_back != null)
        {
            _back.UnregisterCallback<ClickEvent>(OnPlayGameClick);
        }
    }

    private void OnPlayGameClick(ClickEvent evt)
    {
        Debug.Log("You Pressed the Back Button");
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length); // Wait for the sound to finish
        SceneManager.LoadScene("Main"); // Load the "Main" scene after the sound finishes
    }



}
