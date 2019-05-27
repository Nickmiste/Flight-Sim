using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab = null;

    public int dataId = 0;

    private int cooldownLeft = 0;
    void FixedUpdate()
	{
        transform.localEulerAngles = new Vector3(0, 0, 0);
        transform.LookAt(TargetManager.target, PlayerMovement.instance.transform.up);
        if (cooldownLeft-- > 0)
            return;

		if (IsFireButtonDown() && Player.instance.weaponsEnabled && TargetManager.targetState != TargetState.FRIENDLY)
        {
            cooldownLeft = GetData().cooldown;
            Fire();
        }
	}

    private TurretData GetData()
    {
        return TurretManager.turretDataArray[dataId];
    }

    private static bool IsFireButtonDown()
    {
        if (PlayerMovement.instance.GetControlScheme() == PlayerMovement.ControlScheme.JOYSTICK)
            return Input.GetButton("Fire");
        if (PlayerMovement.instance.GetControlScheme() == PlayerMovement.ControlScheme.MOUSE_AND_KEYBOARD)
            return Input.GetButton("FireMouse");
        return false;
    }

	private void Fire()
	{
        GameObject beam = Instantiate(beamPrefab, this.transform.position, this.transform.rotation) as GameObject;
        beam.GetComponent<Beam>().data = GetData();

        GetComponent<AudioSource>().Play();
        GetComponentInChildren<Muzzle>().Fire();
    }
}