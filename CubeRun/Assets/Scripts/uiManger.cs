using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiManger : MonoBehaviour
{
    private GameObject startUI;
    private GameObject shopUI;
    private GameObject gameUI;

    private player player;
    private MapManger map;

    Text scoreText;
    Text gem;

    void Start()
    {
        startUI = transform.Find("start_ui").gameObject;
        shopUI= transform.Find("shop_ui").gameObject;
        gameUI = transform.Find("game_ui").gameObject;
        shopUI.SetActive(false);
        gameUI.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        map = GameObject.Find("MapManger").GetComponent<MapManger>();
        gem = gameUI.transform.Find("gem").GetComponentInChildren<Text>();
        scoreText = gameUI.transform.Find("score").GetComponent<Text>();

        startUI.transform.Find("gem").GetComponentInChildren<Text>().text = PlayerPrefs.GetInt("gem", 0) + "/100";
        startUI.transform.Find("scoreText").transform.Find("score").GetComponentInChildren<Text>().text = PlayerPrefs.GetInt("score", 0) + "";

    }

    public void startButtonClick()
    {
        startUI.SetActive(false);
        gameUI.SetActive(true);
        player.move();
        player.isFollow=true;
        map.startCon();

    }
    private void Update()
    {
        if (gameUI)
        {
            scoreText.text = player.score+" ";
            gem.text = player.gemCount + " ";
        }
    }

}
