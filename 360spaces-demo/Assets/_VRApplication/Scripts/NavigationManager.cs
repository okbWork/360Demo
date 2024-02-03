using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Navigation Manager chooses a starting node and sends the user there on start
/// The starting node must be selected manually so that we know what position the user should start at
/// </summary>
public class NavigationManager : MonoBehaviour
{
    public VRBranch startingNode;

    public void PositionUser()
    {
        startingNode.OnClick();
    }
}