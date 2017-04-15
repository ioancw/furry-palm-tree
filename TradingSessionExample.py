#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Created on Fri Apr 14 16:33:38 2017

@author: ioanwilliams
"""

import queue as queue
import pandas as pd
import numpy as np
from pandas_datareader import data as web

class YahooDailyBarHandler(object):
    
    def __init__(self, ticker, start_date, end_date, event_queue):
        self.symbol = ticker
        self.start = start_date
        self.end = end_date
        self.event_queue = event_queue
        self.continue_backtest = True
        self.get_data()

    def get_data(self):
        ''' Retrieves and prepares the data.
        '''
        raw = web.DataReader(self.symbol, data_source='yahoo',
                             start=self.start, end=self.end)['Adj Close']
        raw = pd.DataFrame(raw)
        raw.rename(columns={'Adj Close': 'price'}, inplace=True)
        raw['return'] = np.log(raw / raw.shift(1))
        self.data = raw.dropna()

    def stream_next(self):
        """
        Place the next BarEvent onto the event queue.
        """
        self.event_queue.put("Test Message")
        self.continue_backtest = False


class TradingSession(object):

    def __init__(self, event_queue, start_date, end_date, ticker):
        self.ticker = ticker
        self.start_date = start_date
        self.end_date = end_date
        self.event_queue = event_queue
        self.price_handler = YahooDailyBarHandler(self.ticker, 
                                                  self.start_date,
                                                  self.end_date, 
                                                  self.event_queue)
        
    def start_trading(self):
        self._run_session()
        
    def _run_session(self):
        while self.price_handler.continue_backtest:
            try:
                event = event_queue.get(False)
            except queue.Empty:
                print('Next')
                self.price_handler.stream_next()
            else:
                if event is not None:
                    print(event)

event_queue = queue.Queue()
trading_session = TradingSession(event_queue, '2010-01-01','2015-01-01','DIS')
trading_session.start_trading()
        