// A program to load and save stories to storage.

StoryManager storyManager = new();
bool exit;
do
{
    exit = storyManager.Run();
} while (!exit);
