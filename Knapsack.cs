/// <file> Knapsack.cs </file>
/// <author> Jakob Balkovec (CPSC 4100) </author>
/// <instructor> A. Dingle (CPSC 4100) </instructor>
/// 
/// <date> Tue 29th Oct </date>
/// 
/// <version>
/// Revision History
/// - [1.0] Recursive Solution Design, Debugging
/// - [1.1] DP Solution Implementation, Initial Documentation
/// - [1.2] Performance Measurement and Heuristic Implementation
/// - [1.3] Bug fixes, performance impovements
/// - [1.4] Prettified the output
/// - [1.5] Unit Tests
/// </version>
/// 
/// <summary>
/// - This file implements both recursive and dynamic programming (DP) solutions for the 0/1 knapsack problem. 
///   It provides two methods: KnapsackRecursive for a recursive solution and KnapsackDP for an optimized DP approach.
///   Various heuristics to approximate solutions are included, and execution times for each method are recorded 
///   separately and exported to a file for performance analysis.
/// </summary>
///
/// <note>
/// - This implementation uses a basic recursive solution with exponential time complexity (O(2^n)) and 
///   a DP solution with pseudo-polynomial time complexity (O(n x W)), where n is the number of items and 
///   W is the knapsack capacity.
/// </note>
///
/// <dependencies>
/// - System (for Console output)
/// </dependencies>

using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Drawing;

namespace KnapsackProject
{
    /// <summary>
    /// A Logger class to facilitate logging of information, errors, execution times, and solutions.
    /// </summary>
    ///
    /// <param>
    /// "E" - Execution log
    /// "O" - Log everything else
    /// </param>
    public class Logger
    {
        /// <summary>
        /// The file path where the log is stored.
        /// </summary>
        private readonly string Execution_LogFilePath;
        private readonly string Other_LogFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// Creates the log file if it doesn't exist and adds a timestamp to the log.
        /// </summary>
        public Logger()
        {
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                                             "CPSC4100", "FinalProject", "FinalProject4100");

            Execution_LogFilePath = Path.Combine(directoryPath, "FPexecution_log.log");
            Other_LogFilePath = Path.Combine(directoryPath, "FP_log.log");

            using (StreamWriter writer = new StreamWriter(Execution_LogFilePath, true))
            {
                writer.WriteLine("*** [TIME STAMP]: " + DateTime.Now.ToString("yyyy-MM-dd") + "} ***\n\n");
            }

            using (StreamWriter writer = new StreamWriter(Other_LogFilePath, true))
            {
                writer.WriteLine("*** [TIME STAMP]: " + DateTime.Now.ToString("yyyy-MM-dd") + "} ***\n\n");
            }
        }

        /// <summary>
        /// Logs an informational message to the log file.
        /// </summary>
        /// <param name="message">The information message to log.</param>
        public void Info(string message, string type)
        {
            if (type == "E")
            {
                using StreamWriter writer = new StreamWriter(Execution_LogFilePath, true);
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [INFO]: {message}");
            }
            else if (type == "O")
            {
                using StreamWriter writer = new StreamWriter(Other_LogFilePath, true);
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [INFO]: {message}");
            }
            else
            {
                throw new ArgumentException("[Wrong Log Type]");
            }
        }

        /// <summary>
        /// Logs an error message to the log file.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public void Error(string message, string type)
        {
            if (type == "E")
            {
                using StreamWriter writer = new StreamWriter(Execution_LogFilePath, true);
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [ERROR]: {message}");
            }
            else if (type == "O")
            {
                using StreamWriter writer = new StreamWriter(Other_LogFilePath, true);
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [ERROR]: {message}");
            }
            else
            {
                throw new ArgumentException("[Wrong Log Type]");
            }
        }

        /// <summary>
        /// Logs the execution time to the log file.
        /// </summary>
        /// <param name="time">The execution time in milliseconds.</param>
        public void Time(decimal time, string type)
        {
            if (type == "E")
            {
                using StreamWriter writer = new StreamWriter(Execution_LogFilePath, true);
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [TIME]: {time}");
            }
            else if (type == "O")
            {
                using StreamWriter writer = new StreamWriter(Other_LogFilePath, true);
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [TIME]: {time}");
            }
            else
            {
                throw new ArgumentException("[Wrong Log Type]");
            }

        }

        /// <summary>
        /// Logs the solution to the log file.
        /// </summary>
        /// <param name="solution">The solution value to log.</param>
        public void Solution(uint solution, string type)
        {
            if (type == "E")
            {
                using StreamWriter writer = new StreamWriter(Execution_LogFilePath, true);
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [SOLUTION]: {solution}\n");
            }
            else if (type == "O")
            {
                using StreamWriter writer = new StreamWriter(Other_LogFilePath, true);
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [SOLUTION]: {solution}\n");
            }
            else
            {
                throw new ArgumentException("[Wrong Log Type]");
            }
        }

        /// <summary>
        /// Clears the contents of the log file.
        /// This method overwrites the existing log file with an empty file,
        /// effectively removing all previous log entries.
        /// </summary>
        ///
        /// <remarks>
        /// *******                                                                *******
        /// ******* USE WITH EXTREME CAUTION >>> DON'T DELETE THE PREVIOUS RESULTS *******
        /// *******                                                                *******
        /// </remarks>
        public void ClearLog()
        {
            using StreamWriter writer = new(Execution_LogFilePath, false);
        }
    }

    /// <summary>
    /// Dictionaries used for file reading and some intermediate computations.
    /// </summary>
    public static class UtilityMaps
    {
        public static readonly IReadOnlyDictionary<string, string> Paths = new Dictionary<string, string>
        {
            { "XS", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemsxsmalldata_06_11_2024.json" },
            { "S", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemssmalldata_06_11_2024.json" },
            { "M", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemsmediumdata_06_11_2024.json" },
            { "L", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemslargedata_06_11_2024.json" },
            { "XL", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemsxlargedata_06_11_2024.json" },
            { "C", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/capacity/capacitydata_06_11_2024.json"}
        };

        public static readonly IReadOnlyDictionary<string, uint> Sizes = new Dictionary<string, uint>
        {
            { "X", 50 },
            { "S", 100 },
            { "M", 250 },
            { "L", 500 },
            { "XL", 1000 }
        };
    }

    /// <summary>
    /// Represents an item that can be included in the knapsack.
    /// </summary>
    public class KnapsackItem
    {
        ///<item>
        /// Weight - Gets the weight of the item
        /// Value - Gets the value of the item.
        /// ValuePerWeight - Yields a ratio(double) of Value/Weight
        ///</item>
        public uint Weight { get; }
        public uint Value { get; }

        /// <summary>
        /// Initializes a new instance of the KnapsackItem class.
        /// </summary>
        /// 
        /// <param name="weight">The weight of the item.</param>
        /// <param name="value">The value of the item.</param>
        public KnapsackItem(uint weight, uint value)
        {
            this.Weight = weight;
            this.Value = value;
        }
    }

    /// <summary>
    /// Represents an item with a specified capacity.
    /// </summary>
    public class CapacityItem
    {
        public uint Capacity { get; set; }
    }

    /// <summary>
    /// Holds the methods (DP, Recursive, Memo)
    /// </summary>
    public class Knapsack
    {

        /// <summary>
        /// Recurisve solution O(2^n)
        /// </summary>
        /// <param name="items">A list of <see cref="KnapsackItem"/> representing the items to choose from.</param>
        /// <param name="capacity">The maximum weight capacity of the knapsack.</param>
        /// <returns>The maximum value that can be obtained within the given weight capacity.</returns>
        public static uint KnapsackRecursive(List<KnapsackItem> items, uint capacity)
        {
            return KnapsackRecursiveHelper(items, capacity, items.Count);
        }

        /// <summary>
        /// Helper method for the <see cref="KnapsackRecursive"/> function to facilitate recursion.
        /// </summary>
        /// <param name="items">A list of <see cref="KnapsackItem"/> representing the items to choose from.</param>
        /// <param name="capacity">The remaining weight capacity of the knapsack.</param>
        /// <param name="n">The number of items currently considered.</param>
        /// <returns>The maximum value that can be obtained for the given remaining capacity and items.</returns>
        private static uint KnapsackRecursiveHelper(List<KnapsackItem> items, uint capacity, int n)
        {
            // Base case: no items or no capacity
            if (n == 0 || capacity == 0) { return 0; }

            if (items[n - 1].Weight > capacity)
            {
                return KnapsackRecursiveHelper(items, capacity, n - 1);
            }
            else
            {
                uint includeItem = items[n - 1].Value + KnapsackRecursiveHelper(items, capacity - items[n - 1].Weight, n - 1);
                uint excludeItem = KnapsackRecursiveHelper(items, capacity, n - 1);
                return Math.Max(includeItem, excludeItem);
            }
        }

        /// <summary>
        /// Helper method for solving the knapsack problem recursively with threading.
        /// </summary>
        /// <param name="items">A list of knapsack items, where each item has a weight and value.</param>
        /// <param name="capacity">The remaining weight capacity of the knapsack.</param>
        /// <param name="n">The number of items to consider from the list.</param>
        /// <returns>The maximum value that can be obtained by including or excluding the current item, recursively solving the subproblem.</returns>
        public static uint KnapsackRecursiveThreading(List<KnapsackItem> items, uint capacity)
        {
            return KnapscakRecursiveThreadingHelper(items, capacity, items.Count);
        }

        /// <summary>
        /// Solves the knapsack problem using recursion and threading.
        /// </summary>
        /// <param name="items">A list of knapsack items, where each item has a weight and value.</param>
        /// <param name="capacity">The maximum weight capacity of the knapsack.</param>
        /// <returns>The maximum value that can be obtained by filling the knapsack with the given items.</returns>
        private static uint KnapscakRecursiveThreadingHelper(List<KnapsackItem> items, uint capacity, int n)
        {
            // Base case: no items or no capacity
            if (n == 0 || capacity == 0) { return 0; }

            if (items[n - 1].Weight > capacity)
            {
                return KnapscakRecursiveThreadingHelper(items, capacity, n - 1);
            }
            else
            {
                var includeTask = Task.Run(() =>
                    items[n - 1].Value + KnapscakRecursiveThreadingHelper(items, capacity - items[n - 1].Weight, n - 1));

                var excludeTask = Task.Run(() =>
                    KnapscakRecursiveThreadingHelper(items, capacity, n - 1));

                Task.WaitAll(includeTask, excludeTask);

                return Math.Max(includeTask.Result, excludeTask.Result);
            }
        }

        /// <summary>
        /// Dynamic programming solution O(n * W); W = capacity, n = number of items
        /// </summary>
        /// 
        /// <param name="items">The list of items to choose from.</param>
        /// <param name="capacity">The maximum weight capacity of the knapsack.</param>
        /// 
        /// <remarks>
        /// This method implements a DP solution with O(n x W) time complexity, making it suitable for 
        /// larger inputs. The 2D array dp[i][w] stores maximum values for the first i items with 
        /// capacity W.
        /// </remarks>
        /// 
        /// <returns>
        /// - The maximum value that can be obtained within the given capacity.
        /// </returns>
        public static uint KnapsackDP(List<KnapsackItem> items, uint capacity)
        {
            uint n = (uint)items.Count;
            uint[,] dp = new uint[n + 1, capacity + 1];

            for (uint i = 0; i <= n; i++)
            {
                for (uint w = 0; w <= capacity; w++)
                {
                    if (i == 0 || w == 0)
                    {
                        dp[i, w] = 0;
                    }
                    else if (items[(int)(i - 1)].Weight <= w)
                    {
                        dp[i, w] = Math.Max(items[(int)(i - 1)].Value + dp[i - 1, w - items[(int)(i - 1)].Weight], dp[i - 1, w]);
                    }
                    else
                    {
                        dp[i, w] = dp[i - 1, w];
                    }
                }
            }
            return dp[n, capacity];
        }

        /// <summary>
        /// A dictionary to store previously calculated results for memoization.
        /// </summary>
        private static readonly Dictionary<(uint, uint), uint> memo = new();

        /// <summary>
        /// Dynammic programming solution using memoization O(n * W); W = capacity, n = number of items
        /// </summary>
        /// <param name="capacity">The maximum weight capacity of the knapsack.</param>
        /// <param name="items">The list of items to choose from.</param>
        /// <returns>The maximum value that can be obtained within the given capacity.</returns>
        public static uint KnapsackMemo(List<KnapsackItem> items, uint capacity)
        {
            memo.Clear(); //Clear cache
            return SolveKnapsackHelper(items, capacity, 0);
        }

        /// <summary>
        /// A helper method that uses recursion and memoization to find the maximum value for the given items.
        /// </summary>
        /// 
        /// <param name="items">The list of items to choose from.</param>
        /// <param name="remainingCapacity">The remaining weight capacity of the knapsack.</param>
        /// <param name="currentIndex">The current index of the item being considered.</param>
        /// 
        /// <returns>
        /// - The maximum value obtainable with the remaining capacity and items from the current index onwards.
        /// </returns>
        /// <remark>
        /// - Have to index the list with a cast to (int), since List<T> cannot be indexed with an unsigned intiger
        /// </remark>
        private static uint SolveKnapsackHelper(List<KnapsackItem> items, uint remainingCapacity, uint currentIndex)
        {
            if (currentIndex >= (uint)items.Count || remainingCapacity <= 0)
                return 0;

            var key = (remainingCapacity, currentIndex);

            if (memo.ContainsKey(key))
            {
                // DEBUG
                //Console.WriteLine($"Cache hit for key: {key}");
                return memo[key];
            }
            else
            {
                // DEBUG
                //Console.WriteLine($"Cache miss for key: {key}");
            }

            uint maxValueWithoutCurrent = SolveKnapsackHelper(items, remainingCapacity, currentIndex + 1);
            uint maxValueWithCurrent = 0;

            if (items[(int)currentIndex].Weight <= remainingCapacity)
            {
                maxValueWithCurrent = items[(int)currentIndex].Value +
                                      SolveKnapsackHelper(items, remainingCapacity - items[(int)currentIndex].Weight, currentIndex + 1);
            }

            // Cache
            uint result = Math.Max(maxValueWithoutCurrent, maxValueWithCurrent);
            memo[key] = result;
            return result;
        }

        /// <summary>
        /// Reads a list of items of type <typeparamref name="T"/> from a JSON file.
        /// </summary>
        /// 
        /// <typeparam name="T">
        /// * The type of items to read from the JSON file.
        /// </typeparam>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <returns>A list of items of type <typeparamref name="T"/> or null if an error occurs.</returns>
        /// <remarks>
        /// - Function does the error handling
        /// Expected JSON format:
        /// <list type="bullet">
        ///     <item><description>For KnapsackItem: { "Weight": value, "Value": value }</description></item>
        ///     <item><description>For CapacityItem: { "Capacity": value }</description></item>
        /// </list>
        /// </remarks>
        public static List<T>? ReadItemsFromJsonFile<T>(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                //Console.WriteLine(jsonContent);
                //Console.ReadLine();
                return JsonSerializer.Deserialize<List<T>>(jsonContent);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The file '{filePath}' was not found.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Error: Access to the file '{filePath}' is denied.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO Error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Prints the details of a list of Knapsack items to the console.
        /// </summary>
        /// <param name="items">A list of <see cref="KnapsackItem"/> objects to print. Can be null or empty.</param>
        /// <exception cref="ArgumentException">Thrown when the items list is null or empty.</exception>
        public static void PrintItems(List<KnapsackItem>? items)
        {
            try
            {
                if (items == null || items.Count == 0)
                {
                    throw new ArgumentException("** [NO DATA] **");
                }

                Console.WriteLine("Knapsack Items:");
                foreach (var item in items)
                {
                    Console.WriteLine($"Weight: {item.Weight}, Value: {item.Value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message); // No need to log error --> Testing Purposes Only!
            }

        }

        /// <summary>
        /// Prints the capacities from a list of <see cref="CapacityItem"/> objects to the console.
        /// </summary>
        /// <param name="capacities">A list of <see cref="CapacityItem"/> objects to print. Can be null or empty.</param>
        /// <exception cref="ArgumentException">Thrown when the capacities list is null or empty.</exception>
        public static void PrintCapacities(List<CapacityItem>? capacities)
        {
            try
            {
                if (capacities == null || capacities.Count == 0)
                {
                    throw new ArgumentException("** [NO DATA] **");
                }

                Console.WriteLine("Capacities:");
                foreach (var element in capacities)
                {
                    Console.WriteLine($"Capacity: {element.Capacity}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message); // No need to log error --> Testing Purposes Only!
            }

        }

        /// <summary>
        /// Picks a random <see cref="CapacityItem"/> from the provided list.
        /// </summary>
        /// <param name="capacities">A list of <see cref="CapacityItem"/> from which to select a random item.</param>
        /// <returns>A randomly selected <see cref="CapacityItem"/>, or null if the list is null or empty.</returns>
        public static CapacityItem? YieldRandomCapacity(List<CapacityItem>? capacities)
        {
            if (capacities == null || capacities.Count == 0) { return null; }

            Random random = new Random();
            int randomIndex = random.Next(capacities.Count);
            return capacities[randomIndex];
        }

        /// <summary>
        /// Collects data on the execution time of different knapsack algorithm methods (Recursive, DP, Memo)
        /// and logs the results using the specified <see cref="Logger"/>.
        /// </summary>
        /// <param name="logger">The logger instance to record execution time, method details, and solutions.</param>
        public static void CollectData(Logger logger)
        {

            Stopwatch stopwatch = new Stopwatch();
            const string CapacityFile = "C";
            uint testNumber = 1;

            var methods = new (string MethodName, Func<List<KnapsackItem>, uint, uint> Algorithm)[]
            {
                ("Recursive", KnapsackRecursive),
                //("Recursive + Threading", KnapsackRecursiveThreading),
                ("DP", KnapsackDP),
                ("Memo", KnapsackMemo)
            };

            foreach (var method in methods)
            {
                foreach (var itemKey in UtilityMaps.Paths.Keys.Where(k => k != CapacityFile))
                {
#pragma warning disable // Assume not null

                    List<KnapsackItem> items = ReadItemsFromJsonFile<KnapsackItem>(UtilityMaps.Paths[itemKey]);
                    List<CapacityItem> capacities = ReadItemsFromJsonFile<CapacityItem>(UtilityMaps.Paths[CapacityFile]);

                    CapacityItem randomCapacity = YieldRandomCapacity(capacities);
                    uint unpackedCapacity = randomCapacity.Capacity;
#pragma warning restore
                    if ((method.MethodName == "Recursive"
                        || method.MethodName == "Recursive + Threading") &&
                        (itemKey == "XL" ||
                         itemKey == "L" ||
                         itemKey == "M"))
                    {
                        LogResults(logger, testNumber, method.MethodName, itemKey, unpackedCapacity, execTime: -1, solution: 0);
                        testNumber++;
                        continue;
                    }

                    // Measure execution time
                    stopwatch.Reset();
                    stopwatch.Start();
                    uint solution = method.Algorithm(items, unpackedCapacity);
                    stopwatch.Stop();
                    decimal execTime = (decimal)stopwatch.Elapsed.TotalMilliseconds;

                    // Log results
                    LogResults(logger, testNumber, method.MethodName, itemKey, unpackedCapacity, execTime, solution);
                    testNumber++;
                }
            }
        }

        /// <summary>
        /// Logs the results of a knapsack algorithm test to the specified <see cref="Logger"/>.
        /// </summary>
        /// <param name="logger">The logger instance to record test details.</param>
        /// <param name="testNumber">The unique test number for identification.</param>
        /// <param name="method">The algorithm method name used in this test (e.g., "Recursive", "DP", "Memo").</param>
        /// <param name="itemKey">The item category or file key (e.g., "SmallItems", "LargeItems") used in the test.</param>
        /// <param name="capacity">The capacity value used in the knapsack problem for this test.</param>
        /// <param name="execTime">The execution time in milliseconds, or -1 if the test was skipped.</param>
        /// <param name="solution">The solution value returned by the algorithm, or 0 if the test was skipped.</param>
        private static void LogResults(Logger logger, uint testNumber, string method, string itemKey, uint capacity, decimal execTime, uint solution)
        {
            logger.Info($" *** [TEST #{testNumber}] *** \n", "E");
            logger.Info($"Method: {method}", "E");
            logger.Time(execTime, "E");
            logger.Info($"FOR: [ITEMS]: {itemKey}, [CAPACITY]: {capacity}", "E");
            logger.Solution(solution, "E");
            logger.Info("\n" + new string('-', 40) + "\n", "E");
        }

        // The entry point of the application. Initializes the logger, measures the execution time of the 
        // knapsack algorithm, logs the results.
        public static void Main()
        {
            var logger = new Logger();
            logger.ClearLog();

            const uint NumTests = 5;

            try
            {
                for (int i = 0; i < NumTests; i++)
                {
                    CollectData(logger);
                }
            }
            catch (Exception ex)
            {
                logger.Info($"Critical error in the main loop: {ex.Message}", "O");
            }
        }

    }

} // namespace: KnapsackProject


