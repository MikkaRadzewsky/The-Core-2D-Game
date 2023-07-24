using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpeningSceneScript : MonoBehaviour
{
    public TextMeshProUGUI messageDisplay;
    public TextMeshProUGUI gameTextDisplay;
    public string[] sentences;
    public float typingSpeed;
    private int index;

    public GameObject continueButton;

    public AudioSource source;


    private void Start()
    {
        StartCoroutine(Typing());
    }


    private void FixedUpdate()
    {
        if (messageDisplay.text == sentences[index])
        {
            nextSentence();
        }

    }

    IEnumerator Typing()
    {
        if (index == 0)
        {
            foreach (char letter in sentences[index].ToCharArray())
            {
                messageDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        else if( index == 1)
        {
            foreach (char letter in sentences[index].ToCharArray())
            {
                gameTextDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            continueButton.SetActive(true);
            source.Play();
        }
    }

    public void nextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(Typing());
        }
    }
}
