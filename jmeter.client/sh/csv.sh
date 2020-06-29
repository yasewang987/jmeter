#!/bin/bash
filename="$1"

chmod u+x /j/jmx/$filename.jmx

jmeter -n -t  /j/jmx/$filename.jmx -l /j/csv/$filenaSme.csv

exit 0