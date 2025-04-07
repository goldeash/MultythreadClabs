using System;
using System.Reflection;

namespace ReflectionLab1.ReflectionLibrary
{
    public class Program
    {
        public const string ClassLibraryPath = @"..\..\..\..\ClassLibrary\bin\Debug\net8.0\ClassLibrary.dll";
        public const string CreateMethodName = "Create";
        public const string PrintObjectMethodName = "PrintObject";
        public const string ProgramClassName = "Program";
        public const string ClassInfoHeader = "=== Task 2: Class members info ===";
        public const string ObjectCreationHeader = "=== Task 3: Object creation and info ===";
        public const string ClassFormat = "Class: {0}";
        public const string PropertyFormat = "  Property: {0} ({1})";
        public const string FieldFormat = "  Field: {0} ({1})";
        public const string MethodFormat = "  Method: {0} ({1})";
        public const string WorkingWithClassFormat = "Working with class: {0}";
        public const string CreateMethodNotFound = "  Method 'Create' not found.";
        public const string InstanceCreatedMessage = "  Instance created.";
        public const string PrintMethodNotFound = "  Method 'PrintObject' not found.";
        public const string ErrorMessageFormat = "Error: {0}";

        public static void Run()
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(ClassLibraryPath);
                Type[] types = assembly.GetTypes();

                Console.WriteLine(ClassInfoHeader);
                foreach (Type type in types)
                {
                    if (type.IsEnum || type.Name == ProgramClassName) continue;

                    Console.WriteLine(string.Format(ClassFormat, type.Name));

                    foreach (PropertyInfo property in type.GetProperties())
                    {
                        Console.WriteLine(string.Format(PropertyFormat, property.Name, property.PropertyType.Name));
                    }

                    foreach (FieldInfo field in type.GetFields())
                    {
                        Console.WriteLine(string.Format(FieldFormat, field.Name, field.FieldType.Name));
                    }

                    foreach (MethodInfo method in type.GetMethods())
                    {
                        if (method.DeclaringType == type && !method.IsSpecialName)
                        {
                            Console.WriteLine(string.Format(MethodFormat, method.Name, method.ReturnType.Name));
                        }
                    }

                    Console.WriteLine();
                }

                Console.WriteLine(ObjectCreationHeader);
                foreach (Type type in types)
                {
                    if (type.IsEnum || type.Name == ProgramClassName) continue;

                    Console.WriteLine(string.Format(WorkingWithClassFormat, type.Name));

                    MethodInfo createMethod = type.GetMethod(CreateMethodName);
                    if (createMethod == null)
                    {
                        Console.WriteLine(CreateMethodNotFound);
                        continue;
                    }

                    object[] createParams = new object[createMethod.GetParameters().Length];
                    for (int i = 0; i < createParams.Length; i++)
                    {
                        ParameterInfo param = createMethod.GetParameters()[i];
                        if (param.ParameterType == typeof(string))
                            createParams[i] = "TestString";
                        else if (param.ParameterType == typeof(int))
                            createParams[i] = 123;
                        else if (param.ParameterType.IsEnum)
                            createParams[i] = Enum.GetValues(param.ParameterType).GetValue(0);
                    }

                    object instance = createMethod.Invoke(null, createParams);
                    Console.WriteLine(InstanceCreatedMessage);

                    MethodInfo printMethod = type.GetMethod(PrintObjectMethodName);
                    if (printMethod == null)
                    {
                        Console.WriteLine(PrintMethodNotFound);
                        continue;
                    }

                    printMethod.Invoke(instance, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(ErrorMessageFormat, ex.Message));
            }
        }
    }
}