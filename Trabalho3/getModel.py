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

x= np.load("./DataSets/x_set.npy")
x_dat= np.array([i[0] for i in x]).reshape(-1, tamanho, tamanho,1)
x_class= [i[1] for i in x]

y= np.load("./DataSets/y_set.npy")
y_dat= np.array([i[0] for i in y]).reshape(-1,tamanho,tamanho,1)
y_class= [i[1] for i in y]

def getModel():
    tf.reset_default_graph()

    #Input layer
    convNet=input_data([None,tamanho,tamanho,1], name= "input")

    #Primeira layer
    convNet= conv_2d(convNet,32,filtro,activation= "relu")
    convNet= max_pool_2d(convNet,8)

    #Segunda layer
    convNet= conv_2d(convNet,64,filtro,activation= "relu")
    convNet= max_pool_2d(convNet,8)

    #Terceira layer
    convNet= conv_2d(convNet,128,filtro,activation= "relu")
    convNet= max_pool_2d(convNet,8)

    #Quarta layer
    convNet= conv_2d(convNet,64,filtro,activation= "relu")
    convNet= max_pool_2d(convNet,8)

    #Layer conectada
    convNet = fully_connected(convNet, 1024, activation= "relu")

    #Output layer
    convNet= fully_connected(convNet, 10, activation="softmax")
    convNet= regression( convNet, optimizer= "adam", learning_rate=5,  loss= "categorical_crossentropy", name= "targets")

    #Cria modelo

    model = tflearn.DNN(convNet)
    model.save("./model/untrained-model.tflearn")
    return model

def trainModel():

    model= getModel()
    
    model.fit({'input': x_dat}, {'targets': x_class}, n_epoch=10,
    validation_set=({'input': y_dat}, {'targets': y_class}),
    snapshot_epoch=True, show_metric=True)

    model.save("./model/trained-model.tflean")

def PredictInput(imgPath):
    model = GetModel()
    model.load('./saved-models/mlgame.tflearn')
    img = cv2.imread(imgPath, 0)
    img = cv2.resize(img, (imgSize, imgSize))
    return model.predict(np.array(img).reshape(-1, 64, 64, 1))

def	GetPrediction(imgPath):
    result = PredictInput(imgPath)
    highIndex = -1
    for x in range(0,4):
        if(highIndex == -1):
            highIndex = 0
        if(result[0][x] > result[0][highIndex]):
            highIndex = x

    obj = ""
    if(highIndex == 0):
        obj = "Carro"
    elif(highIndex == 1):
        obj = "Moto"
    elif(highIndex == 2):
        obj = "Barco"
    elif(highIndex == 3):
        obj = "Avião"

    print(result[0])
    print("Isso é um:",obj,"com %.2f" % (result[0][highIndex] * 100),"% de certeza.")

    return highIndex