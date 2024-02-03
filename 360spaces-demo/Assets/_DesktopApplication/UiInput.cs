using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UiInput : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject pfNode;    //when user adds a new node prefab has image, and name

    private bool leftClickHold, rightClickHold = false;
    [SerializeField] private bool leftClick;
    [SerializeField] private bool rightClick;

    // Dragging UI Elements
    private bool isDragging;
    private GameObject draggingObj;

    [Header("Spawning line stuff")]
    public Transform nodes;
    public Transform lines;
    public GameObject newLine;
    public GameObject pfLine;
    void Update()
    {
        LeftMouseInput();
        RightMouseInput();

    }
    private void LeftMouseInput()
    {
        leftClick = Mouse.current.leftButton.isPressed;
        if (Mouse.current.leftButton.isPressed)
        {
            if (!leftClickHold)
            {
                if(Mouse.current.leftButton.wasPressedThisFrame)
                    Raycast();
            }
            leftClickHold = true;
        }
        else if (!Mouse.current.leftButton.isPressed && leftClickHold)
        {
            leftClickHold = false;
            if (Mouse.current.leftButton.wasReleasedThisFrame)
                Raycast();
        }
    }
    private void RightMouseInput()
    {
        rightClick = Mouse.current.rightButton.isPressed;
        if (Mouse.current.rightButton.isPressed)
        {
            if (!rightClickHold)
            {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    DragUI();
                }
            }
            rightClickHold = true;
        }
        else if (!Mouse.current.rightButton.isPressed && rightClickHold)
        {
            rightClickHold = false;
            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                isDragging = false;
                draggingObj = null;
            }
        }

        if(rightClick && draggingObj != null)
        {
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(draggingObj.transform.parent.GetComponent<RectTransform>(), Mouse.current.position.ReadValue(), Camera.main, out canvasPosition);

            // Update the UI element's position
            draggingObj.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
        }
    }
    private void DragUI()
    {
        if (!isDragging)
        {
            isDragging = true;

            Vector2 mousePosition = Mouse.current.position.ReadValue();

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (RaycastResult result in results)
            {
                // Check if the game object has a name that matches "Node"
                if (result.gameObject.name == "Node")
                {
                    draggingObj = result.gameObject;
                    break;
                }
            }
        }
    }
    private void Raycast()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool nodeHit = false;
        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                // Check if the game object has a name that matches "Node"
                if (result.gameObject.name == "Node")
                {
                    nodeHit = true;
                    // connect node to line
                    if (leftClick)
                    {
                        AddBranch(result.gameObject.transform);
                    }
                    else if (!leftClick)
                    {
                        EndBranch(result.gameObject.transform);
                    }
                }
            }
        }
        if(results.Count == 0 || nodeHit == false)
        {
            //in the event that a line was created but no valid node was selected we remove the line
            if (newLine != null)
                Destroy(newLine);
        }
        else
        {
            if(newLine!=null)
                newLine.transform.parent = lines;
        }
    }
    public void AddBranch(Transform startingTransform)
    {
        //instantiate line line
        newLine = Instantiate(pfLine, transform.position, Quaternion.identity);
        newLine.name = "line";
        newLine.transform.SetParent(lines);
        //set line points between transform and mouse position
        LineRenderer newLineController = newLine.GetComponent<LineRenderer>();
        newLineController.points[0] = startingTransform;
    }
    public void EndBranch(Transform endTransform)
    {
        newLine.GetComponent<LineRenderer>().points[1] = endTransform;

        //add connection ids to each other
        LineRenderer controller = newLine.GetComponent<LineRenderer>();
        controller.points[0].GetComponent<UINode>().ids.Add(controller.points[1].GetComponent<UINode>().id);

        newLine = null;
    }
    public void AddNode()
    {
        //change cursor to mini node to show that user is placing a new node
        GameObject newNode = Instantiate(pfNode, canvas.transform, false);
        //add node at next click at mouse position on canvas
        newNode.transform.localPosition = Vector3.zero;
        newNode.name = "Node";
        newNode.GetComponent<UINode>().id = nodes.childCount + 1;
        newNode.transform.SetParent(nodes);
    }
    public GameObject ReturnNode()
    {
        //same as addNode but returns the gameobject
        //change cursor to mini node to show that user is placing a new node
        GameObject newNode = Instantiate(pfNode, canvas.transform, false);
        //add node at next click at mouse position on canvas
        newNode.transform.localPosition = Vector3.zero;
        newNode.name = "Node";
        newNode.GetComponent<UINode>().id = nodes.childCount;
        newNode.transform.SetParent(nodes);

        return newNode;
    }
}