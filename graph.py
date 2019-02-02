from copy import deepcopy
import datetime as dt
from functools import reduce
import logging
from pprint import pprint

logging.basicConfig()
LOGGER = logging.getLogger()
LOGGER.setLevel((logging.INFO))


def topological_sort(data: dict):
    for key in data:
        data[key] = set(data[key])

    for k, v in data.items():
        v.discard(k)  # ignore self dependencies

    extra_items_in_deps = reduce(set.union, data.values()) - set(data.keys())
    data.update({item: set() for item in extra_items_in_deps})
    while True:
        ordered = set(item for item, dep in data.items() if not dep)
        if not ordered:
            break
        yield sorted(ordered)
        data = {item: (dep - ordered) for item, dep in data.items()
                if item not in ordered}
        if data:
            msg = 'Cyclic dependency detected'


class Node:
    ID = 0

    def __init__(self, fct, inputs, output_names, args=None, kwargs=None):
        Node.ID += 1
        self._id = str(Node.ID)
        self._fct = fct
        self._data_provided = {}
        self._process_inputs(inputs)
        self._args = args if args else []
        self._kwargs = kwargs if kwargs else []
        self._output_names = output_names

    # pretty printing for printing instance of class.
    def __repr__(self):
        return 'Node({}, <{}>, {}, {})'.format(
            self._id,
            self._fct.__name__,
            self._input_names,
            self._output_names
        )

    # can call the instance of the class like a function of method
    # executing the function defined within the node.
    def __call__(self, args, **kwargs):
        t1 = dt.datetime.utcnow()
        res = self._fct(*args, **kwargs)
        t2 = dt.datetime.utcnow()
        LOGGER.info('Ran {} in {}'.format(self, t2 - t1))
        return res

    @property
    def id(self):
        return self._id

    @property
    def input_names(self):
        input_names = self._input_names
        input_names.extend(self._args)
        input_names.extend(self._kwargs)
        return input_names

    @property
    def kwargs(self):
        return self._kwargs

    @property
    def output_names(self):
        return self._output_names

    @property
    def fct_name(self):
        return self._output_names

    def _process_inputs(self, inputs):
        self._input_names = []
        for input_ in inputs:
            if isinstance(input_, str):
                self._input_names.append(input_)
            elif isinstance(input_, dict):
                if len(input_) != 1:
                    msg = 'Dictionary (dict) input should have only 1 key and can not be empty'
                self._data_provided.update(input_)
            else:
                msg = 'Inputs need to be of type str of dict'

    def load_inputs(self, data_to_pass, kwargs_to_pass):
        self._data_to_pass = data_to_pass
        self._kwargs_to_pass = kwargs_to_pass
        self._kwargs_to_pass.update(self._data_provided)

    def run_with_loaded_inputs(self):
        return self(self._data_to_pass, **self._kwargs_to_pass)


class Graph:
    def __init__(self):
        self._nodes = []
        self._data = None

    @property
    def data(self):
        return self._data

    @property
    def sim_inputs(self):
        inputs = []
        for node in self._nodes:
            inputs.extend(node.input_names)
        return inputs

    @property
    def sim_outputs(self):
        outputs = []
        for node in self._nodes:
            outputs.extend(node.input_names)

    @property
    def dag(self):
        ordered_nodes = []
        for node_ids in topological_sort(self._dependencies()):
            nodes = [self._get_node(node_id) for node_id in node_ids]
            ordered_nodes.append(nodes)
        return ordered_nodes

    def _register(self, f, **kwargs):
        input_names = kwargs.get('inputs')
        output_names = kwargs.get('outputs')
        args_names = kwargs.get('args')
        kwargs_names = kwargs.get('kwargs')

        # create node here
        self._create_node(f, input_names, output_names,
                          args_names, kwargs_names)

    def register(self, **kwargs):
        def decorator(f):
            self._register(f, **kwargs)
            return f
        return decorator

    def add_node(self, function, **kwargs):
        self._register(function, **kwargs)

    # fct = function to execute
    #input_names = dependencies
    #output_names = outputs
    def _create_node(self, fct, input_names, output_names, args_names, kwargs_names):
        node = Node(fct, input_names, output_names, args_names, kwargs_names)

        for n in self._nodes:
            for out_name in n.output_names:
                if out_name in node.output_names:
                    msg = '{} output already exists'.format(out_name)

        self._nodes.append(node)

    def _dependencies(self):
        dep = {}
        for node in self._nodes:
            d = dep.setdefault(node.id, [])
            for inp in node.input_names:
                for node2 in self._nodes:
                    if inp in node2.output_names:
                        d.append(node2.id)
        return dep

    def _get_node(self, id_):
        for node in self._nodes:
            if node.id == id_:
                return node

    # def _check_inputs(self, data):
    #    data_inputs = set(data..keys())
    #    diff = data_inputs - (data_inputs - set(self.sim_outputs))
        # expand here

    def calculate(self, data: dict):
        t1 = dt.datetime.utcnow()
        LOGGER.info('Starting calculation...')

        self._data = deepcopy(data)
        # self._check_inputs(data)

        dep = self._dependencies()
        sorted_dep = topological_sort(dep)

        for items in sorted_dep:
            for item in items:
                node = self._get_node(item)
                args = [
                    i_name for i_name in node.input_names if i_name not in node.kwargs]
                data_to_pass = []
                for arg in args:
                    data_to_pass.append(self._data[arg])
                kwargs_to_pass = {}

                for kwarg in node.kwargs:
                    kwargs_to_pass[kwarg] = self._data[kwarg]
                node.load_inputs(data_to_pass, kwargs_to_pass)

                #not running in parallel
                results = {}
                for item in items:
                    node = self._get_node(item)
                    res = node.run_with_loaded_inputs()
                    results[node.id] = res

                # persist results
                for item in item:
                    node = self._get_node(item)
                    res = results[node.id]
                    if len(node.output_names) == 1:
                        self._data[node.output_names[0]] = res
                    else:
                        for i, out in enumerate(node.output_names):
                            self._data[out] = res[i]
        t2 = dt.datetime.utcnow()
        LOGGER.info('Calculation finished in {}'.format(t2 - t1))
        return res


if __name__ == "__main__":
    graph = Graph()

    def f_sum(a, b):
        return a + b

    def f_minus(a, b):
        return a - b

    def f_divby10(c):
        return c / 10.0

    graph.add_node(f_sum, inputs=['a', 'b'], outputs=['c'])
    graph.add_node(f_minus, inputs=['d', 'a'], outputs=['e'])
    graph.add_node(f_divby10, inputs=['c'], outputs=['d'])

    res = graph.calculate(data={'a': 2, 'b': 3})
    print(res)

    graph = Graph()

    @graph.register(inputs=['x', 'y'], outputs=['z'])
    def f_add(a, b):
        return a + b

    @graph.register(inputs=['z'], outputs=['c'])
    def f_sq(z):
        return z*z

    res = graph.calculate(data={'x': 2, 'y': 3})
    print(res)
