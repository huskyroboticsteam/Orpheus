#! /bin/sh
filename=$1
firstline=$2
lastline=$3

# Basics of sed:
#   1. sed commands have a matching part and a command part.
#   2. The matching part matches lines, generally by number or regular expression.
#   3. The command part executes a command on that line, possibly changing its text.
#
# By default, sed will print everything in its buffer to standard output.  
# The -n option turns this off, so it only prints what you tell it to.
#
# The -e option gives sed a command or set of commands (separated by semicolons).
# Below, we use two commands:
#
# ${firstline},${lastline}p
#   This matches lines firstline to lastline, inclusive
#   The command 'p' tells sed to print the line to standard output
#
# ${lastline}q
#   This matches line ${lastline}.  It tells sed to quit.  This command 
#   is run after the print command, so sed quits after printing the last line.
#   
sed -ne "${firstline},${lastline}p;${lastline}q" < ${filename}
