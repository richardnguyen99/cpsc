import unittest

from typing import List

from word_ladder import word_ladder


class WordLadderTesting(unittest.TestCase):

    @classmethod
    def setUp(self):
        self.word_list: List[str] = []
        with open('data/words.txt', 'r') as f:
            for line in f:
                self.word_list.append(line.strip())

    def test_1(self):
        expect_ladder = ["cat", "bat", "bot", "hot"]
        actual_ladder = word_ladder("cat", "hot", self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_2(self):
        expect_ladder = ["cat", "cut", "cub", "fub", "fib"]
        actual_ladder = word_ladder("cat", "fib", self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_3(self):
        expect_ladder = []
        actual_ladder = word_ladder("yak", "zyz", self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)


def suite():
    suite = unittest.TestSuite()

    suite.addTest(WordLadderTesting('test_1'))
    suite.addTest(WordLadderTesting('test_2'))
    suite.addTest(WordLadderTesting('test_3'))

    return suite


if __name__ == "__main__":
    runner = unittest.TextTestRunner()
    runner.run(suite())
