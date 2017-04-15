#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Wed Apr 12 21:20:50 2017

@author: ioanwilliams
"""

import queue as queue

def event_handler(event_queue):
    while event_queue.qsize() >= 1:
        try:
            event = event_queue.get(False)
        except queue.Empty:
            print("Empty queue")
        else:
            if event is not None:
                print(event)

def add_events(event_queue):
    for i in range(10):
        event_queue.put("message " + str(i))

event_queue = queue.Queue()
event_queue.put("test")
add_events(event_queue)
event_handler(event_queue)