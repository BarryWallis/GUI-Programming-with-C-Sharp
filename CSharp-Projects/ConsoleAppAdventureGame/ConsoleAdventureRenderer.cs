namespace ConsoleAppAdventureGame;

/// <summary>
/// Renders story content and choices to the console.
/// </summary>
public class ConsoleAdventureRenderer : IAdventureRenderer
{
    /// <summary>
    /// Displays the text associated with the specified story node to the console.
    /// </summary>
    /// <param name="node">The story node whose text should be rendered.</param>
    public void Render(StoryNode node)
    {
        foreach (string line in node.Text)
        {
            Console.WriteLine(line);
        }
    }

    /// <summary>
    /// Prompts the user to select one of the choices available from the specified story node.
    /// </summary>
    /// <param name="node">The story node that contains the available choices.</param>
    /// <returns>The selected choice.</returns>
    public Choice GetChoice(StoryNode node)
    {
        Console.WriteLine("What do you want to do?");
        Console.WriteLine();

        Choice? choice = null;
        do
        {
            for (int i = 0; i < node.Choices.Length; i++)
            {
                string text = node.Choices[i].Text;
                Console.WriteLine($"{i + 1}. {text}");
            }

            Console.WriteLine();
            Console.Write("Enter your choice: ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int index) && index > 0 && index <= node.Choices.Length)
            {
                choice = node.Choices[index - 1];
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        } while (choice is null);

        Console.WriteLine($"You chose: {choice.Text}");
        return choice;
    }

    /// <summary>
    /// Displays the outcome text associated with the specified choice to the console.
    /// </summary>
    /// <param name="choice">The choice whose follow-up action should be rendered.</param>
    public void RenderChoiceAction(Choice choice)
    {
        foreach (string line in choice.WhenChosen)
        {
            Console.WriteLine(line);
        }
    }
}
