"""
word_ladder.py: From a given starting word, find the shortest "ladder" of single letter changes which leads to some final word, where each intermediate state is also a word.
"""

__author__ = "Richard Nguyen"

from typing import Deque, List, Set, Tuple
from collections import deque


def word_ladder(src: str, dest: str, word_list: List[str]) -> List[str]:
    """
    Word ladder is to find the shortest path of word transformantion from SRC
    to DEST by changing one letter each step.

    """

    # Remove all possible dups
    word_set: Set[str] = set(word_list)
    if dest not in word_set:
        return []

    word_queue: Deque[str] = deque()
    word_queue.append(src)

    visited_word_set: List[str] = list()
    visited_word_set.append(src)

    # word_ladder() is a two-step process. First, it builds the word ladder
    # tree that contains all possible transformation starting from SRC
    #
    # Then, when there is a word matched with DEST, it performs a simple
    # shortest path algorithm to find all the steps
    visited_word_tree: List[Tuple[int, str]] = list()

    # Each node on the tree contains 1. an index of its parent
    # and 2. the content of itself. The SRC will have an index
    # of -1 to indicate it's the root
    word_index: Tuple[int, str] = (-1, src)
    visited_word_tree.append(word_index)

    return_word_ladder: List[str] = list()

    count = -1

    while word_queue:
        length: int = len(word_queue)

        for i in range(0, length):
            word: str = word_queue.popleft()
            count += 1

            if word == dest:    # building the tree is done
                word_set_index = visited_word_set.index(word)

                while word_set_index != -1:
                    parent_index, current_word = visited_word_tree[word_set_index]
                    return_word_ladder.append(current_word)
                    word_set_index = parent_index

                return_word_ladder.reverse()
                return (return_word_ladder)

            for j in range(len(word)):
                for k in range(ord('a'), ord('z') + 1):
                    char_list = [char for char in word]
                    char_list[j] = chr(k)

                    new_string: str = "".join(char_list)
                    if (new_string in word_set) and (new_string not in visited_word_set):
                        word_queue.append(new_string)
                        visited_word_set.append(new_string)

                        # build
                        word_index = (count, new_string)
                        visited_word_tree.append(word_index)

    return []
