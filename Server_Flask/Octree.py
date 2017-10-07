from Node import Node
import math


class Octree(object):
    """docstring for Octree"""

    nodes = dict()
    tree = dict()
    labels = dict()
    label_count = 1
    cluster_size = dict()

    def __init__(self, graph):
        self.root = Node(size=4096)  # 2 ^ 12
        # self.root=Node(size=256)
        self.graph = graph

    def constructTree(self, root, depth):
        depth = depth - 1
        if (depth >= 0):
            root.subNode.append(
                Node(size=root.size / 2, x=root.x + root.size / 4, y=root.y + root.size / 4,
                     z=root.z + root.size / 4))
            root.subNode.append(
                Node(size=root.size / 2, x=root.x + root.size / 4, y=root.y + root.size / 4,
                     z=root.z - root.size / 4))
            root.subNode.append(
                Node(size=root.size / 2, x=root.x - root.size / 4, y=root.y + root.size / 4,
                     z=root.z - root.size / 4))
            root.subNode.append(
                Node(size=root.size / 2, x=root.x - root.size / 4, y=root.y + root.size / 4,
                     z=root.z + root.size / 4))
            root.subNode.append(
                Node(size=root.size / 2, x=root.x + root.size / 4, y=root.y - root.size / 4,
                     z=root.z + root.size / 4))
            root.subNode.append(
                Node(size=root.size / 2, x=root.x + root.size / 4, y=root.y - root.size / 4,
                     z=root.z - root.size / 4))
            root.subNode.append(
                Node(size=root.size / 2, x=root.x - root.size / 4, y=root.y - root.size / 4,
                     z=root.z - root.size / 4))
            root.subNode.append(
                Node(size=root.size / 2, x=root.x - root.size / 4, y=root.y - root.size / 4,
                     z=root.z + root.size / 4))

            self.constructTree(root.subNode[0], depth)
            self.constructTree(root.subNode[1], depth)
            self.constructTree(root.subNode[2], depth)
            self.constructTree(root.subNode[3], depth)
            self.constructTree(root.subNode[4], depth)
            self.constructTree(root.subNode[5], depth)
            self.constructTree(root.subNode[6], depth)
            self.constructTree(root.subNode[7], depth)

    def mapGraph2Tree(self, node, graph):
        if node.subNode:
            for sub in node.subNode:
                self.mapGraph2Tree(sub, graph)

        if graph != {}:
            (k, v) = graph.popitem()

            self.labels[v['tid']] = self.label_count
            self.label_count += 1
            self.cluster_size[v['tid']] = len(v['sub'])

            node.tid = v['tid']
            node.followers_count = v['followers_count']
            node.time = v['time']
            node.isEmpty = False

            ### sort the sub nodes according to the time stamp
            v['sub'].sort(key=lambda k: (k.get('time', 0)))

            if len(v['sub']) <= 8:
                self.constructTree(node, 1)
                for i in range(len(v['sub'])):
                    node.subNode[i].tid = v['sub'][i]['tid']
                    node.subNode[i].followers_count = v['sub'][i]['followers_count']
                    node.subNode[i].time = v['sub'][i]['time']
                    node.subNode[i].linked = v['tid']
                    node.subNode[i].isEmpty = False
            else:
                self.constructTree(node, math.ceil(math.log(len(v['sub']), 8)))
                pivot = 0
                stack = [node]
                while stack:
                    current_node = stack.pop()
                    if current_node.subNode:
                        stack.append(current_node.subNode[7])
                        stack.append(current_node.subNode[6])
                        stack.append(current_node.subNode[5])
                        stack.append(current_node.subNode[4])
                        stack.append(current_node.subNode[3])
                        stack.append(current_node.subNode[2])
                        stack.append(current_node.subNode[1])
                        stack.append(current_node.subNode[0])
                    else:
                        if pivot < len(v['sub']):
                            current_node.tid = v['sub'][pivot]['tid']
                            current_node.followers_count = v['sub'][pivot]['followers_count']
                            current_node.time = v['sub'][pivot]['time']
                            current_node.linked = v['tid']
                            current_node.isEmpty = False
                            pivot = pivot + 1

    def travel(self, node):
        stack = [node]
        while stack:
            current_node = stack.pop()
            if current_node.subNode:
                stack.append(current_node.subNode[7])
                stack.append(current_node.subNode[6])
                stack.append(current_node.subNode[5])
                stack.append(current_node.subNode[4])
                stack.append(current_node.subNode[3])
                stack.append(current_node.subNode[2])
                stack.append(current_node.subNode[1])
                stack.append(current_node.subNode[0])

            if not current_node.isEmpty:

                if current_node.linked is None:
                    self.nodes[current_node.tid] = {'tid': current_node.tid,
                                                    'followers_count': current_node.followers_count,
                                                    'time': current_node.time,
                                                    'x': current_node.x, 'y': current_node.y, 'z': current_node.z,
                                                    'linked': current_node.linked,
                                                    'label': self.labels[current_node.tid],
                                                    'size': self.cluster_size[current_node.tid]}
                else:
                    self.nodes[current_node.tid] = {'tid': current_node.tid,
                                                    'followers_count': current_node.followers_count,
                                                    'time': current_node.time,
                                                    'x': current_node.x, 'y': current_node.y, 'z': current_node.z,
                                                    'linked': current_node.linked,
                                                    'label': self.labels[current_node.linked],
                                                    'size': self.cluster_size[current_node.linked]}

    def travel_g(self, node):
        stack = [node]
        while stack:

            current_node = stack.pop()
            if current_node.subNode:
                stack.append(current_node.subNode[7])
                stack.append(current_node.subNode[6])
                stack.append(current_node.subNode[5])
                stack.append(current_node.subNode[4])
                stack.append(current_node.subNode[3])
                stack.append(current_node.subNode[2])
                stack.append(current_node.subNode[1])
                stack.append(current_node.subNode[0])

            if not current_node.isEmpty:
                if current_node.linked is None:
                    self.tree[current_node.tid] = {'tid': current_node.tid,
                                                   'followers_count': current_node.followers_count,
                                                   'time': current_node.time,
                                                   'x': current_node.x, 'y': current_node.y, 'z': current_node.z,
                                                   'subNode': [],
                                                   'label': self.labels[current_node.tid],
                                                   'size': self.cluster_size[current_node.tid]}

                else:

                    if current_node.linked in self.tree:
                        self.tree[current_node.linked]['subNode'].append({'tid': current_node.tid,
                                                                          'followers_count': current_node.followers_count,
                                                                          'time': current_node.time,
                                                                          'x': current_node.x, 'y': current_node.y,
                                                                          'z': current_node.z})
