namespace MontyHallProblem;

public class Game
{
    private readonly int _winningDoor;
    private readonly List<int> _doors = [1, 2, 3];
    private int _contestantDoorChoice;

    public Game(int winningDoor)
    {
        if (winningDoor is < 1 or > 3)
        {
            throw new ArgumentException("Winning door must be 1, 2, or 3.");
        }

        _winningDoor = winningDoor;
    }

    public bool ContestantWon => _contestantDoorChoice == _winningDoor;

    public void ContestantChooseDoor(int contestantDoorChoice)
    {
        _contestantDoorChoice = contestantDoorChoice;
    }

    public int HostRevealOneLosingDoor()
    {
        int[] losingDoors = _doors.Except([_winningDoor, _contestantDoorChoice]).ToArray();

        int losingDoor = losingDoors[new Random().Next(0, losingDoors.Length)];

        _doors.Remove(losingDoor);

        return losingDoor;
    }

    public void ContestantKeepOrSwitchDoor(KeepOrSwitch keepOrSwitch)
    {
        if (keepOrSwitch == KeepOrSwitch.Switch)
        {
            _contestantDoorChoice = _doors.Except([_contestantDoorChoice]).Single();
        }
    }

    public int HostRevealWinningDoor()
    {
        return _winningDoor;
    }
}