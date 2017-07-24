using Scarlet.Components;
using Science.Systems;

namespace Science
{
    class IOHandler
    {
        public readonly ISubsystem RailController;
        public readonly ISubsystem TurntableController;
        public readonly ISubsystem ToolheadController;
        public readonly ISubsystem DrillController;

        public readonly Microscope Microscope; // TODO: Evaluate whether this should also be a subsystem.

        public IOHandler()
        {
            this.RailController = new Rail();
            this.TurntableController = new Turntable();
            this.ToolheadController = new Toolhead();
            this.DrillController = new Drill();
            this.Microscope = new Microscope();
        }

        /// <summary>
        /// Prepares all systems for use by zeroing them. This takes a while.
        /// </summary>
        public void InitializeSystems()
        {
            this.RailController.Initialize();
            this.TurntableController.Initialize();
            this.ToolheadController.Initialize();
            this.DrillController.Initialize();
        }

        /// <summary>
        /// Immediately stops all systems.
        /// </summary>
        public void EmergencyStop()
        {
            this.RailController.EmergencyStop();
            this.TurntableController.EmergencyStop();
            this.ToolheadController.EmergencyStop();
            this.DrillController.EmergencyStop();
        }
    }
}
