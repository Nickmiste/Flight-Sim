using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetState { DEFAULT, LOCKED, FRIENDLY }
public class TargetManager : MonoBehaviour
{
    public static TargetState targetState { get; private set; } = TargetState.DEFAULT;
    [SerializeField] private Texture2D reticleTextureDefault = null;
    [SerializeField] private Texture2D reticleTextureLocked = null;
    [SerializeField] private Texture2D reticleTextureFriendly = null;
    public static Dictionary<TargetState, Color> reticleColors = new Dictionary<TargetState, Color>() {
        {TargetState.DEFAULT, Color.cyan}, {TargetState.LOCKED, Color.red}, {TargetState.FRIENDLY, Color.green}
    };

    [Tooltip("The radius of the reticle.")]
    [SerializeField] private int reticleSize = 8;
    [SerializeField] private int aimAssistThreshold = 0;

    private const float VERTICAL_OFFSET = 1f;
    public const float MAX_TARGET_RANGE = 500;

    public static Vector3 target { get; private set; }

    public static GameObject targetedObject { get; private set; } = null;
    public static float targetDistance { get; private set; } = -1;
    public static string targetInfo { get; private set; } = null;

    private void FixedUpdate()
    {
        //Set target state
        if (CustomRaycast(out RaycastHit hit))
        {
            targetedObject = hit.collider.gameObject;
            targetDistance = hit.distance;

            if (targetedObject.GetComponent<IUnshootable>() != null)
                targetState = TargetState.FRIENDLY;
            else if (targetedObject.GetComponent<ILockable>() != null && targetDistance <= MAX_TARGET_RANGE)
                targetState = TargetState.LOCKED;
            else
                targetState = TargetState.DEFAULT;
        }
        else
        {
            targetState = TargetState.DEFAULT;
            targetedObject = null;
            targetDistance = -1;
        }

        //Update target
        if (targetState == TargetState.LOCKED)
			target = hit.point;
		else
			target = transform.position + (transform.up * VERTICAL_OFFSET) + (transform.forward * MAX_TARGET_RANGE);
        
        Debug.DrawLine(transform.position + (transform.up * VERTICAL_OFFSET), target, reticleColors[targetState]);
    }

    public static bool IsTargetingObject()
    {
        return targetedObject != null;
    }

    private bool CustomRaycast(out RaycastHit hit)
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + (transform.up * VERTICAL_OFFSET), aimAssistThreshold, transform.forward, PlayerUtil.GetViewDistance());
        for (int i = 0; i < hits.Length; i++)
        {
            hit = hits[i];
            targetInfo = hit.collider.gameObject.GetComponent<ITargetable>()?.GetTargetInfo();

            if (targetInfo != null)
                return true;
        }

        hit = new RaycastHit();
        targetInfo = null;
        return false;
    }

    private void OnGUI()
    {
        if (!Event.current.type.Equals(EventType.Repaint) || !Player.instance.weaponsEnabled)
            return;

        //Draw reticle
        Vector3 reticlePos = Player.instance.GetComponentInChildren<Camera>().WorldToScreenPoint(target);
        reticlePos.y = Screen.height - reticlePos.y;
        Rect reticleRect = Rect.MinMaxRect(reticlePos.x - reticleSize, reticlePos.y - reticleSize, reticlePos.x + reticleSize, reticlePos.y + reticleSize);

        Texture2D reticleTexture;
        if (targetState == TargetState.LOCKED)
            reticleTexture = reticleTextureLocked;
        else if (targetState == TargetState.FRIENDLY)
            reticleTexture = reticleTextureFriendly;
        else reticleTexture = reticleTextureDefault;

        GUI.DrawTexture(reticleRect, reticleTexture);

        //Draw target distance
        if (targetDistance != -1)
        {
            Rect distanceRect = Rect.MinMaxRect(reticlePos.x - 50, reticleRect.max.y, reticlePos.x + 50, reticleRect.max.y + 50);
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.UpperCenter;

            GUI.Label(distanceRect, Mathf.Floor(targetDistance) + "m", style);
        }
    }
}