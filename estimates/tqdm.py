# !!! Abandoned, the tqdm approach doesn't work !!!

# Description: Recursive Knapsack Algorithm with a tqdm bar
# Author: Jakob Balkovec
# Date: Sun 10 Nov
# Purpose: Testing only

import json
from tqdm import tqdm

# XS : itemsxsmalldata
# S  : itemssmalldata
# M  : itemsmediumdata
# L  : itemslargedata
# XL : itemsxlargedata
# C  : capacitydata

FILES: dict = {"XS" : "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemsxsmalldata_06_11_2024.json",
               "S" : "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemssmalldata_06_11_2024.json",
               "M" : "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemsmediumdata_06_11_2024.json",
               "L" : "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemslargedata_06_11_2024.json",
               "XL" : "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/items/itemsxlargedata_06_11_2024.json",
               "C" : "/Users/jbalkovec/Desktop/CPSC4100/FinalProject/FinalProject4100/data/capacity/capacitydata_06_11_2024.json"}

# Items list
items = []
capacity = 0

# Read the items from the json files
def read_data(file):
    with open(file, "r") as f:
        data = json.load(f)  
        items.extend(data)

# Extract a capacity             
def get_capacity(file):
    with open(file, "r") as f:
        data = json.load(f)
        capacities = [item["Capacity"] for item in data]
        return capacities[0] if capacities else 0 

# Recursive Algorithm
def knapsack_recursive(items, capacity, n): 
    if n == 0 or capacity == 0:
        return 0

    if items[n - 1]["Weight"] > capacity:
        return knapsack_recursive(items, capacity, n - 1)
    
    include_item = items[n - 1]["Value"] + knapsack_recursive(items, capacity - items[n - 1]["Weight"], n - 1)
    exclude_item = knapsack_recursive(items, capacity, n - 1)

    return max(include_item, exclude_item)
  
def main():
    items.clear()

    capacity_file = FILES["C"]
    capacity = get_capacity(capacity_file)
    
    for size, filepath in tqdm(FILES.items(), desc="Processing Files", unit="file"):
        if size == "C":
            continue

        read_data(filepath)
        result = knapsack_recursive(items, capacity, len(items))
        print(f"Max value for {size} data: {result}")

        items.clear()

# Run
if __name__ == "__main__":
    main()