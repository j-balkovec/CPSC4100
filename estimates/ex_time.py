# The purpose of this file is to paint the "client" a picture/graph of what the 
# execution time of the program looks like (by algorithm).
#
# Author: Jakob Balkovec
# Date: Tue 12th of Nov 2024

from decimal import Decimal # import to handle large numbers with percision

# Dictionaries
# Structure:
# {
#     "execution_times": [
          # Ran with a list of 10 items
#         {"timestamp": "2024-11-10 20:31:12,938", "description": "Recursive Knapsack completed"}
#     ],
#     "estimates": [
          # where m and n are some arbitrary integers > 0
#         {"items": m, "estimated_time_seconds": Decimal(n)},

# Recursive Knapsack Data
recursive_knapsack = {
    "execution_times": [
        {"timestamp": "2024-11-10 20:31:12,938", "description": "Recursive Knapsack completed"}
    ],
    "estimates": [
        {"items": 50, "estimated_time_ms": Decimal(8.2459)},
        {"items": 100, "estimated_time_ms": Decimal(9284058041833594.0000)},
        {"items": 250, "estimated_time_ms": Decimal(13250650419155208995516115365435966498741912896930642873286656.0000)},
        {"items": 500, "estimated_time_ms": Decimal(23973757746676581393500984290966260754355865231450429464749433415377646154189127744248911677894853948153002635308972467477497510409273344.0000)},
        {"items": 1000, "estimated_time_ms": Decimal(78475473443948495098707037354979661522064580470903595069840123771006214524751889525694864973146810974870646140797315102588136778827230164075541649317145692619300495481466542034208964665384161871188882553639833296585341715394928164375016097338765161755581073903139102175479270860174393344.0000)}
    ]
}

# Memoized Knapsack Data
dp_knapsack = {
    "execution_times": [
        {"timestamp": "2024-11-10 20:31:12,939", "description": "DP Knapsack completed"}
    ],
    "estimates": [
        {"items": 50, "estimated_time_ms": Decimal(0.0361)},
        {"items": 100, "estimated_time_ms": Decimal(0.0722)},
        {"items": 250, "estimated_time_ms": Decimal(0.1805)},
        {"items": 500, "estimated_time_ms": Decimal(0.3610)},
        {"items": 1000, "estimated_time_ms": Decimal(0.7220)}
    ]
}

# DP Knapsack Data
memoized_knapsack = {
    "execution_times": [
        {"timestamp": "2024-11-10 20:31:12,939", "description": "Memo Knapsack completed"}
    ],
    "estimates": [
        {"items": 50, "estimated_time_ms": Decimal(0.3355)},
        {"items": 100, "estimated_time_ms": Decimal(0.6710)},
        {"items": 250, "estimated_time_ms": Decimal(1.6775)},
        {"items": 500, "estimated_time_ms": Decimal(3.3550)},
        {"items": 1000, "estimated_time_ms": Decimal(6.7100)}
    ]
}

def convert_time(milliseconds):
    # Time unit conversions starting from milliseconds
    units = [
        ("milliseconds", Decimal(1)),
        ("seconds", Decimal(1000)),
        ("minutes", Decimal(1000 * 60)),
        ("hours", Decimal(1000 * 60 * 60)),
        ("days", Decimal(1000 * 60 * 60 * 24)),
        ("weeks", Decimal(1000 * 60 * 60 * 24 * 7)),
        ("years", Decimal(1000 * 60 * 60 * 24 * 365.25)),
        ("decades", Decimal(1000 * 60 * 60 * 24 * 365.25 * 10)),
        ("centuries", Decimal(1000 * 60 * 60 * 24 * 365.25 * 100)),
        ("millennia", Decimal(1000 * 60 * 60 * 24 * 365.25 * 1000))
    ]
    
    # Loop through units to find the appropriate one for the time value
    for i in range(len(units) - 1, -1, -1):
        name, unit_milliseconds = units[i]
        if milliseconds >= unit_milliseconds:
            time_value = milliseconds / unit_milliseconds
            # Use scientific notation if time_value is very large
            if time_value >= Decimal('1e6'):
                time_value_str = f"{time_value:.2e}"
                return f"{time_value_str} {name}"
            else:
                return f"{time_value:.2f} {name}"

    # If the time is less than 1 millisecond, keep it in milliseconds
    return f"{milliseconds:.2f} milliseconds"

  
def print_estimates(knapsack_data, knapsack_type):
    print(f"Estimates for {knapsack_type} Knapsack:")
    for estimate in knapsack_data["estimates"]:
        items = estimate["items"]
        time_ms = estimate["estimated_time_ms"]
        readable_time = convert_time(time_ms)
        print(f"Estimated time for {knapsack_type} Knapsack with {items} items: {readable_time}")
    print("\n")

# Call the function for each knapsack type
print_estimates(recursive_knapsack, "Recursive")
print_estimates(memoized_knapsack, "Memoized")
print_estimates(dp_knapsack, "DP")