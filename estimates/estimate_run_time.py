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
import time

# Sample 10 items list for testing
items = [
    {"weight": 2, "value": 3},
    {"weight": 3, "value": 4},
    {"weight": 4, "value": 5},
    {"weight": 5, "value": 8},
    {"weight": 6, "value": 10},
    {"weight": 7, "value": 12},
    {"weight": 8, "value": 14},
    {"weight": 9, "value": 16},
    {"weight": 10, "value": 18},
    {"weight": 11, "value": 20}
]


# 0/1 Knapsack Recursive Solution
def knapsack_recursive_util(capacity, weight, value, n):
    if n == 0 or capacity == 0:
        return 0
    if weight[n - 1] > capacity:
        return knapsack_recursive_util(capacity, weight, value, n - 1)
    else:
        return max(
            value[n - 1] + knapsack_recursive_util(capacity - weight[n - 1], weight, value, n - 1),
            knapsack_recursive_util(capacity, weight, value, n - 1)
        )


# Measure runtime of recursive knapsack
def knapsack_recursive(capacity, weight, value, n):
    logging.info("Running Recursive Knapsack Algorithm...")
    start_time = time.time()
    solution = knapsack_recursive_util(capacity, weight, value, n)
    end_time = time.time()
    exec_time = end_time - start_time
    logging.info(f"Recursive Knapsack completed in {exec_time:.4f} seconds")
    return exec_time, solution


# 0/1 Knapsack with Memoization
def knapsack_memo(capacity, weight, value, n):
    memo = {}
    
    def knapsack_memo_util(capacity, n):
        if n == 0 or capacity == 0:
            return 0
        if (capacity, n) in memo:
            return memo[(capacity, n)]
        if weight[n - 1] > capacity:
            memo[(capacity, n)] = knapsack_memo_util(capacity, n - 1)
        else:
            memo[(capacity, n)] = max(
                value[n - 1] + knapsack_memo_util(capacity - weight[n - 1], n - 1),
                knapsack_memo_util(capacity, n - 1)
            )
        return memo[(capacity, n)]
    
    logging.info("Running Memoized Knapsack Algorithm...")
    start_time = time.time()
    solution = knapsack_memo_util(capacity, n)
    end_time = time.time()
    exec_time = end_time - start_time
    logging.info(f"Memoized Knapsack completed in {exec_time:.4f} seconds")
    return exec_time, solution


# 0/1 Knapsack with DP
def knapsack_dp(capacity, weight, value, n):
    dp = [[0] * (capacity + 1) for _ in range(n + 1)]
    
    for i in range(n + 1):
        for w in range(capacity + 1):
            if i == 0 or w == 0:
                dp[i][w] = 0
            elif weight[i - 1] <= w:
                dp[i][w] = max(value[i - 1] + dp[i - 1][w - weight[i - 1]], dp[i - 1][w])
            else:
                dp[i][w] = dp[i - 1][w]
    
    solution = dp[n][capacity]
    logging.info("Running DP Knapsack Algorithm...")
    start_time = time.time()
    end_time = time.time()
    exec_time = end_time - start_time
    logging.info(f"DP Knapsack completed in {exec_time:.4f} seconds")
    return exec_time, solution


def estimate_runtime(base_time, base_size, full_size, algorithm_type, capacity):
    if algorithm_type == "recursive":
        # O(2^n) complexity scaling
        estimated_time = base_time * (2 ** (full_size - base_size))
    elif algorithm_type == "memo" or algorithm_type == "dp":
        # O(n * W) complexity scaling
        estimated_time = base_time * (full_size / base_size)
    else:
        estimated_time = base_time
    
    return estimated_time


def collect_data():
    logging.basicConfig(filename='/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/FP_log.log',
                        level=logging.INFO, 
                        format='%(asctime)s - %(levelname)s - %(message)s')
    
    capacity = 250 
    n = len(items) 

    weights = [item["weight"] for item in items]
    values = [item["value"] for item in items]

    for algorithm_name, algorithm_func in [("Recursive", knapsack_recursive), 
                                           ("Memo", knapsack_memo), 
                                           ("DP", knapsack_dp)]:
        
        base_time, solution = algorithm_func(capacity, weights, values, n)

        sizes = {
            "X": 50,
            "S": 100,
            "M": 250,
            "L": 500,
            "XL": 1000
        }

        for size_label, full_size in sizes.items():
            estimated_time = estimate_runtime(base_time, n, full_size, algorithm_name.lower(), capacity)
            
            logging.info(f"Running {algorithm_name} Knapsack Algorithm...")
            logging.info(f"{algorithm_name} Knapsack completed in {base_time:.4f} seconds")
            logging.info(f"Estimated time for {algorithm_name} with {full_size} items: {estimated_time:.4f} seconds\n")
        
if __name__ == "__main__":
    collect_data()
    print("\nOpen Log \"FP_log.log\"\n")

