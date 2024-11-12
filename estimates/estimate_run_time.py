# The purpose of this script is to estimate the runtime for the 4 algorithms
# Author: Jakob Balkovec
# Date: Sun 10th Nov 2024

# Notes:
# Recursive & Recursive + Threading 
# Complexity: O(2^n)
#
# Memo & DP
# Complexity: O(n * W)

# Idea:
# Get an time per function call for and multiply to get the final result
import logging
import math
from decimal import Decimal

"""
[C#] 
- Results/Estimates obtained from FPexecution_log.log
- Calculated in Excel
- Use median to get rid of the outliers

--- [RECURSIVE] ---
Time Complexity: O(2^n)
# of items: 50
Median: 8,2459 ms
Time per call: 
--- [RECURSIVE] ---

--- [DP] ---
Time Complexity: O(n * W)
# of items: 50
Median: 0,0361 ms
Time per call:
--- [DP] ---

--- [MEMO] ---
Time Complexity: O(n * W)
# of items: 50
Median: 0,3355 ms
Time per call:
--- [MEMO] ---
"""
import math
import logging

# Constants used in the script
CONSTANTS = {"n" : 50,
             "W" : 100,
             "median_recursive_time" : 8.2459,
             "median_dp_time" : 0.0361,
             "median_memo_time" : 0.3355,
             "sizes" : [50, 100, 250, 500, 1000],
             "file_name" : "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/FP_log.log"
             }

# Yield time per call for each algorithm
def yield_time_per_call():             
    total_recursive_calls = 2 ** CONSTANTS["n"]
    recursive_time_per_call = CONSTANTS["median_recursive_time"] / total_recursive_calls
    dp_and_memo_calls = CONSTANTS["n"] * CONSTANTS["W"]
    dp_time_per_call = CONSTANTS["median_dp_time"] / dp_and_memo_calls
    memo_time_per_call = CONSTANTS["median_memo_time"] / dp_and_memo_calls
    
    return (recursive_time_per_call, dp_time_per_call, memo_time_per_call)

# Get an estimate for the recursive knapsack based on passed in size
def total_time_estimate_recursive(size: int, time_per_call: float):
    return 2 ** size * time_per_call

# Get an estimate for the DP knapsack based on passed in size
def total_time_estimate_DP(size: int, time_per_call: float):
    return size * CONSTANTS["W"] * time_per_call

# Get an estimate for the memoized knapsack based on passed in size
def total_time_estimate_memo(size: int, time_per_call: float):
    return size * CONSTANTS["W"] * time_per_call

# Log results to file
def log_results_to_file(filename=CONSTANTS["file_name"]):
    recursive_time_per_call, dp_time_per_call, memo_time_per_call = yield_time_per_call()
    with open(filename, "w") as log_file:
        # Recursive Knapsack estimates
        log_file.write("Recursive Knapsack Time Estimates\n")
        log_file.write("-------------------------------\n")
        for size in CONSTANTS["sizes"]:
            time_estimate = total_time_estimate_recursive(size, recursive_time_per_call)
            log_file.write(f"Estimated time for Recursive Knapsack with {size} items: {time_estimate:.4f} ms\n")
        
        log_file.write("\n-------------------------------\n")

        # DP Knapsack estimates
        log_file.write("DP Knapsack Time Estimates\n")
        log_file.write("-------------------------------\n")
        for size in CONSTANTS["sizes"]:
            time_estimate = total_time_estimate_DP(size, dp_time_per_call)
            log_file.write(f"Estimated time for DP Knapsack with {size} items: {time_estimate:.4f} ms\n")
        
        log_file.write("\n-------------------------------\n")

        # Memoized Knapsack estimates
        log_file.write("Memoized Knapsack Time Estimates\n")
        log_file.write("-------------------------------\n")
        for size in CONSTANTS["sizes"]:
            time_estimate = total_time_estimate_memo(size, memo_time_per_call)
            log_file.write(f"Estimated time for Memoized Knapsack with {size} items: {time_estimate:.4f} ms\n")
        
        log_file.write("\n-------------------------------\n")

# Clears Log File
def clear_log_file(filename=CONSTANTS["file_name"]):
    with open(filename, "w") as log_file:
        log_file.write("")

# Enty point
def main():
    clear_log_file()
    log_results_to_file()
    print("Check " + "\"FP_log.log\"" + " for the results")
     
if __name__ == "__main__":
    main()
