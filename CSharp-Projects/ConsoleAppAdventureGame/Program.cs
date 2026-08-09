using ConsoleAppAdventureGame;

StoryNode stranded = new("Stranded")
{
    Text = ["It seems you failed to account for the [yellow italic]Earth being at different points in its orbit over time."]
};

StoryNode destroy = new("Destroy")
{
    Text = ["The device collapses, compressing all of time and space along with it."]
};

StoryNode start = new("Start")
{
    Text =
    [
        "Your time machine is ready to go.",
        "Do you dare turn it on?"
    ],

    Choices =
    [
        new Choice("Turn it on.")
        {
            WhenChosen = ["You are now adrift in space without a spacesuit."],
            NextNodeId = stranded.Id
        },
        new Choice("Destroy it!")
        {
            WhenChosen = ["You smash it to pieces!"],
            NextNodeId = destroy.Id,
        }
    ]
};

Adventure adventure = new([start, stranded, destroy]);

SimpleConsoleRenderer renderer = new();
SimpleConsoleRenderer.Render(adventure.CurrentNode!);
Choice choice = SimpleConsoleRenderer.GetChoice(adventure.CurrentNode!);
SimpleConsoleRenderer.RenderChoiceAction(choice);
