using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RoboticsLibrary.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class Command
    {

        private Thread RunThread; // Run Thread
        
        /// <summary>
        /// 
        /// </summary>
        public Command()
        {
            this.RunThread = new Thread(new ThreadStart(this.Run));
        }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Terminate() { }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Run() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFinished() { return false; }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            this.Initialize();
            this.RunThread.Start();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            this.Terminate();
            this.RunThread.Join(2); // To ms join override.
        }
        
    }
}
