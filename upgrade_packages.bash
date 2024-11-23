#!/bin/bash

# Upgrade all outdated Python packages
echo "Upgrading outdated Python packages..."
pip list --outdated --format=columns | awk 'NR > 2 {print $1}' | xargs -n 1 pip install --upgrade
echo "All outdated packages have been upgraded."
