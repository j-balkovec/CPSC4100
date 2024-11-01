/// <file> Knapsack.cs </file>
/// <author> Jakob Balkovec (CPSC 4100) </author>
/// <instructor> A. Dingle (CPSC 4100) </instructor>
/// 
/// <date> Tue 29th Oct </date>
/// 
/// <version>
/// Revision History
/// - [1.0] Recursive Solution Design, Debugging
/// - [2.0] DP Solution Implementation, Initial Documentation
/// - [3.0] Performance Measurement and Heuristic Implementation (TBD)
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


/// START TODO
/// * Fix capacity problem
/// * Recursive solution extra slow sometimes
/// * Remove Print functions
/// * Add Heursitics
/// * Analyze data in R
/// END TODO

using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace KnapsackProject
{
    /// <summary>
    /// A Logger class to facilitate logging of information, errors, execution times, and solutions.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// The file path where the log is stored.
        /// </summary>
        private readonly string LogFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// Creates the log file if it doesn't exist and adds a timestamp to the log.
        /// </summary>
        public Logger()
        {
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                                             "CPSC4100", "FinalProject", "FinalProject4100");

            LogFilePath = Path.Combine(directoryPath, "FPexecution_log.log");


            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine("*** [TIME STAMP]: " + DateTime.Now.ToString("yyyy-MM-dd") + "} ***\n\n");
            }
        }

        /// <summary>
        /// Logs an informational message to the log file.
        /// </summary>
        /// <param name="message">The information message to log.</param>
        public void Info(string message)
        {
            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [INFO]: {message}");
            }
        }

        /// <summary>
        /// Logs an error message to the log file.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public void Error(string message)
        {
            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [ERROR]: {message}");
            }
        }

        /// <summary>
        /// Logs the execution time to the log file.
        /// </summary>
        /// <param name="time">The execution time in milliseconds.</param>
        public void Time(decimal time)
        {
            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [TIME]: {time}");
            }
        }

        /// <summary>
        /// Logs the solution to the log file.
        /// </summary>
        /// <param name="solution">The solution value to log.</param>
        public void Solution(uint solution)
        {
            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [SOLUTION]: {solution}");
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
            using (StreamWriter writer = new StreamWriter(LogFilePath, false)) { }
        }
    }

    //DOCUMENT
    public static class FilePaths
    {
        public static readonly IReadOnlyDictionary<string, string> Paths = new Dictionary<string, string>
    {
        { "XSmallItems", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemsxsmalldata_01_11_2024.json" },
        { "SmallItems", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemssmalldata_01_11_2024.json" },
        { "MediumItems", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemsmediumdata_01_11_2024.json" },
        { "LargeItems", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemslargedata_01_11_2024.json" },
        { "XLargeItems", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemsxlargedata_01_11_2024.json" },
        { "Capacity", "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/capacity/capacitydata_01_11_2024.json"}
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

    class Knapsack
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
        /// Dynammic programming solution using memoization O(n * W); W = capacity, n = number of items
        /// </summary>
        /// <param name="capacity">The maximum weight capacity of the knapsack.</param>
        /// <param name="items">The list of items to choose from.</param>
        /// <returns>The maximum value that can be obtained within the given capacity.</returns>
        public static uint KnapsackMemo(List<KnapsackItem> items, uint capacity)
        {
            return SolveKnapsackHelper(items, capacity, 0);
        }

        /// <summary>
        /// A dictionary to store previously calculated results for memoization.
        /// </summary>
        private static readonly Dictionary<(uint, uint), uint> memo = new();

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
            if (memo.ContainsKey(key)) { return memo[key]; }

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
        /// The entry point of the application. Initializes the logger, measures the execution time of the 
        /// knapsack algorithm, logs the results.
        /// </summary>
        public static void Main()
        {
            var logger = new Logger();
            Stopwatch stopwatch = new Stopwatch();

            const string? ItemsFile = "XSmallItems";
            const string? CapacityFile = "Capacity";
            
            logger.Info("COLLECTING DATA...\n");

            List<KnapsackItem>? items = ReadItemsFromJsonFile<KnapsackItem>(FilePaths.Paths[ItemsFile]);
            List<CapacityItem>? capacities = ReadItemsFromJsonFile<CapacityItem>(FilePaths.Paths[CapacityFile]);
            Knapsack KnapsackSolver = new();
            CapacityItem? RandomCapacity = YieldRandomCapacity(capacities);
            uint UnpackedCapacity = RandomCapacity.Capacity;


            // ****** RECURSIVE ******
            logger.Info($"Method: Recursive");
            stopwatch.Reset();
            stopwatch.Start();

            uint SolutionRecursive = KnapsackRecursive(items, UnpackedCapacity);

            stopwatch.Stop();

            decimal ExecTimeRecursive = (decimal)stopwatch.Elapsed.TotalMilliseconds;

            logger.Time(ExecTimeRecursive);
            logger.Info($"FOR: [ITEMS]: {ItemsFile}, [CAPACITY]: {CapacityFile}");
            logger.Solution(SolutionRecursive);
            // ****** RECURSIVE ******

            // ****** DP ******
            logger.Info("\n");
            logger.Info($"Method: DP");
            stopwatch.Reset();
            stopwatch.Start();

            uint SolutionDP = KnapsackDP(items, UnpackedCapacity);

            stopwatch.Stop();

            decimal ExecTimeDP = (decimal)stopwatch.Elapsed.TotalMilliseconds;

            logger.Time(ExecTimeDP);
            logger.Info($"FOR: [ITEMS]: {ItemsFile}, [CAPACITY]: {CapacityFile}");
            logger.Solution(SolutionDP);
            // ****** DP ******

            // ****** MEMO ******
            logger.Info("\n");
            logger.Info($"Method: Memo");
            stopwatch.Reset();
            stopwatch.Start();

            uint SolutionMemo = KnapsackMemo(items, UnpackedCapacity);

            stopwatch.Stop();

            decimal ExecTimeMemo = (decimal)stopwatch.Elapsed.TotalMilliseconds;

            logger.Time(ExecTimeMemo);
            logger.Info($"FOR: [ITEMS]: {ItemsFile}, [CAPACITY]: {CapacityFile}");
            logger.Solution(SolutionMemo);
            // ****** MEMO ******

        }
    }

}




