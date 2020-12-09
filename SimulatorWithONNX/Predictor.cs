using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace SimulatorWithONNX
{
    class Predictor
    {
       
        public void inference()
        {
            var session = new InferenceSession("data/upupup.onnx");
            float[] day1_inputs = {0.0000f,  0.0000f,  8.0000f, -1.0000f,  0.7000f,  0.8286f, -0.4000f,  0.0000f};
            //Console.WriteLine(day1_inputs[0]);
            
            //var t1 = new DenseTensor<float>(day1_inputs, new int[] { 1, 8 });
            //Console.WriteLine(inputs[2]);
            //var results = session.Run(inputs);

            var inputMeta = session.InputMetadata;
            var container = new List<NamedOnnxValue>();

            foreach (var name in inputMeta.Keys)
            {
                var tensor = new DenseTensor<float>(day1_inputs, new int[] { 1, 8 });
                container.Add(NamedOnnxValue.CreateFromTensor<float>(name, tensor));
            }

            // Run the inference
            using (var results = session.Run(container))
            {
                // Get the results
                foreach (var r in results)
                {
                    Console.WriteLine("Output Name: {0}", r.Name);
                    var prediction = r.AsTensor<float>()[0];
                    Console.WriteLine("Prediction: " + prediction.ToString());
                }
            }

        }
    }
}