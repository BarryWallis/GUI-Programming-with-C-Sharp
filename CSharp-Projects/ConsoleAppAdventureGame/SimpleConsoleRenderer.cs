using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppAdventureGame;

public class SimpleConsoleRenderer
{
    public static void Render(StoryNode node)
    {
        foreach (string line in node.Text)
        {
            Console.WriteLine(line);
        }
    }

    public static Choice GetChoice(StoryNode node)
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

    public static void RenderChoiceAction(Choice choice)
    {
        foreach (string line in choice.WhenChosen)
        {
            Console.WriteLine(line);
        }
    }
}
