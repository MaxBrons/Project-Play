using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Comp_StartScreen : MonoBehaviour
{
    private Button m_Button;

    private void Start() {
        m_Button = GetComponent<Button>();

        if (m_Button)
            m_Button.onClick.AddListener(() => StartCoroutine(OnButtonClicked()));
    }

    public IEnumerator OnButtonClicked() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }
}
