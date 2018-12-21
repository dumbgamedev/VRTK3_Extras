using UnityEngine;
using VRTK;

public class PlaymakerVRTKPointerEventProxy : MonoBehaviour
{
    [Header("Setup")]
    public VRTK_DestinationMarker pointer;

    [Header("Playmaker Events")]
    public string enterEventName = "enter";

    public string hoverEventName = "hover";

    public string exitEventName = "exit";

    public string setEventName = "click";

    [Header("Debug Settings")]
    public bool logEnterEvent = false;

    public bool logHoverEvent = false;
    public bool logExitEvent = false;
    public bool logSetEvent = false;

    [Header("Extra Options")]
    public bool disableClickEvents;

    private void OnEnable()
    {
        disableClickEvents = false;
        pointer = (pointer == null ? GetComponent<VRTK_DestinationMarker>() : pointer);

        if (pointer != null)
        {
            pointer.DestinationMarkerEnter += DestinationMarkerEnter;
            pointer.DestinationMarkerHover += DestinationMarkerHover;
            pointer.DestinationMarkerExit += DestinationMarkerExit;
            pointer.DestinationMarkerSet += DestinationMarkerSet;
        }
        else
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
        }
    }

    private void OnDisable()
    {
        if (pointer != null)
        {
            pointer.DestinationMarkerEnter -= DestinationMarkerEnter;
            pointer.DestinationMarkerHover -= DestinationMarkerHover;
            pointer.DestinationMarkerExit -= DestinationMarkerExit;
            pointer.DestinationMarkerSet -= DestinationMarkerSet;
        }
    }

    private void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
    {
        if (disableClickEvents) return;

        SendPlaymakerEvent(e.target, enterEventName);

        if (logEnterEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER ENTER", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }
    }

    private void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
    {
        if (disableClickEvents) return;

        SendPlaymakerEvent(e.target, hoverEventName);

        if (logHoverEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER HOVER", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }
    }

    private void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
    {
        if (disableClickEvents) return;

        SendPlaymakerEvent(e.target, exitEventName);

        if (logExitEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER EXIT", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }
    }

    private void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
    {
        if (disableClickEvents) return;

        SendPlaymakerEvent(e.target, setEventName);

        if (logSetEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER SET", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }
    }

    private void SendPlaymakerEvent(Transform target, string eventName)
    {
        var myFSMs = target.GetComponents<PlayMakerFSM>();

        if (myFSMs != null)
        {
            foreach (var fsm in myFSMs)
            {
                fsm.SendEvent(eventName);
            }
        }
    }

    private void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
    {
        string targetName = (target ? target.name : "<NO VALID TARGET>");
        string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
        VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
    }
}