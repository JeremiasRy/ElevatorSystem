using ElevatorSystem.Src.Controllers;
using ElevatorSystem.Src.Graphics;
using ElevatorSystem.Src.Inputs;
using System.Drawing;

namespace ElevatorSystem.Src;

public class MasterState
{
    int _selectedFloor = -1;
    bool _openInputPanel = false;
    int _openInputPanelElevatorId = -1;
    int _openInputPanelUserId = -1;
    int _keyPressCoolDown = -1;
    readonly ElevatorOrchestrator _elevatorOrchestrator;
    readonly ViewController _viewController;
    readonly KeyboardInput _keyboardInput;
    readonly Constants _constants = new ();
    readonly List<UserCall> _userCalls = new();
    public void StartTick()
    {
        int tickCount = 0;
        while (true)
        {
            if (tickCount++ == 2)
            {
                Tick();
                tickCount = 0;
            }
            if (!_openInputPanel)
            {
                CheckUserCalls();
            }
            if (_keyPressCoolDown == 0)
            {
                CheckKeyboardInput();
            }
            _keyPressCoolDown = _keyPressCoolDown - 1 < 0 ? 0 : _keyPressCoolDown - 1;
            SetupView();
            _viewController.Draw(_openInputPanel);
            Thread.Sleep(20);
        }
    }
    void CheckUserCalls()
    {
        var matches = _userCalls
            .Where(userCall => _elevatorOrchestrator.GetElevatorsIdleAtFloor().Any(elevator => elevator.Floor == userCall.StartFloor) && userCall.State == UserCall.UserCallState.WaitingForElevator)
            .Select(userCall => new
            {
               Call = userCall,
                ElevatorId = _elevatorOrchestrator.GetElevatorsIdleAtFloor().First(elevator => elevator.Floor == userCall.StartFloor).Id,
           });
        if (matches.Any())
        {
            var match = matches.First();
            _openInputPanel = true;
            _openInputPanelElevatorId = match.ElevatorId;
            _openInputPanelUserId = match.Call.Id;
        }
    }
    void CheckKeyboardInput()
    {
        if (_keyboardInput.KeyboardKeyDown(out List<ConsoleKey> keys))
        {
            _keyPressCoolDown = 5;
            foreach (var key in keys)
            {
                if (KeyboardInput.ConvertConsoleKeyToInt(key, out int floor))
                {
                    if (floor == -1)
                    {
                        return;
                    }
                    if (_openInputPanel)
                    {
                        ActivateElevatorInputPanel(floor);
                        return;
                    }
                    ActivateFloorCallPanel(floor);
                    return;
                } else if (KeyboardInput.ConvertConsoleKeyToDirection(key, out UserCall.Direction direction) && _selectedFloor != -1)
                {
                    ActivateFloorCall(direction);
                    return;
                }
            }
        }
    }
    void SetupView()
    {
        _viewController.SetOpenFloorInput(_selectedFloor);
        _viewController.SetInfoData(_elevatorOrchestrator.GetElevatorDataPoints(), _elevatorOrchestrator.GetFloorDataPoints());
        _viewController.SetElevatorPositions(_elevatorOrchestrator.GetElevatorPositions());
        if (_openInputPanel)
        {
            _viewController.SetInputPanel(_elevatorOrchestrator.GetElevatorControllerById(_openInputPanelElevatorId).ReturnData().InputPanel);
        }
    }
    void ClearPanels()
    {
        _openInputPanel = false;
        _openInputPanelElevatorId = -1;
        _openInputPanelUserId = -1;
    }
    void ActivateElevatorInputPanel(int floor)
    {
        if (_elevatorOrchestrator.CallElevatorPanelInput(floor, _openInputPanelElevatorId)) 
        {
            var call = _userCalls.First(userCall => userCall.Id == _openInputPanelUserId);
            call.EndFloor = floor;
            call.InElevatorId = _openInputPanelElevatorId;
            call.State = UserCall.UserCallState.Travelling;
            ClearPanels();
        }
    }
    void ActivateFloorCallPanel(int floor)
    {
        if (floor >= _constants.FloorCount - 1)
        {
            _selectedFloor = -1;
            return;
        }
        _selectedFloor = floor;
    }
    void ActivateFloorCall(UserCall.Direction direction)
    {
        var result = _elevatorOrchestrator.CallElevator(_selectedFloor, direction);
        if (result)
        {
            _userCalls.Add(new UserCall(_selectedFloor, direction));
        }
        _selectedFloor = -1;
    }
    void Tick()
    {
        _elevatorOrchestrator.Tick();
        if (_openInputPanel)
        {
            bool[] inputPanel = _elevatorOrchestrator.GetElevatorData(_openInputPanelElevatorId).InputPanel;
            _viewController.SetInputPanel(inputPanel);
        }
    }
    public MasterState()
    {
        _keyboardInput = new KeyboardInput();
        _elevatorOrchestrator = new ElevatorOrchestrator();
        _viewController = new ViewController();
    }
}
