# A few disabled warnings to keep pylint from losing its mind over style
# pylint: disable=C0103
# pylint: disable=C0111

# load modules
import csv
import argparse
import random

if __name__ == '__main__':
    parser = argparse.ArgumentParser("Shuffles the lines in a CSV file.")
    parser.add_argument("infile", metavar="Input filename")
    parser.add_argument("outfile", metavar="Output filename")

    arg = parser.parse_args()
    input_filename = arg.infile
    output_filename = arg.outfile

    rows = []
    columns = []

    with open(input_filename, 'r') as csvfile:
        csvreader = csv.DictReader(csvfile)
        columns = csvreader.fieldnames
        for row in csvreader:
            rows.append(row)

    with open(output_filename, 'w') as outfile:
        csvwriter = csv.DictWriter(outfile, fieldnames=columns)
        csvwriter.writeheader()
        while rows:
            row = random.choice(rows)
            rows.remove(row)
            csvwriter.writerow(row)
