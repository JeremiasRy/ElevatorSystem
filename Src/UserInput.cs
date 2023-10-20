using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSystem.Src;

public class UserInput
{
    public int Id { get; set; }
    public int From { get; set; }
    public int To { get; set; } = -1;

    public static RequestState State(int value)
    {
        if (value < 0)
        {
            return (RequestState)value;
        }
        return RequestState.OnGoing;
    }

    public enum RequestState
    {
        NotInitialized = -1,
        Complete = -2,
        OnGoing = 2
    }
}
