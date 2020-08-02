using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class characterSelection : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public GameObject[] weaponShowcasePreFabs;

    public TMP_Dropdown[] playerSelectDropDowns = new TMP_Dropdown[2];

    public Transform[] playerSelectShowCaseTransforms = new Transform[2];

    public Transform[] playerSelectWeaponShowcaseTransforms = new Transform[2];

    public GameObject[] shownPlayers = new GameObject[2];
    public GameObject[] shownWeapons = new GameObject[2];

    private GameController gameController; //James' work

    //public showCaseScript;
    void Start()
    {

        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        for (int i = 0; i < shownPlayers.Length; i++)
        {
            shownPlayers[i] = Instantiate(characterPrefabs[playerSelectDropDowns[i].value], playerSelectShowCaseTransforms[i].position, characterPrefabs[playerSelectDropDowns[i].value].transform.rotation,playerSelectShowCaseTransforms[i]);
            shownWeapons[i] = Instantiate(weaponShowcasePreFabs[playerSelectDropDowns[i].value], playerSelectWeaponShowcaseTransforms[i].position, weaponShowcasePreFabs[playerSelectDropDowns[i].value].transform.rotation, playerSelectWeaponShowcaseTransforms[i]);

            for (int a = 0; a < playerSelectDropDowns[i].options.Count; a++)
            {
                playerSelectDropDowns[i].options[a].text = characterPrefabs[a].name;
            }
        }
        gameController.UpdateChosenCharacters(); //James' work
    }

    public void dropDownUpdate()
    {
        for (int i = 0; i < shownPlayers.Length; i++)
        {
            Destroy(shownPlayers[i]);
            Destroy(shownWeapons[i]);
            shownPlayers[i] = Instantiate(characterPrefabs[playerSelectDropDowns[i].value], playerSelectShowCaseTransforms[i].position, characterPrefabs[playerSelectDropDowns[i].value].transform.rotation, playerSelectShowCaseTransforms[i]);
            shownWeapons[i] = Instantiate(weaponShowcasePreFabs[playerSelectDropDowns[i].value], playerSelectWeaponShowcaseTransforms[i].position, weaponShowcasePreFabs[playerSelectDropDowns[i].value].transform.rotation, playerSelectWeaponShowcaseTransforms[i]);

        }
        gameController.UpdateChosenCharacters(); //James' work
    }


    private void Update()
    {

    }

}
