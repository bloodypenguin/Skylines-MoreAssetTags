using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using MoreAssetTags.Redirection;

namespace MoreAssetTags.Detours
{
    [TargetType(typeof(SteamHelper))]
    public class SteamHelperDetour
    {

        [RedirectMethod]
        private static string ServiceToTag(ItemClass.Service service, PrefabAI ai)
        {
            switch (service)
            {
                case ItemClass.Service.Residential:
                    return "Residential";
                case ItemClass.Service.Commercial:
                    return "Commercial";
                case ItemClass.Service.Industrial:
                    return "Industrial";
                case ItemClass.Service.Office:
                    return "Office";
                case ItemClass.Service.Electricity:
                    return "Electricity";
                case ItemClass.Service.Water:
                    return "Water & Sewage";
                case ItemClass.Service.Garbage:
                    return "Garbage";
                case ItemClass.Service.HealthCare:
                    if (ai is HospitalAI)
                        return "Healthcare";
                    if (ai is CemeteryAI)
                        return "Deathcare";
                    return (string)null;
                //begin mod
                case ItemClass.Service.Beautification:
                    if (ai is DecorationBuildingAI || ai is DummyBuildingAI || ai is DecorationWallAI)
                        return "Decoration";
                    return null;
                //end mod
                case ItemClass.Service.PoliceDepartment:
                    return "Police Department";
                case ItemClass.Service.Education:
                    return "Education";
                case ItemClass.Service.Monument:
                    return "Unique Building";
                case ItemClass.Service.FireDepartment:
                case ItemClass.Service.Disaster:
                    return "Fire Department";
                case ItemClass.Service.PublicTransport:
                    return "Transport";
                case ItemClass.Service.PlayerIndustry:
                    return ai is UniqueFactoryAI ? "Unique Factory" : "Industry Area";
                case ItemClass.Service.PlayerEducation:
                    return "Campus Area";
                case ItemClass.Service.VarsitySports:
                    return "Varsity Sports";
                case ItemClass.Service.Fishing:
                    return "Fishing Industry";
                default:
                    return (string)null;
            }
        }

        [RedirectReverse]
        private static string SubServiceToTag(ItemClass.SubService service)
        {
            UnityEngine.Debug.LogError("SubServiceToTag");
            return null;
        }

        [RedirectMethod]
        public static string[] GetSteamTags(PrefabInfo info)
        {
            List<string> stringList = new List<string>();
            if ((UnityEngine.Object)info == (UnityEngine.Object)null)
                return stringList.ToArray();
            if (info is BuildingInfo)
            {
                PrefabAI ai = info.GetAI();
                if (ai is IntersectionAI)
                    stringList.Add("Intersection");
                else if (ai is ParkAI)
                {
                    stringList.Add("Park");
                }
                else
                {
                    stringList.Add("Building");
                    BuildingAI buildingAi = ai as BuildingAI;
                    if ((UnityEngine.Object)buildingAi != (UnityEngine.Object)null && buildingAi.IsWonder())
                        stringList.Add("Monument");
                    string tag1 = ServiceToTag(info.GetService(), ai);
                    if (tag1 != null)
                        stringList.Add(tag1);
                    string tag2 = SubServiceToTag(info.GetSubService());
                    if (tag2 != null)
                        stringList.Add(tag2);
                }
                //begin mod
                var building = (BuildingInfo)info;
                if (building.m_subBuildings != null && building.m_subBuildings.Length > 0)
                {
                    stringList.Add("Sub-buildings");
                }
                //end mod
            }
            else if (info is PropInfo)
            {
                stringList.Add("Prop");
                //begin mod
                var propInfo = (PropInfo) info;
                if (propInfo.m_isDecal)
                {
                    stringList.Add("Prop Decal");
                }
                if (propInfo.m_effects != null && propInfo.m_effects.Any(effect => effect.m_effect is LightEffect))
                {
                    stringList.Add("Prop Light Source");
                }
                //end mod
            }
            else if (info is TreeInfo)
                stringList.Add("Tree");
            else if (info is VehicleInfo)
            {
                stringList.Add("Vehicle");
                //begin mod
                PrefabAI ai = info.GetAI();
                if (ai is AircraftAI)
                {
                    stringList.Add("Vehicle Aircraft");
                    if (ai is PassengerPlaneAI)
                    {
                        stringList.Add("Vehicle Plane");
                        stringList.Add("Vehicle Passenger Plane");
                    } else if (ai is CargoPlaneAI)
                    {
                        stringList.Add("Vehicle Plane");
                        stringList.Add("Vehicle Cargo Plane");
                    }
                }
                else if (ai is BlimpAI)
                {
                    stringList.Add("Vehicle Aircraft");
                    stringList.Add("Vehicle Blimp");
                }
                else if (ai is HelicopterAI)
                {
                    stringList.Add("Vehicle Aircraft");
                    stringList.Add("Vehicle Helicopter");
                    if (ai is PoliceCopterAI)
                    {
                        stringList.Add("Vehicle Police Helicopter");
                    }
                    else if (ai is AmbulanceCopterAI)
                    {
                        stringList.Add("Vehicle Ambulance Helicopter");
                    }
                    else if (ai is FireCopterAI)
                    {
                        stringList.Add("Vehicle Fire Helicopter");
                    }
                    else if (ai is DisasterResponseCopterAI)
                    {
                        stringList.Add("Vehicle Rescue Helicopter");
                    }
                }
                else if (ai is BalloonAI)
                {
                    stringList.Add("Vehicle Aircraft");
                    stringList.Add("Vehicle Balloon");
                }
                else if (ai is PassengerHelicopterAI)
                {
                    stringList.Add("Vehicle Aircraft");
                    stringList.Add("Vehicle Helicopter");
                    stringList.Add("Vehicle Passenger Helicopter");
                }
                else if (ai is PrivatePlaneAI)
                {
                    stringList.Add("Vehicle Aircraft");
                    stringList.Add("Vehicle Plane");
                    stringList.Add("Vehicle Private Plane");
                }
                else if (ai is TrolleybusAI)
                {
                    stringList.Add("Vehicle Trolleybus");
                }
                else if (ai is TramAI)
                {
                    stringList.Add("Vehicle Tram");
                }
                else if (ai is BusAI)
                {
                    stringList.Add("Vehicle Bus");
                    if (info.GetService() == ItemClass.Service.Disaster)
                    {
                        stringList.Add("Vehicle Evacuation Bus");
                    }
                    if (info.GetSubService() == ItemClass.SubService.PublicTransportTours)
                    {
                        stringList.Add("Vehicle Sightseeing Bus");
                    }
                    else
                    {
                        switch (info.GetClassLevel())
                        {
                            case ItemClass.Level.Level1:
                                stringList.Add("Vehicle City Bus");
                                break;
                            case ItemClass.Level.Level2:
                                stringList.Add("Vehicle Biofuel Bus");
                                break;
                            case ItemClass.Level.Level3:
                                stringList.Add("Vehicle Intercity Bus");
                                break;
                        }
                    }
                }
                else if (ai is BicycleAI)
                {
                    stringList.Add("Vehicle Bicycle");
                }
                else if (ai is CableCarAI)
                {
                    stringList.Add("Vehicle Cable Car");
                }
                else if (ai is RocketAI)
                {
                    stringList.Add("Vehicle Rocket");
                }
                else if (ai is CarAI)
                {
                    stringList.Add("Vehicle Car");
                    if (ai is PassengerCarAI)
                    {
                        stringList.Add("Vehicle Passenger Car");
                    } 
                    else if (ai is PostVanAI)
                    {
                        stringList.Add("Vehicle Post Van");
                    }
                    else if (ai is CargoTruckAI)
                    {
                        stringList.Add("Vehicle Cargo Truck");
                        var cargoTruckAi = (CargoTruckAI)ai;
                        stringList.Add(cargoTruckAi.m_isHeavyVehicle
                            ? "Vehicle Heavy Cargo Truck"
                            : "Vehicle Light Cargo Truck");
                        if (info.GetSubService() == ItemClass.SubService.PublicTransportPost)
                        {
                            stringList.Add("Vehicle Post Truck");    
                        }
                    }
                    else if (ai is AmbulanceAI)
                    {
                        stringList.Add("Vehicle Ambulance");
                    }
                    else if (ai is PoliceCarAI)
                    {
                        if (info.GetClassLevel() == ItemClass.Level.Level4)
                        {
                            stringList.Add("Vehicle Police Van");  
                        }
                        else
                        {
                            stringList.Add("Vehicle Police Car");
                        }
                    }
                    else if (ai is FireTruckAI)
                    {
                        stringList.Add("Vehicle Fire Truck");
                    }
                    else if (ai is SnowTruckAI)
                    {
                        stringList.Add("Vehicle Snow Truck");
                    }
                    else if (ai is DisasterResponseVehicleAI)
                    {
                        stringList.Add("Vehicle Rescue Truck");
                    }
                    else if (ai is GarbageTruckAI)
                    {
                        stringList.Add("Vehicle Garbage Truck");
                        switch (info.GetClassLevel())
                        {
                            case ItemClass.Level.Level1:
                                stringList.Add("Vehicle Regular Garbage Truck");
                                break;
                            case ItemClass.Level.Level2:
                                stringList.Add("Vehicle Biofuel Garbage Truck");
                                break;
                            case ItemClass.Level.Level3:
                                stringList.Add("Vehicle Garbage Collection Truck");
                                break;
                            case ItemClass.Level.Level4:
                                stringList.Add("Vehicle Garbage Transfer Truck");
                                break;
                        }
                    }
                    else if (ai is HearseAI)
                    {
                        stringList.Add("Vehicle Hearse");
                    }
                    else if (ai is MaintenanceTruckAI)
                    {
                        stringList.Add("Vehicle Maintenance Truck");
                    }
                    else if (ai is WaterTruckAI)
                    {
                        stringList.Add("Vehicle Water Truck");
                    }
                    else if (ai is TaxiAI)
                    {
                        stringList.Add("Vehicle Taxi");
                    }
                    else if (ai is ParkMaintenanceVehicleAI)
                    {
                        stringList.Add("Vehicle Park Maintenance");
                    }
                }
                else if (ai is MetroTrainAI)
                {
                    stringList.Add("Vehicle Metro");
                }
                else if (ai is TrainAI)
                {
                    if (info.GetSubService() == ItemClass.SubService.PublicTransportMonorail)
                    {
                        if (ai is PassengerTrainAI)
                        {
                            stringList.Add("Vehicle Monorail");
                        }
                    }
                    else
                    {
                        stringList.Add("Vehicle Train");
                        if (ai is PassengerTrainAI)
                        {
                            stringList.Add("Vehicle Passenger Train");
                        }
                        else if (ai is CargoTrainAI)
                        {
                            stringList.Add("Vehicle Cargo Train");
                        }
                    }
                }
                else if (ai is ShipAI)
                {
                    stringList.Add("Vehicle Ship");
                    if (ai is PassengerShipAI)
                    {
                        stringList.Add("Vehicle Passenger Ship");
                    }
                    else if (ai is CargoShipAI)
                    {
                        if (info.GetClassLevel() == ItemClass.Level.Level5)
                        {
                            stringList.Add("Vehicle Barge");
                        }
                        else
                        {
                            stringList.Add("Vehicle Cargo Ship");   
                        }
                    }
                }
                else if (ai is FishingBoatAI)
                {
                    stringList.Add("Vehicle Ship");
                    stringList.Add("Vehicle Fishing Ship");
                }
                else if (ai is FerryAI)
                {
                    stringList.Add("Vehicle Ship");
                    stringList.Add("Vehicle Ferry");
                }
                else if (ai is MeteorAI)
                {
                    stringList.Add("Vehicle Disaster");
                    stringList.Add("Vehicle Meteor");
                }
                else if (ai is VortexAI)
                {
                    stringList.Add("Vehicle Disaster");
                    stringList.Add("Vehicle Vortex");
                }
                //end mod
            } else if (info is CitizenInfo)
            {
                stringList.Add("Citizen");
            }
            else if (info is NetInfo)
            {
                stringList.Add("Road");
                PrefabAI ai = info.GetAI();
                string tag1 = ServiceToTag(info.GetService(), ai);
                if (tag1 != null)
                    stringList.Add(tag1);
                string tag2 = SubServiceToTag(info.GetSubService());
                if (tag2 != null)
                    stringList.Add(tag2);
                switch (((NetInfo)info).m_netAI)
                {
                    case QuayAI _:
                        stringList.Add("Quay");
                        break;
                    case DecorationWallAI _:
                        stringList.Add("Decoration Wall");
                        break;
                    case FloodWallAI _:
                        stringList.Add("Flood Wall");
                        break;
                }
            }
            return stringList.ToArray();
        }
    }
}