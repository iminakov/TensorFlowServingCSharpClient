How to train model

1. Install TensorFLow: follow official site instructions: https://www.tensorflow.org/install/ 

2. Open Visual Studio Code with curent folder and execute in terminal:
python ./mnist_deep_training.py --training_iteration=1000 --model_version=1 <Save Model Folder Path>

This is start python script and train MNIST deep model with 1000 iterations.
After that it will save model in <Save Model Folder Path> directory.
Please keep i mind to increament model_version parameter value after every training.

3. Install TensorFlow Serving: follow official site instructions: https://www.tensorflow.org/serving/

4. Start TensorFlow Serving with the following command:
tensorflow_model_server --port=9000 --model_name=mnist --model_base_path=<Save Model Folder Path>