using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RoboticsLibrary.Commands
{
    public class Command
    {

        private Thread RunThread;

        public Command()
        {
            this.RunThread = new Thread(new ThreadStart(this.Run));
        }
        
        public virtual void Initialize() { }

        public virtual void Terminate() { }

        public virtual void Run() { }

        public virtual bool IsFinished() { return false; }

        public void Start()
        {
            this.Initialize();
            this.RunThread.Start();
        }
        
        public void Stop()
        {
            this.Terminate();
            this.RunThread.Join(2); // To ms join override.
        }
        
    }
}
