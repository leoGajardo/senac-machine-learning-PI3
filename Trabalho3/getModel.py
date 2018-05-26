import tensorflow as tf
import numpy as np
import os
import h5py
import tflearn
from tflearn.layers.conv import conv_2d, max_pool_2d
from tflearn.layers.core import input_data, dropout, fully_connected
from tflearn.layers.estimator import regression
import tflearn.datasets


dataset = h5py.File('dataset.h5','r')

tamanho=32
filtro=10

x_data= np.array([i[0] for i in dataset])
test_x= np.array([i[0] for i in dataset])

#Input layer
convNet=input_data(shape=[None,tamanho,tamanho,1], name= "input")

#Primeira layer
convNet= conv_2d(convNet,32,filtro,activation= "relu")
convNet= max_pool_2d(convNet,filtro)

#Segunda layer
convNet= conv_2d(convNet,64,filtro,activation= "relu")
convNet= max_pool_2d(convNet,filtro)

#Terceira layer
convNet= conv_2d(convNet,128,filtro,activation= "relu")
convNet= max_pool_2d(convNet,filtro)


#Layer conectada
convNet = fully_connected(convNet, 1024, activation= "relu")

#Output layer
convNet= fully_connected(convNet, 10, activation="softmax")
convNet= regression( convNet, optimizer= "adam", learning_rate=0.01, loss= "categorical_crossentropy", name= "targets")

#Cria modelo

model = tflearn.DNN(convNet)
model.save("./model/untrained-model.tflearn")
