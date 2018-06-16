import tensorflow as tf
import numpy as np
import os
import cv2
import tflearn
from tflearn.layers.conv import conv_2d, max_pool_2d
from tflearn.layers.core import input_data, dropout, fully_connected
from tflearn.layers.estimator import regression
from tflearn.metrics import Accuracy


tamanho=32
filtro=10


def getModel():
    tf.reset_default_graph()

    #Input layer
    network=input_data([None,tamanho,tamanho,1], name= "input")

    #Primeira layer
    network= conv_2d(network,32,filtro,activation= "relu")
    network= max_pool_2d(network,2)

    #Segunda layer
    network= conv_2d(network,64,filtro,activation= "relu")
    network= max_pool_2d(network,2)

    #Terceira layer
    network= conv_2d(network,128,filtro,activation= "relu")
    network= max_pool_2d(network,2)

    #Quarta layer
    network= conv_2d(network,256,filtro,activation= "relu")
    network= max_pool_2d(network,2)

    #Layer conectada
    network = fully_connected(network, 1024, activation= "relu")
    network = dropout(network, 0.8)

    #Output layer
    network= fully_connected(network, 10, activation="softmax")
    network= regression( network, optimizer= "adam", learning_rate=0.01,  loss= "categorical_crossentropy", name= "targets")

    #Cria modelo

    model = tflearn.DNN(network)
    model.save("./model/untrained-model.tflearn")
    return model

def Prediction(image_path):
    model = getModel()
    model.load("./model/trained-model-ownData.tflean")
    image_data = cv2.imread(image_path, 0)
    image_data = cv2.resize(image_data, (tamanho, tamanho))
    return model.predict(np.array(image_data).reshape(-1, tamanho, tamanho, 1))

def	GetPrediction(image_path):
    result = Prediction(image_path)
    highIndex = -1
    for x in range(0,10):
        if(highIndex == -1):
            highIndex = 0
        if(result[0][x] > result[0][highIndex]):
            highIndex = x   

    print(result[0])
    print("Isso é um:",highIndex,"com %.2f" % (result[0][highIndex] * 100),"% de certeza.")

    return highIndex


prediction = GetPrediction("./predict/num.jpg")
arquivo = open("./predict/result.txt", 'w')
arquivo.writelines(prediction)
arquivo.close()
