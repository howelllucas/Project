using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterCreation : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    private GameObject[] characterGameobject;
    public InputField inputname;
    private int characterIndex = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        characterGameobject = new GameObject[characterPrefabs.Length];
    }
    void Start()
    {
        for (int i = 0; i < characterPrefabs.Length; i++)
        {
            
            characterGameobject[i] = GameObject.Instantiate(characterPrefabs[i], this.transform.position, this.transform.rotation) as GameObject;
        }
        showCharacter();
    }

    //更新角色显示
    void showCharacter()
    {
        characterGameobject[characterIndex].SetActive(true);
        for (int i = 0; i < characterPrefabs.Length; i++)
        {
            if (i!= characterIndex)
            {
                characterGameobject[i].SetActive(false);
            }
        }
    }

    public void onNextButtonClick()
    {
        characterIndex++;
        if (characterIndex>=characterPrefabs.Length)
        {
            characterIndex = 0;
        }
        showCharacter();
    }
    public void onPrevButtonClick()
    {
        characterIndex--;
        if (characterIndex ==-1)
        {
            characterIndex = characterPrefabs.Length-1;
        }
        showCharacter();
    }

    public void onOKButton()
    {
        PlayerPrefs.SetInt("selectCharacter", characterIndex);
        PlayerPrefs.SetString("name", inputname.text);
    }
}
