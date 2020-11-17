# Analysis

## Investigation
### Introduction to Client
Mr Kevin Ashley is the managing director of Ashley Quality Improvement Services, a company that provides consultancy services for drug manufacture in the pharmaceutical industry. His role involves frequent travel around the UK and the EU, with schedules often changing to suit client needs. As such, he makes frequent use of public transport methods such as air and rail travel. However, during this travel he is often disrupted due to delays (particularly in the UK) - and as such has asked myself to attempt a program that would be able to dynamically reroute his journey as a result of delays.

### Outline of the Problem
When travelling by train in the UK, Mr Ashley's journeys often involve at least one connection between services. However, disruption on the rail network can result in him missing his connection, and having to wait at a connecting station. Due to the nature of his work and lack of facilities at train stations, a missed connection typically results in a disruption to his workflow; potentially reducing billable hours. Furthermore, due to the complex nature of his journeys, a missed connection early in the journey typically results in having to find alternative connections later in the journey - causing greater disruption to his workflow. 

For example, Mr Ashley regularly travels from his home station of Ulverston to Chester to visit a client. This journey involves a minimum of two changes; from the Ulverston train to the West Coast Main Line (WCML), then from the WCML to Chester. There are various routings available from the WCML to Chester - via Warrington, and via Crewe. The earliest arrival time on this route depends on available trains in the timetable. As such, a missed connection onto a WCML service can mean a routing that was originally quicker via Crewe is quicker via Warrington - however, this is harder to identify during a journey without recalculating the entire route again manually.

Furthermore, recalculating the route manually typically does not take into account factors such as connection times from one service to another. In the case of the Warrington/Crewe problem, a train running 15 minutes late at Warrington is highly likely to miss the connection onto the service to Chester at Crewe - therefore, it is highly likely that changing at Warrington would achieve the quickest overall journey time. However, this is difficult to work out manually during delays and as such he would prefer for this process to be automated.

Therefore, Mr Ashley is seeking a solution that informs him of any disruption to his journey in real time; and if the disruption is so great as to cause a missed connection, inform him of available options to complete his journey.

### Interview with Client
**How do you travel normally in the UK?**   
I use the Trainline app to book my tickets prior to commencing my journey. Then during my journey, I use station departure boards and the Trainline app to check if there has been any disruption to my journey. When I am aware of any significant disruption to my journey, such as a late arrival time, I use the Trainline app again to determine if I need to change my journey. However, this is particularly difficult to do when travelling as there may be poor loading times or my journey may need to change further. This is much worse than the solution I use for travel in the Netherlands, where the NS (Dutch Railways) app automatically informs me of any disruption and quickly informs me of my available options, including alternate routes.

**What benefits does your current system have?**  
The current system is particularly useful during the initial planning of a journey, checking my available options. It is also useful in terms of providing some information about disruption that may impact my journey, such as engineering works or a broad description of disruption. My current system has also adopted well to the COVID-19 crisis - which has resulted in a more contactless railway and frequent timetable changes. The availability of mobile tickets is particularly useful for me.

**What are the drawbacks of your current system?**  
While the apps I currently use are good in terms of informing me about general disruption, they aren't as good at telling me when my journey has been impacted. This means that when I am impacted by disruption, I have to manually check my alternate options. The nature of a delayed journey means that I often have to spend more time worrying about issues such as contacting clients about my late arrival and is generally disruptive to my workflow. Furthermore, the current system is somewhat difficult to use when claiming my money back through Delay Repay.

**Are there any particular features you feel are essential to a new system (must haves)?**  
The most important thing for me is having a solution that is able to remove the majority of the manual work that is required when re-routing a journey. Ideally, I would like a solution that notifies me in real-time if there is any disruption to my journey, before providing me with alternative options. I should then be able to choose from these options before being navigated on the new journey. It would also be good to have information about stations that aren't on my route - particularly during circumstances that involve tight connections.

I will provide you with an example journey after the meeting to show you what I mean about alternative options - ideally, they should not just be those available through normal journey planners but give me tight connections as options too.

**Are there any features that you feel would improve the system but do not require? (nice to haves)**   
As I mentioned before, I often spend a disproportinate amount of time having to claim back tickets through Delay Repay schemes. It would be beneficial if it was able to provide the information necessary for a Delay Repay claim, and if possible make the claim automatically. It would also be useful if the app was able to inform others about the progress of my journey if needed - for example, if I was visiting a client and would be at least 15 minutes late then it would be good if the client could be emailed about this, or even informed through the app itself.

Another potentially useful feature is information about quicker methods than rail - for example, it can be quicker to take the bus if a train to Barrow is cancelled. I do appreciate that this may involve much greater complexity - but it would certainly be nice to have!

**How would you envisage using this new system? (web app / mobile app?)** 
Ideally, either an app or through a webpage. There definitely should be notifications on my phone about any changes to the journey. 

**Do you envisage a need to connect this system with other corporate services?**  
I use Google Workspace for managing my email services. If you were able to integrate the emailing of clients that I discussed earlier, it would be good if this was done directly through my work account. I'd also appreciate it if the app was able to automatically create a calendar appointment when I input my journey. 

**Is there anything else you would like to add?**   
I would highly recommend that you look at the NS app that I mentioned before - it pretty much shows the majority of what I'd want to see in your final solution.

### Current System
Currently, Mr Ashley books his journeys on the 'Trainline' app prior to the journey - but typically does not have time to check the status of his journey. As such, delays are often realised upon arriving at the station or during the journey. If the delay is sufficient enough to cause a missed connection, many of his journeys have the potential to have quicker arrival at the destination via a different routing - meaning that he often replans his whole journey from that point. However, when involved in disruption, this does not always enable him to achieve the earliest arrival at his destination due to the real time nature of delays.

## Analysis of Investigation

### Interview Analysis
The interview has provided me with several useful points of information that will contribute greatly in developing a proposed solution. There are a lot of features that could be implemented to improve the experience of the application that we discussed; however, it is clear that the main focus of the program will be on dynamic journey planning. I also intend to work on some form of Delay Repay information system to help the client, and integration with the client's Google Workspace setup.

Following the interview, I first researched the NS (Dutch Railways) app that Mr Ashley mentioned as a solution he had found worked well for him previously. When attempting to plan a journey, the app provided me with nearby stations for departure. After selecting a departure, arrival and departure time, the app gave me a list containing all available journeys. It also provided information about trains that have already departed - a feature that would be redundant in this solution due to the requirement to replan journeys.

One feature I liked that I hadn't considered previously was the provision of additional information such as train length and type, onboard services as well as overcrowding information. Information on train length is provided in the UK by the Darwin API - making this a potential additional feature for the solution.

I noted in a later discussion with Mr Ashley that the NS app does have additional functionality in terms of ticket booking - but this is outside the scope of the project. 

Mr Ashley also expressed his wish to have Delay Repay, where passengers are refunded based on delays to their journey, integrated into the system and able to automatically handle claims. However, it would also be suitable if the system instead provided the information needed for the claim. When researching potential solutions to implementing Delay Repay, it became clear very early on that there is fragmentation in terms of both access available from a developers perspective and also in terms of the availability the scheme itself.

As such, in terms of solution integration, it will be more focused on letting the user know when they can claim Delay Repay and providing the necessary information to allow a successful claim. I have spoken to Mr Ashley and he feels this will be sufficient.

During the interview, Mr Ashley also mentioned his wish to see some integration with Google Workspace. I have researched this issue further, and have found that Google provide an API to allow third party applications to authenticate and integrate with their services. As such, I am able to implement the ability to sign in with a Google account,and also the desired calendar integration. This will be done if there is remaining development time - however, the design of the system will be done with the aim of integrating these features easily.

### Analysis of Example Journey
Following the interview, Mr Ashley has kindly provided me with an example of his typical journey from Ulverston to Chester. This is illustrated in the diagram below:

![JourneyAnalysis_ExampleJourney](https://user-images.githubusercontent.com/24415853/94245017-96ef1800-ff11-11ea-85e2-1ef96cde6c5c.png)

On a typical journey, Mr Ashley travels on the 17:04 to Preston, before changing at Preston for the 18:17 to Crewe. He then waits for 27 minutes at Crewe for the 19:24 service to Chester. This is the journey that Mr Ashley is given in the Trainline app when booking prior to the journey; it does not account for any potential disruption. During our initial interview, Mr Ashley highlighted this journey as being one which is often disrupted.

This journey also highlights some of the complications involved in journey planning in the UK. While ultimately dictated by available services, a factor that is introduced on journeys involving connections is that of the Minimum Connection Time (MCT). This time is a reasonable amount of time for a person travelling to make a change between services, even at a station they are unfamiliar with. It should be noted that the minimum connection time is **not** the minimum time actually required to change between two services, but rather a figure for journey planning purposes. As such, while it is a reasonable assumption to make that when a connection is below the MCT that a new route may be needed, a tight/unofficial connection can often still be made.

Following been given this example, I have also found the timetable data for the period following the 1704 departure from Ulverston. This can be found in **Appendix I**.

#### Example 1: Disruption to first journey
*Mr Ashley boards the 17:04 service from Ulverston, which departs on time. However, prior to arriving at Preston, the train encounters cows on the line – causing a 9 minute delay. The train arrives into Preston at 18:15 – below the minimum connection time of 8 minutes.*

In this example, there is still the possibility of Mr Ashley being able to connect onto the 18:17 service that he originally intended to travel on. However, the connection is extremely short, so for the purposes of this example, we shall assume that he missed it and needs to find a new routing. These potential routings are highlighted in the diagram below.

![JourneyAnalysis_Example1](https://user-images.githubusercontent.com/24415853/94919199-046bed00-04ac-11eb-8222-f47649b96a9d.png)

There are three viable reroutes for this journey - one following the original routing via Crewe, and two via Warrington Bank Quay.

The first rerouting, via Crewe, is the most obvious. It would involve joining the next service to Crewe, then the next service to Chester. This journey is indeed the one that many people would follow in this situation, but is in fact the longest; causing a 60 minute delay, and a longer wait at Preston.

The other two reroutes involve changing at Warrington Bank Quay (WBQ) - with the difference between the routings being the connection time at WBQ. In the case of the second option, there is a four minute connection time at WBQ. This is below the MCT for the station of 5 minutes - and as such, is not classified as an official journey option and would not be displayed on normal journey planners. However, this does not take into account the cross platform nature of the interchange - and as such, this reroute, while being an unofficial connection, is in fact a viable option. In the case of further disruption, however, it is highly likely that there may be a missed connection.

The third option is more resillient, with a 17 minute connection time at WBQ - above the minimum five minutes, which in turn provides more time in case of further disruption.

As such, it is logical to plan for the second option but be prepared to take the third. However, there are a few key points we can take from our analysis of this scenario:
* **The most obvious routing is often not the quickest.** In the case of this sample, we deliver a maximum delay (assuming no significant further disruption) of 16 minutes routing via WBQ, and can deliver an 8 minute maximum delay if there is no disruption from Preston.
* **The most efficient routings aren't always obvious.** The quickest routing, with a delay of 8 minutes, would not be displayed using a journey planner due to the unofficial connection at WBQ. This shows that while journey planners are good for initial planning, the usage of baseline MCTs can actually cause further delay as users aren't informed of potential quicker options.

#### Example 2: Disruption while enroute
*Mr Ashley boards the 17:04 service from Ulverston, which arrives at Preston as scheduled. He then boards the 18:17 service to Crewe as scheduled. At Wigan North Western, police are called due to a disturbance in the shop. This causes a 23 minute delay to the service, resulting in an estimated arrival time into Crewe of 19:20.*

In this example, there is again still the potential to follow the originally intended routing and arrive at Chester as scheduled. However, Crewe is a large station, and has a 10 minute MCT. As such, it is reasonable to assume that the connection is potentially not possible, especially when unfamiliar with the station - which is shown in the diagram below.

![Crewe](./assets/Crewe.svg)
*Diagram copyright National Rail Enquiries - used under fair use.*

The diagram above also includes the routing from Platform 5 to Platform 9, the timetabled platforms for these two services. It is at this point where it is important to make the distinction between a connection which is 'potentially not possible' and 'impossible'. While 4 minutes is a very tight connection at a large station like Crewe, for an experienced traveller it may still be very possible.
Indeed, speaking to Mr Ashley, he estimates his standard connection time at Crewe is about 5 minutes anyway. It is also possible possible to make the connection process quicker through forward planning - for example, knowing which doors to leave the train at to access the bridge quickly.

Considering these factors, two potential options become apparent following the delay at Wigan:
- **Continue to Crewe:** while a tight connection, still potentially possible. Joining the original service would be most likely to allow an on time arrival. However, the tight nature of the connection may not be suited for an inexperienced traveller.
- **Leave the train at WBQ:** This would ensure that an official connection would be possible (to the 19:26 service mentioned in Example 1) - however, potentially causing an unneccessary 8 minutes of delay.      

The decision to change services is hardly clear cut, and depends on many factors and a lot of 'what if?' scenarios. The service to Crewe may make up time before arriving, allowing a more relaxed connection - equally, there is the chance that the service may be delayed further. It may also be difficult for a passenger travelling to find out information about available services from Warrington in time.

It is in this situation that having data accessible to determine the likelihood of further delay, or less delay, would be helpful. Equally, having the journey options available at WBQ would also be beneficial. These are both issues that the system proposed would be able to assist with - looking at past performance data of the services involved, and being able to display information to the user to help make an informed decision.

We can therefore take the following points from our analysis of this journey:
* **Making a decision during disruption is not clear cut.** It is difficult to make a decision that is not simply 'chancing' it while travelling - it is near impossible to check past performance data on the move, while finding alternative journey options while travelling may also be difficult.
* **Human factors play a major role.** Someone who is inexperienced at travelling may find a tight connection much more difficult than an experienced traveller who commutes on a route weekly. As such, it is difficult for a system to accurately determine the probability in situations such as this example.

#### Conclusion
Overall, looking into the example journey and the different circumstances that can impact it have given me both ideas for the solution while also presenting some problems, most notably when involving tight connections where the users actions are the biggest influence. Before starting the design of the program, I will research minimum connection times further to see what assumptions I can reasonably make, and which ones are more nuanced and require direct user intervention.

### Input, Process, Output, Storage
One of the functions that the system will perform is the processing of data related to a users journey. To be able to define the scope of this in my proposed solution, I need to first understand the data used by the system. As I already have a full set of data from the example journey from Ulverston to Chester, I will use this data to determine what is inputted, processed, outputted and stored by the system.

|   IPOS  |                               Information                               |                                                      Example                                                      |                                  When?                                 |
|:-------:|:-----------------------------------------------------------------------:|:-----------------------------------------------------------------------------------------------------------------:|:----------------------------------------------------------------------:|
| Input   | User Information: Username; Password; Email Address                     | agentsquash; passw0rd; john.appleseed@gmail.com                                                                   | When the user enters the system                                        |
| Input   | Journey Information: Departure Station; Arrival Station; Departure Time | Ulverston, Chester, 17:00                                                                                         | When the user creates a new journey                                    |
| Input   | Timetable Data from Darwin Push Port;                                   | -                                                                                                                 | First running of program                                               |
| Input   | Real Time Journey Data from Darwin Live Departure Board (LDB)           | -                                                                                                                 | When necessary                                                         |
| Input   | Journey Disruption from Disruptions WS                                  | -                                                                                                                 | When necessary                                                         |
| Input   | Station Data from NR Knowledgebase (Stations)                           | -                                                                                                                 | When requested by the user                                             |
| Process | Create user account using inputted username, password and email.        | -                                                                                                                 | If the user does not have an existing account                          |
| Process | Determine routing using departure station and time, arrival station     | -                                                                                                                 | After creating a journey; Following journey intervention by the system |
| Process | Check if the journey has started                                        | -                                                                                                                 | Following the creation of a journey until a journey starts             |
| Process | Check journey progress                                                  | At Ulverston: 17:04 timetabled. 17:04 departure. On time.                                                         | At each timing point following the commencement of a journey.          |
| Process | Check if intervention is required                                       | -                                                                                                                 | If journey is delayed.                                                 |
| Process | Determine intervention required                                         | -                                                                                                                 | If intervention is required                                            |
| Process | Change routing to determine intervention                                | -                                                                                                                 | Following selection of intervention by end user.                       |
| Process | Delay Repay check                                                       | -                                                                                                                 | If journey is intervened.                                              |
| Output  | Available journeys                                                      | 17:04 Ulverston-Preston; 18:17 Preston-Crewe; 19:24 Crewe-Chester                                                 | When the user creates a journey                                        |
| Output  | Available reroutings following journey intervention                     | 1921 via Crewe; 1905 via Warrington                                                                               | When the program determines journey intervention is necessary.         |
| Output  | Journey information                                                     | -                                                                                                                 | When on journey; journey history                                       |
| Output  | Delay repay information                                                 | Your journey was delayed by 37 minutes by Avanti West Coast. You can claim Delay Repay at: avantiwestcoast.co.uk. | If journey is eligible for Delay Repay.                                |
| Storage | Timetable data                                                          | -                                                                                                                 | To be fetched as required, but stored locally otherwise.               |
| Storage | Previous journeys                                                       | 09/11/20: ULV-CTR; 08/11/20: CTR-ULV                                                                              | For use in Journey History                                             |
| Storage | Current journey information                                             | -                                                                                                                 | Acts as a cache so data does not need to be fetched continuously.      |

### Further research
I have looked into the APIs available for accessing National Rail data, of which there are three:
- Darwin - real time train running information.
- Knowledgebase - contains customer facing information about stations and disruption
- Online Journey Planner - contains a Real Time Journey Planner (RTJP) and the Disruption Webservice. Unfortunately, due to cost constraints, I am unable to use the RTJP in the scope of this project.

From my research, I have found that there is large quantities of data that need to be processed, especially when working with timetable data. This is something that my solution will need to take into account - using a web based solution to process the data and provide it in a user friendly format.

### Entity Relationship Diagram
![ERD](./assets/ERD.svg)    
User represents the end-user using the program. One user can have many journeys using the application, having a one-to-many relationship. Each journey using the application will have at least one service, and due to the nature of the application will likely involve many services - again giving a one to many relationship. Each service will call at multiple stations, which will be treated by the application as a data point to determine disruption. This is also representative of a one-to-many relationship.

### Data Flow   
To help me visualise the flow of data within the system, I have created a series of data flow diagrams (DFD). This will also help me identify areas that could be optimised.

**Current System - Level 0**   
![DFD Level 0](./assets/DFD%20Level%200.png)

The current system is relatively simple. The checking of journey progress is typically done through the Trainline mobile app, as is 'Find Routing'. I do not envisage the program becoming more complex for the user at the point of use - however, there will be additional functionality added to reduce the need for the user to request data.

I have determined there is no need for a Level 1 DFD for the current system as I am unable to accurately determine the data flow to that level.

## Proposed Solution

### Flow Diagram
![Flow Diagram](./assets/Flow%20Chart.svg)

To better understand what needs to be included in the program, I have created a flow diagram that illustrates the journey checking and login features of the program. This provides a high level overview of how the program will work.

### Data Flow
![DFD Level 0](./assets/DFD%20Level%200%20-%20New%20System.png)

The Data Flow for the Proposed System is similar to that of the original system, but is also reflective of the systems ability to request user input on alternative routing options through the 'Disruption Handler' process.

### Data Dictionary
**User Information**
| Field            | Data Type | Example                                                            | Validation                                                                                                                    |
|------------------|-----------|--------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------|
| UserID           | INT(16)   | 1                                                                  | Must be increment.                                                                                                            |
| Username         | CHAR(64)  | "agentsquash"                                                      | Username should be less than 20 characters if a local account; Username should be alphanumeric;                               |
| isOAuth          | BOOLEAN   | FALSE                                                              | If user is authenticating via Google; should be TRUE. If user is using an app specific account - should be FALSE.             |
| Password         | CHAR(64)  | "9e66a118b9a0fb8cada5eb0f357806a21cc067fa4b9d9f76eb9773a24e022438" | Password should be 64 characters long. Passwords are hashed and salted. If authenticating through OAuth, this should be null. |
| OAuthAccessToken | CHAR(256) | null                                                               |                                                                                                                               |
| Email            | CHAR(320) | "john.appleseed@icloud.com"                                        | See https://tools.ietf.org/html/rfc3696#section-3                                                                             |
| accessLevel      | INT(1)    | 2                                                                  | Between 0 and 2.                                                                                                              |                                                                |

This table will be used to store end user login information. There will be a class which encapsulates all authentication behaviours and interfaces with the website. The class will contain OAuth integration to allow users to use their Google account to login instead of directly creating an account on the system. Alternatively, users can create an account by providing a username, password (which will be hashed and has a salt) and email address. This table also handles User Access Levels - where an admin user is able to access usage statistics for all users (anonymised) while user access grants users only access to their data.

**Journey**
| Field             | Datatype    | Example                | Validation                                                                                                                     |
|-------------------|-------------|------------------------|--------------------------------------------------------------------------------------------------------------------------------|
| JourneyID         | INT(32)     | 12                     |                                                                                                                                |
| UserID            | INT(16)     | 1                      |                                                                                                                                |
| departureCRS      | CHAR(3)     | "ULV"                  | Must be valid National Rail routing point. When a station name is entered, it will be converted using the National Rail API.   |
| arrivalCRS        | CHAR(3)     | "CTR"                  | Must be a valid National Rail routing point. When a station name is entered, it will be converted using the National Rail API. |
| planDepTime       | DateTime    | 04/12/2002 04:38:00 PM | Time should be in the future when creating a journey.                                                                          |
| actDepTime        | DateTime    | 04/12/2002 04:39:00 PM | Time should be in past when entered.                                                                                           |
| planArrTime       | DateTime    | 04/12/2002 05:38:00 PM | Time should be in the future when creating a journey.                                                                          |
| actArrTime        | DateTime    | 04/12/2002 06:39:00 PM | Time should be in past when entered.                                                                                           |
| delayRepayEligble | BOOLEAN     | TRUE                   | If actArrTime >= planArrTime+15, then True                                                                                     |
| delayRepayBand    | INT         | 2                      | Check that band is equal to delay incurred.                                                                                    |
| delayRepayTOC     | CHAR(2)     | "VT"                   | Company that caused the delay.                                                                                                 |
| service_1(-10)    | VARCHAR(36) | _I-TViCwPv8uNOTk-oNeJQ | Service name is the same as the web-safe Darwin GUID.                                                                          |

This class contains the journey information that will be stored locally by the system. It is used in the creation of new journeys within the system, and will also be used to determine whether a journey is eligble for Delay Repay. The class also the unique service identifier as provided by the National Rail Darwin API to allow the identification of unique services by the application. The information contained within the Journey class will be stored long term by the system to provide journey history.

**Service Progress Class**      
The Service Progress Class is created dynamically by the application when a journey is in progress. It consists of the following data:
| Field          | DataType    | Example                | Validation                                                               |
|----------------|-------------|------------------------|--------------------------------------------------------------------------|
| service_guid   | VARCHAR(36) | _I-TViCwPv8uNOTk-oNeJQ | The Darwin GUID of the current service. Set to NULL if connection = TRUE |
| connection     | BOOLEAN     | FALSE                  | Set to TRUE if a connection is in progress.                              |
| nextCRS        | CHAR(3)     | "BIF"                  | Must be a valid National Rail routing point.                             |
| nextTimetable  | DateTime    | 04/12/2002 05:38:00 PM | Matches a timetable entry for the CRS                                    |
| nextETA        | DateTime    | 04/12/2002 05:42:00 PM | Matches the Darwin feed ETA.                                             |
| lastCRS        | CHAR(3)     | "ROO"                  | Must be a valid National Rail routing point.                             |
| lastTimetable  | DateTime    | 04/12/2002 05:32:00 PM | Matches a timetable entry for the CRS                                    |
| lastETA        | DateTime    | 04/12/2002 05:36:00 PM | Matches the Darwin feed actual arrival.                                  |
| currentDelay   | INT         | 5                      | Difference between nextTimetable and nextETA                             |
| intervene      | BOOLEAN     | TRUE                   | If currentDelay is below MCT, then TRUE                                  |
| interveneLevel | INT         | 2                      | Defined in Design.                                                       |

The Service Progress Class handles the real time disruption handling part of the system. It tracks the progress of the current journey against the planned routing, and determines whether there is a need to intervene, and the level of intervention (notification) required. At this stage in development, there are provisionally four Intervention Levels.

* -1: No intervention required.
* 0: Unofficial/tight connection. No rerouting deemed necessary.
* 1: Unofficial/tight connection. Information about rerouting/connection available to user.
* 2: Missed connection. Journey options shown to user.

The class will contain algorithms that respond to the intervention level and provide the necessary information for the end user.

### Data Volumes
Due to the nature of the National Rail timetable, the amount of data that will be provided and used by the system at any one time is variable. At the time of analysis in November 2020, the timetable is changing at a particularly high rate due to service changes as a result of the COVID-19 pandemic. As such, it is impossible to provide a fully accurate estimate of the amount of data that will be used by the system as a whole, as the timetable is a core feature of the application that is fetched on a regular basis.

However, it is still possible to make some estimates about the size of the application in terms of persistent data that is stored long term. It is also possible to state that the size of the system for the end user will be minimal as the system will be accessed as a web app.

In terms of the server side storage, it is possible to make estimates to the size of some aspects of the program.

#### User Information
The User Information table consists of 7 elements: userID, username, password, email, admin, isOAuth and an OAuthAccessToken. The usage of OAuth means that there are two potential scenarios for data storage:
- a user authenticating using OAuth
- a user authenticating using a local system account

If a user choses to authenticate using OAuth (Google Sign In), then five fields are used: userID, username, email, isOAuth, OAuthAccessToken and Admin. The username will be stored in a UTF-8 format, annd have a maximum length of 64 characters. This results in a maximum size of 64 bytes (512 bits). The email field has a maximum length of 320 characters. Again using UTF-8, this results in a maximum size of 320 bytes (2560 bits), and a cumulative total of 384 bytes (3072 bits). Two boolean variables are used in the storage of an OAuth account - isOAuth and Admin. These both take up an additional bit of storage each. The userID is stored as a 16 bit integer. To store a 16 bit nWhen authenticating using OAuth, an additional Access Token is used. This will be stored as a UTF-8 sintrg, taking up 256 bytes (2048 bits) of storage. In total, this results in a size of 649.5 bytes (5196 bits) per OAuth user stored by the application.

If a user choses to authenticate using a local account, then five fields are used: username, email, isOAuth, Admin and password. The username, email, userID, isOAuth and Admin fields both take up the same space as an OAuth user - however, instead of using a 256 character access token, a 64 bit hashed (and with a salt) password is used. This password is stored in a UTF-8 format, resulting in a size of 64 bytes (512 bits). This results in a total size of 3660 bits (or 457.5 bytes) in the worst case scenario.

Assuming of the application by 20 users, with the majority (90%) choosing to authenticate via OAuth, this results in a storage size of 100,848 bits (12.6KB).

At this point in development, it is not possible to provide an accurate estimate for the size of the journey and service classes, due to their highly variable nature.

### Data Representation
One of the key features of the proposed system, the dynamic journey rerouting feature, poses challenges due to the need to represent the UK rail network in a manner that can easily be used for journey planning. National Rail and the Rail Delivery Group (RDG) have an API available to prevent this issue; however, due to tight cost restraints from the client, I am unable to use this interface. Therefore, it is necessary to create a new representation of the rail network for this purpose.

I am able to use the Darwin Live Departure Board (LDB) to check the real time status of services. I can also pull timetable data from National Rail using their Push Port facility - which also gives me timetable snapshots when updated. I can also request routing data from National Rail as necessary to assist with routing.

With this data, it is possible to represent the UK rail network as a weighted graph which can then be searched, potentially augmented by the routing data provided by National Rail. 

### Acceptable Limitations
Unfortunately, it is unviable to obtain access to the National Rail Journey Planning system due to cost. This means that the journey planning solution will have to be manually maintained, and as such may lack fully up to date information on all stations on the National Rail network, such as changed MCTs and new alternative routings. Furthermore, due to time constraints, my solution will only have limited integration with other forms of public transport which may be more viable options (such as taking a bus). 

The system will also be unable to determine whether the users ticket is valid for the routing taken, as it is not possible to fully integrate ticketing restrictions within the time frame of the project. However, due to the focus of the solution largely being on finding new journeys during disruption (where ticket restrictions are typically relaxed following missing the original service) this issue in particular should have minimal impact on the usability of the solution.

## Objectives

**Account Authentication, Integration and History**
1. The user **must** not be able to access the system without logging in.
2. The user **must** have the ability to create an account to access the system.
   1. The user should be able to use their Google Workspace account to create a system account through Google Sign-In.
   2. The user **must** be able to create a local account on the system by providing a username, email address and password. 
      1. Passwords for local accounts **must** be stored securely (hashed and with a salt)
      2. Usernames **must** be entered in a valid format (alphanumeric, length less than or equal to 20)
      3. Email addresses **must** be entered in a valid format (as per RFC3696)
      4. Email addresses should be authenticated through a verification email.
   3. All new accounts created **must** be 'user' accounts, without administrator access.
3. The user **must** have the ability to login to the system.
   1. The user should be able to login using their Google Workspace account through Google Sign-In.
   2. The user **must** be able to login using their local system account by providing a correct username/email address and password combination.
4. Users **must** have the ability to reset a forgotten password.
   1. The users account ownership should be verified through a verification email.
   2. The users previous password **must** be overwritten, and never provided to the user.
   3. The user **must** receive confirmation that their password has been reset.
5. Once logged in, the user **must** be able to see information about previous and new journeys.
   1. Users should be able to see their journey history.
   2. Users **must** be able to create a new journey.
   3. Users should be able to perform basic administrative tasks.
      1. Local system accounts should be able to link to a Google Workspace account after creation.
      2. Local system accounts should be able to change their password.
         1. The users account ownership should be verified through a verification email.
         2. The users previous password should be overwritten, and never provided to the user.
         3. The user should receive confirmation that their password has been reset.
6. Administrators should be able to upgrade an existing accounts access level.

**Journey Planning, History and Delay Repay**
1. The user **must** have the ability to create a journey within the system.
   1. The user **must** be able to specify a departure station by name.
   2. The user **must** be able to specify an arrival station by name.
   3. The user **must** be able to specify a departure time.
   4. The user **must** be able to select a journey that meets the search criteria above.
      1. If the journey involves a service which allows seat reservations, the user should have the option to enter these into the application.
2. The user **must** have the ability to cancel a journey within the system.
3. The user **must** be notified if the first service in their journey is cancelled prior to them commencing the journey in the system.
4. The user **must** be notified if there is significant disruption impacting their planned journey prior to its commencement.
5. The user **must** have the ability to 'start' a journey within the system.
6. The system should provide up to date journey information at each reporting point.
   1. This information should be provided on the website; it should not be pushed to the user.
   2. This information should also include additional information where possible, such as train lengths and service related issues.
7. The system **must** provide up to date journey information at any connection point.
   1. The system **must** provide information regarding the next train on the journey, and platform information.
   2. The system should include additional information such as train length and other specific service information.
      1. If the service has a seat reservation **and** there are defined station boarding points (such as on the West Coast Main Line), the user should be told where to stand for the easiest access to their next service.
   3. If the connection is deemed 'tight' (Intervention Level 0/1), the application should attempt to provide additional information where possible to allow the fastest connection.
8. The system **must** inform the user if the journey is delayed enough to become a tight connection.
   1. The system **must** provide alternative journey opportunities if it determines that a tight connection may not be possible.
   2. The user **must** be able to select from alternative journey opportunities if it is deemed a connection may not be possible.
9. The system **must** provide alternative routing opportunities if the connecting service is cancelled, or the user will miss the service due to a delayed arrival.
   1. The user **must** be able to select the alternative journey they take.
10. The user's journey history **must** be stored by the system.
    1.  The user should be able to access their previous journey history.
1.  The user should be advised if a journey is delayed enough to be eligble for Delay Repay (where available)
    1.  The user should be provided with the necessary information for a Delay Repay claim.
    2.  The user should be redirected to the correct Train Operating Company to make their Delay Repay claim.
