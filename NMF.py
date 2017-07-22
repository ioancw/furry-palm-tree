# -*- coding: utf-8 -*-
"""
Created on Mon Jul 17 12:25:37 2017

@author: iwilliam
"""
from nltk.stem.porter import PorterStemmer
import re
import string
import math
from sklearn.feature_extraction.text import TfidfVectorizer, CountVectorizer
from sklearn.decomposition import NMF, LatentDirichletAllocation
from collections import defaultdict

import pandas as pd
import numpy as np

def tokenise(message):
    message = message.lower()                       # convert to lowercase
    all_words = re.findall("[a-z0-9']+", message)   # extract the words
    return all_words

def count_words(training_set, stop_words):
    counts = defaultdict(lambda: 0)
    for message in training_set:
        for word in tokenise(message):
            if word not in stop_words:
                counts[word] += 1
    return (counts, set(counts))

def sorted_counts(word_counts):
    sorted_counts = []
    for word, count in word_counts.items():
        sorted_counts.append((count,word))
    sorted_counts.sort(key=lambda tup: tup[0], reverse = True)
    return sorted_counts

def display_topics(model, feature_names, no_top_words):
    for topic_idx, topic in enumerate(model.components_):
        print("Topic %d:" % (topic_idx))
        print(" ".join([feature_names[i] 
                        for i in topic.argsort()[:-no_top_words - 1:-1]]))
    
def print_top_words(model, feature_names, n_top_words):
    for topic_idx, topic in enumerate(model.components_):
        print("--------------- " + "Topic #%d:" % topic_idx + " ---------------")
        print(''.join([feature_names[i] + ' ' + str(round(topic[i], 2))
              + ' | ' for i in topic.argsort()[:-n_top_words - 1:-1]]))
        print("")
        
def display_topics2(H, W, feature_names, documents, no_top_words, no_top_documents):
    for topic_idx, topic in enumerate(H):
        print("Topic %d:" % (topic_idx))
        print(" ".join([feature_names[i]
                        for i in topic.argsort()[:-no_top_words - 1:-1]]))
        top_doc_indices = np.argsort( W[:,topic_idx] )[::-1][0:no_top_documents]
        
        for doc_index in top_doc_indices:
            print(documents[doc_index])


#my_file = open('m:\emails.txt','w')
#my_file.close()
#
#import win32com.client
#
#outlook = win32com.client.Dispatch("Outlook.Application").GetNamespace("MAPI")
#
#inbox = outlook.GetDefaultFolder(6)
#
#messages = inbox.Items
#
#messages = [message.subject for message in messages]
#
#messages_set = set(messages)
#
#for m in messages_set:
#    my_file.write("%s\n" % m)
#
#my_file.close()

documents = [line.strip() for line in open('m:\Churchill.txt')]

#pass stop words to relevant model
stop_words_file = r"m:\stopwords.txt"
stop_words = [line.strip() for line in open(stop_words_file)]

text_post_stemmer = []
stemmer = PorterStemmer()
for doc in documents:
    tokens = tokenise(doc)
    stemmed = []
    
    for token in tokens:
        token = token.strip()
        if len(token) > 4:
            stemmed.append(stemmer.stem(token))
        else:
            stemmed.append(token)      
        
    text_post_stemmer.append(' '.join(stemmed))

documents_post_stemmer = text_post_stemmer
words_count = sorted_counts(count_words(documents_post_stemmer, stop_words)[0])

mingram,maxgram = 1,2
### LDA/NMF inputs    
n_features = 100
tfidf_vectorizer = TfidfVectorizer(#tokenizer=tokenise,
                                   #max_df = 0.95,
                                   #min_df = 1,
                                   max_features = n_features,
                                   #stop_words = stop_words,
                                   lowercase = True,
                                   #ngram_range = (mingram,maxgram)
                                   )  

tfidf = tfidf_vectorizer.fit_transform(documents)
pd.DataFrame(tfidf.toarray(),columns = tfidf_vectorizer.get_feature_names()).to_csv(r"M:\matrix_test.csv")

n_topics = 5
nmf = NMF(n_components=n_topics,
          random_state=1,
          alpha=.1,
          l1_ratio=.5).fit(tfidf)

tfidf_feature_names = tfidf_vectorizer.get_feature_names()
display_topics(nmf, tfidf_feature_names, 10)

nmf_W = nmf.transform(tfidf)
nmf_H = nmf.components_
#display_topics2(nmf_H,nmf_W, tfidf_feature_names, documents, 15, 5)

tokenised_docs = [tokenise(d) for d in documents]

tokenize = lambda doc: doc.lower().split(" ")

document_0 = "China has a strong economy that is growing at a rapid pace. However politically it differs greatly from the US Economy."
document_1 = "At last, China seems serious about confronting an endemic problem: domestic violence and corruption."
document_2 = "Japan's prime minister, Shinzo Abe, is working towards healing the economic turmoil in his own country for his view on the future of his people."
document_3 = "Vladimir Putin is working hard to fix the economy in Russia as the Ruble has tumbled."
document_4 = "What's the future of Abenomics? We asked Shinzo Abe for his views"
document_5 = "Obama has eased sanctions on Cuba while accelerating those against the Russian Economy, even as the Ruble's value falls almost daily."
document_6 = "Vladimir Putin was found to be riding a horse, again, without a shirt on while hunting deer. Vladimir Putin always seems so serious about things - even riding horses."

all_documents = [document_0, document_1, document_2, document_3, document_4, document_5, document_6]


def tf_idf(documents):
    tokenized_documents = [tokenize(d) for d in documents]
    idf = inverse_document_frequencies(tokenized_documents)
    tfidf_documents = []
    for document in tokenized_documents:
        doc_tfidf = []
        for term in idf.keys():
            tf = sublinear_term_frequency(term, document)
            doc_tfidf.append(tf * idf[term])
        tfidf_documents.append(doc_tfidf)
    return tfidf_documents

def inverse_document_frequencies(tokenized_documents):
    idf_values = {}
    all_tokens_set = set([item for sublist in tokenized_documents for item in sublist])
    for tkn in all_tokens_set:
        contains_token = map(lambda doc: tkn in doc, tokenized_documents)
        idf_values[tkn] = 1 + math.log(len(tokenized_documents)/(sum(contains_token)))
    return idf_values

def sublinear_term_frequency(term, tokenized_document):
    count = tokenized_document.count(term)
    if count == 0:
        return 0
    return 1 + math.log(count)

tfidf_representation = tf_idf(all_documents)
print(tfidf_representation[0], document_0)

#http://scikit-learn.org/stable/modules/feature_extraction.html