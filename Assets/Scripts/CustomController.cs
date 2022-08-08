using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using CustomEnums;

/// <summary>
/// This class will handle what tool is being selected, as well as how to respond when custom action is being called
/// </summary>
public class CustomController : MonoBehaviour
{
    // Keep state of the tool
    int enumCount = Tool.GetNames(typeof(Tool)).Length;
    [SerializeField] Tool tool;

    // Keep state of the menu
    [SerializeField] SelectScript menu;
    bool menuToggle = false;
    MenuChoice menuChoosing = MenuChoice.None;

    // Keep state to manipulate library locomotion system
    [SerializeField] Controller controller;

    // Keep reference of Controller Manager
    [SerializeField] CustomManager manager;

    // Keep reference of the Interactor
    XRRayInteractor interactor;

    // Keep state of tools
    bool locomotionActive;
    bool transformActive;
    bool examineActive;

    // Keep state of the Transformation tool
    // bool scaleToggle;  // No longer needed
    bool rotateToggle;
    float scaleSpeed = 1.0f;
    float translateSpeed = 1.0f;
    float rotateSpeed = 180.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Start by not having the menu open
        menu.gameObject.SetActive(false);
        // Innitiate menu
        menu.setCurrent(tool);
        // Set up tool
        ConfigureTool();
        ActivateTool();
        // Need to keep track of the interactor
        interactor = GetComponent<XRRayInteractor>();
    }

    // Set tool state and ask manager to configure tool
    void ConfigureTool() 
    { 
        if (tool == Tool.Locomotion) {
        } else if (tool == Tool.Tranformation) {
            // scaleToggle = false;
            rotateToggle = false;
        } else if (tool == Tool.Examine) {
        }
        manager.ConfigureToolFor(controller, tool);    
    }

    // Set tool active and ask manager to activate tool
    void ActivateTool()
    {
        if (tool == Tool.Locomotion) {
            locomotionActive = true;
        } else if (tool == Tool.Tranformation) {
            transformActive = true;
        } else if (tool == Tool.Examine) {
            examineActive = true;
        }
        manager.ActivateToolFor(controller);
    }

    // Set tool inactive and ask manager to disable tool
    void DisableTool()
    {
        if (tool == Tool.Locomotion) {
            locomotionActive = false;
        } else if (tool == Tool.Tranformation) {
            transformActive = false;
        } else if (tool == Tool.Examine) {
            examineActive = false;
        }
        manager.DisableToolFor(controller);
    }

    // To open/close menu
    public void OnOpenHandSelectionMenu() 
    {
        // If menu is already open
        if (menuToggle) {
            // Hide menu
            menu.gameObject.SetActive(false);
            ActivateTool();
        } else {
            // Show menu
            menu.gameObject.SetActive(true);
            DisableTool();
        }
        // Adjust state
        menuToggle = !menuToggle;
        menuChoosing = MenuChoice.None;
    }

    // To choose menu, implement later
    public void OnHandSelectionMenuSelect()
    {
        if (menuToggle && menuChoosing != MenuChoice.None) {
            tool = (Tool) menuChoosing;
            ConfigureTool();
            menu.setCurrent(tool);
        }
    }

    // No longer needed
    // public void OnScaleToggle()
    // {
    //     if (transformActive) {
    //         scaleToggle = !scaleToggle;
    //         // Debug.Log($"[EDV] Scale Toggle set to {scaleToggle}");
    //     }
    // }

    // Have not implemented
    public void OnRotationModifier(InputValue value)
    {
        if (transformActive) {
            float rawValue = value.Get<float>();
            // Enter condition
            if (rawValue > 0.6f)
            {
                rotateToggle = true;
            }

            if (rawValue < 0.3f)
            {
                rotateToggle = false;
            }
            // Debug.Log($"[EDV] Rotate Modifier set to {rotateToggle}");
        }
    }

    // Have not implemented
    public void OnThumbstick(InputValue value)
    {
        Vector2 rawValue = value.Get<Vector2>();
        if (menuToggle) {
            // Do things for menu
            if (rawValue.magnitude < 0.5f) {
                menuChoosing = MenuChoice.None;
            } else {
                float angle = Vector2.SignedAngle(Vector2.right, rawValue);
                if (30.0f <= angle && angle < 150.0f) {
                    menuChoosing = MenuChoice.Locomotion;
                } else if (150.0f <= angle || angle < -90.0f) {
                    menuChoosing = MenuChoice.Tranformation;
                } else if (-90.0f <= angle && angle < 30.0f) {
                    menuChoosing = MenuChoice.Examine;
                } else {
                    Debug.LogError("[EDV] Math is not right OnThumbstick");
                }
            }
            menu.setChoosing(menuChoosing);
        } 
        else if (transformActive && interactor.hasSelection) {
            // Grab current object being selected
            Transform interactableTransform = interactor.firstInteractableSelected.transform;
            if (rotateToggle) {
                // Do things for rotate X (Veritcal) Z (Horizontal)
                RotateAnchor(interactableTransform, Vector3.forward, rawValue.x);
                RotateAnchor(interactableTransform, Vector3.right, rawValue.y);
            } else {
                // Default, also called anchor, vertical push and pull, horizontal rotate Y
                // Debug.Log("[EDV] Thumbstick for achor");
                RotateAnchor(interactableTransform, Vector3.up, rawValue.x);
                TranslateAnchor(rawValue.y);
            }
        } else if (examineActive && interactor.hasSelection) {
            Transform interactableTransform = interactor.firstInteractableSelected.transform;
            ScaleAnchor(interactableTransform, rawValue.y);
        }
        // Debug.Log($"[EDV] Thumbstick register X : {rawValue.x}, Y : {rawValue.y}");
    }

    // Quality of life, just push/pull the object to 3 meter away
    public void OnResetPosition()
    {
        if ((transformActive || examineActive) && interactor.hasSelection) {
            var rayOrigin = interactor.rayOriginTransform;
            var anchor = interactor.attachTransform;

            // Set anchor to 3 meters away
            var originPosition = rayOrigin.position;
            var originForward = rayOrigin.forward;

            var resultingPosition = originPosition + Vector3.Normalize(originForward) * 3;      
            anchor.position = resultingPosition;
        }
    }

    // Function copied from XR Toolkit Library, and modified a bit
    void TranslateAnchor(float directionAmount)
    {
        if (Mathf.Approximately(directionAmount, 0f))
            return;

        var rayOrigin = interactor.rayOriginTransform;
        var anchor = interactor.attachTransform;
        
        var originPosition = rayOrigin.position;
        var originForward = rayOrigin.forward;

        var resultingPosition = anchor.position + originForward * (directionAmount * translateSpeed * Time.deltaTime);

        // Check the delta between the origin position and the calculated position.
        // Clamp so it doesn't go further back than the origin position.
        var posInAttachSpace = resultingPosition - originPosition;
        var dotResult = Vector3.Dot(posInAttachSpace, originForward);

        anchor.position = dotResult > 0f ? resultingPosition : originPosition;
    }

    // Function copied from XR Toolkit Library
    void RotateAnchor(Transform interactable, Vector3 direction, float directionAmount)
    {
        if (Mathf.Abs(directionAmount) <= 0.2f)
            return;

        var rotateAngle = directionAmount * (rotateSpeed * Time.deltaTime);

        interactable.Rotate(direction, rotateAngle, Space.World);
    }

    // Modified, since interactable don't track scale
    void ScaleAnchor(Transform interactable, float directionAmount)
    {
        if (Mathf.Approximately(directionAmount, 0f))
            return;

        var oldScale = interactable.localScale;
        var newScale = oldScale + Vector3.one * directionAmount * scaleSpeed * Time.deltaTime;
        var limitScale = (Vector3.one * 0.05f);
        interactable.localScale = limitScale.magnitude > newScale.magnitude ? limitScale : newScale;
    }

}
