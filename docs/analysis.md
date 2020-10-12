# Analysis

## Investigation
### Introduction to Client
Mr Kevin Ashley is the managing director of Ashley Quality Improvement Services, a company that provides consultancy services for drug manufacture in the pharmaceutical industry. His role involves frequent travel around the UK and the EU, with schedules often changing to suit client needs. As such, he makes frequent use of public transport methods such as air and rail travel. However, during this travel he is often disrupted due to delays (particularly in the UK) - and as such has asked myself to attempt a program that would be able to dynamically reroute his journey as a result of delays.

### Outline of the Problem
When travelling by train in the UK, Mr Ashley's journeys often involve at least one connection between services. However, disruption on the rail network can result in him missing his connection, and having to wait at a connecting station. Due to the nature of his work and lack of facilities at train stations, a missed connection typically results in a disruption to his workflow; potentially reducing billable hours. Furthermore, due to the complex nature of his journeys, a missed connection early in the journey typically results in having to find alternative connections later in the journey - causing greater disruption to his workflow. 

For example, Mr Ashley regularly travels from his home station of Ulverston to Chester to visit a client. This journey involves a minimum of two changes; from the Ulverston train to the West Coast Main Line (WCML), then from the WCML to Chester. There are various routings available from the WCML to Chester - via Warrington, and via Crewe. The earliest arrival time on this route depends on available trains in the timetable. As such, a missed connection onto a WCML service can mean a routing that was originally quicker via Crewe is quicker via Warrington - however, this is harder to identify during a journey without recalculating the entire route again manually.

Furthermore, recalculating the route manually typically does not take into account factors such as connection times from one service to another. In the case of the Warrington/Crewe problem, a train running 15 minutes late at Warrington is highly likely to miss the connection onto the service to Chester at Crewe - therefore, it is highly likely that changing at Warrington would achieve the quickest overall journey time. However, this is difficult to work out manually during delays and as such he would prefer for this process to be automated.

Therefore, Mr Ashley is seeking a solution that informs him of any disruption to his journey in real time; and if the disruption is so great as to cause a missed connection, recalculate his route based on the fastest possible routing.

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

### Research
During the interview, Mr Ashley pointed out the NS (Dutch Railways) app as an example of a suitable solution. 

## Analysis of Investigation

### Interview Analysis
The interview has provided me with several useful points of information that will contribute greatly in developing a proposed solution. Overall, the system needs to be

### Further Research
During my initial interview, Mr Ashley pointed out his desire for a service which would enable him to automatically claim money back for delays caused during the journey through the Delay Repay scheme. 

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

The first rerouting, via Crewe, is the most obvious. It would involve joining the next service to Crewe, then the next service to Chester. This journey is indeed the one that many people would follow in this situation - however, it causes the most delay at 60 minutes while also involving a longer wait at Preston due to the timetabling of Crewe services.

The other two reroutes both involve changing at Warrington Bank Quay (WBQ) - with the difference between the routings being the connection time at Warrington. In the case of the second option, there is a four minute connection time at WBQ - below the minimum connection time for the station, but a cross platform interchange and as such very reasonable to plan for. It does not, however, allow for further disruption due to the tight/unofficial connection. The third reroute resolves this issue with a 17 minute connection time at WBQ - above the minimum five minutes, and allowing for further disruption.

In this scenario, it is logical to plan for the second option but be prepared to take the third. These reroutes, however, deliver a massive time saving over the most obvious reroute - with a theoretical maximum delay of 16 minutes routing via Warrington compared to 60 minutes via Crewe. These time savings highlight the benefits of rerouting - however, this process is often not easy when travelling.




### Data Flow Overview
**Input:**

**Process:**

**Output:**

**Storage:**

### Data Dictionary


### Data Flow Diagram



## Proposed Solution

### Data Volumes
I intend to provide the solution as a website for the end-user, necessitating minimal storage space for the end user.

At the server side, most information is not stored locally but rather fetched as necessary from the National Rail/TFL APIs. 

### Acceptable Limitations
Unfortunately, it is unviable to obtain access to the National Rail Journey Planning system due to cost. This means that the journey planning solution will have to be manually maintained, and as such may lack fully up to date information on all stations on the National Rail network, such as changed MCTs and new alternative routings. Furthermore, due to time constraints, my solution will only have limited integration with other forms of public transport which may be more viable options (such as taking a bus). 
The system will also be unable to determine whether the users ticket is valid for the routing taken, as it is not possible to fully integrate ticketing restrictions within the time frame of the project. However, due to the focus of the solution largely being on finding new journeys during disruption (where ticket restrictions are typically relaxed following missing the original service) this issue in particular should have minimal impact on the usability of the solution.
// Not here? // Mr Ashley has also stated that while he would like to see the implementation of automatic delay repay within the solution, it would be acceptable if the application only provided the information necessary for the claim. Further research into this has highlighted the unsuitability of many TOCs web services for automatic solution-based claims - typically requiring registration. The introduction of this feature would also require the storage of payment information, which would introduce new complexity to the solution.
