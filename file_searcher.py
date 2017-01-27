#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Sat Dec 17 14:45:55 2016

@author: ioanwilliams
"""

import fnmatch
import os

images = ['*.jpg', '*.jpeg', '*.png', '*.tif', '*.tiff']
matches = []

for root, dirnames, filenames in os.walk(r'/Users/ioanwilliams/dropbox'):
    for extensions in images:
        for filename in fnmatch.filter(filenames, extensions):
            matches.append(os.path.join(root, filename))

count = len(matches)
print('Items found: %s' % count)

#for item in matches:
#    print(matches)