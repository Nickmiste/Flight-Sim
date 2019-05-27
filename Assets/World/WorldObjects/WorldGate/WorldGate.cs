using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGate : MonoBehaviour, IInitializable, IUnshootable
{
    public const float ALIGN_DST = 50;


    public static Color basePortalColor = new Color();
    private MeshRenderer portalMeshRenderer;

    public const float MIN_PORTAL_SPEED = 0.2f;
    public const float MAX_PORTAL_SPEED = 1.0f;

    private float dangerLevel;

    public void Init(object[] data) 
    {
        this.dangerLevel = (float)data[0];
    }

    private void Start()
    {
        portalMeshRenderer = transform.Find("Portal").GetComponent<MeshRenderer>();
        if (basePortalColor.Equals(new Color()))
            basePortalColor = portalMeshRenderer.material.GetColor("Color_540006CE");
        portalMeshRenderer.material.SetColor("Color_540006CE", new Color(0, 0, 0, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() == null)
            return;

        TaskManager.ScheduleTask(new WorldJumpTask(gameObject, dangerLevel, portalMeshRenderer));
    }

    string ITargetable.GetTargetInfo()
    {
        return "Danger Level: " + dangerLevel;
    }

    private class WorldJumpTask : TaskManager.Task
    {
        public GameObject gate { get; private set; }
        public float dangerLevel { get; private set; }
        public MeshRenderer portalMeshRenderer { get; private set; }

        public WorldJumpTask(GameObject gate, float dangerLevel, MeshRenderer portalMeshRenderer)
        {
            this.gate = gate;
            this.dangerLevel = dangerLevel;
            this.portalMeshRenderer = portalMeshRenderer;
        }

        protected override void OnStart()
        {
            PlayerMovement.instance.SetControlsEnabled(false);
            Player.instance.weaponsEnabled = false;
            gate.GetComponent<Collider>().enabled = false;
        }

        protected override void OnFinish()
        {
            WorldManager.instance.LoadWorld(dangerLevel);
            PlayerMovement.instance.SetControlsEnabled(true);
            Player.instance.weaponsEnabled = true;
        }

        protected override TaskPhase[] GetPhases() => new TaskPhase[] { new AlignPhase(this, 250), new ChargePhase(this, 400), new WarpTransitionPhase(this, 20), new WarpPhase(this, 500) };

        private class AlignPhase : TaskPhase
        {
            private Vector3 startPos;
            private Quaternion startRot;
            private Vector3 targetPos;
            private Quaternion targetRot;

            public AlignPhase(WorldJumpTask task, int length) : base(task, length) { }

            protected override void OnStart()
            {
                startPos = PlayerMovement.instance.transform.position;
                startRot = PlayerMovement.instance.transform.rotation;
                targetPos = ((WorldJumpTask)task).gate.transform.position -
                            ((WorldJumpTask)task).gate.transform.forward * 200 -
                            ((WorldJumpTask)task).gate.transform.up * 5;
                targetRot = Quaternion.LookRotation(((WorldJumpTask)task).gate.transform.forward, ((WorldJumpTask)task).gate.transform.up);
            }

            protected override void OnFixedUpdate()
            {
                PlayerMovement.instance.transform.position = Vector3.Lerp(startPos, targetPos, GetPhaseProgress());
                PlayerMovement.instance.transform.rotation = Quaternion.Lerp(startRot, targetRot, GetPhaseProgress());
            }
        }

        private class ChargePhase : TaskPhase
        {
            public ChargePhase(WorldJumpTask task, int length) : base(task, length) { }

            protected override void OnStart()
            {
                ((WorldJumpTask)task).gate.GetComponent<MeshRenderer>().material.SetColor("Color_540006CE", new Color(0, 0, 0, 0));
            }

            protected override void OnFixedUpdate()
            {
                ((WorldJumpTask)task).portalMeshRenderer.material.SetColor("Color_540006CE", WorldGate.basePortalColor * GetPhaseProgress());
                ((WorldJumpTask)task).portalMeshRenderer.material.SetFloat("Vector1_672CC21B", Mathf.Lerp(WorldGate.MIN_PORTAL_SPEED, WorldGate.MAX_PORTAL_SPEED, GetPhaseProgress()));
            }
        }

        private class WarpTransitionPhase : TaskPhase
        {
            private Vector3 startPos;
            private Vector3 targetPos;

            public WarpTransitionPhase(WorldJumpTask task, int length) : base(task, length) { }

            protected override void OnStart()
            {
                startPos = PlayerMovement.instance.transform.position;
                targetPos = ((WorldJumpTask)task).gate.transform.position + ((WorldJumpTask)task).gate.transform.forward * 50;
                ((WorldJumpTask)task).gate.transform.Find("FTL").gameObject.SetActive(true);
            }

            protected override void OnFixedUpdate()
            {
                PlayerMovement.instance.transform.position = Vector3.Lerp(startPos, targetPos, GetPhaseProgress());
            }
        }

        private class WarpPhase : TaskPhase
        {
            public WarpPhase(WorldJumpTask task, int length) : base(task, length) { }

            protected override void OnStart()
            {

            }

            protected override void OnFixedUpdate()
            {

            }
        }
    }
}