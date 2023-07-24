using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dialogueEngine : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI nameDisplay;
    public SpriteRenderer spriteDisplay;
    public string[] sentences;
    public Character[] sprites;
    public float typingSpeed;
    private int index;
    //private SpriteRenderer renderer;

    public GameObject continueButton;
    public GameObject nextLevelButton;

    public AudioSource source;


    private void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(Typing());
        //renderer = spriteDisplay.GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        if(textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
    }

    IEnumerator Typing()
    {
        nameDisplay.text = sprites[index].characterName + ":";
        spriteDisplay.sprite = sprites[index].image;
        foreach (char letter in sentences[index].ToCharArray())
        {
            Debug.Log($"letter");
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void nextSentence()
    {
        source.Play();
        continueButton.SetActive(false);

        if(index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            nameDisplay.text = "";
            spriteDisplay.sprite = null;
            StartCoroutine(Typing());
        }
        else
        {
            textDisplay.text = "";
            nameDisplay.text = "";
            spriteDisplay.sprite = null;
            if (nextLevelButton != null)
            {
                nextLevelButton.SetActive(true);
            }
        }
    }
}
