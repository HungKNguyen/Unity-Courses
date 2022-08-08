using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimePresets;
using UnityEngine.XR.Interaction.Toolkit;
using CustomEnums;

public class CustomManager : MonoBehaviour
{
    // Array to control SnapTurnProvider Settings
    [SerializeField] Preset[] snapTurnProviderSettings;

    // Array to control Ray Interactor Settings
    [SerializeField] Preset[] rayInteractorSettings;

    // Array to control Line Visual Settings;
    [SerializeField] Preset[] lineVisualSettings;

    // Reference to Interaction Manager
    [SerializeField] XRInteractionManager xRInteractionManager;

    // Reference to LocomotionSystem
    [SerializeField] LocomotionSystem locomotionSystem;

    // Reference to SnapTurnProvider
    [SerializeField] ActionBasedSnapTurnProvider snapTurnProvider;

    // Reference to Left Controller Components
    [SerializeField] XRRayInteractor leftRayInteractor;
    [SerializeField] XRInteractorLineVisual leftLineVisual;
    [SerializeField] LineRenderer leftLineRenderer;
    [SerializeField] ActionBasedController leftXRController;

    // Reference to Right Controller Components
    [SerializeField] XRRayInteractor rightRayInteractor;
    [SerializeField] XRInteractorLineVisual rightLineVisual;
    [SerializeField] LineRenderer rightLineRenderer;
    [SerializeField] ActionBasedController rightXRController;

    // Keep track of the controller snap feature
    bool leftSnapSetting;
    bool leftSnapState;
    bool rightSnapSetting;
    bool rightSnapState;

    void EnableSnapTurnFor(Controller controller)
    {
        if (controller == Controller.Left) {
            leftSnapState = leftSnapSetting;
        } else {
            rightSnapState = rightSnapSetting;
        } 
        ConfigureSnapTurnProvider();
    }

    void DisableSnapTurnFor(Controller controller)
    {
        if (controller == Controller.Left) {
            leftSnapState = false;
        } else {
            rightSnapState = false;
        } 
        ConfigureSnapTurnProvider();
    }

    void ConfigureSnapTurnFor(Controller controller, Tool tool)
    {
        if (controller == Controller.Left) {
            leftSnapSetting = (tool == Tool.Locomotion) ? true : false;
        } else {
            rightSnapSetting = (tool == Tool.Locomotion) ? true: false;
        }
    }

    void ConfigureSnapTurnProvider()
    {
        // Change if allow hands
        if (leftSnapState && rightSnapState) {
            snapTurnProviderSettings[0].ApplyTo(snapTurnProvider);
        } else if (leftSnapState) {
            snapTurnProviderSettings[1].ApplyTo(snapTurnProvider);
        } else if (rightSnapState) {
            snapTurnProviderSettings[2].ApplyTo(snapTurnProvider);
        } else {
            snapTurnProviderSettings[3].ApplyTo(snapTurnProvider);
        }

        // Hook up locomotion system
        snapTurnProvider.system = locomotionSystem;
    }

    void DisableInteractorFor(Controller controller) {
        if (controller == Controller.Left) {
            leftRayInteractor.enabled = false;
            leftLineRenderer.enabled = false;
            leftLineVisual.enabled = false;
        } else {
            rightRayInteractor.enabled = false;
            rightLineRenderer.enabled = false;
            rightLineVisual.enabled = false;
        }
    }

    void EnableInteractorFor(Controller controller) {
        if (controller == Controller.Left) {
            leftRayInteractor.enabled = true;
            leftLineRenderer.enabled = true;
            leftLineVisual.enabled = true;
        } else {
            rightRayInteractor.enabled = true;
            rightLineRenderer.enabled = true;
            rightLineVisual.enabled = true;
        }
    }

    void ConfigureInteractorFor(Controller controller, Tool tool)
    {
        // Grab relevant component
        XRRayInteractor rayInteractor = (controller == Controller.Left) ? leftRayInteractor : rightRayInteractor;
        XRInteractorLineVisual lineVisual = (controller == Controller.Left) ? leftLineVisual : rightLineVisual;
        // Apply setting
        rayInteractorSettings[(int) tool].ApplyTo(rayInteractor);
        lineVisualSettings[(int) tool].ApplyTo(lineVisual);
        // Hooking up interaction manager
        rayInteractor.interactionManager = xRInteractionManager;
        // Hooking up the controller
        rayInteractor.xrController = (controller == Controller.Left) ? leftXRController : rightXRController;
        // Hooking up the attach transform
        rayInteractor.attachTransform = rayInteractor.transform.GetChild(2);
        // Hooking up the ray origin
        rayInteractor.rayOriginTransform = rayInteractor.transform.GetChild(3);
    }

    public void ActivateToolFor(Controller controller) {
        EnableSnapTurnFor(controller);
        EnableInteractorFor(controller);

    }

    public void DisableToolFor(Controller controller) {
        DisableSnapTurnFor(controller);
        DisableInteractorFor(controller);
    }

    public void ConfigureToolFor(Controller controller, Tool tool) {
        ConfigureSnapTurnFor(controller, tool);
        ConfigureInteractorFor(controller, tool);
    }
}
