using AR.Drone.Avionics;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;
using AR.Drone.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneControl
{
    class AutopilotWrapper
    {
        private Autopilot _autopilot;
        private DroneClient _droneClient;

        public AutopilotWrapper(DroneClient droneclient)
        {
            _droneClient = droneclient;
            _autopilot = new Autopilot(_droneClient);
            _autopilot.BindToClient();
        }

        public void start()
        {
            _autopilot.Start();
            _autopilot.Active = true;
        }

        public void EnqueueTakeoff()
        {
            _autopilot.EnqueueObjective(new FlatTrim(500));
            _autopilot.EnqueueObjective(new Takeoff(2500));
            _autopilot.EnqueueObjective(
                Objective.Create(5000,
                    new VelocityX(0.0f),
                    new VelocityY(0.0f),
                    new Altitude(1.0f)
                )
            );
        }

        public void EnqueueTurnAround()
        {
            const float turn = (float)(Math.PI); //180 graden
            float heading = _droneClient.NavigationData.Yaw;
            _autopilot.EnqueueObjective(Objective.Create(6000, new Heading(heading + turn, 0.2f, false)));
        }

        public void EnqueueStrafeLeft()
        {
            
            _autopilot.EnqueueObjective(
                Objective.Create(8000,
                    new VelocityX(0.0f),
                    new VelocityY(-0.5f),
                    new Altitude(1.0f)
                )
            );
            _autopilot.EnqueueObjective(
                Objective.Create(1000,
                    new VelocityX(0.0f),
                    new VelocityY(0.0f),
                    new Altitude(1.0f)
                )
            );
        }

        public void EnqueueStrafeRight()
        {
            _autopilot.EnqueueObjective(
                Objective.Create(8000,
                    new VelocityX(0.0f),
                    new VelocityY(0.5f),
                    new Altitude(1.0f)
                )
            );
            _autopilot.EnqueueObjective(
                Objective.Create(5000,
                    new VelocityX(0.0f),
                    new VelocityY(0.0f),
                    new Altitude(1.0f)
                )
            );
        }

        public void EnqueueLand()
        {
            _autopilot.EnqueueObjective(new Land(2500));
        }

        public void stop()
        {
            _autopilot.Active = false;
            _autopilot.ClearObjectives();
        }

    }
}
