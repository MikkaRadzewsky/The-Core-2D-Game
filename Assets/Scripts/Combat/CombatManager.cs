using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

public class CombatManager : MonoBehaviour
{
    private CharacterEngine characterEngine;
    public CharacterInstance[] allies;
    public CharacterInstance[] enemies;
    public bool playerTurn;
    public AttackType attackType;
    private CharacterInstance[] allCharacters;
    public Queue<CharacterTurn> turnQueue;
    private CharacterInstance activeCharacter;
    private float maxSpeed;
    private float minSpeed;

    public Button basicAttackButton;
    public TextMeshProUGUI basicAttackDiscriptionText;
    public GameObject basicAttackDiscriptionBox;
    public Button specialAttackButton;
    public GameObject specialAttackDiscriptionBox;
    public TextMeshProUGUI specialAttackDiscriptionText;

    public GameObject victoryScreen;
    public GameObject lossScreen;

    public GameObject turnIndicator;
    public GameObject targetIndicator;


    public void Start()
    {
        Time.timeScale = 1;
        targetIndicator.SetActive(false);
        characterEngine = GetComponent<CharacterEngine>();
        allCharacters = allies.Concat(enemies).ToArray();
        foreach(CharacterInstance character in allCharacters)
        {
            //Debug.Log($"Current Stats of {character.character.characterName}: {character.currentStats}");
        }
        maxSpeed = allCharacters.Max(x => x.currentStats.speed);
        minSpeed = allCharacters.Min(x => x.currentStats.speed);

        turnQueue = new Queue<CharacterTurn>(allCharacters.Select(x => new CharacterTurn()
        {
            characterInstance = x,
            initiative = (float)maxSpeed / x.currentStats.speed
        }));

        activeCharacter = GetNextTurn();
        ChangeButtons();
        HighlightActiveCharacter();
    }

    private void Update()
    {
        if (enemies.Count() <= 0 || allies.Count()<=0)
        {
            StartCoroutine(WinOrLoseCoroutine());
        }
    }

    private IEnumerator WinOrLoseCoroutine()
    {
        yield return new WaitForSeconds(2);
        turnIndicator.SetActive(false);
        targetIndicator.SetActive(false);
        basicAttackButton.gameObject.SetActive(false);
        specialAttackButton.gameObject.SetActive(false);
        PauseGame();
        if (allies.Count() <= 0)
        {
            lossScreen.SetActive(true);
        }
        else
        {
            victoryScreen.SetActive(true);
        }
    }

    public void OnBasicAttack()
    {
        attackType = AttackType.Basic;
        StartCoroutine(AttackCoroutine(attackType));
    }
    

    public void OnSpecialAttack()
    {
        attackType = AttackType.Special;
        StartCoroutine(AttackCoroutine(attackType));
    }



    private IEnumerator AttackCoroutine(AttackType attackType)
    {
        basicAttackButton.interactable = false;
        specialAttackButton.interactable = false;

        var attacks = activeCharacter.character.attacks;
        var currentAttack = Array.Find(attacks, element => element.type == attackType);

        switch (attackType)
        {
            case AttackType.Basic:
                activeCharacter.animator.SetTrigger("BasicAttack");
                break;
            case AttackType.Special:
                activeCharacter.animator.SetTrigger("SpecialAttack");
                activeCharacter.cooldownTurnsLeft = currentAttack.coolDown+1;
                break;
        }
  
        var target = GetTarget(currentAttack);
        HighlightTarget(target);
        yield return new WaitForSeconds(1);
        Debug.Log($"{activeCharacter.character.characterName} is attacking : {target}");
        characterEngine.startAttack(currentAttack, activeCharacter, target);
        yield return new WaitForSeconds(2);
        RemoveDeadCharacters();
        turnQueue = new Queue<CharacterTurn>(turnQueue.Where(x => x != null && x.characterInstance != null));
        yield return new WaitForSeconds(2);
        TurnCycle();
    }


    private void HighlightActiveCharacter()
    {
        if (activeCharacter != null)
        {
            var activeHighlightBar = activeCharacter.transform.GetChild(1).gameObject;
            var highlightPosition = activeHighlightBar.transform.position;
            turnIndicator.transform.position = highlightPosition;
            Debug.Log($"{activeCharacter.character.characterName}'s Turn");
        }
    }

    private void HighlightTarget(CharacterInstance target)
    {
        var enemyPlacemnet = target.transform.GetChild(1).gameObject;
        var highlightPosition = enemyPlacemnet.transform.position;
        targetIndicator.transform.position = highlightPosition;
        targetIndicator.SetActive(true);
    }

    private void TurnCycle()
    {
        targetIndicator.SetActive(false);
        Debug.Log("turn is cycling");
        activeCharacter = GetNextTurn();
        HighlightActiveCharacter();
        ChangeButtons();
        if (!playerTurn)
        {
            PlayEnemyTurn();
        }
    }

    private void ChangeButtons()
    {
        if (playerTurn)
        {
            basicAttackButton.gameObject.SetActive(true);
            basicAttackButton.interactable = true;
            basicAttackDiscriptionText.text = activeCharacter.character.attacks[0].description;
            specialAttackButton.gameObject.SetActive(true);
            specialAttackDiscriptionText.text = activeCharacter.character.attacks[1].description;
            CoolDowns();

            var attacks = activeCharacter.character.attacks;
            var basicAttack = Array.Find(attacks, element => element.type == AttackType.Basic);
            var specialAttack = Array.Find(attacks, element => element.type == AttackType.Special);
            basicAttackButton.image.sprite = basicAttack.sprite;
            specialAttackButton.image.sprite = specialAttack.sprite;
        }
        else
        {
            basicAttackButton.gameObject.SetActive(false);
            specialAttackButton.gameObject.SetActive(false);
        }
    }

    private void CoolDowns()
    {
        var buttonText = specialAttackButton.gameObject.GetComponentInChildren<TMP_Text>(true);
        activeCharacter.cooldownTurnsLeft--;

        if (activeCharacter.cooldownTurnsLeft <= 0)
        {
            buttonText.text = "";
            specialAttackButton.interactable = true;

        }

        else if(activeCharacter.cooldownTurnsLeft > 0)
        {
            specialAttackButton.interactable = false;
            buttonText.text = activeCharacter.cooldownTurnsLeft.ToString();
        }
    }


    private void PlayEnemyTurn()
    {
        attackType = AttackType.Basic;
        StartCoroutine(AttackCoroutine(attackType));
    }


    private CharacterInstance GetNextTurn()
    {
        turnQueue = new Queue<CharacterTurn>(turnQueue.Where(x => x != null && x.characterInstance != null));
        turnQueue = new Queue<CharacterTurn>(turnQueue.OrderBy(x => x.initiative));
        while (turnQueue.Peek().initiative > 0)
        {
            foreach (var turn in turnQueue)
            {
                turn.initiative -= 0.1;
            }
        }

        var nextTurn = turnQueue.Dequeue();

        turnQueue.Enqueue(new CharacterTurn()
        {
            characterInstance = nextTurn.characterInstance,
            initiative = (float)maxSpeed / nextTurn.characterInstance.currentStats.speed
        });

        //activeCharacter = turnQueue.Peek().Character;
        activeCharacter = nextTurn.characterInstance;
        if (activeCharacter.character.aleigence == Aleigence.Ally)
        {
            playerTurn = true;
        }
        else
        {
            playerTurn = false;
        }
        return activeCharacter;
    }


    private void RemoveDeadCharacters()
    {
        // Remove destroyed CharacterTurn objects from the queue
        foreach (CharacterTurn turn in turnQueue)
        {
            if (turn.characterInstance.currentStats.health <= 0)
            {
                if(turn!=null && turn.characterInstance != null)
                {
                    turn.characterInstance.isDead = true;
                    turn.characterInstance.animator.SetTrigger("Death");
                }
            }
        }

        foreach (CharacterTurn turn in turnQueue)
        {
            if (turn != null && turn.characterInstance != null)
            {
                if (turn.characterInstance.isDead)
                {
                    Debug.Log($"{turn.characterInstance.character.characterName} is beeing destroyed!");
                    //turnQueue.Dequeue();
                    StartCoroutine(DestroyCoroutine(turn));
                }
            }
        }

        foreach (CharacterInstance ally in allies)
        {
            if (ally.currentStats.health <= 0 || ally.isDead)
            {
                Debug.Log($"{ally.character.characterName} is dead - ally");
                ally.isDead = true;
                allies = allies.Where(a => a != ally).ToArray();
            }
        }

        foreach (CharacterInstance enemy in enemies)
        {
            if (enemy.currentStats.health <= 0 || enemy.isDead)
            {
                Debug.Log($"{enemy.character.characterName} is dead - enemy");
                enemy.isDead = true;
                enemies = enemies.Where(e => e != enemy).ToArray();
            }
        }
    }


   private IEnumerator DestroyCoroutine(CharacterTurn turn)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(turn.characterInstance.gameObject);
    }




    private CharacterInstance GetTarget(Attack attack)
    {
        //var numberOfTargets = attack.numberOfTargets;
        var options = allies;
        switch (attack.targetsFoe)
        {
            case true:
                if (playerTurn)
                {
                    options = enemies;
                }
                else
                {
                    options = allies;
                }
                break;
            default:
                if (playerTurn)
                {
                    options = allies;
                }
                else
                {
                    options = enemies;
                }
                break;
        }

        //add for each when attacks can have mor than 1 intent...
        if (attack.intent[0] == Intent.Heal)
        {
            CharacterInstance optionWithlowestHealth = options[0];
            foreach (CharacterInstance character  in options)
            {
                if (character.currentStats.health < optionWithlowestHealth.currentStats.health)
                    optionWithlowestHealth = character;
            }
            return optionWithlowestHealth;
        }

        var rnd = new System.Random();
        return options[rnd.Next(0, options.Count())];
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
    }


    public void OnBasicAttackButtonHover()
    {
        basicAttackDiscriptionBox.SetActive(true);
    }

    public void OnSpecialAttackButtonHover()
    {
        specialAttackDiscriptionBox.SetActive(true);
    }

    public void OnMouseOff()
    {
        basicAttackDiscriptionBox.SetActive(false);
        specialAttackDiscriptionBox.SetActive(false);
    }
}




public class CharacterTurn
{
    public CharacterInstance characterInstance;
    public double initiative;
}
