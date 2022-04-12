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

    def test_4(self):
        expect_ladder = ['aces', 'apes', 'opes',
                         'oped', 'sped', 'seed', 'reed', 'reef']
        actual_ladder = word_ladder("aces", "reef", self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_5(self):
        expect_ladder = ['awed', 'owed', 'oped', 'sped',
                         'seed', 'weed', 'weet', 'west', 'pest']
        actual_ladder = word_ladder('awed', 'pest', self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_6(self):
        expect_ladder = []
        actual_ladder = word_ladder('abed', 'expo', self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_7(self):
        expect_ladder = []
        actual_ladder = word_ladder('abed', 'bbbb', self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_8(self):
        expect_ladder = []
        actual_ladder = word_ladder('aces', 'clown', self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_9(self):
        expect_ladder = ['clash', 'class', 'claws',
                         'craws', 'crows', 'crown', 'clown']

        actual_ladder = word_ladder('clash', 'clown', self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_10(self):
        # expect_ladder = ['clash', 'class', 'clans', 'clons',
        # 'cions', 'lions', 'linns', 'lines', 'lives']
        expect_ladder = ['clash', 'class', 'clans', 'clons',
                         'cions', 'lions', 'limns', 'limes', 'lives']
        actual_ladder = word_ladder('clash', 'lives', self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_11(self):
        # expect_ladder = ['clash', 'class', 'claps', 'clops',
        # 'clots', 'slots', 'sluts', 'slums', 'plums', 'plumb']
        expect_ladder = ['clash', 'slash', 'swash', 'swath',
                         'swats', 'slats', 'sluts', 'slums', 'plums', 'plumb']
        actual_ladder = word_ladder('clash', 'plumb', self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_12(self):
        expect_ladder = ['gloves', 'groves', 'graves', 'grapes', 'gripes', 'grimes', 'primes', 'prides', 'prudes',
                         'crudes', 'cruses',  'causes', 'caules', 'carles', 'carves', 'carver', 'carper', 'capper', 'tapper', 'topper']
        actual_ladder = word_ladder('gloves', 'topper', self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)

    def test_13(self):
        expect_ladder = []
        actual_ladder = word_ladder('concoct', 'connect', self.word_list)

        self.assertEqual(expect_ladder, actual_ladder)


def suite():
    suite = unittest.TestSuite()

    suite.addTest(WordLadderTesting('test_1'))
    suite.addTest(WordLadderTesting('test_2'))
    suite.addTest(WordLadderTesting('test_3'))
    suite.addTest(WordLadderTesting('test_4'))
    suite.addTest(WordLadderTesting('test_5'))
    suite.addTest(WordLadderTesting('test_6'))
    suite.addTest(WordLadderTesting('test_7'))
    suite.addTest(WordLadderTesting('test_8'))
    suite.addTest(WordLadderTesting('test_9'))
    suite.addTest(WordLadderTesting('test_10'))
    suite.addTest(WordLadderTesting('test_11'))
    suite.addTest(WordLadderTesting('test_12'))
    suite.addTest(WordLadderTesting('test_13'))

    return suite


def main():
    word_list: List[str] = []
    with open('data/words.txt', 'r') as f:
        for line in f:
            word_list.append(line.strip())

    ladder = word_ladder("clash", "plumb", word_list)
    print(ladder)


if __name__ == "__main__":
    runner = unittest.TextTestRunner()
    runner.run(suite())

    # main()
