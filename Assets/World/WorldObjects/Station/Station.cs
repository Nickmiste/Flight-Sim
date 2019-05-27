using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() == null)
            return;

        TaskManager.ScheduleTask(new DockTask(gameObject));
    }

    private class DockTask : TaskManager.Task
    {
        public GameObject station { get; private set; }

        public DockTask(GameObject station)
        {
            this.station = station;
        }

        protected override void OnStart()
        {
            PlayerMovement.instance.SetControlsEnabled(false);
        }

        protected override void OnFinish()
        {
            PlayerMovement.instance.SetControlsEnabled(true);
        }

        protected override TaskPhase[] GetPhases() => new TaskPhase[] { new AlignPhase(this, 200)};

        private class AlignPhase : TaskPhase
        {
            private Vector3 startPos;
            private Quaternion startRot;
            private Vector3 targetPos;
            private Quaternion targetRot;

            public AlignPhase(DockTask task, int length) : base(task, length) { }

            protected override void OnStart()
            {
                startPos = PlayerMovement.instance.transform.position;
                startRot = PlayerMovement.instance.transform.rotation;
                targetPos = ((DockTask)task).station.transform.position -
                            ((DockTask)task).station.transform.forward * 15;
                targetRot = Quaternion.LookRotation(((DockTask)task).station.transform.forward, ((DockTask)task).station.transform.up);
            }

            protected override void OnFixedUpdate()
            {
                PlayerMovement.instance.transform.position = Vector3.Lerp(startPos, targetPos, GetPhaseProgress());
                PlayerMovement.instance.transform.rotation = Quaternion.Lerp(startRot, targetRot, GetPhaseProgress());
            }
        }
    }
}
