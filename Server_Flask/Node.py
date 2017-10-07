class Node(object):
    """docstring for Node"""
    def __init__(self, size=0, x=0, y=0, z=0, tid=0, followers_count=0, time='0', linked=None, isEmpty=True, subNode=None):
        self.size = size
        self.x = x
        self.y = y
        self.z = z
        self.tid = tid
        self.followers_count = followers_count
        self.time = time
        self.linked = linked
        self.isEmpty = isEmpty
        if subNode is None:
            subNode = []
        self.subNode = subNode