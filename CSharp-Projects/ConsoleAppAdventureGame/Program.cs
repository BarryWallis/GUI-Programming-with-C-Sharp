using ConsoleAppAdventureGame;

StoryNode floating = new("Floating")
{
    Text = ["You spend the rest of your short life floating in space."],
};

StoryNode suffocated = new("Suffocated")
{
    Text = ["You suffocate and die."],
};

StoryNode stranded = new("Stranded")
{
    Text = ["It seems you failed to account for the Earth being at different points in its orbit over time."],
    Choices =
    [
        new Choice("Put on a space suit.")
        {
            WhenChosen = ["You put on a space suit and float in space."],
            NextNodeId = floating.Id
        },
        new Choice("Float unencumbered")
        {
            WhenChosen = ["You float unencumbered in space."],
            NextNodeId = suffocated.Id
        }
    ]
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

Adventure adventure = new([start, stranded, destroy, floating, suffocated]);
ConsoleAdventureRenderer renderer = new();
adventure.Run(renderer);
