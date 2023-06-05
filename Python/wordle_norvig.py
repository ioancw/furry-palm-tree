from dataclasses import dataclass
from collections import defaultdict, Counter
from typing import Dict, List, Counter, Union
from pathlib import Path
# https://docs.python.org/3/library/dataclasses.html

word_list = Path(r"wordle-small.txt").read_text().split()

Green, Yellow, Miss = "GY."
Word = str
Score = int
Reply = str
Partition = Dict[Reply, List[str]]

@dataclass
class Node:
    guess: Word
    size: int
    branches: Dict[Reply, 'Tree']

Tree = Union[Node, Word]

def reply_for(guess: Word, target: Word) -> Reply:
    "The five-character reply for this guess on this target."
    # We'll start by having each element of the reply be either Green or Miss ...
    reply = [Green if guess[i] == target[i] else Miss for i in range(5)]
    # ... then we'll change the elements that should be yellow
    counts = Counter(target[i] for i in range(5) if guess[i] != target[i])
    for i in range(5):
        if reply[i] == Miss and counts[guess[i]] > 0:
            counts[guess[i]] -= 1
            reply[i] = Yellow
    return ''.join(reply)

def partition(guess, targets) -> Partition:
    branches = defaultdict(list)
    for target in targets:
        branches[reply_for(guess, target)].append(target)
    return branches

def partition_counts(guess, targets) -> List[int]:
    "The sizes of the branches of a partition of targets by guess."
    counter = Counter(reply_for(guess, target) for target in targets)
    return sorted(counter.values())

reply_for("AROSE", "SKIRT")

few = word_list[::10] #every 100 elements
partition("AROSE", few)
partition_counts("ROAST", few)

def round(guess, actual, targets):
    partitions = partition(guess, targets)
    counts = partition_counts(guess, targets)
    print(guess, "Largest group:", max(counts), "Number of groups:", len(counts))
    reply = reply_for(guess, actual)
    remaining_words = partitions[reply]
    print("Remaining words", remaining_words)
    return partitions, remaining_words
# January 2nd 23
#round 1
partitions_r1, words_r1 = round('AROSE', 'SKIRT', word_list)
#round 2
partitions_r2, words_r2 = round('SHIRT', 'SKIRT', words_r1)
# maximise the number of groups and minimise the largest group

results = []
for target in few:
    counts = partition_counts(target, few)
    results.append((target, max(counts), len(counts)))
    print(target, max(counts), len(counts))
guess = min(few, key=lambda guess: max(partition_counts(guess, few)))
print("Guess", guess)
results.sort(key=lambda y: y[1])

def minimizing_tree(metric, targets) -> Tree:
    """Make a tree that picks guesses that minimize metric(partition_counts(guess, targets))."""
    if len(targets) == 1:
        return targets[0]
    else:
        guess = min(targets, key=lambda guess: metric(partition_counts(guess, targets)))
        branches = partition(guess, targets)
        return Node(guess, len(targets),
                    {reply: minimizing_tree(metric, branches[reply])
                     for reply in branches})

test_tree = minimizing_tree(max, few)

