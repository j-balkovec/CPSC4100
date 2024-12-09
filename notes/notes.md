# CPSC 4100 Design and Analysis of Algorithms - Final Project Plan
---

## 1. Implementation of the 0/1 Knapsack Problem

**Languages**: C# for algorithm implementations

### a) Optimal Algorithm (Dynamic Programming Approach)
- **Description**: Use a 2D array to store the maximum value achievable with a given weight. This approach has a time complexity of \(O(n \times W)\) and space complexity of \(O(n \times W)\), where \(n\) is the number of items and \(W\) is the knapsack capacity.
- **Implementation Steps**:
  1. Create a 2D array `K` where `K[i][w]` holds the maximum value for `i` items and weight `w`.
  2. Fill the array using the recurrence relation:
     - If the item can be included:  
       \( K[i][w] = \max(K[i-1][w], K[i-1][w - \text{weight}[i]] + \text{value}[i]) \)
     - Else:  
       \( K[i][w] = K[i-1][w] \)

### b) Greedy Heuristic
- **Heuristic Choice**: Fractional Knapsack (value/weight ratio)
- **Justification**: While the greedy approach doesn’t yield optimal solutions for the 0/1 Knapsack, it can serve as a fast, approximative method and helps illustrate the differences in performance.
- **Implementation Steps**:
  1. Calculate the ratio of value to weight for each item.
  2. Sort items based on this ratio in descending order.
  3. Add items to the knapsack until the capacity is reached, considering the remaining space for each item.

---

## 2. Data Generation and Construction

- **Languages**: Python for data generation
- **Data Set Construction**:
  - Generate random data sets with varying sizes (e.g., 10, 50, 100, 500 items).
  - Each item should have a random weight and value, ensuring diversity in data.
  - Consider generating both small and large datasets to test scalability.

### Example Data Generation Code in Python:
```python
import random

def generate_data(num_items, max_weight, max_value):
    items = [(random.randint(1, max_weight), random.randint(1, max_value)) for _ in range(num_items)]
    return items
```

---

## 3. Running and Timing Implementations

### Execution:
- Run both implementations on the same data sets.
- Measure execution time using built-in timing functions (e.g., Stopwatch in C#).
- Clearly specify what time is measured (e.g., total execution time for each algorithm).

### Example Timing Code in C#:
```csharp
Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();
// Call your knapsack function here
stopwatch.Stop();
Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
```

---

## 4. Analysis of Each Implementation

### Analysis Criteria:
- **Algorithmic Complexity**: Compare the time and space complexities of both implementations.
- **Code Complexity**: Evaluate the readability and maintainability of the code.
- **Data Analysis**: Discuss how data is handled within each implementation.
#### Observed Performance:
  -  Run times on different data sets.
  -  Present results in a clear table format.

## 5. Summary of Findings

<!-- @import "[TOC]" {cmd="toc" depthFrom=1 depthTo=6 orderedList=false} -->

### Comparison of Implementations:
- Discuss differences in:
  - **Algorithmic Complexity**: Optimal vs. Greedy.
  - **Code Complexity**: Ease of understanding and modification.
  - **Data Analysis**: How effectively each approach utilizes the generated data.
  - **Performance**: Include observed run times and comment on:
    - Scalability.
    - Impact of input size and knapsack capacity.

---

## 6. Submission on Canvas

### Files to Submit:
- Individual C# code files for both the optimal and greedy implementations.
- Python code for data generation.
- A document (doc/docx/pdf) that includes:
  - Analysis and findings as per #4 & #5.
  - Details of runs conducted.
  - Tables with results and execution times.

---

### Key Points for Implementation and Presentation
- **Clarity and Organization**: Keep your code and documentation organized.
- **Visualizations**: Use graphs in R to visualize performance differences.
- **Engagement**: Practice your presentation to ensure clear communication of complex ideas.
