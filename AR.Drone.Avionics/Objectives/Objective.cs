using System.Collections.Generic;
using AR.Drone.Avionics.Objectives.IntentObtainers;
using AR.Drone.Avionics.Tools.Time;

namespace AR.Drone.Avionics.Objectives
{
    /// <summary>
    /// A generic objective that has a list of intents and an set execution duration
    /// </summary>
    public class Objective : IEarlyObtainer
    {
        /// <summary>
        /// List of Intent Obtainers objects, that are to be considered at a givden period of time
        /// </summary>
        protected List<IntentObtainer> List;

        /// <summary>
        /// Determines whther the  objective hadstarted it life cycle
        /// </summary>
        internal bool Started { get; private set; }

        /// <summary>
        /// Start objective's life cycle
        /// </summary>
        internal void Start()
        {
            if (Started) return;
            
            Expiration.Elapse();
            Started = true;
        }

        /// <summary>
        /// Determines whether an objective can be obtained prior to its expiration by time (for example altitude reach)
        /// </summary>
        public bool CanBeObtained { get; private set; }

        /// <summary>
        /// True whether the set objective has been obtained, for example, altitude reached
        /// </summary>
        public bool Obtained { get; private set; }

        /// <summary>
        /// Expiration time, after which the Intent looses its actuality
        /// </summary>
        public Expiration Expiration;

        /// <summary>
        /// Create a new Objective providing its duration in milliseconds
        /// </summary>
        public Objective(long aDuration, bool aCanBeObtained = true) : this(new Expiration(aDuration), aCanBeObtained)
        {
            /* Do Nothing */
        }

        /// <summary>
        /// Create a new Objective providing its duration as an Expiration object
        /// </summary>
        public Objective(Expiration aExpiration, bool aCanBeObtained = true)
        {
            Expiration = aExpiration;
            List = new List<IntentObtainer>();

            CanBeObtained = aCanBeObtained;
            Obtained = false;
        }

        /// <summary>
        /// Add an IntentObtainer to the Objective
        /// </summary>
        /// <param name="aIntentObtainer">IntentObtainer to be added</param>
        public void Add(IntentObtainer aIntentObtainer)
        {
            List.Add(aIntentObtainer);
        }

        /// <summary>
        /// Returns true if Objective contains no IntentObtainers
        /// </summary>
        public bool Empty
        {
            get { return List.Count == 0; }
        }

        /// <summary>
        /// Returns number of IntentObtainers in the Objective
        /// </summary>
        public int Count
        {
            get { return List.Count; }
        }

        /// <summary>
        ///     Makes the Objective to run throug all of its IntentObtainers and
        ///     have them contribute their intents into the final ApparatusInput
        ///     that is to be sent to drone.
        /// </summary>
        /// 
        /// <param name="aApparatusOutput">
        ///     Information that came down from the drone and the last input
        ///     that has been sent to the drone from autopilot.
        /// </param>
        ///
        /// <param name="aApparatusInput">
        ///     A reference to an existing Apparatus.Input variable which will
        ///     be filled up with values by the intent obtainers involved with
        ///     the Objective.
        /// </param>
        /// 
        /// <remarks>
        ///     This is called automtically by the Autopilot, but it has been
        ///     made public to allow the user to analyze created objectives.
        /// </remarks>
        public void Contribute(Apparatus.Output aApparatusOutput, ref Apparatus.Input aApparatusInput)
        {
            bool canBeObtained = false;
            bool obtained = true;

            foreach (IntentObtainer item in List)
            {
                item.Contribute(aApparatusOutput, ref aApparatusInput);
                if (CanBeObtained && item.CanBeObtained)
                {
                    canBeObtained = true;
                    obtained &= item.Obtained;
                }
            }

            if (canBeObtained) Obtained = obtained;
        }

        /// <summary>
        /// Returns true if Objective has expired.
        /// </summary>
        public bool IsExpired
        {
            get { return Expiration.IsOverdue; }
        }

        /// <summary>
        /// A convinient way to create Objectives and fill them with IntentObtainers
        /// </summary>
        /// 
        /// <param name="aDuration">Objective duration time in milliseconds</param>
        /// <param name="aIntentObtainerList">A list of IntentObtainers to be added upon creation of the Objective</param>
        /// 
        /// <returns>A newly created Objective with set duration and added intent obteners privded in the parameters</returns>
        /// 
        /// <example>
        ///     Makes the drone to hover facing north and at altitude of 1.5 meters for one second:
        ///     <code>
        ///         auto_pilot.EnqueueObjective(
        ///             Objective.Create(1000,
        ///                 new VelocityX(0.0f),
        ///                 new VelocityY(0.0f),
        ///                 new Altitude(1.5f),
        ///                 new Heading(0.0f)
        ///             )
        ///         );
        ///     </code>
        /// </example>
        public static Objective Create(long aDuration, params IntentObtainer[] aIntentObtainerList)
        {
            return Create(new Expiration(aDuration), aIntentObtainerList);
        }

        /// <see>
        ///     <cref>Create(long aDuration, params IntentObtainer[] aIntentObtainerList)</cref>
        /// </see>
        public static Objective Create(Expiration aExpiration, params IntentObtainer[] aIntentObtainerList)
        {
            var obj = new Objective(aExpiration);
            foreach (IntentObtainer io in aIntentObtainerList) obj.Add(io);
            return obj;
        }
    }
}