using System;
using System.Collections.Concurrent;
using System.Threading;

using AR.Drone.Infrastructure;
using AR.Drone.Client;
using AR.Drone.Avionics.Tools.Time;
using AR.Drone.Avionics.Objectives;

namespace AR.Drone.Avionics
{
    /// <summary>
    /// Autopilot is a class that works in a separate thread, handling chains
    /// of user commands queued up for droen control.
    /// </summary>
    /// 
    /// <example>
    ///     Typical use:
    ///     <code>
    ///         DroneClient droneClient = new DroneClient("192.168.1.3");
    ///         droneClient.Start();
    ///
    ///         Autopilot autopilot = new Autopilot(droneClient);
    ///         autopilot.BindToClient();
    ///         autopilot.Start();
    ///         
    ///         autopilot.EnqueueObjective(Objective.Create(50, new FlatTrim()));
    ///         autopilot.EnqueueObjective(Objective.Create(5000, new Takeoff()));
    ///         autopilot.EnqueueObjective(Objective.Create(5000, new Hover()));
    ///         autopilot.EnqueueObjective(Objective.Create(5000, new Land()));
    ///     </code>
    /// </example>
    public class Autopilot : WorkerBase
    {
        private Apparatus.Input _last_input;
        private bool _active = false;
        private bool _commencing_objective = false;

        // A queue of apparatus output (NavigationData) awaiting to be proccessed
        protected ConcurrentQueue<Apparatus.Output> ApparatusOutputQueue;
        // A queue of objectives waiting in line for their period of time to start contributing to drone control
        protected ConcurrentQueue<Objective> ObjectiveQueue;

        /// <summary>
        /// A callback event, occuring everytime a new objective is being started
        /// </summary>
        public Action<Objective> OnObjectiveStarted { get; set; }

        /// <summary>
        /// A callback event, occuring everytime a an objective had just expired
        /// </summary>
        public Action<Objective> OnObjectiveCompleted { get; set; }

        /// <summary>
        /// A callback event, occuring when an objective had just expired and there are no new objectives waiting in ObjectiveQueue
        /// </summary>
        public Action OnOutOfObjectives { get; set; }

        /// <summary>
        /// A callback event, occuring every time the Autopilot object sends a command to the DroneClient object
        /// </summary>
        public Action<Apparatus.Input> OnCommandSend { get; set; }

        /// <summary>
        /// DroneClient which the Autpilot object is bound to
        /// </summary>
        public readonly DroneClient DroneClient;

        /// <summary>
        ///     Default action of the autopilot, whenever it is out off objectives.
        ///     If not specified in a constructor, DefaultObjective will be 'Hover'
        ///     by default.
        /// </summary>
        /// 
        /// <remarks>
        ///     When the 'OnOutOfObjectives' is called, user may enqueue new
        ///     commands for the autopilot, stopping it (the autopilot) from
        ///     executing the 'DefaultObjective' objective.
        /// </remarks>
        public readonly Objective DefaultObjective;

        /// <summary>
        /// Returns the last input to the ArDrone done by the autopilot
        /// </summary>
        public Apparatus.Input LastInput {
            get { lock (this) { Apparatus.Input __value = _last_input; return __value; } }
            private set { lock (this) { _last_input = value; } }
        }

        /// <summary>
        /// Returns true if autopilot bound to the 'DroneClient.NavigationDataUpdated' event
        /// </summary>
        public bool BoundToClient { get; private set; }

        /// <summary>
        /// Returns true if autopilot is enabled
        /// </summary>
        public bool Active {
            get { lock (this) { return _active; } }
            set { lock (this) { _active = value; } }
        }

        private void _clear_output_queue()
        {
            Apparatus.Output __output;
            while (ApparatusOutputQueue.TryDequeue(out __output));
        }

        // Event method for 'DroneClient.NavigationPacketAcquired' action
        private void NavigationDataAcquired(AR.Drone.Data.Navigation.NavigationData aPacket)
        {
            if (Active)
            {
                Apparatus.Output __data = new Apparatus.Output();
                __data.Navigation = aPacket;
                __data.LastInput = LastInput;
                EnqueueOutput(__data);
            }
        }

        /// <summary>
        ///     Manages objective's list and provides the caller with current
        ///     objective. If objective is expired, the next one in list is
        ///     returned, if list is empty, then DefaultObjective is returned.
        /// </summary>
        /// 
        /// <returns>
        ///     Current objective, DefaultObjective if ObjectiveQueue is empty
        /// </returns>
        protected Objective GetCurrentObjective()
        {
            Objective __objective;
            bool __has_objective;
            while ((__has_objective = ObjectiveQueue.TryPeek(out __objective)) == true)
            {
                if (__objective.Empty || __objective.IsExpired || __objective.Obtained)
                {
                    ObjectiveQueue.TryDequeue(out __objective);
                    if (OnObjectiveCompleted != null) OnObjectiveCompleted.Invoke(__objective);
                    __objective = null;
                }
                else break;
            }

            if (!__has_objective) __objective = null;
            else _commencing_objective = true;

            if (__objective == null)
            {
                if (_commencing_objective)
                {
                    if (OnOutOfObjectives != null)
                    {
                        OnOutOfObjectives.Invoke();
                        _commencing_objective = ObjectiveQueue.TryPeek(out __objective); // Check whether user just added a new task
                    }
                    else _commencing_objective = false;
                }
                if (!_commencing_objective) return DefaultObjective;
            }
            //else _commencing_objective = true;

            if (!__objective.Started)
            {
                __objective.Start();
                if (OnObjectiveStarted != null) OnObjectiveStarted.Invoke(__objective);
            }

            return __objective;
        }

        /// <summary>
        /// Thread looping method resposible for Autopilot to queued new objectives.
        /// </summary>
        protected override void Loop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (!Active) { _clear_output_queue(); Thread.Sleep(10); }
                else
                {
                    Apparatus.Output __output;

                    if (ApparatusOutputQueue.TryDequeue(out __output))
                    {
                        Objective __current_intent = GetCurrentObjective();

                        Apparatus.Input __input = new Apparatus.Input();
                        __input.Reset();

                        __current_intent.Contribute(__output, ref __input);
                        __input.Send(DroneClient);

                        LastInput = __input;
                        if (OnCommandSend != null) OnCommandSend.Invoke(__input);
                    }
                    else Thread.Sleep(3);
                }
            }
        }

        /// <summary>
        /// Initializes the Autopilot object and associates it with provided DroneClient
        /// </summary>
        /// <param name="aDroneClient">DroneClient object which Autopilot will be controlling</param>
        /// <remarks>Using this constructor will force DeafultObjective to be 'Hover'</remarks>
        public Autopilot(DroneClient aDroneClient) : this(aDroneClient, new Hover(Expiration.Never)) { /* Do Nothing */ }

        /// <summary>
        /// Initializes the Autopilot object and associates it with provided DroneClient and sets DeafultObjective
        /// </summary>
        /// <param name="aDroneClient">DroneClient object which Autopilot will be controlling</param>
        /// <param name="aDefaultObjective">DefaultObjective to execute when ObjectiveQeueue is depleted</param>
        /// <remarks>Using this constructor will force DeafultObjective to be 'Hover'</remarks>
        public Autopilot(DroneClient aDroneClient, Objective aDefaultObjective)
        {
            DroneClient = aDroneClient;
            DefaultObjective = aDefaultObjective;

            ObjectiveQueue = new ConcurrentQueue<Objective>();
            ApparatusOutputQueue = new ConcurrentQueue<Apparatus.Output>();

            BoundToClient = false;
        }

        /// <summary>
        /// Enqueue drone's output data for autopilot handling based on current objective.
        /// </summary>
        /// <param name="aData">Output data to be enqueued</param>
        public void EnqueueOutput(Apparatus.Output aData)
        {
            ApparatusOutputQueue.Enqueue(aData);
        }

        /// <summary>
        /// Bind autopilot to 'DroneClient.NavigationPacketAcquired' action,
        /// allowing autopilot to respond to DroneClient output automaticaly.
        /// </summary>
        public void BindToClient()
        {
            if (!BoundToClient)
            {
                DroneClient.NavigationDataAcquired += this.NavigationDataAcquired;
                BoundToClient = true;
            }
        }

        /// <summary>
        /// Unbind autopilot from 'DroneClient.NavigationPacketAcquired' action,
        /// ceasing its automatic reponse to DroneClient output
        /// </summary>
        public void UnbindFromClient()
        {
            if (BoundToClient)
            {
                DroneClient.NavigationDataAcquired -= this.NavigationDataAcquired;
                BoundToClient = false;
            }
        }

        /// <summary>
        /// Add a new objective into autopilot's action queue
        /// </summary>
        /// <param name="aObjective">Objective to be added/queued</param>
        public void EnqueueObjective(Objective aObjective)
        {
            ObjectiveQueue.Enqueue(aObjective);
        }

        /// <summary>
        /// Clears current objective qeueue and adds the provided one into the empty queue
        /// </summary>
        /// <param name="aObjective">Objective to replace the others with</param>
        public void SetObjective(Objective aObjective)
        {
            ClearObjectives();
            ObjectiveQueue.Enqueue(aObjective);
        }

        /// <summary>
        /// Clear all enqueued objectives
        /// </summary>
        public void ClearObjectives()
        {
            bool __active = Active;
            Active = false;

            Objective __task;
            while (ObjectiveQueue.TryDequeue(out __task)) ;

            if (__active) Active = true;
        }

        /// <summary>Returns true, if autopilot has any tasks in queue</summary>
        public bool HasObjectives { get { return !ObjectiveQueue.IsEmpty; } }
        /// <summary>Enable autopilot</summary>
        public void Activate() { Active = true; }
        /// <summary>Disable autopilot</summary>
        public void Deactivate() { Active = false; }
    }
}
