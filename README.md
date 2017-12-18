# TensorFlow Serving MNIST Deep C# client

This is example of C# clients for TensorFlow Serving gRPC service.
Repository contains the following content:
- [learning](https://github.com/Wertugo/TensorFlowServingCSharpClient/tree/master/learning) - python script with MNIST deep training model prepare and Readme short instructions how to execute TensorFlow serving with this model.
- [ClientBaseLib](https://github.com/Wertugo/TensorFlowServingCSharpClient/tree/master/src/BaseLibs/TensorFlowServingClient) - base library with TF Serving gRPC generated classes and utils classes to create Tensors.
- [Console Client](https://github.com/Wertugo/TensorFlowServingCSharpClient/tree/master/src/Clients/ConsoleTensorFlowServingClient) - simple console client application MNIST prediction example 
- [ASP.NET Core 2.0/ ReactJS Client](https://github.com/Wertugo/TensorFlowServingCSharpClient/tree/master/src/Clients/WebTensorFlowServingClient) - SPA application gRPC client for MNIST prediction TensorFlow Serving 

## How to start web application
- Run TensorFlow Serving with instructions [here](https://github.com/Wertugo/TensorFlowServingCSharpClient/tree/master/learning)
- Open web solution and start web site with IIS express. 
- Update appsetting.json with TensorFlow Serving address:
```sh
"TfServer": {
        "ServerUrl": "192.168.1.38:9000"
    }
```

## Tools
- For learning script use [Visual Studio Code](https://code.visualstudio.com/). Windows 7 - 10 or Ubuntu 16.0.6.
- For ASP.NET Core 2.0/ ReactJS project use [MSVS 2017](https://www.visualstudio.com/) or later
