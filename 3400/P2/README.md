# CPSC 3400 - Python Assignment 2 -- Family Tree

This second Python project has the following goals:

- Experience using Python classes
- Understand and manipulate an interesting recursively-defined data structure
- Understand and extend code written by someone else

----

The project involves processing genealogy information found in GEDCOM files, a standard interchange format used by genealogists and genealogy software ([online GEDCOM specification](https://gedcom.io/specifications/FamilySearchGEDCOMv7.html)). More details about how to get started are included below.

The program reads in a **GEDCOM** file and builds a structure of objects that represent some of the information in the file, namely basic info about individuals and families.  It does this using two classes, Person and Family, which have obvious roles corresponding to their names.  More details can be found in the comments within the program.

The functions `processPerson()` and `processFamily()` parse the GEDCOM file, leading to creation of `Person` and `Family` objects.  Many kinds of **GEDCOM** records are currently skipped by these functions.  One of your tasks will be to  interpret some of the skipped lines.

The `Person` class includes a method named `printDescendants()` that initiates a traversal of the stored objects in a way that prints out a tree of descendants from the referenced instance of Person.  This method uses a corresponding method in `Family` (called `printFamily()`) to produce its output.  `printFamily()` calls `printDescendants()` on each of the children of the family.  It is important for you to understand this two-step recursive process.  Look at the output it produces for a simple example and identify where how each line gets printed.

Project requirements:

- Add a method `isDescendant(personID)` to Person which takes a person identifier string as an argument and returns True if the identified person is an descendant of self or False if this is not the case.  The answer should be True when the identified person is self.
- Add a method `printAncestors()` to Person that does what its name suggests.  You aren't likely to manage to make this look as good as the output of printDescendants, but each line should include some indication how many generations up the tree the ancestor is.  Just starting the line with a number and appropriate indentation is sufficient.
- Extend the `processPerson()` and `processFamily()` functions to handle birth, death and marriage records.  Look at the **GEDCOM** files to see just what these records look like.  You will find that all of them include specification of a date and a place, but either can be missing.  You will want to define an `Event` class that can store the date and place information (as strings) and can produce a string that represents all of the information it holds.  Any available event information should appear in the output produced by `printDescendants()`.  See the html file in the project folder for an idea of what the output might look like.
- Add a method `printCousins(n)` to Person that finds and prints out the nth cousins of the person.  You will get partial credit for submitting a method that only works for the special case of first cousins (n=1). First cousins are other grandchildren of a person's grandparents.  Second cousins are other great grandchildren of a person's great grandparents and I'll assume you can work out the induction from those two examples. (Sample output will be provided.)

The Python file you submit should have a comment block at the beginning that includes your name and a copy of the text from the syllabus about originality of code to indicate your assertion that you have written what you are submitting, beyond the started code in the `descendants.py` file.  Your new code should use appropriate variables names and adequate comments to make it readable.  Matching the comment density in `descendants.py` is sufficient.

## Testing your progam and submission requirements

You will be using the file `GEDtest.py` to test you program while you are working on it. That file imports `familyTree.py` and runs tests on it for all of the requirements of this assignment.

All of the printing and tests included in the test module are conditional, depending on input prompts that should be answered with a y or n.  You will be able to use this test driver before you have all of the required functions, since it will only run the ones you select.

You will be turning in only your program file, not the test file, so you can edit `GEDtest.py` as you see fit.  I'll make use of  a similar test file to execute your program for grading.

## Advice about implementing printCousins

The children of your parents' siblings are your first cousins.  The cleanest way to implement printCousins is by writing functions to implement that definition quite literally.  The alternative is to write a block of code that does all of the necessary tree walking to get from a person to their cousins -- which definitely are not all in one cluster in the tree.  Dealing with all of the special cases (no parents, no siblings, no children, etc.) within a single block of code is guaranteed to get very complex.  Do it with multiple simple functions and you will be much more likely to succeed with your sanity intact!

In addition, there is a simple extension of the above definition to get to the more general function for the final 5 points.  The children of your parents' first cousins are your second cousins.  The children of your parents' second cousins are your third cousins.  I hope that you can see from these statements that there is a simple recursive solution to create a general version of a cousin finding method.  Note that printing doesn't have to happen until all cousins are collected, so printCousins itself doesn't have to be recursive.  That can be left to a helper function that literally implements the cousins definition and collects them all to pass back for printing.

Your output should look like this:

```txt
First cousins for Caroline Bouvier KENNEDY
    Kathleen Hartington KENNEDY n: 4 Jul 1951
    Joseph Patrick KENNEDY II n: 24 Sep 1952
    Robert Francis KENNEDY Jr. n: 17 Jan 1954 Washington, DC
    Edwin Moore KENNEDY Jr. n: 1961

First cousins for Joseph Patrick KENNEDY
    No cousins

Second cousins for John Bouvier Kennedy Schlossberg
    Joseph Patrick KENNEDY III n: 1980
    Matthew Rauch KENNEDY
```
