using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class endSceneController : MonoBehaviour
{
    private string endtext;
    private Label label;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        endtext = StateNameController.endGameText;
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        label = rootVisualElement.Q<Label>("endtext0");
        label.text = $"{endtext}";
        button = rootVisualElement.Q<Button>("Quit");
        button.RegisterCallback<ClickEvent>(ev => GoBack());
    }


    private void GoBack()
    {
        SceneManager.LoadScene("MainScene");
    }
}
