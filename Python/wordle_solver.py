import random
from collections import Counter
from functools import reduce


class Solver:
    def __init__(self, words_file_path):
        self.words = []
        self.simulation_results = []
        for line in open(words_file_path):
            word = line.strip().lower()
            self.words.append(word)
        random.shuffle(self.words)
        self.letter_frequencies = self.get_letter_frequencies()
        self.word_frequencies = self.get_word_frequency_scores(self.letter_frequencies)

    def get_letter_frequencies(self):
        letter_counter = Counter()
        for word in self.words:
            letter_counter.update(word)
        return letter_counter

    def get_word_frequency_scores(self, letter_frequencies):
        freq_score = {}
        for word in self.words:
            letters = list(word)
            freq = 0
            for letter in letters:
                freq += letter_frequencies[letter]
            freq_score[word] = freq
        return freq_score

    @staticmethod
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

    @staticmethod
    def get_random_word(words):
        return random.choice(words)

    @staticmethod
    def get_most_frequent_word(words):
        return words[0]

    def find_matching_words(self, word_list, guess, mask):
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
            score = self.word_frequencies[word]
            word_freqs.append((score, word))
        word_freqs = sorted(word_freqs, reverse=True)

        return [w[1] for w in word_freqs]

    def run_simulation(self, target_words, sims, guess_function, seed_words=[], verbose=True):
        for sim in range(sims):
            game_results = {}
            N = 6
            for target_word in target_words:
                print("Target: " + target_word)
                words_for_round = self.words.copy()
                for game_round in range(1, N + 1):
                    seed = seed_words[game_round - 1]
                    guess = seed or guess_function(words_for_round)
                    # remove the word being used as the guess, as it could be chosen again.
                    if guess in words_for_round:
                        words_for_round.remove(guess)
                    if verbose:
                        print(str(game_round) + ": " + guess)

                    if guess == target_word:
                        print("Game Won on round " + str(game_round))
                        game_results[target_words.index(target_word)] = (game_round, target_word)
                        break

                    words_for_round = self.find_matching_words(words_for_round, guess, self.get_answer_mask(target_word, guess))
                else:
                    game_results[target_words.index(target_word)] = ("X", target_word)
                    print("Game not won")
            self.simulation_results.append(game_results)
        return self.simulation_results

    @staticmethod
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
        print("Played " + str(games_played) + ": Won " + str(len(games_won)) + ": Lost " + str(
            games_lost) + ": Average round " + str(average))

    def print_game_simulations(self):
        for sim in self.simulation_results:
            self.print_game_results(sim)


wordle_solver = Solver(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt")
wordle_solver.run_simulation(wordle_solver.words.copy(), 10, wordle_solver.get_random_word, seed_words=["arose", "unlit", None, None, None, None])
wordle_solver.print_game_simulations()

wordle_solver2 = Solver(r"C:\Users\ioan_\GitHub\furry-palm-tree\Python\wordle_words.txt")
wordle_solver2.run_simulation(wordle_solver2.words.copy(), 10, wordle_solver2.get_most_frequent_word, seed_words=["arose", "unlit", None, None, None, None])
wordle_solver2.print_game_simulations()

wordle_solver.words
wordle_solver.find_matching_words(wordle_solver.words, "modal", [0, 2, 2, 2, 0])
wordle_solver.find_matching_words(wordle_solver.words, "goaty", [0, 2, 1, 1, 2])

r1 = wordle_solver.find_matching_words(wordle_solver.words, "arose", [1, 0, 1, 0, 0])
r2 = wordle_solver.find_matching_words(r1, "unlit", [0, 0, 0, 0, 1])