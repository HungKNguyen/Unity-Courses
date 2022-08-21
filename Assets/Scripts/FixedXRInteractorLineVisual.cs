using UnityEngine.XR.Interaction.Toolkit;

public class FixedXRInteractorLineVisual : XRInteractorLineVisual
{
    new protected void Awake()
        {
            base.Awake();

            if (base.reticle != null) {
                base.reticle = Instantiate(base.reticle);
            }
        }

    protected void OnDestroy()
    {
        Destroy(base.reticle);
    }

    new protected void OnEnable()
    {
        base.OnEnable();
        base.reticle.SetActive(false);
    }

    new protected void OnDisable()
    {
        base.OnDisable();
        base.reticle.SetActive(false);
    }

}


