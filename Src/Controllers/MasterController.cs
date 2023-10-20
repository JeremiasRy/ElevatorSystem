using ElevatorSystem.Src.Graphics;

namespace ElevatorSystem.Src;

public class MasterController
{
    readonly ElevatorController _elevatorController;
    readonly ViewController _viewController;
    public void StartTick()
    {
        while (true)
        {
            Tick();
            Thread.Sleep(20);
        }
    }
    public void CallElevator(int floor)
    {
    }
    void Tick()
    {
        _elevatorController.Tick();
        _viewController.Draw();
    }
    public MasterController()
    {
        _elevatorController = new ElevatorController();
        _viewController = new ViewController(_elevatorController);
    }
}
