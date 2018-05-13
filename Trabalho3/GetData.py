import numpy as np
import cv2
import os


def get_data():
    dataSet_path = os.listdir("./Raw_DataSets")
    data = []
    for folder in dataSet_path:
        for image in os.listdir("./Raw_DataSets/" + folder):
            image_path = "./Raw_DataSets/" + folder + "/" + image
            # Load an color image in grayscale
            image_data = cv2.imread("image_path" , 0 )
            data.append(image_data,get_classe(image))

    return data



def get_classe(fileName):
    classe = fileName.split("_")[0]
    if classe == "CLASS0":
        return np.array([1,0,0,0,0,0,0,0,0,0])
    elif classe == "CLASS1":
        return np.array([0,1,0,0,0,0,0,0,0,0])
    elif classe == "CLASS2":
        return np.array([0,0,1,0,0,0,0,0,0,0])
    elif classe == "CLASS3":
        return np.array([0,0,0,1,0,0,0,0,0,0])
    elif classe == "CLASS4":
        return np.array([0,0,0,0,1,0,0,0,0,0])
    elif classe == "CLASS5":
        return np.array([0,0,0,0,0,1,0,0,0,0])
    elif classe == "CLASS6":
        return np.array([0,0,0,0,0,0,1,0,0,0])
    elif classe == "CLASS7":
        return np.array([0,0,0,0,0,0,0,1,0,0])
    elif classe == "CLASS8":
        return np.array([0,0,0,0,0,0,0,0,1,0])
    else:
        return np.array([0,0,0,0,0,0,0,0,0,1])
            

