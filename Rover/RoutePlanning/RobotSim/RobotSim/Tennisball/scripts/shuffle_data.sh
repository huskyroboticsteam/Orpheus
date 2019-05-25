#!/bin/bash
echo "Shuffling training data..."
rm -f ./data/training/training_shuffled.csv
python scripts/csvshuffle.py ./data/training/training.csv ./data/training/training_shuffled.csv
echo "Shuffling testing data..."
rm -f ./data/testing/testing_shuffled.csv
python scripts/csvshuffle.py ./data/testing/testing.csv ./data/testing/testing_shuffled.csv
