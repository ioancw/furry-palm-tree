# two words and compare them.
from collections import Counter

# load the words - this is the official mystery word list
words = []
for line in open(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt"):
    word = line.strip().lower()
    words.append(word)

# for now use another source of words, but we need to filter for 5 chars.
for line in open(r"C:\Users\ioan_\GitHub\furry-palm-tree\words.txt"):
    word = line.strip().lower()
    if len(word) == 5:
        words.append(word)

# create a dict of words
freqs = {}
for word in words:
    letters = list(word)

    for letter in letters:
        if letter not in freqs:
            freqs[letter] = 0
        freqs[letter] += 1

histogram = []
for (l, c) in freqs.items():
    histogram.append((c, l))
histogram.sort(reverse=True)

# use a counter example
# https://realpython.com/python-counter/
letter_counter = Counter()
for word in words:
    letter_counter.update(word)

# go through all words and now find out the frequency score for each.

# are there any letters not present in any of the words.
letter_set = set()
for word in words:
    letters = list(word)
    for letter in letters:
        letter_set.add(letter)
# all letters are present.

freq_score = {}
for word in words:
    letters = list(word)
    freq = 0
    for letter in letters:
        freq += freqs[letter]
    freq_score[word] = freq

word_scores = []
for (w, c) in freq_score.items():
    # ignore words with same letter, as this doesn't maximise the entropy
    word_set = set(w)
    if len(word_set) == 5:
        word_scores.append((c, w))
word_scores.sort(reverse=True)

# words with most frequent letters are:
# later, alter, alert - these are anagrams
# arose
# irate
# stare

# what about positions of letters, i.e. what is the most frequent letter in each position.
l_freqs = Counter({1: Counter(), 2: Counter(), 3: Counter(), 4: Counter(), 5: Counter()})
for word in words:
    letters = list(word)
    # calculate the freqs for each letter position
    for position in range(5):
        letter = letters[position]
        letter_dict = l_freqs[position + 1]
        letter_dict.update(letter)

first_freq = sorted(l_freqs[1], key=l_freqs[1].get, reverse=True)

histogram = []
for (l, c) in l_freqs.items():
    histogram.append((c, l))
histogram.sort(reverse=True)

contains = []
for word in words:
    letters = list(word)
    first = letters[0]
    second = letters[1]
    vowels = ['a', 'e', 'i', 'o', 'u']
    if (first not in vowels) & (second not in vowels):
        contains.append(first + second)
nonVowelStarting = Counter(contains)

#('st', 65), ('sh', 52), ('cr', 45), ('sp', 45), ('ch', 40), ('gr', 38), ('fl', 36), ('tr', 36), ('br', 35),
# ('bl', 32), ('cl', 31), ('dr', 29), ('sc', 29), ('sl', 28), ('pr', 26), ('th', 24), ('sw', 23), ('fr', 21),
# ('pl', 19), ('wh', 18), ('sn', 18), ('gl', 16), ('sm', 15), ('wr', 12), ('kn', 10), ('tw', 9), ('sk', 9), ('ly', 4),
# ('hy', 4), ('ph', 4), ('dw', 3), ('cy', 3), ('sq', 3), ('rh', 2), ('ny', 2), ('gh', 2), ('gn', 2), ('sy', 2), ('fj', 1),
# ('kh', 1), ('ll', 1), ('dy', 1), ('ps', 1), ('by', 1), ('vy', 1), ('ty', 1), ('gy', 1), ('kr', 1), ('my', 1), ('py', 1)]

nonVowels = []
for word in words:
    letters = list(word)
    vowels = ['a', 'e', 'i', 'o', 'u']
    vowel = False
    for letter in letters:
        if letter in vowels:
            vowel = True
    if not vowel:
        nonVowels.append(word)
# non vowels without repeating words
['lynch', 'nymph', 'glyph', 'lymph', 'crypt']
# non vowels with repeating words
['lynch', 'nymph', 'tryst', 'glyph', 'lymph', 'wryly', 'dryly', 'shyly', 'crypt', 'gypsy', 'myrrh', 'slyly', 'pygmy']

wordsContainingVowels = []
for word in words:
    letters = list(word)
    vowels = ['a', 'e', 'i', 'o', 'u']
    vowelsinWord = ""
    for letter in letters:
        if letter in vowels:
            vowelsinWord += letter
        if len(vowelsinWord) > 0:
            wordsContainingVowels.append(vowelsinWord)
counterWithVowels = Counter(wordsContainingVowels)
[('a', 1641), ('o', 1053), ('i', 1046), ('e', 918), ('u', 810), ('ea', 294), ('ee', 238), ('ae', 237), ('ie', 225),
 ('ai', 201), ('oe', 197), ('ou', 182), ('oo', 152), ('ao', 147), ('oa', 119), ('ei', 103), ('ui', 102), ('ue', 100),
 ('aa', 98), ('au', 94), ('oi', 94), ('eo', 62), ('io', 56), ('ua', 56), ('ia', 51), ('eu', 49), ('ii', 38), ('uo', 26),
 ('eae', 19), ('aie', 17), ('aue', 15), ('oue', 14), ('uu', 14), ('uie', 13), ('iu', 13), ('aoe', 10), ('ooe', 10),
 ('aae', 9), ('oie', 8), ('eue', 8), ('aio', 7), ('aou', 7), ('uee', 6), ('eie', 6), ('aia', 6), ('aai', 6), ('iee', 5),
 ('aoo', 5), ('aea', 4), ('eee', 4), ('iio', 4), ('oea', 4), ('aui', 3), ('eoe', 3), ('aoa', 3), ('iia', 3), ('iae', 3),
 ('iue', 3), ('uai', 3), ('eui', 3), ('oio', 3), ('uae', 3), ('oae', 3), ('uio', 3), ('ouo', 2), ('aee', 2), ('ioe', 2),
 ('auu', 2), ('ioi', 2), ('eua', 2), ('oiu', 2), ('uue', 2), ('uua', 2), ('iea', 2), ('eia', 2), ('aua', 2), ('ooi', 2),
 ('aii', 2), ('aoi', 2), ('eai', 2), ('eea', 1), ('iaa', 1), ('aao', 1), ('eei', 1), ('eeie', 1), ('ueu', 1), ('ueue', 1),
 ('uoa', 1), ('ioa', 1), ('iie', 1), ('iao', 1), ('oaa', 1), ('oee', 1), ('ioo', 1), ('ooa', 1), ('auio', 1), ('aeo', 1),
 ('oia', 1), ('ieo', 1), ('uoe', 1), ('uaa', 1), ('oeo', 1)]

# count of vowels in a word
vowelCountInWords = []
for word in words:
    letters = set(word)
    vowels = set("aeiou")
    vowelCount = 5 - len(vowels - letters)
    vowelCountInWords.append(vowelCount)
vowelCountCounter = Counter(vowelCountInWords)

# simulating the game

from functools import reduce

for word in words:
    letters = list(word)
    matchedLetters = []
    for letter in list("auio"):
        matchedLetters.append(letter in letters)

    if all(matchedLetters):
        print(word)


def findWordsContainingLetter(wordList, letterList):
    matched = []
    for word in wordList:
        found = reduce(lambda s, l: s and l in list(word), letterList, True)
        if found:
            matched.append(word)
    return matched

findWordsContainingLetter(words, list("auio"))
findWordsContainingLetter(words, list("au"))


def findWordsMatchingPattern(wordList, pattern):
    matched = []
    for word in words:
        matchedLetters = []
        letters = list(word)
        patternLetters = list(pattern)
        comparisons = zip(patternLetters, letters)
        for (check, actual) in comparisons:
            if check == "_":
                matchedLetters.append(True)
            else:
                matchedLetters.append(check == actual)
        if all(matchedLetters):
            matched.append(word)
    return matched


def findWordsMatchingPattern2(wordList, pattern):
    """
    Finds words that match a pattern, e.g ARO__ or AR_SE
    """
    def compareLetter(state, comparison):
        check, actual = comparison
        match = False
        if check == "_":
            match = True
        else:
            match = check == actual
        return state and match

    matched = []
    for word in wordList:
        comparisons = zip(list(pattern), list(word))
        matchedPattern = reduce(compareLetter, comparisons, True)

        if matchedPattern:
            matched.append(word)
    return matched



findWordsMatchingPattern2(words, "___ee")
findWordsMatchingPattern(words, "___ee")
findWordsMatchingPattern(words, "___th")
findWordsMatchingPattern(words, "___in")
findWordsMatchingPattern(words, "__ain")
findWordsMatchingPattern(words, "__a_n")

# filter words based on feedback.
# simulation code
def find_matching_words(wordList, freqScore, guess, mask):
    grey = []
    yellow = []
    green = []
    guess_counter = Counter(guess)

    # remove grey words
    # careful not to remove words containing letters that match grey mask and another colour.
    # e.g. if actual is MOURN and guess is DUOMO, the mask would be [0,1,1,1,0]
    # and the final O is grey, but we don't want to filter on this final O as it's also yellow.
    for word in wordList:
        zeros = filter(lambda code: code[0] == 0 and (guess_counter[code[1]] == 1), zip(mask, guess))
        keep_word = reduce(lambda s, mask: s and mask[1] not in list(word), zeros, True)
        if keep_word:
            grey.append(word)

    # find ones that match yellow mask
    for word in grey:
        ones = filter(lambda code: code[0] == 1, zip(mask, guess))
        found = reduce(lambda s, mask: s and mask[1] in list(word), ones, True)
        if found:
            yellow.append(word)

    # find ones that match exactly, i.e. green
    for word in yellow:
        letters = list(word)
        twos = filter(lambda code: code[1][0] == 2, enumerate(zip(mask, guess)))
        found = reduce(lambda s, mask: s and mask[1][1] == letters[mask[0]], twos, True)
        if found:
            green.append(word)

    final = green

    word_freqs = []
    for word in final:
        score = freqScore[word]
        word_freqs.append((score, word))
    word_freqs = sorted(word_freqs, reverse=True)

    return [w[1] for w in word_freqs]


round1 = find_matching_words(words, freq_score, "arose", [1, 0, 0, 1, 0])
round1_0 = find_matching_words0(words, freq_score, "arose", [1, 0, 0, 1, 0])
round2 = find_matching_words(round1, freq_score, "unlit", [0, 1, 0, 0, 1])

round1 = find_matching_words(words, freq_score, ["arose"], [[1, 1, 1, 0, 0]])
round2 = find_matching_words(words, freq_score, ["arose", "ratio"], [[1, 1, 1, 0, 0], [1, 2, 0, 0, 1]])
round3 = find_matching_words(words, freq_score, ["arose", "ratio", "carol"], [[1, 1, 1, 0, 0], [1, 2, 0, 0, 1], [0, 2, 1, 2, 0]])
round4 = find_matching_words(
    words, freq_score, ["arose", "ratio", "carol", "manor"],
    [[1, 1, 1, 0, 0], [1, 2, 0, 0, 1], [0, 2, 1, 2, 0], [0, 2, 0, 2, 2]])
round5 = find_matching_words(
    words, freq_score, ["arose", "ratio", "carol", "manor", "vapor"],
    [[1, 1, 1, 0, 0], [1, 2, 0, 0, 1], [0, 2, 1, 2, 0], [0, 2, 0, 2, 2], [1, 2, 0, 2, 2]])

def get_answer_mask(actual, guess):
    not_matched = Counter(a for a, g in zip(actual, guess) if a != g)
    mask = []
    for a, g in zip(actual, guess):
        if a == g:
            mask.append(2)
        elif g in actual and not_matched[g] > 0:
            mask.append(1)
            not_matched[g] -= 1
        else:
            mask.append(0)
    return mask


get_answer_mask("favor", "arose")
get_answer_mask("favor", "ratio")#  [1, 2, 0, 0, 1]
get_answer_mask("favor", "carol") #[0,2,1,2,0]
get_answer_mask("favor", "vapor") #[1,2,0,2,2]
get_answer_mask("arose", "speed") # expect [1, 0, 1, 0, 0]
get_answer_mask("treat", "speed") # expect [0, 0, 2, 0, 0]
get_answer_mask("mourn", "smack")
get_answer_mask("mourn", "quote")
get_answer_mask("mourn", "duomo")

tw = "cigar"
after1 = find_matching_words(words, freq_score, "arose", get_answer_mask(tw, "arose"))
after2 = find_matching_words(after1, freq_score, "unlit", get_answer_mask(tw, "unlit"))
after3 = find_matching_words(after2, freq_score, "caddy", get_answer_mask(tw, "caddy"))

# play wordle
# strategy is to loop the rounds
# can start off with a random starter word or provide a prepopulated list of words to try
# e.g. arose and unlit.

import random


class Solver:
    def __init__(self, words_file_path):
        self.words = []
        self.simulation_results = []
        for line in open(words_file_path):
            word = line.strip().lower()
            self.words.append(word)
        random.shuffle(self.words)

    def get_answer_mask(self, actual, guess):
        not_matched = Counter(a for a, g in zip(actual, guess) if a != g)
        mask = []
        for a, g in zip(actual, guess):
            if a == g:
                mask.append(2)
            elif g in actual and not_matched[g] > 0:
                mask.append(1)
                not_matched[g] -= 1
            else:
                mask.append(0)
        return mask

    def find_matching_words(self, word_list, freqScore, guess, mask):
        grey = []
        yellow = []
        green = []
        guess_counter = Counter(guess)

        # remove grey words
        # careful not to remove words containing letters that match grey mask and another colour.
        # e.g. if actual is MOURN and guess is DUOMO, the mask would be [0,1,1,1,0]
        # and the final O is grey, but we don't want to filter on this final O as it's also yellow.
        for word in word_list:
            zeros = filter(lambda code: code[0] == 0 and (guess_counter[code[1]] == 1), zip(mask, guess))
            keep_word = reduce(lambda s, mask: s and mask[1] not in list(word), zeros, True)
            if keep_word:
                grey.append(word)

        # find ones that match yellow mask
        for word in grey:
            ones = filter(lambda code: code[0] == 1, zip(mask, guess))
            found = reduce(lambda s, mask: s and mask[1] in list(word), ones, True)
            if found:
                yellow.append(word)

        # find ones that match exactly, i.e. green
        for word in yellow:
            letters = list(word)
            twos = filter(lambda code: code[1][0] == 2, enumerate(zip(mask, guess)))
            found = reduce(lambda s, mask: s and mask[1][1] == letters[mask[0]], twos, True)
            if found:
                green.append(word)

        final = green

        word_freqs = []
        for word in final:
            score = freqScore[word]
            word_freqs.append((score, word))
        word_freqs = sorted(word_freqs, reverse=True)

        return [w[1] for w in word_freqs]

    def run_simulation(self, target_words, sims, seed_words=[]):
        for sim in range(sims):
            game_results = {}
            for target_word in target_words:
                print("Target: " + target_word)
                words_for_round = self.words.copy()
                for game_round in range(6):
                    guess = random.choice(words_for_round)

                    # find seed word, which are favourite start words that you can use.
                    seed = seed_words[game_round]
                    if seed is not None:
                        guess = seed
                    if guess in words_for_round:
                        words_for_round.remove(guess)
                    print(str(game_round + 1) + ": " + guess)

                    if guess == target_word:
                        print("Game Won on round " + str(game_round + 1))
                        game_results[target_words.index(target_word)] = (game_round + 1, target_word)
                        break

                    words_for_round = find_matching_words(words_for_round, freq_score, guess,
                                                          get_answer_mask(target_word, guess))
                else:
                    game_results[target_words.index(target_word)] = ("X", target_word)
                    print("Game not won")
            self.simulation_results.append(game_results)
        return self.simulation_results

    def print_game_results(self, game_results):
        games_played = len(game_results)
        scores = []
        games_won = []
        for g, r in game_results.items():
            if r[0] != "X":
                games_won.append(r)
                scores.append(r[0])
        average = sum(scores) / len(games_won)
        games_lost = games_played - len(games_won)
        won_percentage = (len(games_won) / games_played) * 100
        print("Played " + str(games_played) + ": Won " + str(len(games_won)) + ": Lost " + str(
            games_lost) + ": Average round " + str(average))

    def print_game_simulations(self):
        for sim in self.simulation_results:
            print_game_results(sim)

wordle_solver = Solver(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt")
len(wordle_solver.words.copy())
wordle_solver.run_simulation(wordle_solver.words.copy(), 1, seed_words=["arose", "unlit", None, None, None, None])
wordle_solver.print_game_results(wordle_solver.simulation_results[0])

def run_simulation(words, target_words, sims, seed_words=[]):
    sim_results = []
    for sim in range(sims):
        game_results = {}
        for target_word in target_words:
            print("Target: " + target_word)
            words_for_round = words.copy()
            for game_round in range(6):
                guess = random.choice(words_for_round)

                # find seed word, which are favourite start words that you can use.
                seed = seed_words[game_round]
                if seed is not None:
                    guess = seed
                if guess in words_for_round:
                    words_for_round.remove(guess)
                print(str(game_round + 1) + ": " + guess)

                if guess == target_word:
                    print("Game Won on round " + str(game_round + 1))
                    game_results[target_words.index(target_word)] = (game_round + 1, target_word)
                    break

                words_for_round = find_matching_words(words_for_round, freq_score, guess, get_answer_mask(target_word, guess))
            else:
                game_results[target_words.index(target_word)] = ("X", target_word)
                print("Game not won")
        sim_results.append(game_results)
    return sim_results

seed_words = ["arose", "unlit", None, None, None, None]
sims = run_simulation(words.copy(), words.copy(), 10, ["arose", "unlit", None, None, None, None])
sims2 = run_simulation(words.copy(), words.copy(), 10, ["cones", "trial", None, None, None, None])
sims_no_seed = run_simulation(words.copy(), 10, [None, None, None, None, None, None])


def print_game_results(game_results):
    games_played = len(game_results)
    scores = []
    games_won = []
    for g, r in game_results.items():
        if r[0] != "X":
            games_won.append(r)
            scores.append(r[0])
    average = sum(scores) / len(games_won)
    games_lost = games_played - len(games_won)
    won_percentage = (len(games_won) / games_played) * 100
    print("Played " + str(games_played) + ": Won " + str(len(games_won)) + ": Lost " + str(games_lost) + ": Average round " + str(average))

print("results from using AROSE then UNLIT and first, second words.")
for sim in sims:
    print_game_results(sim)
print("results from using random starter words")
for sim in sims2:
    print_game_results(sim)



