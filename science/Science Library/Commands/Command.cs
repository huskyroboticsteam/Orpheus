using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RoboticsLibrary.Commands
{
    /// <summary>
    /// Command architecture used to control
    /// mechanisms on a robot.
    /// Override the Initialize, Terminate, 
    /// Run, and IsFinished methods in your
    /// command subclass; to use the command
    /// start it by calling YourCommand.Start()
    /// and stop it with YourCommand.Stop().
    /// </summary>
    public class Command
    {

        private Thread RunThread; // Run Thread
        
        /// <summary>
        /// Instantiates command.
        /// </summary>
        public Command()
        {
            this.RunThread = new Thread(new ThreadStart(this.RunProcess));
        }

        /// <summary>
        /// Override this method for command initialization
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// Override this method for command termination
        /// </summary>
        protected virtual void Terminate() { }

        /// <summary>
        /// Override this method for command loop
        /// </summary>
        protected virtual void Run() { }

        /// <summary>
        /// Override this method to determine when to stop.
        /// </summary>
        /// <returns>Whether or not to stop the command.</returns>
        protected virtual bool IsFinished() { return false; }

        /// <summary>
        /// Internal use only
        /// Threads the run as long as 
        /// IsFinished returns false,
        /// then stops the command.
        /// </summary>
        private void RunProcess()
        {
            while(!this.IsFinished())
            {
                this.Run();
            }
            this.Stop();
        }

        /// <summary>
        /// Starts the command.
        /// </summary>
        public void Start()
        {
            this.Initialize();
            this.RunThread.Start();
        }
        
        /// <summary>
        /// Stops the command.
        /// </summary>
        public void Stop()
        {
            this.Terminate();
            this.RunThread.Join(2); // To ms join override.
        }
        
    }
}
