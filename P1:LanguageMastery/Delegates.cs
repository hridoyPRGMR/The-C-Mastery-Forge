using System.Diagnostics;

namespace P1.Deligates
{   

    
    /* Delegates are one of the most powerful (and misunderstood) features in C# — they’re the foundation of events, 
    callbacks, LINQ, and async programming.

    🎯 What Are Delegates?
    A delegate is a type-safe function pointer — it holds a reference to a method and can call it later. */


    public class Delegates
    {
        public delegate int MathOperation(int x, int y);
        public static int Add(int a, int b) => a + b;
        public static int Sub(int a, int b) => a > b ? a - b : b - a;


        public delegate void Action();

        public static void LogStartTime()
        {
            Console.WriteLine($"Statring at: {DateTime.Now}");
        }

        public static void Delay()
        {
            // Pause the program for 3 seconds (3000 milliseconds)
            Thread.Sleep(3000);
        }

        public static void LogFinished()
        {
            Console.WriteLine($"Finished at: {DateTime.Now}");
        }


        public static int Calculate(MathOperation operation, int x, int y)
        {
            return operation(x, y);
        }

        public delegate bool Filter(int value);

        public static List<int> FilterList(List<int> numbers, Filter condition)
        {
            List<int> newList = [];
            foreach (int num in numbers)
            {
                if (condition(num)) newList.Add(num);
            }
            return newList;
        }


        public static bool Odd(int num)
        {
            return num % 2 != 0;
        }

        public static bool Even(int num)
        {
            return num % 2 == 0;
        }

        public delegate bool Comp(int a, int b);


        public static bool IsAscending(int a, int b)
        {
            return a < b;
        }

        public static bool IsDescending(int a, int b)
        {
            return a > b;
        }

        public static void SortList(List<int> nums, Comp com)
        {
            int n = nums.Capacity;
            for (int i = 0; i < n; i++)
            {
                int mn_idx = i;

                for (int j = i + 1; j < n; j++)
                {
                    if (com(nums[j], nums[mn_idx]))
                    {
                        mn_idx = j;
                    }
                }

                int temp = nums[i];
                nums[i] = nums[mn_idx];
                nums[mn_idx] = temp;
            }
        }

        public static List<string> ProcessData(List<string> strs, Func<string, string> del)
        {
            // Select applies the delegate to each item and returns a new sequence
            return strs.Select(del).ToList();
        }

        public Delegates()
        {
            MathOperation op = Add;

            //multicast delegate
            op += Sub;
            // both the Add and the Sub methods, which will be executed sequentially.
            /* Return Value Rule: When a multicast delegate that has a return value is invoked, 
            it executes all the methods in its list, but the delegate itself only returns the value of the last method called in the chain. */
            int result = op(10, 20);  //result 10/sub
            Console.WriteLine(result);

            /*  Multicast Delegates and Chaining Actions
             This pattern is extremely useful for:
             Notifications and Logging: A common scenario is to have a delegate that logs a sequence of events.
             Workflow Automation: You can build a chain of methods that represent a business process.
             "Hooks": You can have a delegate that is executed before or after a main function runs, allowing for custom pre- or post-processing. */
            Action a = LogStartTime;
            a += Delay;
            a += LogFinished;
            a();

            /* Customizing Algorithms (The Strategy Pattern)
            This use case directly relates to your MathOperation delegate.
            You have a single delegate type that can represent different operations (Add or Sub). 
            This allows you to write a single, generic method that can perform different actions depending on the delegate it receives. */
            int sum = Calculate(Add, 10, 7);
            int sub = Calculate(Sub, 8, 13);
            Console.WriteLine($"Sub: {sub}, sum: {sum}");

            // Anonymous Methods & Lambdas
            MathOperation multiply = delegate (int x, int y) { return x * y; };
            MathOperation divide = (x, y) => x / y;

            Console.WriteLine($"Multiply: {multiply(3, 6)}");
            Console.WriteLine($"Division of 6/3: {divide(6, 3)}");

            /*  Built-in Delegates: Action, Func, Predicate
             Action<T> → method that returns void
             Func<T, TResult> → method that returns something
             Predicate<T> → returns bool */

            Action<string> greet = name => Console.WriteLine($"Hello, {name}");
            greet("Shaon");

            Func<int, int, double> summation = (a, b) => a + b;
            Console.WriteLine($"summation: {summation(10, 20)}");

            Predicate<int> isEven = x => x % 2 == 0;
            Console.WriteLine($"IsEven: {isEven(21)}");

            /* Events and Event Handling
            This is arguably the most important use case for delegates in C#. Events are built on delegates. 
            When you click a button in a UI, the button doesn't know what code should run—it just calls a delegate. 
            Your code "subscribes" to that event by providing a method to be called when the event occurs.
            C#
            // This is the delegate type for an event
            public event EventHandler MyEvent; 
            // ...later, when the event happens...
            MyEvent?.Invoke(this, EventArgs.Empty);
            The EventHandler is a delegate, and MyEvent is the event instance. Anyone can add their methods to this event using +=. */

            /* Asynchronous and Parallel Programming
            Modern C# uses delegates extensively for asynchronous operations. 
            Methods like Task.Run() take a delegate (often in the form of a lambda expression) to execute a piece of code on a separate thread.
            C#
            // Runs the code inside the lambda expression on a background thread
            Task.Run(() => {
                Console.WriteLine("This code is running in parallel!");
            });
            This allows your application to perform long-running tasks without freezing the user interface. */

            /*  LINQ and Lambda Expressions
             Delegates are the foundation of Language Integrated Query (LINQ). When you write a LINQ query like this:*/
            int[] numbers = [1, 2, 3, 4, 5, 6];
            var evenNumbers = numbers.Where(n => n % 2 == 0);
            /*C#
            The Where method actually takes a delegate (Func<int, bool>) as a parameter. 
            The expression n => n % 2 == 0 is a lambda expression, which is essentially 
            a compact way of defining an anonymous method that can be assigned to a delegate.
            */

            //Practice
            List<int> arr = [1, 2, 3, 4, 5, 6, 7, 8];
            var evenList = FilterList(arr, Even);
            Console.WriteLine($"EvenList: {evenList.Count}");

            SortList(arr, IsDescending);

            // arr.ForEach(n =>
            // {
            //     Console.Write(n + " ");
            // });   


            List<string> names = new() { "bob", "alice", "charlie" };

            Func<string, string> capitalize = name => char.ToUpper(name[0]) + name[1..];
            List<string> capitalizedNames = ProcessData(names, capitalize);
            capitalizedNames.ForEach(name => Console.Write(name + " "));
            Console.WriteLine();

            Func<string, string> addSuffix = name => name + "!";
            List<string> finalizedNames = ProcessData(names, addSuffix);
            finalizedNames.ForEach(name => Console.Write(name + " "));
            Console.WriteLine();

        }
    }
}