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

using System;
using System.Diagnostics;
using System.IO;

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
                writer.WriteLine("*** [TIME STAMP]: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "} ***\n\n");
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
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [INFO]: {message}\n");
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
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [ERROR]: {message}\n");
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
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [TIME]: {time}\n");
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
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - [SOLUTION]: {solution}\n");
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

    class Knapsack
    {
        /// <summary>
        /// Recursive Solution O(2^n)
        /// </summary>
        /// 
        /// <param name="items">The list of items to choose from.</param>
        /// <param name="capacity">The maximum weight capacity of the knapsack.</param>
        /// <param name="n">The number of items available.</param>
        /// 
        /// <returns>
        /// - The maximum value that can be obtained within the given capacity.
        /// </returns>
        /// 
        /// <invariant>
        /// - Weights & Values will contain negative intigers (uint only)
        /// - For any call KnapsackRecursive(weights, values, capacity, n), the method will return the 
        ///   maximum possible value using the first n items within the given capacity.
        /// </invariant>
        public static uint KnapsackRecursive(List<KnapsackItem> items, uint capacity, uint n)
        {
            // Base case: no items or no capacity
            if (n == 0 || capacity == 0)
            {
                return 0;
            }

            // If the weight of the nth item is more than the capacity, skip it
            if (items[(int)(n - 1)].Weight > capacity)
            {
                return KnapsackRecursive(items, capacity, n - 1);
            }
            else
            {
                // Include the item and find the value for remaining capacity
                uint includeItem = items[(int)(n - 1)].Value + KnapsackRecursive(items, capacity - items[(int)(n - 1)].Weight, n - 1);
                // Exclude the item
                uint excludeItem = KnapsackRecursive(items, capacity, n - 1);
                // Return the maximum value of including or excluding the item
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
        public uint SolveKnapsack(uint capacity, List<KnapsackItem> items)
        {
            return SolveKnapsackHelper(items, capacity, 0);
        }

        /// <summary>
        /// A dictionary to store previously calculated results for memoization.
        /// </summary>
        private readonly Dictionary<(uint, uint), uint> memo = new();

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
        private uint SolveKnapsackHelper(List<KnapsackItem> items, uint remainingCapacity, uint currentIndex)
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
        /// The entry point of the application. Initializes the logger, measures the execution time of the 
        /// knapsack algorithm, logs the results.
        /// </summary>
        public static void Main()
        {
            var logger = new Logger();
            Stopwatch stopwatch = new Stopwatch();

            logger.Info("TESTING");

            stopwatch.Reset();
            stopwatch.Start();

            // knapsack call
            uint solution = 0;

            stopwatch.Stop();
            decimal exec_time = (decimal)stopwatch.Elapsed.TotalMilliseconds;
            logger.Time(exec_time);

            logger.Solution(solution);
        }
    }

}




