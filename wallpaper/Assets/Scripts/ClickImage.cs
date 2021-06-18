using UnityEngine;

public class ClickImage : MonoBehaviour
{
    public GameObject sprite = null;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            sprite.GetComponent<Crasher>().Crash();
            sprite.SetActive(false);
        }
    }
}


