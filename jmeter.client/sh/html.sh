#!/bin/bash
filename="$1"

chmod u+r /j/html

jmeter -g /j/csv/$filename.csv -o  /j/html/$filename

exit 0;