import random
import re
from collections import Counter, defaultdict


def get_words(doc):
    doc = doc.lower()
    words = re.findall("[a-z0-9']+", doc)
    return set(words)


class Classifier:
    def __init__(self, get_features, filename=None):
        self.fc = {}
        self.cc = {}
        self.get_features = get_features

    # Increase the count of a feature/category pair
    def add_feature(self, feature, cat):
        self.fc.setdefault(feature, {})
        self.fc[feature].setdefault(cat, 0)
        self.fc[feature][cat] += 1

    # Increase the count of a category
    def add_category(self, cat):
        self.cc.setdefault(cat, 0)
        self.cc[cat] += 1

    # The number of times a feature has appeared in a category
    def feature_count(self, f, cat):
        if f in self.fc and cat in self.fc[f]:
            return float(self.fc[f][cat])
        return 0.0

    # The number of items in a category
    def category_count(self, cat):
        if cat in self.cc:
            return float(self.cc[cat])
        return 0

    # The total number of items
    def total_count(self):
        return sum(self.cc.values())

    # The list of all categories
    def categories(self):
        return self.cc.keys()

    def train(self, item, cat):
        features = self.get_features(item)
        # Increment the count for every feature with this category
        for f in features:
            self.add_feature(f, cat)

        # Increment the count for this category
        self.add_category(cat)

    def f_prob(self, f, cat):
        if self.category_count(cat) == 0:
            return 0

        # The total number of times this feature appeared in this
        # category divided by the total number of items in this category
        return self.feature_count(f, cat) / self.category_count(cat)

    def weighted_prob(self, f, cat, prf, weight=1.0, ap=0.5):
        # Calculate current probability
        basic_prob = prf(f, cat)

        # Count the number of times this feature has appeared in
        # all categories
        totals = sum([self.feature_count(f, c) for c in self.categories()])

        # Calculate the weighted average
        bp = ((weight * ap) + (totals * basic_prob)) / (weight + totals)
        return bp


class NaiveBayes(Classifier):
    def __init__(self, get_features):
        Classifier.__init__(self, get_features)
        self.thresholds = {}

    def docprob(self, item, cat):
        features = self.get_features(item)

        # Multiply the probabilities of all the features together
        p = 1
        for f in features:
            p *= self.weighted_prob(f, cat, self.f_prob)
        return p

    def prob(self, item, cat):
        catprob = self.category_count(cat) / self.total_count()
        docprob = self.docprob(item, cat)
        return docprob * catprob

    def setthreshold(self, cat, t):
        self.thresholds[cat] = t

    def getthreshold(self, cat):
        if cat not in self.thresholds:
            return 1.0
        return self.thresholds[cat]

    def classify(self, item, default=None):
        probs = {}
        # Find the category with the highest probability
        max = 0.0
        for cat in self.categories():
            probs[cat] = self.prob(item, cat)
            if probs[cat] > max:
                max = probs[cat]
                best = cat

        # Make sure the probability exceeds threshold*next best
        for cat in probs:
            if cat == best:
                continue
            if probs[cat] * self.getthreshold(best) > probs[best]:
                return default
        return best


def sample_train(cl):
    cl.train('Nobody owns the water.', 'good')
    cl.train('the quick rabbit jumps fences', 'good')
    cl.train('buy pharmaceuticals now', 'bad')
    cl.train('make quick money at the online casino', 'bad')
    cl.train('the quick brown fox jumps', 'good')


def split_data(data, prob):
    """split data into fractions [prob, 1 - prob]"""
    results = [], []
    for row in data:
        results[0 if random.random() < prob else 1].append(row)
    return results


def spam_train():
    cl = NaiveBayes(get_words)
    file = r"/Users/ioanwilliams/Downloads/smsspamcollection/SMSSpamCollection"
    messages = [line.rstrip() for line in open(file)]

    data_set = []
    # format out output data is (text, classifier)
    for message_no, message in enumerate(messages):
        indicator, message = re.split(r'\t', message)
        data_set.append((message, indicator == "spam"))

    train_data, test_data = split_data(data_set, 0.75)

    for (text, cat) in train_data:
        cl.train(text, cat)

    results = [(text, cat, cl.classify(text)) for text, cat in test_data]
    result_counts = Counter((cat, predict) for _, cat, predict in results)
    return result_counts
