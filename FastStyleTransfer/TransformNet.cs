using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using alb = AlbiruniML.Ops;
using AlbiruniML;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace FastStyleTransfer
{
    public class TransformNet
    {
        Dictionary<string, Tensor> variables = new Dictionary<string, Tensor>();
        public Dictionary<string, Dictionary<string, Tensor>> variableDictionary = new Dictionary<string, Dictionary<string, Tensor>>();
        Tensor timesScalar;
        Tensor plusScalar;
        Tensor epsilonScalar;

        private static float[] ReadW(string filename)
        {
            var d = File.ReadAllBytes(filename);

            float[] s = new float[d.Length / 4];
            int c = 0;
            for (int i = 0; i < d.Length; i += 4)
            {
                byte[] num = new byte[4];
                num[0] = d[i];
                num[1] = d[i + 1];
                num[2] = d[i + 2];
                num[3] = d[i + 3];
                s[c] = ToFloat(num);
                c++;
            }
            return s;
        }
        static float ToFloat(byte[] input)
        {
            byte[] newArray = new[] { input[0], input[1], input[2], input[3] };
            return BitConverter.ToSingle(newArray, 0);
        }
        public Dictionary<string, Tensor> LoadFolder(string folderName)
        {
            Dictionary<string, modelmetainfo> obj =
                JsonConvert.DeserializeObject<Dictionary<string, modelmetainfo>>(File.ReadAllText(folderName + "/manifest.json"));
            Dictionary<string, Tensor> res = new Dictionary<string, Tensor>();
            foreach (var item in obj)
            {
                float[] s = ReadW(folderName + "/" + item.Value.filename);
                int size = item.Value.shape[0];
                for (int i = 1; i < item.Value.shape.Length; i++)
                {
                    size *= item.Value.shape[i];
                }
                if (size != s.Length)
                {
                    throw new Exception("asdasD");
                }

                var t = alb.tensor(s, item.Value.shape);
              
                res.Add(item.Key, t);

            }

            return res;
        }
        public TransformNet()
        {
            this.timesScalar = alb.scalar(150);
            this.plusScalar = alb.scalar(255.0f / 2f);
            this.epsilonScalar = alb.scalar(1e-3f);
            var wave = LoadFolder("styles/wave");
            variableDictionary.Add("wave", wave);

            if (Directory.Exists("styles/la_muse"))
            {
                variableDictionary.Add("la_muse", LoadFolder("styles/la_muse"));
            }
            if (Directory.Exists("styles/rain_princess"))
            {
                variableDictionary.Add("rain_princess", LoadFolder("styles/rain_princess"));
            }
            if (Directory.Exists("styles/scream"))
            {
                variableDictionary.Add("scream", LoadFolder("styles/scream"));
            }
            if (Directory.Exists("styles/udnie"))
            {
                variableDictionary.Add("udnie", LoadFolder("styles/udnie"));
            }
            if (Directory.Exists("styles/wreck"))
            {
                variableDictionary.Add("wreck", LoadFolder("styles/wreck"));
            }  
            this.variables = wave;
        }
        public void ChangeVariable(string name)
        {
            this.variables = this.variableDictionary[name];
        }
        public string varName(int varId)
        {
            if (varId == 0)
            {
                return "Variable";
            }
            else
            {
                return "Variable_" + varId.ToString();
            }
        }
        public Tensor instanceNorm(Tensor input, int varId)
        {

            var height = input.Shape[0];
            var width = input.Shape[1];
            var inDepth = input.Shape[2];
            var moments = alb.moments(input, new int[] { 0, 1 });
            var mu = moments.mean;
            var sigmaSq = moments.variance;
            var shift = this.variables[this.varName(varId)];
            var scale = this.variables[this.varName(varId + 1)];
            var epsilon = this.epsilonScalar;
            var normalized = alb.div(alb.sub(input, mu),
               alb.sqrt(alb.add(sigmaSq, epsilon)));
            var shifted = alb.add(alb.mul(scale, normalized), shift);


            return shifted.as3D(height, width, inDepth);

        }
        public Tensor convLayer(Tensor input, int strides, bool relu, int varId)
        {
            var y = alb.conv2d(input,
     this.variables[this.varName(varId)], new int[] { strides, strides }, PadType.same);

            var y2 = this.instanceNorm(y, varId + 1);

            if (relu)
            {
                var rl = alb.relu(y2);
                return rl;
            }
            return y2;
        }

        public Tensor convTransposeLayer(Tensor input, int numFilters, int strides, int varId)
        {
            var height = input.Shape[0];
            var width = input.Shape[1];
            var newRows = height * strides;
            var newCols = width * strides;
            var newShape = new int[] { newRows, newCols, numFilters };
            var y = alb.conv2dTranspose(input,
      this.variables[this.varName(varId)],
      newShape, new int[] { strides, strides }, PadType.same);
            var y2 = this.instanceNorm(y, varId + 1);
            var y3 = alb.relu(y2);
            return y3;
        }

        public Tensor residualBlock(Tensor input, int varId)
        {
            var conv1 = this.convLayer(input, 1, true, varId);
            var conv2 = this.convLayer(conv1, 1, false, varId + 3);
            return alb.addStrict(conv2, input);
        }
        public delegate void ReportProgressHandler(int progress);
        public event ReportProgressHandler ReportProgress;
        private void InvokeProgressEvent(int progress)
        {
            if (this.ReportProgress != null)
            {
                this.ReportProgress(progress);
            }
        }

        public Tensor Predict(Tensor preprocessedInput)
        {
            var conv1 = this.convLayer(preprocessedInput, 1, true, 0);
            InvokeProgressEvent(10);
            var conv2 = this.convLayer(conv1, 2, true, 3);
            InvokeProgressEvent(20);
            var conv3 = this.convLayer(conv2, 2, true, 6);
            InvokeProgressEvent(30);
            var resid1 = this.residualBlock(conv3, 9);
            InvokeProgressEvent(40);
            var resid2 = this.residualBlock(resid1, 15);
            InvokeProgressEvent(50);
            var resid3 = this.residualBlock(resid2, 21);
            InvokeProgressEvent(60);
            var resid4 = this.residualBlock(resid3, 27);
            InvokeProgressEvent(70);
            var resid5 = this.residualBlock(resid4, 33);

            InvokeProgressEvent(80);
            var convT1 = this.convTransposeLayer(resid5, 64, 2, 39);
            InvokeProgressEvent(90);
            var convT2 = this.convTransposeLayer(convT1, 32, 2, 42);
            InvokeProgressEvent(100);
            var convT3 = this.convLayer(convT2, 1, false, 45);
            InvokeProgressEvent(110);
            var outTanh = alb.tanh(convT3);
            var scaled = alb.mul(this.timesScalar, outTanh);
            var shifted = alb.add(this.plusScalar, scaled);
            var clamped = alb.clipByValue(shifted, 0, 255);
            var normalized = alb.div(
                clamped, alb.scalar(255.0f));

            InvokeProgressEvent(120);
            return normalized;


        }
    }

    public class modelmetainfo
    {

        public int[] shape { get; set; }
        public string filename { get; set; }
    }
}
