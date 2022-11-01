using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetLevelOnButtonPress : MonoBehaviour
{
    private Button _resetButton;

    private void Start() {
        _resetButton = GetComponent<Button>();
        if (_resetButton) {
            _resetButton.onClick.AddListener(() => StartCoroutine(nameof(ResetLevel)));
        }
    }

    private IEnumerator ResetLevel() {
        SceneManager.LoadSceneAsync(0);
        yield return null;
    }
}
