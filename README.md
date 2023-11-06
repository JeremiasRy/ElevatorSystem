# ElevatorSystem
![cool stuff](https://raw.githubusercontent.com/JeremiasRy/ElevatorSystem/master/Elevator.gif)

An elevator simulator? You set human to call an elevator (press the floor number after that arrow key for direction). <br/> 
When the human reaches the elevator shaft it fires the call, an elevator comes to pick him up and a panel opens (this is the panel inside the elevator) <br/>
You choose where this human wants to go and then the elevator does what it does. <br/><br/>

The logic is nowhere near 100% but it gets the basics done, calls inside the elevator override calls from floors except if the calls are on the current path and to the right direction <br/>
And calls inside the elevator are also ordered accordingly. 

## Specs

- elevator count is adjustable in `Constants.cs` as long as it fits the screen it works
- the amount of floors comes from the size of the console window (bigger screen == more floors)
- It does crash if you change the screen while it's running but then you can just restart. (Maybe change to readjust in future...)

## How to run??

Well I'm glad you are so interested in this simulation. Use Visual Studio and run it in the debugger.
