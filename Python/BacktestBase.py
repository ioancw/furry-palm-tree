#
# Python Script with Base Class
# for Event-based Backtesting
#
# Python for Algorithmic Trading
# (c) Dr. Yves J. Hilpisch
# The Python Quants GmbH
#
import math
import numpy as np
import pandas as pd
from pandas_datareader import data as web
import matplotlib.pyplot as plt
import seaborn as sns
sns.set()


class BacktestBase(object):
    ''' Base class for event-based backtesting of trading strategies.

    Attributes
    ==========
    symbol: str
        symbol on which to implement the backtesting
    amount: float
        amount to be invested either once or per trade
    ftc: float
        fixed transaction costs per trade (buy or sell)
    ptc: float
        proportional transaction costs per trade (buy or sell)

    Methods
    =======
    get_data:
        retrieves and prepares the base data set
    plot_data:
        plots the closing price for the symbol
    print_balance:
        prints out the current (cash) balance
    get_date_price:
        returns the date and price for the given bar
    place_buy_order:
        places a buy order
    place_sell_order:
        places a sell order
    close_out:
        closes out a long or short position
    '''

    def __init__(self, symbol, start, end, amount, ftc=0.0, ptc=0.0):
        self.symbol = symbol
        self.start = start
        self.end = end
        self._amount = amount
        self.amount = amount
        self.ftc = ftc
        self.ptc = ptc
        self.units = 0
        self.position = 0
        self.trades = 0
        self.verbose = True
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

    def plot_data(self):
        ''' Plots the (adjusted) closing prices for symbol.
        '''
        self.data['price'].plot(figsize=(10, 6), title=self.symbol)

    def print_balance(self, date=''):
        ''' Print out current cash balance info.
        '''
        print('%s | current cash balance %8d' % (date[:10], self.amount))

    def get_date_price(self, bar):
        ''' Return date and price for bar.
        '''
        date = str(self.data.index[bar])
        price = self.data.price.ix[bar]
        return date, price

    def place_buy_order(self, bar, units=None, amount=None):
        ''' Place a buy order.
        '''
        date, price = self.get_date_price(bar)
        if units is None:
            units = math.floor(amount / price)
        # reduce current cashbalance by number of units bought and transaction
        # cost
        self.amount -= (units * price) * (1 + self.ptc) + self.ftc
        # number of shares
        self.units += units
        self.trades += 1
        if self.verbose:
            print('%s | buying  %4d units at %7.2f' %
                  (date[:10], units, price))
            self.print_balance(date)

    def place_sell_order(self, bar, units=None, amount=None):
        ''' Place a sell order.
        '''
        date, price = self.get_date_price(bar)
        if units is None:
            units = math.floor(amount / price)
        self.amount += (units * price) * (1 - self.ptc) - self.ftc
        self.units -= units
        self.trades += 1
        if self.verbose:
            print('%s | selling %4d units at %7.2f' %
                  (date[:10], units, price))
            self.print_balance(date)

    def close_out(self, bar):
        ''' Closing out a long or short position.
        '''
        date, price = self.get_date_price(bar)
        self.amount += self.units * price
        if self.verbose:
            print('%s | inventory %d units at %.2f' % (date[:10],
                                                       self.units, price))
        if self.verbose:
            print('=' * 55)
        print('Final balance   [$] %13.2f' % self.amount)
        print('Net Performance [%%] %13.2f' % ((self.amount - self._amount) /
                                               self._amount * 100))
        print('=' * 55)


if __name__ == '__main__':
    bb = BacktestBase('DIS', '2010-1-1', '2016-10-31', 10000)
    print(bb.data.info())
    print(bb.data.tail())
    bb.plot_data()
    plt.savefig('/Users/ioanwilliams/Documents/backtestbaseplot.pdf')
