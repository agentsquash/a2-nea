using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainDisruptionHandler
{
	class Journey
	{
		private int startID;
		private int endID;
		private DateTime journeyDateTime;
		private DateTime lastModified;
		private int journeyStatus; // -1 = not started, 0 = in progress, 1 = completed

		public Journey(int startID, int endID, DateTime journeyDateTime)
		{
			this.lastModified = DateTime.Now;
			this.startID = startID;
			this.endID = endID;
			this.journeyDateTime = journeyDateTime;
 
		}



		/* How the journey algorithm should prioritise (put this in the design!!!)
		- Is this the shortest route?
		- Is this an advertised through train (e.g. Manchester Airport)
		- National Routing Guide
		*/


        /*
		- Once initialised, Journey finds a Journey using Routing Guide
		- User selects the journey they are using
		- Waits until journey start time
		- Checks against timetabled journey vs actual journey
		- Determines a disruption level
		- Communicates disruption to user (if necessary)
		- Allows user to choose response to disruption
		- Informs user about Delay Repay
		- Journey class encapsulates the entire process - a bit like a FSM
        */
	}
}
