using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Comp_ResetLevelButton : MonoBehaviour
{
    private Button m_Button;
    private void Start() {
        m_Button = GetComponent<Button>();

        if (m_Button)
            m_Button.onClick.AddListener(() => StartCoroutine(ResetLevel()));
    }
    private IEnumerator ResetLevel() {
        SceneManager.LoadSceneAsync(0);
        yield return null;
    }
}
