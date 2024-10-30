"""_Knapsack Problem Data Factory_
@author:
    Jakob Balkovec

@date:
    Tue Oct 29th

@version:
    1.0

@file:
    data_factory.py

@description:
    This script generates data sets used for the knapsack problem. The data sets are generated 
    based on thec size of the data set and the type of data (items or capacity). The size of the 
    data set is determined by the SIZE_MAP dictionary. The data sets are generated with random 
    values within the bounds specified in the script. The data sets are then exported as .json
    files in the 'data' directory. The script also contains functions to delete all data files 
    in the 'data' directory and to check if a directory is empty.
"""

import random
import datetime
import json
import os

# Office Hours

LOWER_BOUND_ITEMS: int = 6
UPPER_BOUND_ITEMS: int = 37

LOWER_BOUND_CAPACITY: int = 15
UPPER_BOUND_CAPACITY: int = 87

BREAK: str = '\n\n'
  
ITEMS: str = 'items'
CAPACITY: str = 'capacity'

SIZE_MAP: dict = {
    'xsmall': 100,
    'small': 1000,
    'medium': 10000,
    'large': 100000,
    'xlarge': 1000000
}

def create_data_file_name(size: str, type: str) -> str:
    """Generates a data file name based on the specified size and type.

    Args:
        size (str): The size category (e.g., 'small', 'medium').
        type (str): The type of data (either 'items' or 'capacity').

    Returns:
        str: The formatted file name.
    """
    return f"{type}{size}data_{datetime.datetime.now().strftime('%d_%m_%Y')}.json"



def create_items_data(size: str) -> list:
    """Creates a list of dictionaries representing items with random weights and values.

    Args:
        size (str): The size category to determine the number of items.

    Returns:
        list: A list of dictionaries, each containing 'Weight' and 'Value'.
    """
    return [
        {"Weight": random.randint(LOWER_BOUND_ITEMS, UPPER_BOUND_ITEMS),
         "Value": random.randint(LOWER_BOUND_ITEMS, UPPER_BOUND_ITEMS)}
        for _ in range(SIZE_MAP[size])
    ]



def create_capacity_data(size: str) -> list:
    """Creates a list of dictionaries representing capacities with random values.

    Args:
        size (str): The size category to determine the number of capacity entries.

    Returns:
        list: A list of dictionaries, each containing 'Capacity'.
    """
    return [
        {"Capacity": random.randint(LOWER_BOUND_CAPACITY, UPPER_BOUND_CAPACITY)}
        for _ in range(SIZE_MAP[size])
    ]



def create_data_file(size: str, data: list, type: str) -> str:
    """Creates a JSON data file with the specified data and type.

    Args:
        size (str): The size category for the data file name.
        data (list): The data to write to the file.
        type (str): The type of data ('items' or 'capacity').

    Raises:
        ValueError: If the provided type is invalid.

    Returns:
        str: The path of the created file or None if the creation failed.
    """
    valid_types: dict = {ITEMS: "items", CAPACITY: "capacity"}
    
    if type not in valid_types:
        raise ValueError(f"[Invalid type: {type}. Valid types are {ITEMS} and {CAPACITY}]")

    directory: str = os.path.join('data', valid_types[type])
    os.makedirs(directory, exist_ok=True)
    
    filename: str = os.path.join(directory, create_data_file_name(size, type))
    
    try:
        with open(filename, 'w') as file:
            json.dump(data, file, indent=4)
        print(f"[SUCCESS]: Created {filename}")
        return filename  
    except Exception as e:
        print(f"[ERROR]: Failed to create {filename}: {e}")
        return None 



def is_directory_empty(directory_path: str) -> bool:
    """Checks if the specified directory is empty.

    Args:
        directory_path (str): The path of the directory to check.

    Raises:
        ValueError: If the specified path is not a directory.

    Returns:
        bool: True if the directory is empty, False otherwise.
    """
    if os.path.isdir(directory_path):
        return len(os.listdir(directory_path)) == 0
    else:
        raise ValueError(f"{directory_path} is not a valid directory")



def delete_all_data_files(base_directory: str) -> None:
    """Deletes all data files in the specified base directory.

    Args:
        base_directory (str): The base directory containing 'capacity' and 'items' folders.
    """ 
    capacity_dir: str = os.path.join(base_directory, 'capacity')
    items_dir: str = os.path.join(base_directory, 'items')

    print("\n[CAPACITY_DIR]\n")
    if is_directory_empty(capacity_dir):
      print("** [capacity directory is empty] **")
      
    # Capacity directory
    if os.path.exists(capacity_dir):
        for filename in os.listdir(capacity_dir):
            file_path: str = os.path.join(capacity_dir, filename)
            if os.path.isfile(file_path):
                os.remove(file_path)
                print(f"[DELETED]: {file_path}")
    else:
        print(f"Capacity directory does not exist: {capacity_dir}")

    print("\n[ITEMS_DIR]\n")
    if is_directory_empty(items_dir):
      print("** [items directory is empty] **")
      
    # Items directory
    if os.path.exists(items_dir):
        for filename in os.listdir(items_dir):
            file_path: str = os.path.join(items_dir, filename)
            if os.path.isfile(file_path):
                os.remove(file_path)
                print(f"[DELETED]: {file_path}")
    else:
        print(f"Items directory does not exist: {items_dir}")

    print(BREAK)
 
 
    
def run_data_factory() -> None:
    """Runs the data factory to create item and capacity data files for all sizes.

    This function generates data for each size in SIZE_MAP and calls
    the create_data_file function for both items and capacities.
    """
    successful_files = []  
    unsuccessful_files = []

    for size in SIZE_MAP.keys():
        items_data = create_items_data(size)
        capacity_data = create_capacity_data(size)
        
        result = create_data_file(size, items_data, ITEMS)
        if result:
            successful_files.append(result)
        else:
            unsuccessful_files.append(f"items{size}data")

        result = create_data_file(size, capacity_data, CAPACITY)
        if result:
            successful_files.append(result)
        else:
            unsuccessful_files.append(f"capacity{size}data")

    print("\n*** [Successfully created data files] ***")
    for file in successful_files:
        print(f"        - {file}")

    print("\n*** [Unsuccessful file creations] ****")
    if unsuccessful_files:
        for file in unsuccessful_files:
            print(f"        - {file}")
    else:
        print("        - None")
    
    print(BREAK)
   
   
    
def main() -> None:
    """Main function to delete existing data files and run the data factory.

    This function serves as the entry point for the script, orchestrating
    the deletion of old data files and the creation of new ones.
    """
    # delete_all_data_files('data')
    # run_data_factory()
  
if __name__ == '__main__':
    main()
