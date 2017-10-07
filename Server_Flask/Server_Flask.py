from flask import Flask, url_for, jsonify, Response
import json
from Octree import Octree
import pymongo
import time
import math
import email.utils
import os
import threading
import sys
import copy

app = Flask(__name__)

###### preparation when server is starting ########

# Connect to the MongoDB
client = pymongo.MongoClient('mongodb://127.0.0.1:27017/twitter')
db = client.twitter
tweets = db.tweets

# Check if the data has been cached already
nodes = None
tree = None
graph = None
nodes_file_path = os.path.join(app.root_path, 'cache', 'nodes.json')
tree_file_path = os.path.join(app.root_path, 'cache', 'tree.json')
graph_file_path = os.path.join(app.root_path, 'cache', 'graph.json')

try:
    with open(nodes_file_path, 'r') as f:
        nodes = json.load(f)
    with open(graph_file_path, 'r') as f:
        graph = json.load(f)
    with open(tree_file_path, 'r') as f:
        tree = json.load(f)
except Exception as e:
    print(e)
    print('Cache file may not exist...')

isModified = False
isUpdating = False


############ View functions ################

@app.route('/')
def index():
    return 'Data visualization with VR server is running...'


@app.route('/init')
def init_all_structure():
    global nodes
    global graph
    global isModified

    graph_temp = dict()
    for tweet in tweets.find():
        if 'user' in tweet:
            tid = tweet['id_str']
            followers_count = tweet['user']['followers_count']
            time_str = tweet['created_at']
            create_time = time.mktime(email.utils.parsedate(time_str))

            if tid not in graph_temp:
                graph_temp[tid] = {'tid': tid, 'followers_count': followers_count, 'time': create_time, 'sub': []}

            if 'retweeted_status' in tweet:

                original_tweet_id = tweet['retweeted_status']['id_str']

                if original_tweet_id in graph_temp and original_tweet_id != tid:
                    conflic = False
                    for subs in graph_temp[original_tweet_id]['sub']:
                        if subs['tid'] == tid:
                            conflic = True
                    if not conflic:
                        graph_temp[original_tweet_id]['sub'].append(
                            {'tid': tid, 'followers_count': followers_count, 'time': create_time})
                elif original_tweet_id not in graph_temp and original_tweet_id != tid:

                    original_tweet_time_str = tweet['retweeted_status']['created_at']
                    original_tweet_time = time.mktime(email.utils.parsedate(original_tweet_time_str))

                    graph_temp[original_tweet_id] = {'tid': original_tweet_id,
                                                     'followers_count': tweet['retweeted_status']['user'][
                                                         'followers_count'],
                                                     'time': original_tweet_time,
                                                     'sub': [
                                                         {'tid': tid, 'followers_count': followers_count,
                                                          'time': create_time}]}
    print('finish graph_temp')
    max = 0
    graph = dict()
    for (k, v) in graph_temp.items():
        if len(v['sub']) > 0:
            graph[k] = v
            if len(v['sub']) > max:
                max = len(v['sub'])
    print("max is: " + str(max))

    print('saving,,,,')
    graphCP = copy.deepcopy(graph)

    tree = Octree([])
    tree.constructTree(tree.root, math.ceil(math.log(len(graphCP), 8)))
    tree.mapGraph2Tree(tree.root, graphCP)
    tree.travel(tree.root)
    nodes = tree.nodes
    print('finish init the structures..')

    isModified = True
    return 'Finish the init...'


@app.route('/list_nodes')
def list_nodes():
    return Response(json.dumps(nodes), mimetype='application/json')


@app.route('/show_tree')
def show_tree():
    return Response(json.dumps(tree), mimetype='application/json')


@app.route('/update')
def update():
    global isUpdating

    backend = threading.Thread(target=thread_update, name='updateThread')
    backend.start()

    isUpdating = True
    return 'Updating........'


def thread_update():
    global isUpdating
    time.sleep(10)
    isUpdating = False


@app.route('/update_from_cache')
def update_from_cache():
    global graph
    global nodes
    global isModified

    graphCP = copy.deepcopy(graph)
    tree = Octree([])
    tree.constructTree(tree.root, math.ceil(math.log(len(graphCP), 8)))
    tree.mapGraph2Tree(tree.root, graphCP)
    tree.travel(tree.root)
    nodes = tree.nodes

    isModified = True
    return 'Finish updating...'


@app.route('/update_tree_from_cache')
def update_tree_from_cache():
    global graph
    global tree
    global isModified

    graphCP = copy.deepcopy(graph)
    octree = Octree([])
    octree.constructTree(octree.root, math.ceil(math.log(len(graphCP), 8)))
    octree.mapGraph2Tree(octree.root, graphCP)
    octree.travel_g(octree.root)
    tree = octree.tree

    isModified = True
    return 'Finish updating...'


@app.route('/check_update')
def check_update():
    return str(isUpdating)


@app.route('/quit')
def quit():
    print('Closing the connection with database...')
    client.close()

    print(isModified)
    print('Saving graph and nodes')
    if isModified:
        with open(graph_file_path, 'w') as f:
            json.dump(graph, f)
        with open(nodes_file_path, 'w') as f:
            json.dump(nodes, f)
        with open(tree_file_path, 'w') as f:
            json.dump(tree, f)

    print('Completed.')
    return 'Now if you want, you can press Ctrl-C to stop the server.'


if __name__ == '__main__':
    app.run(threaded=True)
