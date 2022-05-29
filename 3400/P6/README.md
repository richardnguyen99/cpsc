# Homework 6: DFAs, Regular Expressions and Turing machines

## Part 1 - DFAs

Draw DFAs for the following language specifications.

1. All strings on $\sum = \{A,B,C\}$ that contain each letter ($A$, $B$, and $C$ at least once).
2. All strings on $\sum = \{X,Y,Z\}$ that contain two secutive $X$s or three consecutive $Z$s (or both).
3. All strings on $\sum = \{A,B,C,D\}$ that match the Python regular expression `^(A?(B|CD)*A+D)$`

## Part 2 - Regular expressions

For each item, write a single regular expression that matches that item. Note that in **All** cases, the entire string must match without additional characters.

1. A string of digits that contains only digits and contains exactly two fives. Exampels of acceptable strings include: "15445", "55", "05563".

   - The string should be rejected if it contains anything other than a digits

2. A regular expression that matches a time expressed in the form "1:45 PM".

    - The hours part must be a number from 1 to 12, the minutes range from 00 to 59, and the time must indicate either AM or PM (uppercase only and preceded by exactly one space).

3. A regular expression that matches a string representing a comma separated list of variable names such as: `hello`, `get_max`, `sum3`.

    - A variable name consists of letters, digits, and underscores but cannot start with a digit.
    - There is exactly one space after every comma. No other spaces are allowed.
    - Commas and spaces are not allowed before the first name and after the last name.
    - An empty string is considered a match

## Part 3 - Turing Machines

Design a Turing Machine on the input alphabet $\{X,Y,Z\}$ that removes all $Z$ characters from the input such there are no gaps. If the input string is $xzzyxzy$, the output should be $xyxy$.

-  The final string does not need to reside on the same part of the tape where it started.
