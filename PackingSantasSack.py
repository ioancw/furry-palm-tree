#!/usr/bin/env python3
# -*- coding: utf-8 -*-
import time
import numpy

class Sack(object):
    def __init__(self):
        self.boxes = []
        self.sum = 0

    def add_box(self, item):
        self.boxes.append(item)
        self.sum += item

    def __str__(self):
        return 'Sack(volume=%d, boxes=%s)' % (self.sum, self.boxes)


def pack_sleigh(boxes, volume, sort_boxes, sort_sacks):
    # sort boxes so that biggest boxes are packed first.
    if sort_boxes:
        boxes = sorted(boxes, reverse=True)
    sleigh = []

    for box in boxes:
        for sack in sleigh:
            if sack.sum + box <= volume:
                sack.add_box(box)|"
                brea
        else:
            sack = Sack()
            sack.add_box(box)
            sleigh.append(sack)
            # sort sacks so largest sacks get filled first.
            if sort_sacks:
                sleigh.sort(key=sack_volume, reverse=True)

    return sleigh


def sack_volume(sack):
    return sack.sum


if __name__ == '__main__':
    def pack(boxes, volume, sort, sort_sacks):
        sleigh = pack_sleigh(boxes, volume, sort, sort_sacks)
        return len(sleigh)


    print('--')
    method1 = []
    method2 = []
    method3 = []
    startTime = time.time()
    volume = 1000000
    for simulation in range(1, 1000001):
        randomVolumeBoxes = numpy.random.randint(1,volume,90)
        method1.append(pack(randomVolumeBoxes, volume, True, False))
        #method2.append(pack(randomVolumeBoxes, volume, True, True))
        #method3.append(pack(randomVolumeBoxes, volume, False, False))
        #print('--')

    endTime = time.time()
    elapsed = endTime - startTime
    print('Time: ', elapsed)
    print('Simulated ', simulation)
    print('Method1 boxes:', sum(method1) / len(method1))
    #print('Method2 boxes:', sum(method2) / len(method2))
    #print('Method3 boxes:', sum(method3) / len(method3))
