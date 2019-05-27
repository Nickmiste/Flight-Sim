using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TaskManager : MonoBehaviour
{
    private static List<Task> activeTasks = new List<Task>();
    private static List<Task> deletionQueue = new List<Task>();
    private static List<Task> scheduled = new List<Task>();

    private void FixedUpdate()
    {
        foreach (Task task in activeTasks)
            task.FixedUpdate();
        foreach (Task task in deletionQueue)
            activeTasks.Remove(task);
        foreach (Task task in scheduled)
            activeTasks.Add(task);

        deletionQueue.Clear();
        scheduled.Clear();
    }

    public static void ScheduleTask(Task task, int delay = 0)
    {
        scheduled.Add(task);
        task.Start();
        foreach (Task dependant in task.dependants)
            ScheduleTask(dependant, delay);
    }

    public static void AddTaskToDeletionQueue(Task task)
    {
        deletionQueue.Add(task);
        task.Finish();
        foreach (Task dependant in task.dependants)
            AddTaskToDeletionQueue(dependant);
    }

    public static bool IsTaskTypeActive<T>()
    {
        foreach (Task task in activeTasks)
            if (task is T)
                return true;
        return false;
    }

    public abstract class Task
    {
        public List<Task> dependants { get; private set; } = new List<Task>();

        protected TaskPhase[] phases;
        private int currentPhaseIndex = 0;
        protected bool loop = false;

        protected int elapsed { get; private set; } = -1;
        protected int elapsedPhase { get; private set; } = -1;

        public Task() { }

        public void Start()
        {
            this.phases = GetPhases();
            OnStart();
            phases?[0].Start();
        }

        public void FixedUpdate()
        {
            elapsed++;
            elapsedPhase++;
            OnFixedUpdate();

            if (ShouldMoveToNextPhase())
            {
                phases[currentPhaseIndex].Finish();
                elapsedPhase = 0;
                if (++currentPhaseIndex == phases.Length)
                {
                    if (loop)
                        currentPhaseIndex = 0;
                    else
                    {
                        QueueFree();
                        return;
                    }
                }
                phases[currentPhaseIndex].Start();
            }
            phases[currentPhaseIndex].FixedUpdate();
        }

        public void Finish() => OnFinish();

        private bool ShouldMoveToNextPhase()
        {
            if (phases[currentPhaseIndex].forceNextPhase)
                return true;

            if (phases[currentPhaseIndex].length == -1)
                return false;
            else
                return elapsedPhase >= phases[currentPhaseIndex].length;
        }

        protected virtual void OnStart() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnFinish() { }

        protected virtual TaskPhase[] GetPhases() => new TaskPhase[] { new DefaultTaskPhase(this) };

        //public void Schedule(int delay = 0) => TaskManager.ScheduleTask(this, delay);
        protected void QueueFree() => TaskManager.AddTaskToDeletionQueue(this);

        private class DefaultTaskPhase : TaskPhase
        {
            public DefaultTaskPhase(Task task) : base(task, -1) { }
        }

        protected abstract class TaskPhase
        {
            protected Task task { get; private set; }
            public int length { get; private set; }
            public bool forceNextPhase { get; private set; }

            protected TaskPhase(Task task, int length)
            {
                this.task = task;
                this.length = length;
            }

            public void Start() => OnStart();
            public void FixedUpdate() => OnFixedUpdate();
            public void Finish() => OnFinish();

            protected virtual void OnStart() { }
            protected virtual void OnFixedUpdate() { }
            protected virtual void OnFinish() { }

            protected void ForceNextPhase() => this.forceNextPhase = true;
            protected int GetElapsedPhase() => task.elapsedPhase;
            protected float GetPhaseProgress() => (float)task.elapsedPhase / length;
        }
    }
}