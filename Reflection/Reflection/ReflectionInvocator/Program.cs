using System;
using System.Reflection;

namespace ReflectionLab1.ReflectionInvokator
{
    public class Program
    {
        public const string Namespace = "TestClasses";
        public const string ClassNamePrompt = "Enter class name: ";
        public const string MethodNamePrompt = "Enter method name: ";
        public const string ArgumentPrompt = "Enter argument {0} ({1}): ";
        public const string ClassNotFoundError = "Error: Class not found.";
        public const string MethodNotFoundError = "Error: Method not found.";
        public const string ArgumentConversionError = "Error: Cannot convert argument {0} to {1}.";
        public const string ResultOutput = "Result: {0}";

        public static void Run()
        {
            Console.Write(ClassNamePrompt);
            string className = Console.ReadLine();
            Type type = Type.GetType($"{Namespace}.{className}");

            if (type == null)
            {
                Console.WriteLine(ClassNotFoundError);
                return;
            }

            Console.Write(MethodNamePrompt);
            string methodName = Console.ReadLine();
            MethodInfo method = type.GetMethod(methodName);

            if (method == null)
            {
                Console.WriteLine(MethodNotFoundError);
                return;
            }

            ParameterInfo[] parameters = method.GetParameters();
            object[] convertedArgs = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Console.Write(string.Format(ArgumentPrompt, i + 1, parameters[i].ParameterType.Name));
                string inputArg = Console.ReadLine();
                try
                {
                    convertedArgs[i] = Convert.ChangeType(inputArg, parameters[i].ParameterType);
                }
                catch
                {
                    Console.WriteLine(string.Format(ArgumentConversionError, i + 1, parameters[i].ParameterType.Name));
                    return;
                }
            }

            object instance = Activator.CreateInstance(type);
            object result = method.Invoke(instance, convertedArgs);

            if (method.ReturnType != typeof(void))
            {
                Console.WriteLine(string.Format(ResultOutput, result));
            }
        }
    }
}