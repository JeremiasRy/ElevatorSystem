using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.Inputs;

namespace ElevatorSystem.Src;

public class MasterState
{
    readonly ElevatorOrchestrator _elevatorOrchestrator;
    readonly ViewController _viewController;
    public void StartTick()
    {
        while (true)
        {
            Tick();
            Thread.Sleep(20);
        }
    }
    public void CallElevator(int floor, FloorCallInput.Direction dir)
    {
        _elevatorOrchestrator.ActivateFloorCall(new FloorCallInput() { Floor = floor, RequestDirection = dir });
    }
    public void CallElevatorPanelInput(int floor, int elevatorIdx)
    {

    }
    void Tick()
    {
        _elevatorOrchestrator.Tick();
        _viewController.Draw();
    }
    public MasterState()
    {
        _elevatorOrchestrator = new ElevatorOrchestrator();
        _viewController = new ViewController(_elevatorOrchestrator);
    }
}
