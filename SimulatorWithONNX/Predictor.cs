using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Linq;

namespace SimulatorWithONNX
{
    class Predictor
    {


        public List<bool> inference(List<(int, int, int, int, int, int, int, double, double, double, int)> daily)
        {

            List<bool> c012 = new List<bool>();

            for (int i = 0; i < 3; i++)
            {
                float[] ci = { daily[i].Item5, daily[i].Item6, daily[i].Item7, 0f, (float)(daily[i].Item8 + 10 / 20), (float)(daily[i].Item9 / 9994), (float)daily[i].Item10, daily[i].Item11  };

                var session = new InferenceSession("data/upupup.onnx");
                //float[] day1_inputs = {0.0000f,  0.0000f,  8.0000f, -1.0000f,  0.7000f,  0.8286f, -0.4000f,  0.0000f};

                var inputMeta = session.InputMetadata;
                var container = new List<NamedOnnxValue>();

                foreach (var name in inputMeta.Keys)
                {
                    var tensor = new DenseTensor<float>(ci, new int[] { 1, 8 });
                    container.Add(NamedOnnxValue.CreateFromTensor<float>(name, tensor));
                }

                // Run the inference
                using (var results = session.Run(container))
                {
                    // Get the results
                    foreach (var r in results)
                    {
                        var prediction = Math.Round(r.AsTensor<float>()[0]);
                        bool boolValue = prediction != 0;
                        c012.Add(boolValue);
                        //Console.WriteLine("company" + i.ToString() + ": " + boolValue.ToString());
                    }
                }

            }

            return c012;

        }
    }
}