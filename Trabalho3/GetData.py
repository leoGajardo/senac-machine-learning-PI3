import os
import cv2
import numpy as np 
from random import shuffle 


def get_data():
    path = os.listdir("./Raw_DataSets")
    train_data= []
    for folderName in path:
        for image in os.listdir("./Raw_DataSets/"+folderName):
            img_path= "./Raw_DataSets/" + folderName + "/" + image
            imageData= cv2.imread(img_path, 0)
            imageData= cv2.resize(imageData,(32,32))
            train_data.append([np.array(imageData), get_class(folderName)])        
    shuffle(train_data)
    quarter= len(train_data) / 10
    np.save("./DataSets/x_set.npy", train_data[int(quarter):])
    np.save("./DataSets/y_set.npy", train_data[:int(quarter)])

def get_class(folderName):
    classe = folderName.split('_')[1]
    if classe == '0':
        return np.array([1,0,0,0,0,0,0,0,0,0])
    elif classe=='1':
        return np.array([0,1,0,0,0,0,0,0,0,0])
    elif classe=='2':
        return np.array([0,0,1,0,0,0,0,0,0,0])
    elif classe=='3':
        return np.array([0,0,0,1,0,0,0,0,0,0])
    elif classe=='4':
        return np.array([0,0,0,0,1,0,0,0,0,0])
    elif classe=='5':
        return np.array([0,0,0,0,0,1,0,0,0,0])
    elif classe=='6':
        return np.array([0,0,0,0,0,0,1,0,0,0])
    elif classe=='7':
        return np.array([0,0,0,0,0,0,0,1,0,0])
    elif classe=='8':
        return np.array([0,0,0,0,0,0,0,0,1,0])
    elif classe=='9':
        return np.array([0,0,0,0,0,0,0,0,0,1])

get_data()
