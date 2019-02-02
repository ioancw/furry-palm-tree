import pytest
from graph import Graph


def f_sum(a, b):
    return a + b


def f_minus(a, b):
    return a - b


def test1():

    graph = Graph()

    def f_divby10(c):
        return c / 10.0

    graph.add_node(f_sum, inputs=['a', 'b'], outputs=['output_sum'])
    graph.add_node(f_divby10, inputs=['output_sum'], outputs=['outout_div'])
    graph.add_node(f_minus, inputs=['output_div', 'a'], outputs=['e'])

    # set the value of the initial nodes.
    res = graph.calculate(data={'a': 2, 'b': 3})
    assert res == -1.5

def test_withdecorator():
    graph = Graph()

    @graph.register(inputs=['x', 'y'], outputs=['z'])
    def f_add(a, b):
        return a + b

    @graph.register(inputs=['z'], outputs=['c'])
    def f_sq(z):
        return z*z

    res = graph.calculate(data={'x': 2, 'y': 3})
    assert res == 24


def test_iterate_on_single_output():
    graph = Graph()

    def f_test(a, b):
        return list(range(a) + [b])

    graph.add_node(f_test, inputs=['a', 'b'], outputs=['c'])
    res = graph.calculate(data={'a': 2, 'b': 3})
    assert res == [0, 1, 3]
    assert graph.data['c'] == [0, 1, 3]
