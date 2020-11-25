using System;
using System.Collections.Generic;
using System.Text;

namespace JourneyPlanner
{
    public class DelayInfo
    {
        public DateTime generatedAt { get; set; }
        public string locationName { get; set; }
        public string crs { get; set; }
        public string filterLocationName { get; set; }
        public string filtercrs { get; set; }
        public bool delays { get; set; }
        public int totalTrainsDelayed { get; set; }
        public int totalDelayMinutes { get; set; }
        public int totalTrains { get; set; }
        public List<Service> delayedTrains { get; set; }
    }

    public class FastestInfo
    {
        public List<Departure> departures { get; set; }
        public DateTime generatedAt { get; set; }
        public string locationName { get; set; }
        public string crs { get; set; }
        public object filterLocationName { get; set; }
        public object filtercrs { get; set; }
        public int filterType { get; set; }
        public object nrccMessages { get; set; }
        public bool platformAvailable { get; set; }
        public bool areServicesAvailable { get; set; }
    }

    public class BoardInfo
    {
        public List<Service> trainServices { get; set; }
        public object busServices { get; set; }
        public object ferryServices { get; set; }
        public DateTime generatedAt { get; set; }
        public string locationName { get; set; }
        public string crs { get; set; }
        public object filterLocationName { get; set; }
        public object filtercrs { get; set; }
        public int filterType { get; set; }
        public object nrccMessages { get; set; }
        public bool platformAvailable { get; set; }
        public bool areServicesAvailable { get; set; }
    }

    public class Departure
    {
        public Service service { get; set; }
        public string crs { get; set; }
    }

    public class Service
    {
        public List<PreviousCallingPoint> previousCallingPoints { get; set; }
        public List<SubsequentCallingPoint> subsequentCallingPoints { get; set; }
        public List<Origin> origin { get; set; }
        public List<Destination> destination { get; set; }
        public object currentOrigins { get; set; }
        public object currentDestinations { get; set; }
        public string rsid { get; set; }
        public string sta { get; set; }
        public string eta { get; set; }
        public string std { get; set; }
        public string etd { get; set; }
        public string platform { get; set; }
        public string @operator { get; set; }
        public string operatorCode { get; set; }
        public bool isCircularRoute { get; set; }
        public bool isCancelled { get; set; }
        public bool filterLocationCancelled { get; set; }
        public int serviceType { get; set; }
        public int length { get; set; }
        public bool detachFront { get; set; }
        public bool isReverseFormation { get; set; }
        public object cancelReason { get; set; }
        public string delayReason { get; set; }
        public string serviceID { get; set; }
        public string serviceIdPercentEncoded { get; set; }
        public string serviceIdGuid { get; set; }
        public string serviceIdUrlSafe { get; set; }
        public object adhocAlerts { get; set; }
    }

    public class CallingPoint
    {
        public string locationName { get; set; }
        public string crs { get; set; }
        public string st { get; set; }
        public object et { get; set; }
        public string at { get; set; }
        public bool isCancelled { get; set; }
        public int length { get; set; }
        public bool detachFront { get; set; }
        public object adhocAlerts { get; set; }
    }

    public class PreviousCallingPoint
    {
        public List<CallingPoint> callingPoint { get; set; }
        public int serviceType { get; set; }
        public bool serviceChangeRequired { get; set; }
        public bool assocIsCancelled { get; set; }
    }

    public class CallingPoint2
    {
        public string locationName { get; set; }
        public string crs { get; set; }
        public string st { get; set; }
        public string et { get; set; }
        public object at { get; set; }
        public bool isCancelled { get; set; }
        public int length { get; set; }
        public bool detachFront { get; set; }
        public object adhocAlerts { get; set; }
    }

    public class SubsequentCallingPoint
    {
        public List<CallingPoint2> callingPoint { get; set; }
        public int serviceType { get; set; }
        public bool serviceChangeRequired { get; set; }
        public bool assocIsCancelled { get; set; }
    }

    public class Origin
    {
        public string locationName { get; set; }
        public string crs { get; set; }
        public object via { get; set; }
        public object futureChangeTo { get; set; }
        public bool assocIsCancelled { get; set; }
    }

    public class Destination
    {
        public string locationName { get; set; }
        public string crs { get; set; }
        public object via { get; set; }
        public object futureChangeTo { get; set; }
        public bool assocIsCancelled { get; set; }
    }
}
