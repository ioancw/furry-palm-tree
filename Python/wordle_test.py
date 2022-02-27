# two words and compare them.

wordle_word = "TIMBER"
guess = "AROSE"

# all words are of length 5, check this.

# for i in range(5):
#     if guess[i] == wordle_word[i]:

from collections import Counter
# create a dict of words
freqs = {}
for line in open(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt"):
    word = line.strip().lower()
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
for line in open(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt"):
    word = line.strip().lower()
    letter_counter.update(word)
letter_counter['e']

# go through all words and now find out the frequency score for each.

# are there any letters not present in any of the words.
letter_set = set()
for line in open(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt"):
    word = line.strip().lower()
    letters = list(word)
    for l in letters:
        letter_set.add(l)
# all letters are present.

freq_score = {}
for line in open(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt"):
    word = line.strip().lower()
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
for line in open(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt"):
    word = line.strip().lower()
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

all_words = []
for line in open(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt"):
    word = line.strip().lower()
    all_words.append(word)
whys = []
for line in open(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt"):
    word = line.strip().lower()
    letters = list(word)
    if letters[4] == 'y':
        whys.append(word)
