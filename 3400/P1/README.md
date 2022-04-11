# Homework 1: Python Assignment 1 -- Finding Word Ladders

Here two sets of answers for the tests in pairs.txt: word-ladder-answe

> Two sets of answers for the tests in pairs.txt: `word-ladder-answers.txt`

I made one change in my program to generate the second version -- I reversed the lists of adjacent words.  Thus the program found different ladders of the same length in a few of the cases.

------------

## Abstraction

A Word Ladder program is similar to a "6 degrees of separation" or [Oracle of (Kevin) Bacon](http://oracleofbacon.org//) game, but using words rather than people or actors. Simply, from a given starting word, find the shortest "ladder" of single letter changes which leads to some final word, where each intermediate state is also a word.

For example:

clash, flash, flask, flack, flock, clock, crock, crook, croon, crown, clown.

As you might infer from the example above, there is no simple algorithm that generates a word ladder.   Rather, you have to search for a ladder by generating all of the possibilities. Doing so will give you an opportunity to use Python data structures like lists, sets and dictionaries.

Your program should first read in the file `words.txt` giving it a large collection of words to work with.  All of the examples in the tests that your program will be given will be of lengths no greater than 7, so you can discard all words that are longer than 7 characters.  However, if you choose the right data structures, keeping all of the words won't have any significant impact on how your program executes.

After processing the file of words, your program should process a sequence of pairs of words from a second file named pairs.txt.  Each pair of words in this file will be found on a single line of text.  There is no association between the lines of this file, so there is no reason to read and store the contents of this file.  Just process the lines one at a time, printing out the ladder for each pair or an indication that no ladder exists.  You should check that the input words are of the same length and produce an appropriate error message if they are of different lengths.  You can assume that every line in the text file will contain two words; you only need to check for potential length problems, as described above.

Design your program using functions that encapsulate key design decisions.  For example, how you find adjacent words and how you implement the breadth-first search are completely independent.  Don't put the code for finding the adjacent words in the middle of your search loop.  Create a function that returns a list of the adjacent words given a single word as an argument.  Use that list to drive the loop that creates new queue entries,

You can download a `pairs.txt` file for testing as you develop the program.  This one starts off with some simple cases and includes some pairs for which no ladder can be found.  It also includes a test to make sure you check for the words being of different length. I may use a file with additional tests to grade your program, but if your program works for all of these examples, you should have any other tests covered.

## Implementation Notes

You must use a **queue** of partial ladders properly to find the shortest possible solution. If you get this wrong, the search may find a solution that is not the shortest possible ladder.  You have to include all of the steps so far in your queue entries in order to be able to show the solution once you find that one exists.

See this video for an explanation of how the search should work:

Since your program will only have to produce ladders for a limited number of tests, it is not acceptable to find all of the adjacent words for every word in words.txt right after you read them in.  This approach will work, but could make your program pause for a long time before it produces any answers.  A better alternative is to find all of the adjacent words to a particular word when they are needed and then remember them for retrieval  if/when they are needed again.  One of the Python data structures handles this task simply and efficiently.

## Implementation Advice

The `words.txt` file is quite large and the only way to find a solution is to do a breadth first search.  Given the size of this file and the fact that string comparisons are relatively slow, poor solutions will run very slowly.

Here are some things you want to avoid in creating your solution:

- Comparing words to every other word -- a membership test is a better idea.
- Making a new copy of all the words as you process every pair of words to find a ladder.  You might find your yourself doing this if you "remember" which words have been used by removing them from the data structure you use to hold the words from words.txt

Here are some recommendations to help create an efficient solution:

- Where appropriate, use Python data structures like dictionary and set that are implemented with hashing, not just lists.
- Figure out how to come with a list of adjacent words without comparing a word to every other word from words.txt.  

    > Hint: there are a limited number of strings that can be created by substituting a different letter for each of the letters in the word.

- Use a deque from the collections library to implement your queue instead of using a list.  It makes popping entries from the front of a list more efficient and this is something you will be doing a lot.  (Not required, but provides some experience with the collections library.)
- A set works well to keep track of the words already used as you search for a ladder between one of the pairs since the question you have to ask repeatedly is efficiently implemented by the "in" operation on a set..

## How to run
