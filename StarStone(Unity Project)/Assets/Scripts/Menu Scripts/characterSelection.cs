using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class characterSelection : MonoBehaviour
{
    public GameObject[] characterPrefabs; //The gameObjects to display within the showcase areas
    public GameObject[] weaponShowcasePreFabs; //The weapon gameObjects to display within the showcase areas

    public TMP_Dropdown[] playerSelectDropDowns = new TMP_Dropdown[2]; //The dropdowns which contain which player the user is playing as

    public Transform[] playerSelectShowCaseTransforms = new Transform[2]; //Where do the characterPrefabs[] spawn

    public Transform[] playerSelectWeaponShowcaseTransforms = new Transform[2];//Where do the weaponShowcasePreFabs[] spawn

    public GameObject[] shownPlayers = new GameObject[2]; //The instanced objects for deletion/changing
    public GameObject[] shownWeapons = new GameObject[2]; //The instanced weapon objects for deletion/changing

    private GameController gameController; //James' work

    public bool shouldRunSetup;

    //public showCaseScript;
    public void InitialSetup()
    {

        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        for (int i = 0; i < shownPlayers.Length; i++)
        {
            shownPlayers[i] = Instantiate(characterPrefabs[playerSelectDropDowns[i].value], playerSelectShowCaseTransforms[i].position, characterPrefabs[playerSelectDropDowns[i].value].transform.rotation,playerSelectShowCaseTransforms[i]);
            shownWeapons[i] = Instantiate(weaponShowcasePreFabs[playerSelectDropDowns[i].value], playerSelectWeaponShowcaseTransforms[i].position, weaponShowcasePreFabs[playerSelectDropDowns[i].value].transform.rotation, playerSelectWeaponShowcaseTransforms[i]);

            for (int a = 0; a < playerSelectDropDowns[i].options.Count; a++)
            {
                //Set the dropdown text to the prefab name for dynamics
                playerSelectDropDowns[i].options[a].text = characterPrefabs[a].name;
            }
        }
        gameController.UpdateChosenCharacters(); //James' work
    }

    public void dropDownUpdate()
    {
        for (int i = 0; i < shownPlayers.Length; i++)
        {
            //Update the players shown within the showcase areas
            Destroy(shownPlayers[i]);
            Destroy(shownWeapons[i]);
            shownPlayers[i] = Instantiate(characterPrefabs[playerSelectDropDowns[i].value], playerSelectShowCaseTransforms[i].position, characterPrefabs[playerSelectDropDowns[i].value].transform.rotation, playerSelectShowCaseTransforms[i]);
            shownWeapons[i] = Instantiate(weaponShowcasePreFabs[playerSelectDropDowns[i].value], playerSelectWeaponShowcaseTransforms[i].position, weaponShowcasePreFabs[playerSelectDropDowns[i].value].transform.rotation, playerSelectWeaponShowcaseTransforms[i]);

        }
        gameController.UpdateChosenCharacters(); //James' work
    }


    public void OnEnable()
    {
        if (shouldRunSetup)
        {
            InitialSetup();
        }
    }

    public void OnDisable()
    {
        for (int i = 0; i < shownPlayers.Length; i++)
        {
            Destroy(shownPlayers[i]);
            Destroy(shownWeapons[i]);
        }
        shouldRunSetup = true;
    }

}
