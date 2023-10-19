using ElevatorSystem.Src.Graphics;

namespace ElevatorSystem.Src.GuiObjects;

public class Elevator : GuiObject
{
    public const int FORCE_TO_MOVE = 50;
    public const int DISTANCE_TO_START_DECELARATE = 30;
    public const int MAX_FORCE = 200;
    int _force = 0;
    int _forceIncrement = 0;
    readonly Queue<ElevatorInstruction> _instructionQueue = new();
    ElevatorInstruction? _instructionAtHand;
    public void ReceiveInstruction(ElevatorInstruction instruction) => _instructionQueue.Enqueue(instruction);
    public void Move()
    {
        if (!_instructionQueue.Any())
        {
            return;
        }
        if (_instructionAtHand is null || _instructionAtHand.Completed)
        {
            _instructionAtHand = _instructionQueue.Dequeue();
            return;
        }

        int distanceFromGoal = Math.Abs(_instructionAtHand.GoToRow - Row);
        if (_forceIncrement < MAX_FORCE && distanceFromGoal > DISTANCE_TO_START_DECELARATE)
        {
            Accelerate();
        }
        if (distanceFromGoal <= 15 && _forceIncrement > 50)
        {
            Decelerate();
        }

        _force += _forceIncrement;

        if (_force >= FORCE_TO_MOVE)
        {
            var amount = _force / FORCE_TO_MOVE;
            Row += _instructionAtHand.GoToRow > Row ? amount : 0 - amount;
            _force = 0;
        }
        _instructionAtHand.Completed = Row == _instructionAtHand.GoToRow;
    }
    void Accelerate() => _forceIncrement += 10;
    void Decelerate() => _forceIncrement -= 20;

    public void BrutalForceMove(int row, int col)
    {
        Row = row;
        Column = col;
    }
    public Elevator(Graphic graphic) : base(graphic)
    {
    }
}
