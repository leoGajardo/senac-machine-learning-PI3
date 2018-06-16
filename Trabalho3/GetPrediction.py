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


prediction = GetPredicttion("./predict/num.jpg")
