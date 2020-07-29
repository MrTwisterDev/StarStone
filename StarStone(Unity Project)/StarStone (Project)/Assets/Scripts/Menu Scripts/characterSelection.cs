using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class characterSelection : MonoBehaviour
{
    public GameObject[] characterPrefabs;

    public TMP_Dropdown[] playerSelectDropDowns = new TMP_Dropdown[2];

    public Transform[] playerSelectShowCaseTransforms = new Transform[2];

    public GameObject[] shownPlayers = new GameObject[2];
    //public showCaseScript;
    void Start()
    {
        for (int i = 0; i < shownPlayers.Length; i++)
        {
            shownPlayers[i] = Instantiate(characterPrefabs[playerSelectDropDowns[i].value], playerSelectShowCaseTransforms[i].position, characterPrefabs[playerSelectDropDowns[i].value].transform.rotation,playerSelectShowCaseTransforms[i]);

            for (int a = 0; a < playerSelectDropDowns[i].options.Count; a++)
            {
                playerSelectDropDowns[i].options[a].text = characterPrefabs[a].name;
            }
        }


    }

    public void dropDownUpdate()
    {
        for (int i = 0; i < shownPlayers.Length; i++)
        {
            Destroy(shownPlayers[i]);
            shownPlayers[i] = Instantiate(characterPrefabs[playerSelectDropDowns[i].value], playerSelectShowCaseTransforms[i].position, characterPrefabs[playerSelectDropDowns[i].value].transform.rotation, playerSelectShowCaseTransforms[i]);

        }
    }


    private void Update()
    {

    }

}
