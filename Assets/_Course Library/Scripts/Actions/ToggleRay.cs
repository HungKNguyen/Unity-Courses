using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Toggle between the direct and ray interactor if the direct interactor isn't touching any objects
/// Should be placed on a ray interactor
/// </summary>
[RequireComponent(typeof(FixedXRInteractorLineVisual))]
public class ToggleRay : MonoBehaviour
{
    [Tooltip("Switch even if an object is selected")]
    public bool forceToggle = false;

    [Tooltip("The direct interactor that's switched to")]
    public XRDirectInteractor directInteractor = null;

    public XRRayInteractor rayInteractor = null;

    private FixedXRInteractorLineVisual rayInteractorLineVisual = null;
    private bool isSwitched = false;

    private void Awake()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        rayInteractorLineVisual = GetComponent<FixedXRInteractorLineVisual>();
        SwitchInteractors(false);
    }

    public void ActivateRay()
    {
        if (!TouchingObject() || forceToggle)
            SwitchInteractors(true);
    }

    public void DeactivateRay()
    {
        if (isSwitched)
            SwitchInteractors(false);
    }

    private bool TouchingObject()
    {
        List<IXRInteractable> targets = new List<IXRInteractable>();
        directInteractor.GetValidTargets(targets);
        return (targets.Count > 0);
    }

    private void SwitchInteractors(bool value)
    {
        isSwitched = value;
        rayInteractor.enabled = value;
        rayInteractorLineVisual.enabled = value;
        directInteractor.enabled = !value;
    }
}
