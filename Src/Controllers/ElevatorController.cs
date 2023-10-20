using ElevatorSystem.Src.Simulation;
using static ElevatorSystem.Src.Constants;

namespace ElevatorSystem.Src;

public class ElevatorController
{
    readonly Constants _constants;
    readonly Shaft[] _shafts = new Shaft[ELEVATOR_COUNT];
    readonly Floor[] _floors;
    public (int Row, int Col)[] ElevatorPositions => _shafts.Select(shaft => shaft.ElevatorPosition).ToArray();
    public void Tick()
    {
        MoveElevators();
    }
    void MoveElevators()
    {
        var activeFloors = _floors.Where(floor => floor.DownCallActive == Floor.FloorCallState.Active || floor.UpCallActive == Floor.FloorCallState.Active);
        foreach (var shaft in _shafts)
        {
            shaft.MoveElevator(activeFloors);
        }
    }
    public ElevatorController()
    {
        _constants = new Constants();
        for (int i = 0; i < _shafts.Length; i++)
        {
            _shafts[i] = new Shaft(new Elevator(), _constants.ShaftPositions[i]);
        }
        _floors = new Floor[_constants.FloorCount];
        for (int i = 0; i < _constants.FloorPositions.Length; i++)
        {
            _floors[i] = new Floor()
            {
                NthFloor = i,
                Row = _constants.FloorPositions[i],
            };
        }
    }
}
