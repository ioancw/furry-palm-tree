#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Thu Jan 12 21:40:30 2017

@author: ioanwilliams
"""

def sort_word(s):
    t = list(s).sort()
    t = ''.join(t)
    return t


def all_anagrams(filename):
    anagrams = {}
    for line in open(filename):
        word = line.strip().lower()
        sorted_word = sort_word(word)

        if sorted_word not in anagrams:
            anagrams[sorted_word] = [word]
        else:
            anagrams[sorted_word].append(word)
    return anagrams


def print_anagram_sets(d):
    for v in d.values():
        if len(v) > 1:
            print(len(v), v)


def print_anagram_sets_in_order(d):
    t = []
    for v in d.values():
        if len(v) > 1:
            t.append((len(v), v))

    t.sort()

    for x in t:
        print(x)


if __name__ == '__main__':
    anagram_map = all_anagrams('/Users/ioanwilliams/Documents/words.txt')
    print_anagram_sets_in_order(anagram_map)