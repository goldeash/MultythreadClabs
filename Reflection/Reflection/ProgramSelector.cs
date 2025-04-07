using System;

namespace ReflectionLab1
{
    public class ProgramSelector
    {
        public const string FirstTaskChoice = "1";
        public const string SecondTaskChoice = "2";
        public const string FirstTaskDescription = " - Method invocation via reflection (Task 1)";
        public const string SecondTaskDescription = " - Assembly analysis via reflection (Task 2)";
        public const string ChoicePrompt = "Your choice: ";
        public const string InvalidChoiceMessage = "Invalid choice";

        public static void Main(string[] args)
        {
            Console.WriteLine("Select task:");
            Console.WriteLine(FirstTaskChoice + FirstTaskDescription);
            Console.WriteLine(SecondTaskChoice + SecondTaskDescription);
            Console.Write(ChoicePrompt);

            string choice = Console.ReadLine();

            switch (choice)
            {
                case FirstTaskChoice:
                    ReflectionInvokator.Program.Run();
                    break;
                case SecondTaskChoice:
                    ReflectionLibrary.Program.Run();
                    break;
                default:
                    Console.WriteLine(InvalidChoiceMessage);
                    break;
            }
        }
    }
}