﻿using System;
using UnityEngine;
using Contracts;
using KSP;
using KSPAchievements;

namespace MissionControllerEC
{
    #region Get Seat Count
    public class GetSeatCount : ContractParameter
    {
        private int seatCount = 0;
        private string title = "none";
        private bool updated = false;

        public GetSeatCount()
        {
        }

        public GetSeatCount(int crewnumber, string titledesc)
        {
            this.seatCount = crewnumber;
            this.title = titledesc;
        }
        protected override string GetHashString()
        {
            return "Amount crew " + seatCount;
        }
        protected override string GetTitle()
        {
            return title + " " + seatCount + " Plus Crew.";
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {

            if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
                CheckCrewValues(FlightGlobals.ActiveVessel);

        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref seatCount, 71000, seatCount, "crewcount");
            Tools.ContractLoadCheck(node, ref title, "Defualts Loaded Error", title, "title");
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("crewcount", seatCount);
            node.AddValue("title", title);
        }

        public void CheckCrewValues(Vessel vessel)
        {
            int currentseats = FlightGlobals.ActiveVessel.GetCrewCapacity();
            int crewedseats = FlightGlobals.ActiveVessel.GetCrewCount();

            int seatavailable = currentseats - crewedseats;

            if (seatavailable >= seatCount)
            {
                base.SetComplete();
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region Max Seat Count
    public class MaxSeatCount : ContractParameter
    {
        private int seatCount = 0;
        private string title = "";
        private bool updated = false;

        public MaxSeatCount()
        {
        }

        public MaxSeatCount(int MaxSeatCount)
        {
            this.seatCount = MaxSeatCount;
        }
        protected override string GetHashString()
        {
            return "Amount crew " + seatCount;
        }
        protected override string GetTitle()
        {
            return title + "Must Have This Many Seats: " + seatCount;
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {

            if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
                CheckMaxSeatCount(FlightGlobals.ActiveVessel);

        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref seatCount, 71000, seatCount, "crewcount");
            Tools.ContractLoadCheck(node, ref title, "Defualts Loaded Error", title, "title");
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("crewcount", seatCount);
            node.AddValue("title", title);
        }

        public void CheckMaxSeatCount(Vessel vessel)
        {
            int currentseats = FlightGlobals.ActiveVessel.GetCrewCapacity();

            if (currentseats >= seatCount)
            {
                base.SetComplete();
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region Get Crew Count
    public class GetCrewCount : ContractParameter
    {
        private int crewCount = 0;
        private bool updated = false;

        public GetCrewCount()
        {
        }

        public GetCrewCount(int crewnumber)
        {
            this.crewCount = crewnumber;
        }
        protected override string GetHashString()
        {
            if (crewCount > 0)
                return "Amount crew " + crewCount;
            else
                return "Vessel is automated: (nocrew)";
        }
        protected override string GetTitle()
        {
            if (crewCount > 0)
                return "Vessel Must Have This Amount Of crew " + crewCount;
            else
                return "Vessel is automated: (nocrew)";
        }

        protected override void OnRegister()
        {
            this.disableOnStateChange = false;
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (FlightGlobals.ActiveVessel && HighLogic.LoadedSceneIsFlight)
                CheckCrewValues(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {
            Tools.ContractLoadCheck(node, ref crewCount, 1, crewCount, "crewcount");
        }
        protected override void OnSave(ConfigNode node)
        {
            node.AddValue("crewcount", crewCount);
        }

        public void CheckCrewValues(Vessel vessel)
        {
            if (vessel.isActiveVessel)
            {
                int currentcrew = FlightGlobals.ActiveVessel.GetCrewCount();
                //Debug.LogError("Current crew is " + currentcrew + " crew can't be over " + crewCount);
                if (currentcrew >= crewCount && currentcrew != 0)
                {
                    base.SetComplete();
                    //Debug.Log("Passed Crew Check");
                }
                if (crewCount == 0 && currentcrew == 0 )
                {
                    base.SetComplete();
                }
            }
        }
        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region Civilian Module
    public class CivilianModule : ContractParameter
    {
        private int crewSpace = 0;
        private int civilianSpace = 0;
        private int FreeSpace = 0;
        private int UsedSpace = 0;

        private string name1 = "none";
        private string name2 = "none";
        private string name3 = "none";
        private string name4 = "none";

        public string vesselID;

        public string destination = "none";
        private bool updated = false;

        public CelestialBody targetBody;

        public CivilianModule()
        {
        }

        public CivilianModule(CelestialBody body, int civspace, string name1, string name2, string name3, string name4, string destination)
        {
            this.targetBody = body;
            this.civilianSpace = civspace;
            this.name1 = name1;
            this.name2 = name2;
            this.name3 = name3;
            this.name4 = name4;
            this.destination = destination;
        }
        public CivilianModule(CelestialBody body, int civspace, string name1, string name2, string name3, string destination)
        {
            this.targetBody = body;
            this.civilianSpace = civspace;
            this.name1 = name1;
            this.name2 = name2;
            this.name3 = name3;
            this.destination = destination;
        }
        public CivilianModule(CelestialBody body, int civspace, string name1, string name2, string destination)
        {
            this.targetBody = body;
            this.civilianSpace = civspace;
            this.name1 = name1;
            this.name2 = name2;
            this.destination = destination;
        }
        protected override string GetHashString()
        {

            return "Bring civilians on space tour";

        }
        protected override string GetTitle()
        {
            if (civilianSpace == 2 || civilianSpace == 1)
            {
                return "Have Room in vessel for following 2 Civilians for the " + destination + "\n\n" + "-" + name1 + "\n" + "-" + name2 + "\n";
            }
            if (civilianSpace == 3)
            {
                return "Have Room in vessel for following 3 Civilians for the " + destination + "\n\n" + "-" + name1 + "\n" + "-" + name2 + "\n" + "-" + name3 + "\n";
            }
            else
            {
                return "Have Room in vessel for following 4 Civilians for the " + destination + "\n\n" + "-" + name1 + "\n" + "-" + name2 + "\n" + "-" + name3 + "\n" + "-" + name4 + "\n";
            }
        }

        protected override void OnRegister()
        {
            updated = false;
            if (Root.ContractState == Contract.State.Active)
            {
                GameEvents.onFlightReady.Add(flightReady);
                GameEvents.onVesselChange.Add(vesselChange);
                updated = true;
            }
        }

        protected override void OnUnregister()
        {
            if (updated)
            {
                GameEvents.onFlightReady.Remove(flightReady);
                GameEvents.onVesselChange.Remove(vesselChange);
            }
        }

        protected override void OnUpdate()
        {
            if (Root.ContractState == Contract.State.Active)
            {
                if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel)
                {
                    if (this.state == ParameterState.Incomplete)
                    {
                        CivilianChecks(FlightGlobals.ActiveVessel);
                    }
                    if (this.state == ParameterState.Complete)
                    {
                        civilianReCheck(FlightGlobals.ActiveVessel);
                    }
                }
            }
        }

        protected override void OnLoad(ConfigNode node)
        {                                 
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
            Tools.ContractLoadCheck(node, ref crewSpace, 2, crewSpace, "crewspace");
            Tools.ContractLoadCheck(node, ref civilianSpace, 2, civilianSpace, "civspace");
            Tools.ContractLoadCheck(node, ref FreeSpace, 4, FreeSpace, "freespace");
            Tools.ContractLoadCheck(node, ref UsedSpace, 2, UsedSpace, "usedspace");
            Tools.ContractLoadCheck(node, ref destination, "Error Defaults Loaded", destination, "destination");
            Tools.ContractLoadCheck(node, ref name1, "George Error", name1, "name1");
            Tools.ContractLoadCheck(node, ref name2, "Sam Error", name2, "name2");
            Tools.ContractLoadCheck(node, ref name3, "Dave Error", name3, "name3");
            Tools.ContractLoadCheck(node, ref name4, "Fracking Error", name4, "name4");           
            Tools.ContractLoadCheck(node, ref vesselID, "Defualt Loaded", vesselID, "vesselid");           
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);

            node.AddValue("crewspace", crewSpace);
            node.AddValue("civspace", civilianSpace);
            node.AddValue("freespace", FreeSpace);
            node.AddValue("usedspace", UsedSpace);
            node.AddValue("destination", destination);
            node.AddValue("name1", name1);
            node.AddValue("name2", name2);
            node.AddValue("name3", name3);
            node.AddValue("name4", name4);
            node.AddValue("vesselid", vesselID);
        }

        public void CivilianChecks(Vessel vessel)
        {
            crewSpace = FlightGlobals.ActiveVessel.GetCrewCount();
            FreeSpace = FlightGlobals.ActiveVessel.GetCrewCapacity();

            UsedSpace = FreeSpace - crewSpace;

            if (UsedSpace >= civilianSpace)
            {
                base.SetComplete();
                vesselID = FlightGlobals.ActiveVessel.id.ToString();
            }
            else
            {
                if (FlightGlobals.ActiveVessel.id.ToString() == vesselID)
                {
                    ScreenMessages.PostScreenMessage("No space for Civilians on this vessel, Make some room!");
                }
            }
        }
        public void civilianReCheck(Vessel vessel)
        {
            crewSpace = FlightGlobals.ActiveVessel.GetCrewCount();
            FreeSpace = FlightGlobals.ActiveVessel.GetCrewCapacity();

            UsedSpace = FreeSpace - crewSpace;

            if (UsedSpace < civilianSpace)
            {
                base.SetIncomplete();
                if (FlightGlobals.ActiveVessel.id.ToString() == vesselID)
                {
                    ScreenMessages.PostScreenMessage("You have taken up the space that is required for your Civilian Passengers.  Please make room for them again!");
                }
            }
        }

        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    #region EVA Goal
    public class EvaGoal : ContractParameter
    {
        CelestialBody targetBody;

        public EvaGoal()
        {
        }

        public EvaGoal(CelestialBody target)
        {
            this.targetBody = target;
        }
        protected override string GetHashString()
        {
            return "EVA outside of your vessel";
        }
        protected override string GetTitle()
        {
            return "Exit your vessel and conduct an EVA";
        }

        protected override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
                isEVA(FlightGlobals.ActiveVessel);
        }

        protected override void OnLoad(ConfigNode node)
        {           
            Tools.ContractLoadCheck(node, ref targetBody, Planetarium.fetch.Home, targetBody, "targetBody");
        }
        protected override void OnSave(ConfigNode node)
        {
            int bodyID = targetBody.flightGlobalsIndex;
            node.AddValue("targetBody", bodyID);
        }

        public void isEVA(Vessel vessel)
        {
            if (FlightGlobals.ActiveVessel.isEVA)
                base.SetComplete();
        }

        public void flightReady()
        {
            base.SetIncomplete();
        }
        public void vesselChange(Vessel v)
        {
            base.SetIncomplete();
        }
    }
    #endregion
    
}
