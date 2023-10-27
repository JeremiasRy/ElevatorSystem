using ElevatorSystem.Src.Controllers;

namespace ElevatorSystem.Src.Data;

public class ElevatorData
{
    public int Destination { get; set; }
    public int CurrentLocation { get; set; }
    public int DestinationFloor { get; set; }
    public bool[] InputPanel { get; set; } = Array.Empty<bool>();
    public static ElevatorData FromElevatorController(ElevatorController elevatorController)
    {
        return new ElevatorData()
        {
            InputPanel = elevatorController.InputPanelValues,
            Destination = elevatorController.DestinationFloor is null ? -1 : elevatorController.DestinationFloor.Row,
            CurrentLocation = elevatorController.ElevatorPosition.ElevatorRow,
            DestinationFloor = elevatorController.DestinationFloor is null ? -1 : elevatorController.DestinationFloor.NthFloor
        };
    }
}
