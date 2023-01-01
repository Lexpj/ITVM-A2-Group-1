using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class endSceneController : MonoBehaviour
{
    private string endtext;
    [SerializeField] private TextMeshProUGUI wintext;

    // Start is called before the first frame update
    void Start()
    {
        endtext = StateNameController.endGameText;
        wintext.text = $"{endtext}";
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainScene 1");
    }
}
