import math
import datetime
import calendar
import re


def lastday_of_month(d):
    '''Takes a datetime.date and returns the date for last day in month'''
    last_day = calendar.monthrange(d.year, d.month)
    return datetime.date(d.year, d.month, last_day[1])


def get_futures_year(expiry_code, today_date):
    '''Determines the date of the 1st day of expiry month and year'''
    futures_codes = ['F', 'G', 'H', 'J', 'K',
                     'M', 'N', 'Q', 'U', 'V', 'X', 'Z']

    match = re.match(
        r'^(?P<month>[FGHJKMNQUVXZ])(?P<year>[0-9][0-9]?)$', expiry_code)

    if not match:
        raise Exception('Expiry code not in correct format.')

    month_string = match.group('month')
    year_string = match.group('year')

    year_part = int(year_string)
    month_part = futures_codes.index(month_string) + 1

    years_mod = 10**len(year_string)

    new_year = today_date.year - today_date.year % years_mod + year_part
    expiry_date = datetime.date(new_year, month_part, 1)

    if lastday_of_month(expiry_date) < today_date:
        rolled_year = expiry_date.year + years_mod
        expiry_date = datetime.date(
            rolled_year, expiry_date.month, expiry_date.day)
    return expiry_date


def get_two_digit_code(expiry_code, today_date, twodigit_year_enabled):
    futures_expiry_year = get_futures_year(expiry_code, today_date)

    if futures_expiry_year.year < twodigit_year_enabled:
        return expiry_code

    return expiry_code[0] + str(futures_expiry_year.year % 100)


assert get_futures_year('H28', datetime.date(2018, 1, 28)).year == 2028
assert get_futures_year('H8', datetime.date(2018, 4, 28)).year == 2028
assert get_futures_year('H0', datetime.date(2018, 4, 28)).year == 2020
assert get_futures_year('H8', datetime.date(2018, 2, 28)).year == 2018
assert get_futures_year('H8', datetime.date(2018, 3, 2)).year == 2018
assert get_futures_year('H8', datetime.date(2018, 3, 26)).year == 2018
assert get_futures_year('H8', lastday_of_month(datetime.date(2018, 3, 2))).year == 2018
assert get_futures_year('H28', lastday_of_month(datetime.date(2018, 3, 2))).year == 2028
assert get_futures_year('H8', datetime.date(2018, 4, 1)).year == 2028
assert get_two_digit_code('H8', datetime.date(2018, 3, 26), 2024) == 'H8'
assert get_two_digit_code('H8', datetime.date(2018, 4, 26), 2024) == 'H28'
assert get_two_digit_code('H4', datetime.date(2018, 1, 28), 2024) == 'H24'
assert get_two_digit_code('H4', datetime.date(2014, 3, 26), 2024) == 'H4'
assert get_two_digit_code('H4', datetime.date(2014, 4, 1), 2024) == 'H24'
assert get_two_digit_code('H4', datetime.date(2024, 4, 1), 2024) == 'H34'

print(2024 % 100)
