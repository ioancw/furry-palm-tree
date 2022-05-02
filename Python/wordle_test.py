# %% imports
import random
from collections import Counter
from functools import reduce

# %% get the data

# load the words - this is the official mystery word list
words = []
for line in open(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt"):
    word = line.strip().lower()
    words.append(word)

# for now use another source of words, but we need to filter for 5 chars.
words = []
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

# %% get frequency scores

# use a counter example
# https://realpython.com/python-counter/
letter_counter = Counter()
for word in words:
    letter_counter.update(word)

letter_set = set()
for word in words:
    letters = list(word)
    for letter in letters:
        letter_set.add(letter)

freq_score = {}
for word in words:
    letters = list(word)
    freq = 0
    for letter in letters:
        freq += freqs[letter]
    freq_score[word] = freq

# %% further analysis on the distribution of letters

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

# %% finding letters in words

# not using reduce
for word in words:
    letters = list(word)
    matchedLetters = []
    for letter in list("auio"):
        matchedLetters.append(letter in letters)

    if all(matchedLetters):
        print(word)


# using reduce
def findWordsContainingLetter(wordList, letterList):
    matched = []
    for word in wordList:
        found = reduce(lambda s, l: s and l in list(word), letterList, True)
        if found:
            matched.append(word)
    return matched


findWordsContainingLetter(words, list("auio"))
findWordsContainingLetter(words, list("au"))


# find words matching a pattern, not using reduce
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


# find words matching a patter, but using reduce
def findWordsMatchingPattern2(wordList, pattern):
    """
    Finds words that match a pattern, e.g ARO__ or AR_SE
    """
    def compareLetter(state, comparison):
        check, actual = comparison
        return state and True if check == "_" else check == actual

    matched = []
    for word in wordList:
        comparisons = zip(list(pattern), list(word))
        matchedPattern = reduce(compareLetter, comparisons, True)

        if matchedPattern:
            matched.append(word)
    return matched


findWordsMatchingPattern2(words, "___er")
findWordsMatchingPattern(words, "___ar")
findWordsMatchingPattern(words, "___th")
findWordsMatchingPattern(words, "___in")
findWordsMatchingPattern(words, "a___r")
findWordsMatchingPattern(words, "__a_n")

# %% Find matching words based on the guess and guess mask


def get_answer_mask(actual, guess, compare_mode=False):
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
        if reduce(lambda s, mask: s and mask[1] not in list(word), zeros, True):
            grey.append(word)

    # find ones that match yellow mask
    for word in grey:
        ones = filter(lambda code: code[0] == 1, zip(mask, guess))
        found = reduce(lambda s, mask: s and mask[1] in list(word), ones, True)
        if found:
            yellow.append(word)

    # find ones that match exactly, i.e. green
    for word in yellow:
        twos = filter(lambda code: code[1][0] == 2, enumerate(zip(mask, guess)))
        found = reduce(lambda s, mask: s and mask[1][1] == word[mask[0]], twos, True)
        if found:
            green.append(word)

    final = green

    word_freqs = []
    for word in final:
        score = freqScore[word]
        word_freqs.append((score, word))
    word_freqs = sorted(word_freqs, reverse=True)

    return [w[1] for w in word_freqs]


# This doesn't work very well as it misses some words
def find_matching_words2(wordList, mask_function, guess, guess_mask):
    target_words = []
    for w in wordList:
        mask = mask_function(w, guess)
        new_mask = []
        for m in mask:
            if m == 2:
                m = 1
            new_mask.append(m)
        if new_mask == guess_mask:
            target_words.append(w)
    return target_words

# %% testing the find matching words function

small = ['cigar', 'arose', 'graph', 'ralph', 'rifts', 'arise', 'their', 'about', 'racer', 'pacer', 'radar', 'augur', 'altar']
guess = "arose"
guess = "lunar"
target = "cigar"
answer_mask = get_answer_mask(target, guess)
small_targets_1 = after1 = find_matching_words(words, freq_score, guess, answer_mask)
small_targets_2 = find_matching_words2(words, get_answer_mask, guess, answer_mask)

small_targets_1 = find_matching_words(small, freq_score, guess, answer_mask)
small_targets_2 = find_matching_words3(small, get_answer_mask, guess, answer_mask)

round1 = find_matching_words(words, freq_score, "arose", [1, 0, 0, 1, 0])
round1_0 = find_matching_words0(words, freq_score, "arose", [1, 0, 0, 1, 0])
round2 = find_matching_words(round1, freq_score, "unlit", [0, 1, 0, 0, 1])

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
after1 = find_matching_words(words, freq_score, "arose", [0,1,0,0,1])
after2 = find_matching_words(after1, freq_score, "unlit", [0,0,0,0,0])
after3 = find_matching_words(after2, freq_score, "cheer", [0,0,1,2,2])

after1_2 = find_matching_words2(words, get_answer_mask, "arose", get_answer_mask(tw, "arose"))




