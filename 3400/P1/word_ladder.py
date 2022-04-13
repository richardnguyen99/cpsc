__author__ = "Richard Nguyen"
__doc__ = "For submission purpose only"

from typing import Deque, List, Set, Tuple
from collections import deque


def read_words_from(filename: str) -> List[str]:
    word_list: List[str] = []
    with open(filename, "r") as f:
        for line in f:
            word_list.append(line.strip())

    return word_list


def read_pairs_from(filename: str) -> List[Tuple[str, str]]:
    pair_list: List[Tuple[str, str]] = []
    with open(filename, "r") as f:
        for line in f:

            pair = tuple(line.strip().split(' '))
            pair_list.append(pair)

    return pair_list


def word_ladder(src: str, dest: str, word_list: List[str]) -> List[str]:
    word_set: Set[str] = set(word_list)
    if dest not in word_set:
        return []

    word_queue: Deque[str] = deque()
    word_queue.append(src)

    visited_word_set: List[str] = list()
    visited_word_set.append(src)

    visited_word_tree: List[Tuple[int, str]] = list()
    word_index: Tuple[int, str] = (-1, src)
    visited_word_tree.append(word_index)

    return_word_ladder: List[str] = list()

    count = -1

    while word_queue:
        word: str = word_queue.popleft()
        count += 1

        if word == dest:
            word_set_index = visited_word_set.index(word)

            while word_set_index != -1:
                parent_index, current_word = visited_word_tree[word_set_index]
                return_word_ladder.append(current_word)
                word_set_index = parent_index

            return_word_ladder.reverse()
            return return_word_ladder

        for j in range(len(word)):
            for k in range(ord('a'), ord('z') + 1):
                char_list = [char for char in word]
                char_list[j] = chr(k)

                new_string: str = "".join(char_list)
                if (new_string in word_set) and (new_string not in visited_word_set):
                    word_queue.append(new_string)
                    visited_word_set.append(new_string)

                    word_index = (count, new_string)
                    visited_word_tree.append(word_index)

    return []


def main():
    word_list = read_words_from("words.txt")
    pair_list = read_pairs_from("pairs.txt")

    for begin, end in pair_list:
        print(f"** Looking for ladder from {begin} to {end}")
        ladder = word_ladder(begin, end, word_list)

        if (ladder):
            print("The ladder is:", end=" ")
            print(" -> ".join(ladder))
            print(f"Changes: {len(ladder)}")
        else:
            print(f"No ladder found from {begin} to {end}")

        print()


if __name__ == '__main__':
    main()
