namespace MontyHallProblem.Tests;

public class Tests
{
    [Test]
    [Arguments(-1)]
    [Arguments(0)]
    [Arguments(4)]
    public void Game_should_not_be_initialized_with_a_winning_door_outside_of_1_and_3(int winningDoor)
    {
        Assert.Throws<ArgumentException>(() => _ = new Game(winningDoor));
    }

    [Test]
    [MatrixDataSource]
    public async Task Host_should_reveal_a_losing_door_after_contestant_makes_first_choice(
        [Matrix(1, 2, 3)] int winningDoor,
        [Matrix(1, 2, 3)] int initialDoorChoice)
    {
        Game game = new Game(winningDoor);
        game.ContestantChooseDoor(initialDoorChoice);
        int losingDoor = game.HostRevealOneLosingDoor();
        await Assert.That(losingDoor).IsNotEqualTo(winningDoor);
        await Assert.That(losingDoor).IsNotEqualTo(initialDoorChoice);
        await Assert.That(losingDoor).IsEqualTo(1).Or.IsEqualTo(2).Or.IsEqualTo(3);
    }

    [Test]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task Host_should_reveal_winning_door(int winningDoor)
    {
        Game game = new Game(winningDoor);
        await Assert.That(game.HostRevealWinningDoor()).IsEqualTo(winningDoor);
    }

    [Test]
    [Arguments(1, 1, KeepOrSwitch.Keep, true)]
    [Arguments(1, 1, KeepOrSwitch.Switch, false)]
    [Arguments(2, 1, KeepOrSwitch.Keep, false)]
    [Arguments(2, 1, KeepOrSwitch.Switch, true)]
    public async Task Should_play_full_game_and_report_win_or_lose(int winningDoor, int initialDoorChoice, KeepOrSwitch keepOrSwitch, bool expectedOutcome)
    {
        Game game = new Game(winningDoor);
        game.ContestantChooseDoor(initialDoorChoice);
        game.HostRevealOneLosingDoor();
        game.ContestantKeepOrSwitchDoor(keepOrSwitch);
        game.HostRevealWinningDoor();
        await Assert.That(game.ContestantWon).IsEqualTo(expectedOutcome);
    }

    [Test]
    [Arguments(KeepOrSwitch.Keep)]
    [Arguments(KeepOrSwitch.Switch)]
    public void Simulate_many_games(KeepOrSwitch keepOrSwitch)
    {
        const int gameCount = 1000;
        int winCount = 0;

        Random random = new Random();

        for (int i = 1; i <= gameCount; i++)
        {
            Game game = new Game(random.Next(1, 4));
            game.ContestantChooseDoor(random.Next(1, 4));
            game.HostRevealOneLosingDoor();
            game.ContestantKeepOrSwitchDoor(keepOrSwitch);
            game.HostRevealWinningDoor();
            if (game.ContestantWon)
            {
                winCount++;
            }
        }

        var winPercentage = winCount / (double)gameCount;
        Console.WriteLine($"{keepOrSwitch} win percentage: {winPercentage:P}");
    }
}