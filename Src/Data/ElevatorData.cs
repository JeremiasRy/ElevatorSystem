using ElevatorSystem.Src.Controllers;

namespace ElevatorSystem.Src.Data;

public class ElevatorData
{
    public int DestinationRow { get; set; }
    public int CurrentLocation { get; set; }
    public int DestinationFloor { get; set; }
    public bool[] InputPanel { get; set; } = Array.Empty<bool>();
}
