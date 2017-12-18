# How to train model

- Install TensorFLow: follow official site instructions: https://www.tensorflow.org/install/
- Open Visual Studio Code with curent folder and execute in terminal: 
python ./mnist_deep_training.py --training_iteration=1000 --model_version=1 "<Model Training Folder Path>"

This is start python script and train MNIST deep model with 1000 iterations. After that it will save model in directory. Please keep in mind to increament model_version parameter value after every training.

- Install TensorFlow Serving: follow official site instructions: https://www.tensorflow.org/serving/

- Start TensorFlow Serving with the following command: 
tensorflow_model_server --port=9000 --model_name=mnist --model_base_path="<Model Training Folder Path>"
