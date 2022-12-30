using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControls : MonoBehaviour
{
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject storyPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject videoPlayer;
    [SerializeField] GameObject audio;
    [SerializeField] GameObject bliksem;

    public void StoryButton()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        storyPanel.SetActive(true);
        videoPlayer.SetActive(true);
        audio.SetActive(false);
        bliksem.SetActive(false);
    }

    public void ControlButton()
    {
        controlsPanel.SetActive(true);
        creditsPanel.SetActive(false);
        storyPanel.SetActive(false);
        videoPlayer.SetActive(false);
        audio.SetActive(true);
        bliksem.SetActive(true);
    }

    public void creditsButton()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(true);
        storyPanel.SetActive(false);
        videoPlayer.SetActive(false);
        audio.SetActive(true);
        bliksem.SetActive(true);
    }

    public void backButton()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        storyPanel.SetActive(false);
        videoPlayer.SetActive(false);
        audio.SetActive(true);
        bliksem.SetActive(true);
    }
}
