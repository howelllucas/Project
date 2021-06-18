using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class playerDir : MonoBehaviour
{
    public GameObject pointEffect;

    bool isMoving = false;

    public Vector3 targetPoint;
    private characterMove charactermove;

    
    // Start is called before the first frame update
    void Start()
    {
        targetPoint = this.transform.position;
        charactermove = this.GetComponent<characterMove>();
    }
    public bool IsPointerOverUIObject(Canvas canvas, Vector2 screenPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition;
        GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();

        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            bool isCollider = Physics.Raycast(ray, out raycastHit);
            if (isCollider && raycastHit.collider.tag == "Ground" )
            {
                isMoving = true;
                showPointEffect(raycastHit.point);
                targetPoint = raycastHit.point;

                changeDir(targetPoint);
            }
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
        }
        if (isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            bool isCollider = Physics.Raycast(ray, out raycastHit);
            if (isCollider && raycastHit.collider.tag == "Ground")
            {
                targetPoint = raycastHit.point;
                
                changeDir(targetPoint);
            }
            else 
            {
                if (charactermove.isMoving)
                {
                    changeDir(targetPoint);
                }
            }
        }
    }
    private void showPointEffect(Vector3 vec)
    {
        vec = new Vector3(vec.x, vec.y+0.1f, vec.z);
        GameObject.Instantiate(pointEffect, vec, Quaternion.identity);
    }

    //改变角色方向
    private void changeDir(Vector3 vec)
    {
        vec = new Vector3(vec.x, this.transform.position.y, vec.z);
        this.transform.LookAt(vec);
    }
}
