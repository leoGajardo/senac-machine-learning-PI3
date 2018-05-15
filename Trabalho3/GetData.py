import numpy as np
import os
import h5py
from tflearn.data_utils import build_hdf5_image_dataset

def build_dataset():
    image_root_folder = "./Raw_DataSets/"
    build_hdf5_image_dataset(image_root_folder, image_shape=(32, 32), mode='folder', output_path='dataset.h5', categorical_labels=True, normalize=True, grayscale=True)

def generating_model():
    # Load HDF5 dataset
    h5f = h5py.File('dataset.h5', 'r')
    
    # Don't what is happening here
    X = h5f['X']
    Y = h5f['Y']

    # Network layers
    # model = DNN(network, ...)
    model.fit(X, Y)
    return model


    # Save a model
    # model.save('my_model.tflearn')
    # Load a model
    # model.load('my_model.tflearn')

def trainModel():
    # Set training mode ON (set is_training var to True)
    # tflearn.is_training(True)
    # Set training mode OFF (set is_training var to False)
    # tflearn.is_training(False)
    # trainop = TrainOp(net=my_network, loss=loss, metric=accuracy)
    # model = Trainer(trainops=trainop, tensorboard_dir='/tmp/tflearn')
    # model.fit(feed_dict={input_placeholder: X, target_placeholder: Y})
    return ""

def predict():
    # model = Evaluator(network)
    # model.predict(feed_dict={input_placeholder: X})
    return ""
    

build_dataset()

